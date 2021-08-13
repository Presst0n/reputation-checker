using RepChecker.Core;
using RepChecker.Extensions;
using RepChecker.Helpers;
using RepChecker.MVVM.Model;
using RepChecker.Repository;
using RepChecker.Services;
using RepChecker.Settings;
using RepDataCollector.Models.Responses;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Input;

namespace RepChecker.MVVM.ViewModel
{
    public class ReputationViewModel : ViewModelBase
    {
        public event EventHandler<bool> OnLoadingReputationsCompleted;

        private LoggedInUserModel _loggedInUser;
        private ObservableCollection<ReputationModel> _testModels;
        private readonly IApiService _apiService;
        private List<ReputationModel> _reputationsCollection;
        private readonly IStandingsRepository _standingsRepository;
        private bool _isDataLoaded;
        private string _reputationsNumber;
        private readonly IApplicationSettings _userAppSettings;
        private IMainViewModel _mainViewModel;

        ~ReputationViewModel()
        {

        }

        public ReputationViewModel(IApiService apiService, IStandingsRepository standingsRepository, LoggedInUserModel loggedInUser, 
            IApplicationSettings userAppSettings, IMainViewModel mainViewModel)
        {
            _apiService = apiService;
            _standingsRepository = standingsRepository;
            _loggedInUser = loggedInUser;
            _userAppSettings = userAppSettings;
            _mainViewModel = mainViewModel;

            IsUserLoggedIn = _loggedInUser.IsLoggedIn;
            _mainViewModel.OnReputationFilter += OnReputationFilter;
            _mainViewModel.OnUserLogIn += OnUserLogIn;
            _mainViewModel.OnLogOut += OnLogOut;
        }

        private void OnLogOut(object sender, EventArgs e)
        {
            _mainViewModel.OnReputationFilter -= OnReputationFilter;
            _mainViewModel.OnUserLogIn -= OnUserLogIn;
            _mainViewModel.OnLogOut -= OnLogOut;
            //_loggedInUser = null;
            //_mainViewModel = null;
            //_testModels = null;
        }

        // TODO: Add logging off feat
        // TODO: Implement changing colors in app based on chosen theme by user.
        // TODO: Create general validation, so that user will get notified when some functions of applications are not available for some reason or error occured.
        // TODO: Need to implement logging to file (maybe .txt) system . I will prolly use some well written and respected 3rd party library (nlog/serilog?).

        public ApplicationUserModel AppUserModel { get; private set; }

        private bool _isUserLoggedIn;

        public bool IsUserLoggedIn 
        { 
            get
            {
                return _isUserLoggedIn;
            }
            set
            {
                _isUserLoggedIn = value;
                OnPropertyChanged();
            }
        }

        public string SelectedReputationLvl { get; set; }

        private string _searchText;

        public string SearchText
        {
            get => _searchText;
            set
            {
                _searchText = value?.ToLowerInvariant();
                //_reputationsCollection.Where(e => e.Standing.Level == TestModels.FirstOrDefault().Standing.Level).Where(x => x.ReputationName.Contains(_searchText)).ToObservableCollection();
                if (TestModels is null)
                    return;
                //if (TestModels.Count == 0)


                /*_reputationsCollection.Where(e => e.Standing.Level == TestModels.FirstOrDefault()?.Standing.Level);*/
                if (_searchText == "")
                {
                    TestModels = _reputationsCollection.Where(e => e.Standing.Level == SelectedReputationLvl).ToObservableCollection();
                    OnPropertyChanged();
                    return;
                }

                var filteredData = _reputationsCollection.Where(e => e.Standing.Level == SelectedReputationLvl).Where(x => x.ReputationName.ToLowerInvariant().Contains(_searchText)).ToObservableCollection(); 

                TestModels = filteredData;
                OnPropertyChanged();
            }
        }

        public string ReputationsNumber
        {
            get => _reputationsNumber;
            set 
            {
                _reputationsNumber = value ?? "0";  // value == null ? "0" : value;
                OnPropertyChanged();
            }
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
                ReputationsNumber = _testModels?.Count.ToString();
                OnPropertyChanged();
            }
        }

        private async void OnUserLogIn(object sender, bool e)
        {
            IsUserLoggedIn = e;

            if (e)
            {
                await LoadReputations();
            }
        }

