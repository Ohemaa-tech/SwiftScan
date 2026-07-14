using System;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;
using SwiftScan.Models;

namespace SwiftScan.Services
{
    public class SettingsService : ISettingsService
    {
        private readonly DatabaseContext _dbContext;

        public SettingsService(DatabaseContext dbContext)
        {
            _dbContext = dbContext;
        }

        public async Task<string> GetValueAsync(string key, string defaultValue = "")
        {
            try
            {
                await _dbContext.InitializeAsync().ConfigureAwait(false);
                var setting = await _dbContext.Connection.FindAsync<AppSettings>(key).ConfigureAwait(false);
                return setting?.Value ?? defaultValue;
            }
            catch (Exception)
            {
                // Handle potential issues during early startup initialization
                return defaultValue;
            }
        }

        public async Task SaveValueAsync(string key, string value)
        {
            await _dbContext.InitializeAsync().ConfigureAwait(false);
            var setting = new AppSettings { Key = key, Value = value };
            await _dbContext.Connection.InsertOrReplaceAsync(setting).ConfigureAwait(false);
        }

        public string HashPin(string pin)
        {
            if (string.IsNullOrWhiteSpace(pin))
                return string.Empty;

            byte[] bytes = SHA256.HashData(Encoding.UTF8.GetBytes(pin));
            var builder = new StringBuilder();
            for (int i = 0; i < bytes.Length; i++)
            {
                builder.Append(bytes[i].ToString("x2"));
            }
            return builder.ToString();
        }
    }
}
