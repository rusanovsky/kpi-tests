using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using System;
using Spendings.Core.Records;
using System.Collections.Generic;
using AutoMapper;

namespace onion_spendings.Records
{
    [ApiController]
    [Route("User/")]
    public class RecordController : Controller
    {
        private readonly IRecordService _service;
        private readonly IMapper _mapper;
        public RecordController(IMapper mapper, IRecordService service)
        {
            _service = service;
            _mapper = mapper;
        }

        [HttpPost("{userId}/Record")]
        public async Task<Record> PostAsync([FromBody] Spendings.Orchrestrators.Records.Record record, int userId)
        {
            var coreRecord = _mapper.Map<Record>(record);
            coreRecord.UserId = userId;

            var addResult = await _service.PostAsync(coreRecord);

            return addResult;
        }

        [HttpGet("{userId}/Record")]
        public async Task<List<Record>> GetAsync(int userId, [FromQuery] Spendings.Orchrestrators.Records.DateInterval interval)
        {
            var fromDate = Convert.ToDateTime(interval.from);
            var tillDate = Convert.ToDateTime(interval.till);

            return await _service.GetAsync(fromDate, tillDate, userId);
        }

        [HttpGet("Record")]
        public async Task<Record> GetAsync(int recordId)
        {
            return await _service.GetAsync(recordId);
        }

        [HttpPut("Record")]
        public async Task<Record> UpdateAsync([FromBody] Spendings.Orchrestrators.Records.Record newRecord, int recordId)
        {
            var coreRecord = _mapper.Map<Record>(newRecord);

            return await _service.UpdateAsync(coreRecord, recordId);
        }
       
        [HttpPatch("Record")]
        public async Task<Record> PatchAsync(int newAmount, int recordId)
        {
            return await _service.PatchAsync(newAmount, recordId);
        }
        
        [HttpDelete("Record")]
        public async Task<IActionResult> DeleteAsync(int recordId)
        {
            var addResult = await _service.DeleteAsync(recordId);
            var ret = _mapper.Map<Spendings.Orchrestrators.Records.Record>(addResult);
            return Ok(ret);
        }

        [HttpDelete("{userId}/Record")]
        public async Task<List<Record>> DeleteListAsync(int userId, [FromQuery] Spendings.Orchrestrators.Records.DateInterval interval)
        {
            var fromDate = Convert.ToDateTime(interval.from);
            var tillDate = Convert.ToDateTime(interval.till);

            return await _service.DeleteListAsync(fromDate, tillDate, userId);
        }
    }
}
