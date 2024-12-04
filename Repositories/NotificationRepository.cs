using Microsoft.EntityFrameworkCore;
using Repositories.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Repositories
{
    public class NotificationRepository
    {
        public List<Notification> GetAll()
        {
            using (var context = new Prn212Context())
            {
                return context.Notifications.Include(e=> e.CreateByNavigation).Include(d=>d.Department).ToList();
            }
        }
        public void AddNotification(Notification notification)
        {
            using (var context = new Prn212Context())
            {
                context.ChangeTracker.Clear();
                context.Notifications.Add(notification);
                context.SaveChanges();
            }
        }

        public List<Notification> GetNotificationForEmployee(int? departmentId)
        {
            using(var context = new Prn212Context())
            {
                 return context.Notifications.Include(Employee => Employee.CreateByNavigation).Where(d=>d.DepartmentId == departmentId  ||  d.DepartmentId == 5).OrderByDescending(date=>date.SendAt).OrderByDescending(time=>time.SendAt).ToList();
            }
        }
        public List<Notification> GetTop3NotificationForEmployee(int? departmentId)
        {
            using (var context = new Prn212Context())
            {
                return context.Notifications.Where(d => d.DepartmentId == departmentId || d.DepartmentId == 5).OrderByDescending(date => date.SendAt).OrderByDescending(time => time.SendAt).Take(3).ToList();
            }
        }
    }
}
