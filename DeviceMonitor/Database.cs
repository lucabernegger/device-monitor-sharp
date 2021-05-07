using System;
using System.Collections.Generic;
using System.Linq;
using DeviceMonitor.Models;
using Newtonsoft.Json;


namespace DeviceMonitor
{
    public static class Database
    {
        public static void Add(WebResponse entry)
        {
            using var db = new ApplicationDbContext();
            if (entry.IsEncrypted)
            {
                db.Saveds.Add(new()
                {
                    Data = (string) entry.Data,
                    Time = DateTime.Now,
                    IsEncrypted = entry.IsEncrypted
                });
            }
            else
            {
                db.Saveds.Add(new()
                {
                    Data = JsonConvert.SerializeObject((SystemInfo)entry.Data),
                    Time = DateTime.Now,
                    IsEncrypted = entry.IsEncrypted
                });
            }

            db.SaveChanges();

        }

        public static List<WebResponse> DataDataFromPastUntilNow(DateTime date,int limit = -1)
        {
            using var db = new ApplicationDbContext();
            if (limit != -1 && limit != 0)
            {
                return db.Saveds.Where(o => o.Time >= date).OrderByDescending(o => o.Time).Take(limit).Select(o=> new WebResponse()
                {
                    Data = o.Data,
                    IsEncrypted = o.IsEncrypted,
                    Time = o.Time
                }).ToList();
            }

            return db.Saveds.Where(o => o.Time >= date).OrderByDescending(o => o.Time).Select(o => new WebResponse()
            {
                Data = (o.IsEncrypted)? o.Data : JsonConvert.DeserializeObject<SystemInfo>(o.Data),
                IsEncrypted = o.IsEncrypted,
                Time = o.Time
            }).ToList();
        }
    }
}
