using System;
using System.Collections.Generic;
using System.Text;
using System.Linq;

namespace Hexx.Server.Db.Entity
{
    public abstract class BaseEntity<T> : BaseEntity where T : BaseEntity<T>
    {
        public T LoadAllReferences(DatabaseContext context)
        {
            foreach (string propertyName in GetType().GetProperties().Where(p=> p.PropertyType.IsSubclassOf(typeof(BaseEntity))).Select(p=>p.Name))
                if(!context.Entry(this).Reference(propertyName).IsLoaded)
                    context.Entry(this).Reference(propertyName).Load();
            return (T)this;
        }
    }

    public abstract class BaseEntity
    {

    }
}
