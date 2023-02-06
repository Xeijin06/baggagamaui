using BaggageApp.Helpers;
using BaggageApp.Models;
using SQLite;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BaggageApp.DataStores
{
    public class PIRDataStore : IDataStore<PIR>
    {
        private SQLiteAsyncConnection database;
        List<PIR> items;
        public PIRDataStore()
        {
            try
            {
                string dbPath = DependencyService.Get<IFileHelper>().GetLocalFilePath("bca.db");
                if (database == null)
                {
                    database = new SQLiteAsyncConnection(dbPath, SQLite.SQLiteOpenFlags.ReadWrite | SQLite.SQLiteOpenFlags.Create | SQLite.SQLiteOpenFlags.SharedCache);
                }
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
            }
        }

        private async Task Init()
        {
            try
            {
                await database.CreateTableAsync<PIR>(CreateFlags.ImplicitPK | CreateFlags.AutoIncPK);
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
            }
        }

        public async Task<bool> AddItemAsync(PIR item)
        {
            await Init();
            await database.InsertAsync(item);

            return await Task.FromResult(true);
        }

        public async Task ClearItems()
        {
            await Init();
            await database.QueryAsync<PIR>("DELETE FROM PIR");
            await database.QueryAsync<PIR>("VACUUM");
        }

        public async Task ClearSomeItems(string id)
        {
            await Init();
            await database.QueryAsync<PIR>(string.Format(@"DELETE FROM PIR WHERE Id = '{0}'", id));
            await database.QueryAsync<PIR>("VACUUM");
        }

        public async Task<bool> DeleteItemAsync(string id)
        {
            await Init();
            throw new NotImplementedException();
        }

        public async Task DeleteSynchronizedItemsAsync()
        {
            await Init();
            await database.QueryAsync<PIR>("DELETE FROM PIR WHERE BpmCaseId IS NOT NULL ");
            await database.QueryAsync<PIR>("VACUUM");
        }

        public async Task<PIR> GetItemAsync(string id)
        {
            await Init();
            items = await database.Table<PIR>().ToListAsync();
            return await Task.FromResult(items.FirstOrDefault(s => s.PassengerId == id));
        }

        public async Task<IEnumerable<PIR>> GetItemsAsync(string id, bool forceRefresh = false)
        {
            await Init();
            items = await database.Table<PIR>().ToListAsync();

            return await Task.FromResult(items.Where(s => s.PassengerId == id).ToList());
        }

        public async Task<IEnumerable<PIR>> GetItemsAsync(bool forceRefresh = false)
        {
            await Init();
            items = await database.Table<PIR>().ToListAsync();

            return await Task.FromResult(items);
        }
        public async Task<IEnumerable<PIR>> GetOffSyncItemsAsync()
        {
            await Init();
            items = await database.Table<PIR>().ToListAsync();
            return await Task.FromResult(items.Where(x => string.IsNullOrEmpty(x.BpmCaseId)).ToList());
        }

        public async Task<bool> UpdateItemAsync(PIR item)
        {
            await Init();
            int status = await database.UpdateAsync(item);
            return await Task.FromResult(status > 0);
        }
    }
}
