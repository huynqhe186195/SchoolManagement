using Microsoft.EntityFrameworkCore;
using Repositories.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Services
{
    public class LeaveRequestServices
    {
        Prn212Context context = new Prn212Context();
        public void AddLeaveRequest(LeaveRequest leaveRequest)
        {
            context.LeaveRequests.Add(leaveRequest);
            context.SaveChanges();
        }

        public List<LeaveRequest> getAllLeaveRequestByEmployeeId(int id)
        {
            return context.LeaveRequests.Include(x => x.LeaveType).Include(x => x.RequestStatus).Where(x => x.EmployeeId == id).ToList();
        }
    }
}
