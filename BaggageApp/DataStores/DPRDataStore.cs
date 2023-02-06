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
    public class DPRDataStore : IDataStore<DamageReport>
    {
        #region Private members variables
        private SQLiteAsyncConnection database;
        List<DamageReport> items;

        #endregion

        #region Constructors, destructor and finalizer
        public DPRDataStore()
        {
            string dbPath = DependencyService.Get<IFileHelper>().GetLocalFilePath("bca.db3");

            if (database == null)
            {
                database = new SQLiteAsyncConnection(dbPath);
            }
        }

        private async Task Init()
        {
            await database.CreateTableAsync<DamageReport>(CreateFlags.ImplicitPK | CreateFlags.AutoIncPK);
        }
        #endregion
        public async Task<bool> AddItemAsync(DamageReport item)
        {
            await Init();
            await database.InsertAsync(item);

            return await Task.FromResult(true);
        }

        public async Task ClearItems()
        {
            await Init();
            await database.QueryAsync<DamageReport>("DELETE FROM DamageReport");
            await database.QueryAsync<DamageReport>("VACUUM");
        }

        public async Task ClearSomeItems(string id)
        {
            await Init();
            await database.QueryAsync<DamageReport>(string.Format(@"DELETE FROM DamageReport WHERE Id = '{0}'", id));
            await database.QueryAsync<DamageReport>("VACUUM");
        }

        public async Task<bool> DeleteItemAsync(string id)
        {
            await Init();
            throw new NotImplementedException();
        }

        public async Task DeleteSynchronizedItemsAsync()
        {
            await Init();
            await database.QueryAsync<DamageReport>(string.Format(@"DELETE FROM DamageReport WHERE BpmCaseId IS NOT NULL"));
            await database.QueryAsync<DamageReport>("VACUUM");
        }

        public async Task<DamageReport> GetItemAsync(string id)
        {
            await Init();
            items = await database.Table<DamageReport>().ToListAsync();
            return await Task.FromResult(items.FirstOrDefault(s => s.CaseId == id));
        }

        public async Task<IEnumerable<DamageReport>> GetItemsAsync(string id, bool forceRefresh = false)
        {
            await Init();
            items = await database.Table<DamageReport>().ToListAsync();

            return await Task.FromResult(items.Where(s => s.CaseId == id).ToList());
        }

        public async Task<IEnumerable<DamageReport>> GetItemsAsync(bool forceRefresh = false)
        {
            await Init();
            items = await database.Table<DamageReport>().ToListAsync();

            return await Task.FromResult(items);
        }

        public async Task<IEnumerable<DamageReport>> GetOffSyncItemsAsync()
        {
            await Init();
            items = await database.Table<DamageReport>().ToListAsync();
            return await Task.FromResult(items.Where(x => string.IsNullOrEmpty(x.BpmCaseId)).ToList());
        }

        public async Task<bool> UpdateItemAsync(DamageReport item)
        {
            await Init();
            int status = await database.UpdateAsync(item);
            return await Task.FromResult(status > 0);
        }
    }
}
