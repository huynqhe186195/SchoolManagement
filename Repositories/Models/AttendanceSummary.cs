using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Repositories.Models
{
    public class AttendanceSummary
    {
        
            public int EmployeeId { get; set; }
            public string FirstName { get; set; }
            public string LastName { get; set; }
            public int PresentDays { get; set; }
            public int AbsentDays { get; set; }
            public int LateDays { get; set; }
            public int OvertimeDays { get; set; }
            public int Year { get; set; }
            public int Month { get; set; }
            
    }
    }

