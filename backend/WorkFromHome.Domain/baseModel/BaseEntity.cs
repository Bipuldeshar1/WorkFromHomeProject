using System;
using System.Collections.Generic;
using System.Text;

namespace WorkFromHome.Domain.baseModel
{
    public abstract class BaseEntity
    {
        public virtual long Id { get; protected set; }
        public DateTime AddedOn { get; protected set; } = DateTime.Now;
        public DateTime? UpdatedOn { get; protected set; }
        public int AddedBy { get; protected set; }
        public int UpdatedBy { get; protected set; }

    }
}
