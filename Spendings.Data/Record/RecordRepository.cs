using AutoMapper;
using Spendings.Data.DB;
using Spendings.Core.Records;
using System.Threading.Tasks;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Spendings.Data.Records
{
    public class RecordRepository : IRecordRepository
    {
        private readonly AppDbContext _context;
        private readonly IMapper _mapper;
        public RecordRepository(IMapper mapper, AppDbContext context)
        {
            _context = context;
            _mapper = mapper;
        }
        public async Task<Core.Records.Record> PostAsync(Core.Records.Record record)
        {    
            var dbRecord = _mapper.Map<Record>(record);

            var addResult = await _context.Records.AddAsync(dbRecord);
            await _context.SaveChangesAsync();

            return _mapper.Map<Core.Records.Record>(addResult.Entity);
        }
        public async Task<Core.Records.Record> UpdateAsync(Core.Records.Record newRecord,int recordId)
        {
            Record elem = (
                from n in _context.Records
                where n.Id == recordId
                select n).First();
            elem.Amount = newRecord.Amount;
            elem.CategoryId = newRecord.CategoryId;
            _context.Update(elem);
            await _context.SaveChangesAsync();

            return _mapper.Map<Core.Records.Record>(elem);
        }
        public async Task<Core.Records.Record> PatchAsync(int newAmount,int recordId)
        {
            Record elem=(
                from n in _context.Records
                where n.Id == recordId
                select n).First();
            elem.Amount = checked(elem.Amount + newAmount);

            _context.Update(elem);
            await _context.SaveChangesAsync();

            return _mapper.Map<Core.Records.Record>(elem);
        }
        public async Task<List<Core.Records.Record>> GetAsync(DateTime startDate, DateTime endDate, int userId)
        {
            List<Record> elems = (
                from n in _context.Records
                where n.Date >= startDate && n.Date <= endDate && n.UserId==userId
                select n).ToList();

            return toCoreRecord(elems);
        }
        public async Task<Spendings.Core.Records.Record> GetAsync(int recordId)
        {
            Record rec = _context.Records.Where(r => r.Id == recordId).Single();
            return _mapper.Map<Core.Records.Record>(rec);
        }
        public async Task<Core.Records.Record> DeleteAsync(int recordId)
        {
            var dbRecord = _context.Records.Where(r => r.Id==recordId).Single();

            _context.Records.Remove(dbRecord);
            await _context.SaveChangesAsync();

            return _mapper.Map<Core.Records.Record>(dbRecord);
        }
        public async Task<List<Core.Records.Record>> DeleteListAsync(DateTime startDate, DateTime endDate, int userId)
        {
            List<Record> elems = (
               from n in _context.Records
               where n.Date >= startDate && n.Date <= endDate && n.UserId == userId
               select n).ToList();

            _context.RemoveRange(elems);
            await _context.SaveChangesAsync();

            return toCoreRecord(elems);
        }
        private List<Core.Records.Record> toCoreRecord(List<Record> core)
        {
            List<Core.Records.Record> result = new List<Core.Records.Record>();

            foreach (Record elem in core)
            {
                result.Add(_mapper.Map<Core.Records.Record>(elem));
            }

            return result;
        }
    }
}
