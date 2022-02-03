using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BuildBlockCore.Models
{
    public abstract class Entity
    {
        public Guid Id { get; protected set; }

        protected Entity()
        {
            Id = Guid.NewGuid();
        }

        public override bool Equals(object obj)
        {
            var compareTo = obj as Entity;

            if (ReferenceEquals(this, compareTo)) return true;
            if (ReferenceEquals(null, compareTo)) return false;

            return Id.Equals(compareTo.Id);
        }

        public static bool operator ==(Entity a, Entity b)
        {
            if (ReferenceEquals(a, null) && ReferenceEquals(b, null))
                return true;

            if (ReferenceEquals(a, null) || ReferenceEquals(b, null))
                return false;

            return a.Equals(b);
        }

        public static bool operator !=(Entity a, Entity b)
        {
            return !(a == b);
        }

        public override int GetHashCode()
        {
            return (GetType().GetHashCode() * 907) + Id.GetHashCode();
        }

        public override string ToString()
        {
            return GetType().Name + " [Id=" + Id + "]";
        }
    }
    public enum EntityState
    {
        Insert = 1,
        Alter = 2,
        Delete = 3
    }

    public abstract class EntityDataBase : Entity
    {

        protected EntityDataBase() : base()
        {

            Active = true;
            EntityState = EntityState.Insert;
        }

        public bool Active { get; set; }
        public DateTime DateRegister { get; set; }
        public DateTime? DateUpdate { get; set; }
        public Guid UserInsertedId { get; set; }
        public Guid? UserUpdatedId { get; set; }
        public DateTime? DeleteDate { get; set; }
        public Guid? UserDeletedId { get; set; }



        [NotMapped]
        public EntityState EntityState { get; set; }

    }

}
