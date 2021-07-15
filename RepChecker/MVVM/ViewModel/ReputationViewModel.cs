using Microsoft.EntityFrameworkCore;
using Nito.AsyncEx;
using RepChecker.Core;
using RepChecker.Data;
using RepChecker.DtoModels;
using RepChecker.Enums;
using RepChecker.EventModels;
using RepChecker.Extensions;
using RepChecker.MVVM.Model;
using RepChecker.Repository;
using RepChecker.Services;
using RepDataCollector.Core;
using RepDataCollector.Models.Responses;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Input;

namespace RepChecker.MVVM.ViewModel
{
    public class ReputationViewModel : ViewModelBase
    {
        private const string Exalted = "Exalted";
        private const string Revered = "Revered";
        private const string Honored = "Honored";
        private const string Friendly = "Friendly";
        private const string Neutral = "Neutral";
        private const string Unfriendly = "Unfriendly";
        private const string Hostile = "Hostile";
        private const string Hated = "Hated";

        public event EventHandler<bool> OnLoadingReputationsCompleted;

        private readonly LoggedInUserModel _loggedInUser;
        private ObservableCollection<ReputationModel> _testModels;
        private readonly IApiService _apiService;
        private List<ReputationModel> _reputationsCollection;
        private ReputationDbContext _dbContext;
        private readonly IStandingsRepository _standingsRepository;
        private bool _isDataLoaded;

        public ReputationViewModel(IApiService apiService, ReputationDbContext dbContext, IStandingsRepository standingsRepository, LoggedInUserModel loggedInUser)
        {
            _apiService = apiService;
            _dbContext = dbContext;
            _standingsRepository = standingsRepository;
            _loggedInUser = loggedInUser;
        }

        public List<ReputationModel> ReputationsCollection
        {
            get => _reputationsCollection;
            set
            {
                _reputationsCollection = value;
                OnPropertyChanged();
            }
        }

        public bool IsDataLoaded
        {
            get => _isDataLoaded;
            set
            {
                _isDataLoaded = value;
                OnPropertyChanged();
            }
        }

        public ObservableCollection<ReputationModel> TestModels
        {
            get => _testModels;
            set
            {
                _testModels = value;
                OnPropertyChanged();
            }
        }

        public ICommand SortInAlphabeticalOrder => new RelayCommand<string>(mode =>
        {
            if (TestModels is null || TestModels?.Count == 0)
                return;

            var sortedData = TestModels.OrderBy(x => x.ReputationName).ToList();
            TestModels = sortedData.ToObservableCollection();
        });

        public ICommand SortInReverseAlphabeticalOrder => new RelayCommand<string>(mode =>
        {
            if (TestModels is null || TestModels?.Count == 0)
                return;

            var sortedData = TestModels.OrderBy(x => x.ReputationName).Reverse().ToList();
            TestModels = sortedData.ToObservableCollection();
        });

        private List<ReputationModel> MapReputationsData(List<ReputationResponse> repResponses)
        {
            var reps = new List<ReputationModel>();

            foreach (var response in repResponses)
            {
                if (response?.Character is null || response?.Reputations is null)
                    continue;

                foreach (var rep in response.Reputations)
                {
                    var repModel = new ReputationModel
                    {
                        ReputationName = rep.Faction.Name,
                        Character = response.Character.Name,
                        Realm = response.Character.Realm.Slug,
                        FactionHref = rep.Faction.Key.Href,
                        ReputationId = Convert.ToInt32(rep.Faction.Id),
                        BattleTag = _loggedInUser?.BattleTag,
                        Standing = new StandingModel
                        {
                            CurrentValue = rep.Standing.Tier == 7 ? 21000 : rep.Standing.Value,
                            Level = rep.Standing.Tier.ToLevel(),
                            Max = rep.Standing.Tier == 7 ? 21000 : rep.Standing.Max,
                            Raw = rep.Standing.Raw,
                            Tier = rep.Standing.Tier
                        }
                    };

                    reps.Add(repModel);
                }
            }

            return reps;
        }

