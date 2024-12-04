using Repositories.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Services
{
    public class RoleServices
    {
        Prn212Context context = new Prn212Context();
        public List<Role> GetRoles()
        {
            return context.Roles.ToList();
        }
    }
}
