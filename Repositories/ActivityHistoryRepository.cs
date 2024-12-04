using Repositories.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Repositories
{
    public class ActivityHistoryRepository
    {
        Prn212Context context=new Prn212Context();

        public void AddActivityHistory(ActivityHistory activityHistory)
        {
            using (var context = new Prn212Context())
            {
                context.ChangeTracker.Clear();
                context.ActivityHistories.Add(activityHistory);
                context.SaveChanges();
            }
        }

    }
}
