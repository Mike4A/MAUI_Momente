using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SQLite;
using System.IO;
using System.Numerics;

namespace Momente
{
    public class DatabaseService
    {
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

        private DatabaseService()
        {
            var dbPath = Path.Combine(FileSystem.AppDataDirectory, "moments.db");
            _database = new SQLiteAsyncConnection(dbPath);
            _database.CreateTableAsync<Moment>().Wait();
        }

        private static readonly Dictionary<int, Moment> _cache = new();
        public async Task<Moment?> GetMomentCachedAsync(int id)
        {
            if (_cache.TryGetValue(id, out Moment? moment))
            { return moment; }

            moment = await _database.Table<Moment>().Where(m => m.Id == id).FirstOrDefaultAsync();

            _cache[id] = moment;

            return moment;
        }

        public async Task<int> AddMomentAsync(Moment moment)
        {
            return await _database.InsertAsync(moment);
        }

        public async Task<List<Moment>> GetMomentsAsync()
        {
            return await _database.Table<Moment>().ToListAsync();
        }

        public async Task<Moment> GetMomentByIdAsync(int id)
        {
            return await _database.Table<Moment>().Where(m => m.Id == id).FirstOrDefaultAsync();
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
            Moment? moment = await GetMomentCachedAsync(id);
            if (moment != null)
            {
                await DeleteMomentAsync(moment);
            }
        }

        internal async Task<Moment?> GetNextCachedAsync()
        {
            Moment? result = null;            

            int lastId = 0;
            if (_cache.Count > 0)
            {
                lastId = _cache.Keys.Last<int>();
            }

            int maxId = 1;
            if (await _database.Table<Moment>().CountAsync() > 0)
            {
                maxId = (await _database.Table<Moment>().OrderByDescending(p => p.Id).ElementAtAsync(0)).Id;
            }            

            for (int id = lastId + 1; result == null && id <= maxId; id++)
            {
                result = await GetMomentCachedAsync(id);
            }
                
            return result;
        }
    }
}