        private void OnReputationFilter(object sender, string e)
        {
            SelectedReputationLvl = e;

            if (ReputationsCollection is null)
                return;

            var filteredCollection = ReputationsCollection.Where(x => x.Standing.Level == e).ToObservableCollection();
            TestModels = filteredCollection;
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
                    if (rep.Faction.Name == "Guild")
                        continue;

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

        private async Task<List<ReputationModel>> LoadNewReputations()
        {
            var characters = await _apiService.GetAllUserWowCharactersAsync();
            if (characters is null)
                return null;

            var repResponses = await _apiService.GetReputationsByCharactersAsync(characters);

            if (repResponses is null || repResponses.Count == 0)
                return null;

            var unfilteredReputations = MapReputationsData(repResponses);
            var filteredReputations = FilterReputations(unfilteredReputations);

            return filteredReputations;
        }

        public async Task LoadReputations()
        {
            if (ReputationsCollection != null)
            {
                if (DateTime.Now < DateTime.Parse(AppUserModel.LastUpdate).Add(_userAppSettings.GetDataRefreshTimeValue()))
                    return;
            }

            AppUserModel = await _standingsRepository.LoadDataAsync(_loggedInUser.BattleTag);

            if (AppUserModel is null)
            {
                var reps = await LoadNewReputations();

                if (reps is null)
                    return;

                ReputationsCollection = reps;

                try
                {
                    await _standingsRepository.SaveDataAsync(new ApplicationUserModel()
                    {
                        BattleTag = _loggedInUser?.BattleTag,
                        Id = IdGenerator.GenerateId(),
                        UserReputations = reps,
                        LastUpdate = DateTime.UtcNow.ToLocalTime().ToString()
                    });
                }
                catch (Exception ex)
                {
                    // display some modal box or red bar with error text in UI.
                    // "Something went wrong during saving data in database. To get more details check logs."
                    throw ex;
                }

                NotifyIfLoaded(true);
                return;
            }

            if (DateTime.Now >= DateTime.Parse(AppUserModel.LastUpdate).Add(_userAppSettings.GetDataRefreshTimeValue()))
            {
                var reps = await LoadNewReputations();
                ReputationsCollection = reps;

                await _standingsRepository.UpdateDataAsync(new ApplicationUserModel()
                {
                    BattleTag = _loggedInUser.BattleTag,
                    LastUpdate = DateTime.Now.ToString(),
                    UserReputations = reps
                });

                NotifyIfLoaded(true);
                return;
            }

            ReputationsCollection = AppUserModel.UserReputations;

            NotifyIfLoaded(true);
        }

        private void NotifyIfLoaded(bool isLoaded)
        {
            IsDataLoaded = isLoaded;
            OnLoadingReputationsCompleted?.Invoke(this, isLoaded);
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
                if (rep.Standing.Level != "Exalted" && unfilteredReputations.Exists(r => r.Standing.Level == "Exalted" && r.ReputationId == rep.ReputationId))
                {
                    repsToRemove.Add(rep);
                }

                if (rep.Standing.Level != "Revered" && rep.Standing.Level != "Exalted" && unfilteredReputations.Exists(r => r.Standing.Level == "Revered" && r.ReputationId == rep.ReputationId))
                {
                    repsToRemove.Add(rep);
                }

                //if (rep.Standing.Level != Revered && rep.Standing.Level != Exalted && unfilteredReputations.Exists(r => r.Standing.Level == Revered && r.ReputationId == rep.ReputationId))
                //{
                //    repsToRemove.Add(rep);
                //}

                if (rep.Standing.Level != "Honored" && rep.Standing.Level != "Exalted" && rep.Standing.Level != "Revered" && unfilteredReputations.Exists(r => r.Standing.Level == "Honored" && r.ReputationId == rep.ReputationId))
                {
                    repsToRemove.Add(rep);
                }

                if (rep.Standing.Level != "Friendly" && rep.Standing.Level != "Exalted" && rep.Standing.Level != "Revered" && rep.Standing.Level != "Honored"
                    && unfilteredReputations.Exists(r => r.Standing.Level == "Friendly" && r.ReputationId == rep.ReputationId))
                {
                    repsToRemove.Add(rep);
                }

                if (rep.Standing.Level != "Neutral" && rep.Standing.Level != "Friendly" && rep.Standing.Level != "Exalted" && rep.Standing.Level != "Revered" && rep.Standing.Level != "Honored"
                    && unfilteredReputations.Exists(r => r.Standing.Level == "Neutral" && r.ReputationId == rep.ReputationId))
                {
                    repsToRemove.Add(rep);
                }

                if (rep.Standing.Level != "Unfriendly" && rep.Standing.Level != "Neutral" && rep.Standing.Level != "Friendly" && rep.Standing.Level != "Exalted" && rep.Standing.Level != "Revered"
                    && rep.Standing.Level != "Honored" && unfilteredReputations.Exists(r => r.Standing.Level == "Unfriendly" && r.ReputationId == rep.ReputationId))
                {
                    repsToRemove.Add(rep);
                }

                if (rep.Standing.Level != "Hostile" && rep.Standing.Level != "Unfriendly" && rep.Standing.Level != "Neutral" && rep.Standing.Level != "Friendly" && rep.Standing.Level != "Exalted"
                    && rep.Standing.Level != "Revered" && rep.Standing.Level != "Honored" && unfilteredReputations.Exists(r => r.Standing.Level == "Hostile" && r.ReputationId == rep.ReputationId))
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

        //public void Dispose()
        //{
        //    _mainViewModel.OnReputationFilter -= OnReputationFilter;
        //    _mainViewModel.OnUserLogIn -= OnUserLogIn;
        //}
    }
}
