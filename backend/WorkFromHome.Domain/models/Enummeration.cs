using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace WorkFromHome.Domain.models
{
    public abstract class Enummeration 
    {
        public string Name { get; private set; }

        public int Id { get; private set; }

        protected Enummeration(int id, string name)
        {
            this.Id = id;
            this.Name = name;
        }

    }
}
