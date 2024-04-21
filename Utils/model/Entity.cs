using System.Security.Permissions;
using System.Runtime.Serialization;
using System;

namespace Utils.model
{
    [Serializable]
    public class Entity<ID>
    {
        protected ID id;

        public ID Id
        {
            get { return id; }
            set { id = value; }
        }

        public override bool Equals(object obj)
        {
            if (this == obj)
                return true;

            if (!(obj is Entity<ID>))
                return false;

            Entity<ID> entity = (Entity<ID>)obj;
            return Equals(Id, entity.Id);
        }

        public override int GetHashCode()
        {
            return Id.GetHashCode();
        }

        public override string ToString()
        {
            return $"Entity{{id={id}}}";
        }

    }
}
