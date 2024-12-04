using Repositories;
using Repositories.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Services
{
   public class ActivityHistoryService
    {
        public void AddActivityHistory(ActivityHistory activityHistory)
        {
            new ActivityHistoryRepository().AddActivityHistory(activityHistory);
        }

    }
}
