using System.Threading.Tasks;
using System;
using System.Collections.Generic;
namespace Spendings.Core.Records
{
    public interface IRecordService
    {
        Task<Record> PostAsync(Record record);
        Task<List<Record>> GetAsync(DateTime startDate, DateTime endDate, int userId);
        Task<Record> GetAsync(int userId);
        Task<Record> PatchAsync(int newAmount, int id);
        Task<Record> UpdateAsync(Core.Records.Record newRecord, int id);
        Task<Record> DeleteAsync(int recordId);
        Task<List<Record>> DeleteListAsync(DateTime startDate, DateTime endDate, int userId);
    }
}
