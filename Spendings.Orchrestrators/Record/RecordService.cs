using System.Threading.Tasks;
using Spendings.Core.Records;
using System;
using System.Collections.Generic;

namespace Spendings.Orchrestrators.Records
{
    public class RecordService : IRecordService
    {
        private readonly IRecordRepository _repo;
        public RecordService(IRecordRepository repo)
        {
            _repo = repo;
        }
        public async Task<List<Core.Records.Record>> GetAsync(DateTime from, DateTime to, int userId)
        {
            return await _repo.GetAsync(from,to,userId);
        }
        public async Task<Core.Records.Record> GetAsync(int recordId)
        {
            return await _repo.GetAsync(recordId);
        }
        public async Task<Core.Records.Record> UpdateAsync(Core.Records.Record newRecord, int recordId)
        {
            return await _repo.UpdateAsync(newRecord, recordId);
        }
        public async Task<Core.Records.Record> PatchAsync(int newAmount, int recordId)
        {
            return await _repo.PatchAsync(newAmount, recordId);
        }
        public async Task<Core.Records.Record> PostAsync(Core.Records.Record record)
        {
            return await _repo.PostAsync(record);
        }
        public async Task<Core.Records.Record> DeleteAsync(int recordId)
        {
            return await _repo.DeleteAsync(recordId);
        }
        public async Task<List<Core.Records.Record>> DeleteListAsync(DateTime startDate, DateTime endDate, int userId)
        {
            return await _repo.DeleteListAsync(startDate, endDate, userId);
        }
    }
}
