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
    public class ForwardDataStore : IDataStore<ForwardItem>
    {
        private SQLiteAsyncConnection database;
        List<ForwardItem> items;
        public ForwardDataStore()
        {
            try
            {
                string dbPath = DependencyService.Get<IFileHelper>().GetLocalFilePath("bca.db");

                if (database == null)
                {
                    database = new SQLiteAsyncConnection(dbPath);
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex);
            }

        }

        private async Task Init()
        {
            try
            {
                await database.CreateTableAsync<ForwardItem>(CreateFlags.ImplicitPK | CreateFlags.AutoIncPK);
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex);
            }

        }
        public async Task<bool> AddItemAsync(ForwardItem item)
        {
            await Init();
            await database.InsertAsync(item);

            return await Task.FromResult(true);
        }

        public async Task ClearItems()
        {
            await Init();
            await database.QueryAsync<ForwardItem>("DELETE FROM ForwardItem");
            await database.QueryAsync<ForwardItem>("VACUUM");
        }

        public async Task ClearSomeItems(string id)
        {
            await Init();
            await database.QueryAsync<ForwardItem>(string.Format(@"DELETE FROM ForwardItem WHERE Id = '{0}'", id));
            await database.QueryAsync<ForwardItem>("VACUUM");
        }

        public async Task<bool> DeleteItemAsync(string id)
        {
            await Init();
            throw new NotImplementedException();
        }

        public async Task DeleteSynchronizedItemsAsync()
        {
            await Init();
            await database.QueryAsync<ForwardItem>("DELETE FROM ForwardItem WHERE BpmCaseId IS NOT NULL");
            await database.QueryAsync<ForwardItem>("VACUUM");
        }

        public async Task<ForwardItem> GetItemAsync(string id)
        {
            await Init();
            items = await database.Table<ForwardItem>().ToListAsync();
            return await Task.FromResult(items.FirstOrDefault(s => s.CaseId == id));
        }

        public async Task<IEnumerable<ForwardItem>> GetItemsAsync(string id, bool forceRefresh = false)
        {
            await Init();
            items = await database.Table<ForwardItem>().ToListAsync();

            return await Task.FromResult(items.Where(s => s.CaseId == id).ToList());
        }

        public async Task<IEnumerable<ForwardItem>> GetItemsAsync(bool forceRefresh = false)
        {
            await Init();
            items = await database.Table<ForwardItem>().ToListAsync();

            return await Task.FromResult(items);
        }
        public async Task<IEnumerable<ForwardItem>> GetOffSyncItemsAsync()
        {
            await Init();
            items = await database.Table<ForwardItem>().ToListAsync();
            return await Task.FromResult(items.Where(x => string.IsNullOrEmpty(x.BpmCaseId)).ToList());
        }

        public async Task<bool> UpdateItemAsync(ForwardItem item)
        {
            await Init();
            int status = await database.UpdateAsync(item);
            return await Task.FromResult(status > 0);
        }
    }
}
