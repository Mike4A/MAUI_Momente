
using SQLite;
using Momente.Resources.Localizations;

namespace Momente
{
    public class DatabaseService
    {
        private DatabaseService()
        {
            var dbPath = Path.Combine(FileSystem.AppDataDirectory, "moments.db");
            _database = new SQLiteAsyncConnection(dbPath);
            _database.CreateTableAsync<Moment>().Wait();
            ResetIdCounter();
        }

        private static DatabaseService? _instance;
        public static DatabaseService Instance
        {
            get
            {
                object x = new object();
                lock (x)
                {
                    if (_instance == null)
                    {
                        _instance = new DatabaseService();
                    }
                    return _instance;
                }
            }
        }

        private readonly SQLiteAsyncConnection _database;

        private int _idCounter;

        public async void ResetIdCounter()
        {
            _idCounter = 0;
            Moment? lastMoment = await GetLastMomentAsync();
            if (lastMoment != null)
            {
                _idCounter = lastMoment.Id;
            }
        }

        public async Task<int> AddMomentAsync(Moment moment)
        {
            return await _database.InsertAsync(moment);
        }

        public async Task<List<Moment>> GetMomentsReversedAsync()
        {
            return await _database.Table<Moment>().OrderByDescending(m => m.Id).ToListAsync();
        }
        
        public async Task<int> UpdateMomentAsync(Moment moment)
        {
            return await _database.UpdateAsync(moment);
        }

        public async Task<int> DeleteMomentAsync(Moment moment)
        {
            return await _database.DeleteAsync(moment);
        }

        internal async Task DeleteMomentAsync(int id)
        {
            Moment? moment = await GetMomentByIdAsync(id);
            if (moment != null)
            {
                await DeleteMomentAsync(moment);
            }
        }

        public async Task<Moment?> GetPreviousMomentAsync()
        {
            Moment? moment;
            do
            {
                _idCounter--;
                moment = await GetMomentByIdAsync(_idCounter);
            } while (_idCounter > 1 && moment == null);
            return moment;
        }
        public async Task<Moment?> GetMomentByIdAsync(int id)
        {
            Moment? momentById = await _database.Table<Moment>().Where(m => m.Id == id).FirstOrDefaultAsync();
            return momentById;
        }
        public async Task<Moment?> GetLastMomentAsync()
        {
            Moment? lastOrDefault = null;
            lastOrDefault = await _database.Table<Moment>().OrderByDescending(m => m.Id).FirstOrDefaultAsync();
            if (lastOrDefault != null)
            {
                int lastId = lastOrDefault.Id;
                return await GetMomentByIdAsync(lastId);
            }
            return lastOrDefault;
        }

        internal async Task AddWelcomeMomentIfEmptyAsync()
        {
            if (await GetLastMomentAsync() == null)
            {
                await AddMomentAsync(new Moment
                {
                    Icon = "👋",
                    Headline = AppResources.WelcomeMomentHeadline,
                    Description = AppResources.WelcomeMomentDescription,
                    Color = Colors.DarkCyan
                });
            }
        }

        internal async Task<int> GetCount()
        {
            return await _database.Table<Moment>().CountAsync();
        }
    }
}
