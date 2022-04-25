using LineMessageAPI.Models.PostModels;
using LineMessageAPI.Services;

namespace LineMessageAPI.Interfaces
{
    public interface ILineMessageService
    {
        Task<string> SaveData(LineMessagePost? postdata, RequestIDService requestIDService);
    }
}