        private async Task<List<ReputationModel>> LoadReputationsDataFromApi()
        {
            var characters = await _apiService.GetAllUserWowCharactersAsync();

            if (characters is null)
                return null;

            var repResponses = await _apiService.GetReputationsByCharactersAsync(characters);

            if (repResponses is null || repResponses.Count == 0)
                return null;

            var unfilteredReputations = MapReputationsData(repResponses);
            var filteredReputations = FilterReputations(unfilteredReputations);
            //await GetFactionNames(filteredReputations);

            return filteredReputations;
        }

        private int GenerateId()
        {
            var now = DateTime.Now;
            var zeroDate = DateTime.MinValue.AddHours(now.Hour).AddMinutes(now.Minute).AddSeconds(now.Second).AddMilliseconds(now.Millisecond);
            int uniqueId = (int)(zeroDate.Ticks / 10000);

            return uniqueId;
        }

        public async Task LoadReputations()
        {
            // Check if reputations of this user exist in database and if so, check if they are older than 4hrs. 
            // If that's false then load them instead requesting API. Otherwise get new data from api. 

            // Validation here needed.
            //await _standingsRepository.SaveDataAsync(new ApplicationUserModel()
            //{
            //    BattleTag = "Vlad#5623",
            //    Id = GenerateId(),
            //    UserReputations = new List<ReputationModel>() 
            //    {
            //        new ReputationModel()
            //        {
            //            BattleTag = "Vlad#5623",
            //            Character = "Vladzioszek",
            //            FactionHref = "https://www.something.com",
            //            Realm = "TwistingNether",
            //            ReputationId = GenerateId(),
            //            ReputationName = "PlaceHolder",
            //            Standing = new StandingModel()
            //            {
            //                CurrentValue = 21000,
            //                Level = "Exalted",
            //                ReputationId = 133,
            //                Max = 21000,
            //                Raw = 45000,
            //                StandingId = GenerateId(),
            //                Tier = 7
            //            }
            //        }
            //    },
            //    LastUpdate = DateTime.UtcNow.ToLocalTime().ToString()
            //});

            var data = await _standingsRepository.LoadDataAsync(_loggedInUser.BattleTag);

            if (data is null)
            {
                var reps = await LoadReputationsDataFromApi();

                if (reps is null)
                    return;

                ReputationsCollection = reps;
                // Maybe I should parse DateTime to string in repository or while mapping with automapper.
                try
                {
                    await _standingsRepository.SaveDataAsync(new ApplicationUserModel()
                    {
                        BattleTag = _loggedInUser?.BattleTag,
                        Id = GenerateId(),
                        UserReputations = reps,
                        LastUpdate = DateTime.UtcNow.ToLocalTime().ToString()
                    });
                }
                catch (Exception ex)
                {
                    // display some modal box or red bar with error text in UI.
                    throw ex;
                }

                //var date = DateTime.Parse(DateTime.UtcNow.ToLocalTime().ToString());
                IsDataLoaded = true;
                OnLoadingReputationsCompleted?.Invoke(this, true);
                return;
            }

            if (DateTime.Now >= DateTime.Parse(data.LastUpdate).AddHours(1))
            {
                var reps = await LoadReputationsDataFromApi();
                ReputationsCollection = reps;

                await _standingsRepository.UpdateDataAsync(new ApplicationUserModel()
                {
                    BattleTag = _loggedInUser.BattleTag,
                    LastUpdate = DateTime.Now.ToString(),
                    UserReputations = reps
                });

                IsDataLoaded = true;
                OnLoadingReputationsCompleted?.Invoke(this, true);
                return;
            }

            ReputationsCollection = data.UserReputations;

            IsDataLoaded = true;
            OnLoadingReputationsCompleted?.Invoke(this, true);
        }

        private List<ReputationModel> FilterReputations(List<ReputationModel> unfilteredReputations)
        {
            var uniqueReputations = RemoveDuplicatedReputations(unfilteredReputations);

            var filteredReputations = RemoveDuplicatesWithLowerTier(unfilteredReputations, uniqueReputations);
            return filteredReputations;
        }

