using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using Dashboard.Models;
using Dashboard.Platforms;
using Newtonsoft.Json;

namespace Dashboard.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class DataController : ControllerBase
    {
        private readonly ApplicationDbContext _db;
        public DataController(ApplicationDbContext db)
        {
            _db = db;
        }
        record GetMemoryResponse(double TotalMemory, IEnumerable<string> Times, IEnumerable<double>UsedMemory, IEnumerable<double>UsedSwap);
        [HttpGet("Memory")]
        public IActionResult GetMemoryData()
        {
            var date = DateTime.Now.AddMinutes(-10);
            var dblist = _db.Saveds.Where(o=>o.Time > date).ToList();
            var list = dblist.Select(o => new WebResponse()
            {
                Data = JsonConvert.DeserializeObject<Platform>(o.Data.ToString()),
                Time = o.Time
            }).ToList();
            if (list.Any())
            {
                var timeList = list.Select(o => o.Time.ToShortTimeString());
                var usedMemoryList = list.Select(o => ((Platform)o.Data).Memory.UsedMemory);
                var usedSwapList = list.Select(o => ((Platform)o.Data).Memory.UsedSwap);
                var totalMemory = Math.Round((list.FirstOrDefault()?.Data is Platform p ? p.Memory.TotalMemory : 0) / 1000) * 1000;
                return Ok(new GetMemoryResponse(totalMemory, timeList, usedMemoryList,
                    usedSwapList));
            }

            return NoContent();
        }

        record GetCpuResponse(IEnumerable<string> Times, IEnumerable<double> TotalPercentage, IEnumerable<uint> ClockSpeed,
            IEnumerable<int> ThreadCount);
        [HttpGet("Cpu")]
        public IActionResult GetCpuData()
        {
            var date = DateTime.Now.AddMinutes(-10);
            var dblist = _db.Saveds.Where(o => o.Time > date).ToList();
            var list = dblist.Select(o => new WebResponse()
            {
                Data = JsonConvert.DeserializeObject<Platform>(o.Data.ToString()),
                Time = o.Time
            }).ToList();
            if (list.Any())
            {
                var timeList = list.Select(o => o.Time.ToShortTimeString());
                var totalPercentageList = list.Select(o => ((Platform)o.Data).Cpu.TotalPercentage);
                var clockSpeedList = list.Select(o => ((Platform)o.Data).Cpu.CurrentClockSpeed);
                var threadListList = list.Select(o => ((Platform)o.Data).Cpu.TotalThreads);
                
                return Ok(new GetCpuResponse(timeList,totalPercentageList,clockSpeedList,threadListList));  
            }

            return NoContent();
        }

        
    }
}
