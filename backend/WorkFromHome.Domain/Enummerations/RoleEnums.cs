using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WorkFromHome.Domain.models;

namespace WorkFromHome.Domain.Enummerations
{
    public class RoleEnums : Enummeration
    {

        public static RoleEnums Manager = new(0, "Manager");

        public static RoleEnums Employee = new(1, "Employee");

        private static readonly List<RoleEnums> AllStatuses = new() { Manager, Employee };
        public RoleEnums(int id, string name) : base(id, name)
        {
        }

        public static RoleEnums FromId(int id)
        {
            return AllStatuses.SingleOrDefault(status => status.Id == id)
                   ?? throw new InvalidOperationException($"Invalid status id: {id}");
        }
        public static RoleEnums FromName(string name)
        {
            return AllStatuses.FirstOrDefault(x =>
                    x.Name.Equals(name, StringComparison.CurrentCultureIgnoreCase)
                ) ?? throw new ArgumentException($"No Role with {name} is found.");
        }
    }

}
