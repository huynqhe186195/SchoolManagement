using Microsoft.EntityFrameworkCore;
using Repositories.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Services
{
    public class ActivityHistoryServices
    {
        Prn212Context context = new Prn212Context();    

        public List<ActivityHistory> getAllHistory()
        {
            return context.ActivityHistories.Include(a => a.Employee).ToList(); 
        }


        public void addActivityHistory(ActivityHistory activityHistory)
        {
            context.ActivityHistories.Add(activityHistory); 
            context.SaveChanges();  
        }
    }
}
