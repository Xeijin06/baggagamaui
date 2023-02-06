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
    public class OnHandDataStore : IDataStore<OnHand>
    {
        readonly SQLiteAsyncConnection database;
        List<OnHand> items;
        public OnHandDataStore()
        {
            string dbPath = DependencyService.Get<IFileHelper>().GetLocalFilePath("bca.db");

            if (database == null)
            {
                database = new SQLiteAsyncConnection(dbPath);
            }
        }

        private async Task Init()
        {
            await database.CreateTableAsync<OnHand>(CreateFlags.ImplicitPK | CreateFlags.AutoIncPK);   
        }

        public async Task<bool> AddItemAsync(OnHand item)
        {
            await Init();
            await database.InsertAsync(item);

            return await Task.FromResult(true);
        }

        public async Task ClearItems()
        {
            await Init();
            await database.QueryAsync<OnHand>("DELETE FROM OnHand");
            await database.QueryAsync<OnHand>("VACUUM");
        }

        public async Task ClearSomeItems(string id)
        {
            await Init();
            await database.QueryAsync<OnHand>(string.Format(@"DELETE FROM OnHand WHERE Id = '{0}'", id));
            await database.QueryAsync<OnHand>("VACUUM");
        }

        public async Task<bool> DeleteItemAsync(string id)
        {
            await Init();
            throw new NotImplementedException();
        }

        public async Task DeleteSynchronizedItemsAsync()
        {
            await Init();
            await database.QueryAsync<OnHand>(string.Format(@"DELETE FROM OnHand WHERE BpmCaseId IS NOT NULL"));
            await database.QueryAsync<OnHand>("VACUUM");
        }

        public async Task<OnHand> GetItemAsync(string id)
        {
            await Init();
            items = await database.Table<OnHand>().ToListAsync();
            return await Task.FromResult(items.FirstOrDefault(s => s.CaseId == id));
        }

        public async Task<IEnumerable<OnHand>> GetItemsAsync(string id, bool forceRefresh = false)
        {
            await Init();
            items = await database.Table<OnHand>().ToListAsync();

            return await Task.FromResult(items.Where(s => s.CaseId == id).ToList());
        }

        public async Task<IEnumerable<OnHand>> GetItemsAsync(bool forceRefresh = false)
        {
            await Init();
            items = await database.Table<OnHand>().ToListAsync();

            return await Task.FromResult(items);
        }
        public async Task<IEnumerable<OnHand>> GetOffSyncItemsAsync()
        {
            await Init();
            items = await database.Table<OnHand>().ToListAsync();
            return await Task.FromResult(items.Where(x => string.IsNullOrEmpty(x.BpmCaseId)).ToList());
        }

        public async Task<bool> UpdateItemAsync(OnHand item)
        {
            await Init();
            int status = await database.UpdateAsync(item);
            return await Task.FromResult(status > 0);
        }
    }
}
