using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DeviceMonitor.Helpers
{
    public static class CsvHelper
    {
        public static string ConvertCsvToJson(string[] lines)
        {
            var csv = new List<List<string>>();

            foreach (string line in lines)
            {
                csv.Add(line.Split(',').ToList());
            }

            var properties = lines[0].Split(',');
            var listObjResult = new List<Dictionary<string, string>>();

            for (int i = 1; i < lines.Length; i++)
            {
                var objResult = new Dictionary<string, string>();

                for (int j = 0; j < properties.Length; j++)
                {
                    if (csv[i].Count < properties.Length)
                    {
                        for (int k = 0; k < properties.Length - csv[i].Count; k++)
                        {
                            csv[i].Add(String.Empty);
                        }
                    }
                    objResult.Add(properties[j], csv[i][j]);

                }


                listObjResult.Add(objResult);
            }

            return JsonConvert.SerializeObject(listObjResult);
        }
    }
}
