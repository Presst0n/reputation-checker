using AutoMapper;
using Microsoft.EntityFrameworkCore;
using RepChecker.Data;
using RepChecker.DtoModels;
using RepChecker.MVVM.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace RepChecker.Repository
{
    public class StandingsRepository : IStandingsRepository
    {
        private readonly ReputationDbContext _db;
        private readonly IMapper _mapper;

        public StandingsRepository(ReputationDbContext db, IMapper mapper)
        {
            _db = db;
            _mapper = mapper;
        }

        public async Task<bool> SaveDataAsync(ApplicationUserModel userModel)
        {
            if (userModel is null || userModel.UserReputations is null || userModel.UserReputations?.Count < 1)
                return false;

            if (_db.ApplicationUsers?.Where(x => x.BattleTag == userModel.BattleTag).Count() > 1)
                return false;

            var userDto = _mapper.Map<ApplicationUserModelDto>(userModel);


            await _db.ApplicationUsers.AddAsync(userDto);
            await _db.SaveChangesAsync();
            
            return true;
        }

        public async Task<ApplicationUserModel> LoadDataAsync(string battleTag, bool userDataOnly = false)
        {
            if (string.IsNullOrEmpty(battleTag))
                return null;

            var btag = battleTag.Trim();

            if (_db.ApplicationUsers.Where(u => u.BattleTag == btag).Count() < 1)
                return null;

            ApplicationUserModelDto unmappedData = null;

            if (userDataOnly == false)
            {
                unmappedData = await _db.ApplicationUsers
                    .Include(u => u.UserReputations)
                    .ThenInclude(s => s.Standing)
                    .FirstOrDefaultAsync(x => x.BattleTag == btag);
            }
            else
            {
                unmappedData = await _db.ApplicationUsers
                    .FirstOrDefaultAsync(x => x.BattleTag == btag);
            }

            return _mapper.Map<ApplicationUserModel>(unmappedData);
        }

        //public async Task<ApplicationUserModel> LoadDataAsync(string battleTag, bool userDataOnly = false)
        //{
        //    if (string.IsNullOrEmpty(battleTag))
        //        return null;

        //    var btag = battleTag.Trim();

        //    if (_db.ApplicationUsers.Where(u => u.BattleTag == btag).Count() < 1)
        //        return null;

        //    var unmappedData = await _db.ApplicationUsers
        //        .FirstOrDefaultAsync(x => x.BattleTag == btag);

        //    return _mapper.Map<ApplicationUserModel>(unmappedData);
        //}

        public async Task<bool> UpdateDataAsync(ApplicationUserModel userModel)
        {
            if (userModel is null)
                return false;

            var userDb = await _db.ApplicationUsers.FirstOrDefaultAsync(x => x.BattleTag == userModel.BattleTag);

            if (userDb is null)
                return false;

            userDb.LastUpdate = userModel.LastUpdate;
            var test = _mapper.Map<List<ReputationModelDto>>(userModel.UserReputations);
            userDb.UserReputations = _mapper.Map<List<ReputationModelDto>>(userModel.UserReputations);


            await _db.SaveChangesAsync();

            return true;
        }

        public async Task<bool> DeleteDataAsync(ApplicationUserModel userModel)
        {
            if (userModel is null)
                return false;

            var data = await _db.ApplicationUsers.FirstOrDefaultAsync(x => x.BattleTag == userModel.BattleTag);

            if (data is null)
                return false;

            _db.ApplicationUsers.Remove(data);

            return await _db.SaveChangesAsync() > 0;
        }
    }
}
