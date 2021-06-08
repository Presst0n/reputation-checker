using Nito.AsyncEx;
using RepChecker.Core;
using RepChecker.Enums;
using RepChecker.EventModels;
using RepChecker.Extensions;
using RepChecker.MVVM.Model;
using RepDataCollector.Core;
using RepDataCollector.Models.Responses;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

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

        public event EventHandler<bool> LoadingReputationsCompleted;

        private ObservableCollection<ReputationModel> _testModels;
        private readonly BattleNetApiRequestsHandler _apiRequestsHandler;

        //private List<ReputationModel> _allReputations;
        private Reputations _reputationsCollection;

        //public ReputationModel Reputation { get; set; }


        public ReputationViewModel(BattleNetApiRequestsHandler apiRequestsHandler)
        {
            _apiRequestsHandler = apiRequestsHandler;
        }

        //public List<ReputationModel> AllReputations
        //{
        //    get => _allReputations;
        //    set
        //    {
        //        _allReputations = value;
        //    }
        //}

        public Reputations ReputationsCollection
        {
            get => _reputationsCollection;
            set
            {
                _reputationsCollection = value;
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

        public void LongRunningTestTask()
        {

            Thread.Sleep(7000);

            var reputations = new Reputations();

            for (int i = 0; i < 542; i++)
            {
                reputations.Exalted.Add(new ReputationModel { Character = "Testnik", Realm = "Kazzuk", ReputationName = "Kartoszki", Standing = new StandingModel 
                { CurrentValue = 36000, Level = "Exalted", Max = 3600, Raw = 0 } });
            }

            ReputationsCollection = reputations;
            LoadingReputationsCompleted?.Invoke(this, true);
        }

        private async Task<List<ReputationModel>> MapRepDataAsync(List<ReputationResponse> repResponses)
        {
            var reps = new List<ReputationModel>();

            foreach (var response in repResponses)
            {
                if (response.Character is null || response.Reputations is null)
                    continue;

                foreach (var rep in response.Reputations)
                {
                    var factionName = await _apiRequestsHandler.GetFactionAsync(rep.Faction.Key.Href);

                    var repModel = new ReputationModel
                    {
                        Character = response.Character.Name,
                        Realm = response.Character.Realm.Slug,
                        ReputationName = factionName.Name.English,
                        Id = rep.Faction.Id,
                        Standing = new StandingModel
                        {
                            CurrentValue = rep.Standing.Value,
                            Level = rep.Standing.Tier.ToLevel(),
                            Max = rep.Standing.Max,
                            Raw = rep.Standing.Raw
                        }
                    };

                    reps.Add(repModel);

                }
            }

            return reps;
        }

        public async Task LoadReputations()
        {
            var characters = await _apiRequestsHandler.GetAllUserWowCharactersAsync();
            var repResponses = await _apiRequestsHandler.GetReputationsFromAllCharactersAsync(characters);

            //AllReputations = new List<ReputationModel>();

            var reps = await MapRepDataAsync(repResponses);

            var reputations = new Reputations
            {
                Exalted = new List<ReputationModel>(),
                Revered = new List<ReputationModel>(),
                Honored = new List<ReputationModel>(),
                Friendly = new List<ReputationModel>(),
                Neutral = new List<ReputationModel>(),
                Unfriendly = new List<ReputationModel>(),
                Hostile = new List<ReputationModel>(),
                Hated = new List<ReputationModel>()
            };

            var repsToRemove = new List<ReputationModel>();

            foreach (var rep in reps)
            {
                if (rep.Standing.Level != Exalted && reps.FindAll(r => r.Standing.Level == Exalted && r.Id == rep.Id).Count > 0)
                {
                    repsToRemove.Add(rep);
                }
            }

            reps.ForEach(rep =>
            {
                if (rep.Standing.Level != Revered && rep.Standing.Level != Exalted && reps.Exists(r => r.Standing.Level == Revered && r.Id == rep.Id))
                {
                    repsToRemove.Add(rep);
                }
            });

            reps.ForEach(rep =>
            {
                if (rep.Standing.Level != Honored && rep.Standing.Level != Exalted && rep.Standing.Level != Revered && reps.Exists(r => r.Standing.Level == Honored && r.Id == rep.Id))
                {
                    repsToRemove.Add(rep);
                }
            });

            reps.ForEach(rep =>
            {
                if (rep.Standing.Level != Friendly && rep.Standing.Level != Exalted && rep.Standing.Level != Revered && rep.Standing.Level != Honored 
                && reps.Exists(r => r.Standing.Level == Friendly && r.Id == rep.Id))
                {
                    repsToRemove.Add(rep);
                }
            });

            reps.ForEach(rep =>
            {
                if (rep.Standing.Level != Neutral && rep.Standing.Level != Friendly && rep.Standing.Level != Exalted && rep.Standing.Level != Revered && rep.Standing.Level != Honored 
                && reps.Exists(r => r.Standing.Level == Neutral && r.Id == rep.Id))
                {
                    repsToRemove.Add(rep);
                }
            });            
            
            reps.ForEach(rep =>
            {
                if (rep.Standing.Level != Unfriendly && rep.Standing.Level != Neutral && rep.Standing.Level != Friendly && rep.Standing.Level != Exalted && rep.Standing.Level != Revered 
                && rep.Standing.Level != Honored && reps.Exists(r => r.Standing.Level == Unfriendly && r.Id == rep.Id))
                {
                    repsToRemove.Add(rep);
                }
            });

            reps.ForEach(rep =>
            {
                if (rep.Standing.Level != Hostile && rep.Standing.Level != Unfriendly && rep.Standing.Level != Neutral && rep.Standing.Level != Friendly && rep.Standing.Level != Exalted 
                && rep.Standing.Level != Revered && rep.Standing.Level != Honored && reps.Exists(r => r.Standing.Level == Hostile && r.Id == rep.Id))
                {
                    repsToRemove.Add(rep);
                }
            });

            reps.ForEach(rep =>
            {
                if (rep.Standing.Level != Hated && rep.Standing.Level != Hostile && rep.Standing.Level != Unfriendly && rep.Standing.Level != Neutral && rep.Standing.Level != Friendly && rep.Standing.Level != Exalted
                && rep.Standing.Level != Revered && rep.Standing.Level != Honored && reps.Exists(r => r.Standing.Level == Hated && r.Id == rep.Id))
                {
                    repsToRemove.Add(rep);
                }
            });

            repsToRemove.ForEach(r =>
            {
                reps.Remove(r);
            });

            reputations.Exalted = reps.FindAll(r => r.Standing.Level == "Exalted");
            reputations.Revered = reps.FindAll(r => r.Standing.Level == "Revered");
            reputations.Honored = reps.FindAll(r => r.Standing.Level == "Honored");
            reputations.Friendly = reps.FindAll(r => r.Standing.Level == "Friendly");
            reputations.Neutral = reps.FindAll(r => r.Standing.Level == "Neutral");
            reputations.Unfriendly = reps.FindAll(r => r.Standing.Level == "Unfriendly");
            reputations.Hostile = reps.FindAll(r => r.Standing.Level == "Hostile");
            reputations.Hated = reps.FindAll(r => r.Standing.Level == "Hated");


            ReputationsCollection = reputations;

            LoadingReputationsCompleted?.Invoke(this, true);
        }



    }
}
