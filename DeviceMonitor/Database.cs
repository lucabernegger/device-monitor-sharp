using System;
using System.Collections.Generic;
using LiteDB;

namespace DeviceMonitor
{
    public static class Database
    {
        public static void Add(WebResponse entry)
        {
            using var db = new LiteDatabase("data.db");
            var col = db.GetCollection<WebResponse>("data");
            col.Insert(entry);
            col.EnsureIndex(o => o.Time);
        }

        public static List<WebResponse> DataDataFromPastUntilNow(DateTime date,int limit = -1)
        {
            using var db = new LiteDatabase("data.db");
            var col = db.GetCollection<WebResponse>("data");
            if (limit != -1 && limit != 0)
            {
                return col.Query().Where(o => o.Time >= date).OrderByDescending(o=>o.Time).Limit(limit).ToList();

            }
            return col.Query().Where(o => o.Time >= date).OrderByDescending(o => o.Time).ToList();
        }
    }
}