        private List<ReputationModel> RemoveDuplicatedReputations(List<ReputationModel> unfilteredReputations)
        {
            var filteredReputations = new List<ReputationModel>();

            foreach (var r in unfilteredReputations)
            {
                if (filteredReputations.Exists(x => x.ReputationId == r.ReputationId && x.Standing.Level == r.Standing.Level))
                    continue;

                var itemWithHighiestCurrentValue = unfilteredReputations.Where(x => x.ReputationId == r.ReputationId && x.Standing.Level == r.Standing.Level)
                                                                        .ToList()
                                                                        .OrderByDescending(i => i.Standing.CurrentValue)
                                                                        .FirstOrDefault();

                filteredReputations.Add(itemWithHighiestCurrentValue);
            }

            return filteredReputations;
        }

        private List<ReputationModel> RemoveDuplicatesWithLowerTier(List<ReputationModel> unfilteredReputations, List<ReputationModel> uniqueReputations)
        {
            if (uniqueReputations is null || uniqueReputations is null)
                return null;

            var repsToRemove = new List<ReputationModel>();

            foreach (var rep in uniqueReputations)
            {
                if (rep.Standing.Level != Exalted && unfilteredReputations.Exists(r => r.Standing.Level == Exalted && r.ReputationId == rep.ReputationId))
                {
                    repsToRemove.Add(rep);
                }

                if (rep.Standing.Level != Revered && rep.Standing.Level != Exalted && unfilteredReputations.Exists(r => r.Standing.Level == Revered && r.ReputationId == rep.ReputationId))
                {
                    repsToRemove.Add(rep);
                }

                if (rep.Standing.Level != Revered && rep.Standing.Level != Exalted && unfilteredReputations.Exists(r => r.Standing.Level == Revered && r.ReputationId == rep.ReputationId))
                {
                    repsToRemove.Add(rep);
                }

                if (rep.Standing.Level != Honored && rep.Standing.Level != Exalted && rep.Standing.Level != Revered && unfilteredReputations.Exists(r => r.Standing.Level == Honored && r.ReputationId == rep.ReputationId))
                {
                    repsToRemove.Add(rep);
                }

                if (rep.Standing.Level != Friendly && rep.Standing.Level != Exalted && rep.Standing.Level != Revered && rep.Standing.Level != Honored
                    && unfilteredReputations.Exists(r => r.Standing.Level == Friendly && r.ReputationId == rep.ReputationId))
                {
                    repsToRemove.Add(rep);
                }

                if (rep.Standing.Level != Neutral && rep.Standing.Level != Friendly && rep.Standing.Level != Exalted && rep.Standing.Level != Revered && rep.Standing.Level != Honored
                    && unfilteredReputations.Exists(r => r.Standing.Level == Neutral && r.ReputationId == rep.ReputationId))
                {
                    repsToRemove.Add(rep);
                }

                if (rep.Standing.Level != Unfriendly && rep.Standing.Level != Neutral && rep.Standing.Level != Friendly && rep.Standing.Level != Exalted && rep.Standing.Level != Revered
                    && rep.Standing.Level != Honored && unfilteredReputations.Exists(r => r.Standing.Level == Unfriendly && r.ReputationId == rep.ReputationId))
                {
                    repsToRemove.Add(rep);
                }

                if (rep.Standing.Level != Hostile && rep.Standing.Level != Unfriendly && rep.Standing.Level != Neutral && rep.Standing.Level != Friendly && rep.Standing.Level != Exalted
                    && rep.Standing.Level != Revered && rep.Standing.Level != Honored && unfilteredReputations.Exists(r => r.Standing.Level == Hostile && r.ReputationId == rep.ReputationId))
                {
                    repsToRemove.Add(rep);
                }
            }

            repsToRemove.ForEach(r =>
            {
                uniqueReputations.Remove(r);
            });

            return uniqueReputations;
        }
    }
}
