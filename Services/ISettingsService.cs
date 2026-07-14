using System.Threading.Tasks;

namespace SwiftScan.Services
{
    public interface ISettingsService
    {
        Task<string> GetValueAsync(string key, string defaultValue = "");
        Task SaveValueAsync(string key, string value);
        string HashPin(string pin);
    }
}
