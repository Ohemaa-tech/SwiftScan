using SQLite;
using System.IO;
using System.Threading.Tasks;
using SwiftScan.Models;

namespace SwiftScan.Services
{
    public class DatabaseContext
    {
        private SQLiteAsyncConnection? _database;
        private readonly string _dbPath;

        public DatabaseContext()
        {
            var folder = Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData);
            _dbPath = Path.Combine(folder, "swiftscan.db3");
        }

        public async Task InitializeAsync()
        {
            if (_database != null)
                return;

            _database = new SQLiteAsyncConnection(_dbPath, SQLiteOpenFlags.ReadWrite | SQLiteOpenFlags.Create | SQLiteOpenFlags.SharedCache);
            await _database.CreateTableAsync<AppSettings>().ConfigureAwait(false);
        }

        public SQLiteAsyncConnection Connection
        {
            get
            {
                if (_database == null)
                {
                    // Fallback to synchronous/immediate initialization if accessed before InitializeAsync
                    _database = new SQLiteAsyncConnection(_dbPath, SQLiteOpenFlags.ReadWrite | SQLiteOpenFlags.Create | SQLiteOpenFlags.SharedCache);
                }
                return _database;
            }
        }
    }
}
