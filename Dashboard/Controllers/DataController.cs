using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
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
        private readonly IMemoryCache _cache;
        public DataController(IMemoryCache cache)
        {
            _cache = cache;
        }
        
        record GetMemoryResponse(double TotalMemory, List<string> Times,List<double>UsedMemory,List<double>UsedSwap);
        [HttpGet("Memory/{timestamp}/{limit?}")]
        public async Task<IActionResult> GetMemoryData(double timestamp, int limit)
        {
            List<WebResponse> list = await GetData(timestamp, limit);
            if (list != null)
            {
                var timeList = list.Select(o => o.Time.ToShortTimeString());
                var usedMemoryList = list.Select(o => ((Platform)o.Data).Memory.UsedMemory);
                var usedSwapList = list.Select(o => ((Platform)o.Data).Memory.UsedSwap);
                var totalMemory = Math.Round((list.FirstOrDefault()?.Data is Platform p ? p.Memory.TotalMemory : 0) / 1000) * 1000;
                return Ok(new GetMemoryResponse(totalMemory, timeList.ToList(), usedMemoryList.ToList(),
                    usedSwapList.ToList()));
            }

            return NoContent();
        }

        record GetCpuResponse(List<string> Times, List<double> TotalPercentage, List<uint> ClockSpeed,
            List<int> ThreadCount);
        [HttpGet("Cpu/{timestamp}/{limit?}")]
        public async Task<IActionResult> GetCpuData(double timestamp, int limit)
        {

            List<WebResponse> list = await GetData(timestamp, limit);
            if (list != null)
            {
                var timeList = list.Select(o => o.Time.ToShortTimeString()).ToList();
                var totalPercentageList = list.Select(o => ((Platform)o.Data).Cpu.TotalPercentage).ToList();
                var clockSpeedList = list.Select(o => ((Platform)o.Data).Cpu.CurrentClockSpeed).ToList();
                var threadListList = list.Select(o => ((Platform)o.Data).Cpu.TotalThreads).ToList();
                
                return Ok(new GetCpuResponse(timeList,totalPercentageList,clockSpeedList,threadListList));
            }

            return NoContent();
        }

        private async Task<List<WebResponse>> GetData(double timestamp, int limit)
        {
            using var client = new HttpClient();
            string json = await client.GetStringAsync($"http://localhost:8000?from={timestamp}&limit={limit}");
            List<WebResponse> list = JsonConvert.DeserializeObject<List<WebResponse>>(json);
            if (list != null)
            {
                foreach (var webResponse in list)
                {
                    if (webResponse.IsEncrypted)
                    {
                        webResponse.Data = JsonConvert.DeserializeObject<Platform>(
                            StringCipher.Decrypt(webResponse.Data.ToString(), Startup.EncryptionKey));
                    }
                }

                list.Reverse();
                return list;
            }

            return null;
        }
    }
}
