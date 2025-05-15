using System;
using System.Collections.Generic;
using System.Linq;
using WorkFromHome.Domain.models;

namespace WorkFromHome.Domain.Enummerations
{
    public class RequestStatusEnums: Enummeration
    {
     
        public static RequestStatusEnums Pending = new(0, nameof(Pending));
        public static RequestStatusEnums Approved = new(1, nameof(Approved));
        public static RequestStatusEnums Rejected = new(2, nameof(Rejected));

        private static readonly List<RequestStatusEnums> AllStatuses = new() {  Pending, Approved, Rejected };
        public RequestStatusEnums(int id, string name) : base(id, name)
        {
        }

        public static RequestStatusEnums FromId(int id)
        {
            return AllStatuses.SingleOrDefault(status => status.Id == id)
                   ?? throw new InvalidOperationException($"Invalid status id: {id}");
        }
        public static int forDb(string name)
        {
            var role= AllStatuses.FirstOrDefault(x =>
                    x.Name.Equals(name, StringComparison.CurrentCultureIgnoreCase)
                ) ?? throw new ArgumentException($"No Role with {name} is found.");
            return role.Id;

        }
        public static RequestStatusEnums FromName(string name)
        {
            return AllStatuses.FirstOrDefault(x =>
                    x.Name.Equals(name, StringComparison.CurrentCultureIgnoreCase)
                ) ?? throw new ArgumentException($"No Role with {name} is found.");
        }
    }
}
