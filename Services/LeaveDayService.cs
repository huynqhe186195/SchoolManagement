using Repositories;
using Repositories.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Services
{
    public class LeaveDayService
    {
        public List<LeaveRequest> GetAllLeaveDay()
            => new LeaveRequestRepository().GetAll();

        public void Update(LeaveRequest leaveRequest,int statusid) => new LeaveRequestRepository().Update(leaveRequest,statusid);

    }
}
