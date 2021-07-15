using RepChecker.DtoModels;
using RepChecker.MVVM.Model;
using System.Threading.Tasks;

namespace RepChecker.Repository
{
    public interface IStandingsRepository
    {
        Task<bool> DeleteDataAsync(ApplicationUserModel userModel);
        Task<ApplicationUserModel> LoadDataAsync(string battleTag);
        Task<bool> SaveDataAsync(ApplicationUserModel userModel);
        Task<bool> UpdateDataAsync(ApplicationUserModel userModel);
    }
}