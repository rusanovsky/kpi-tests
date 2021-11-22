using System.Threading.Tasks;
using System.Collections.Generic;
using System;

namespace Spendings.Core.Records
{
    public interface IRecordRepository
    {
        Task<Record> PostAsync(Record record);
        Task<List<Record>> GetAsync(DateTime startDate, DateTime endDate, int userId);
        Task<Record> GetAsync(int recordId);
        Task<Record> UpdateAsync(Record newRecord, int id);
        Task<Record> PatchAsync(int newAmount, int id);
        Task<Record> DeleteAsync(int recordId);
        Task<List<Record>> DeleteListAsync(DateTime startDate, DateTime endDate, int userId);
    }
}
