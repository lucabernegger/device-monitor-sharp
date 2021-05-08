using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;
using Dashboard.Platforms;
using Microsoft.Extensions.Caching.Memory;
using Newtonsoft.Json;

namespace Dashboard.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class DataController : ControllerBase
    {
        record GetMemoryResponse(double TotalMemory, IEnumerable<string> Times, IEnumerable<double>UsedMemory, IEnumerable<double>UsedSwap);
        [HttpGet("Memory")]
        public IActionResult GetMemoryData()
        {
            var list = Startup.Data;
            if (list is not null)
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

            var list = Startup.Data;
            if (list is not null)
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
