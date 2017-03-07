using Announcer.Db;
using System;
using System.Linq;

namespace Announcer.Models
{
    public class DataUpdate
    {
        public uint Id { get; set; }

        public DateTime RunDate { get; set; }

        public static DataUpdate GetLastUpdate()
        {
            using (var context = new Context())
            {
                return context.DataUpdates.OrderByDescending(x => x.Id).FirstOrDefault();
            }
        }

        public static void AddEntry()
        {
            using (var context = new Context())
            {
                context.DataUpdates.Add(new DataUpdate
                {
                    RunDate = DateTime.Now
                });

                context.SaveChanges();
            }
        }
    }
}
