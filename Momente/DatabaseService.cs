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
            Moment? moment = await GetMomentByIdAsync(id);
            if (moment != null)
            {
                await DeleteMomentAsync(moment);
            }
        }
    }
}
