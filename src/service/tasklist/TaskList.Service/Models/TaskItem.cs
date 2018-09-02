using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using TaskList.Common;

namespace TaskList.Service.Models
{
    public class TaskItem : IEntity
    {
        public TaskItem()
        {
            this.DateOpened = DateTime.UtcNow;
        }

        public Guid Id { get; set; }

        public string Name { get; set; }

        public bool HasDone { get; set; }

        public DateTime DateOpened { get; set; }

        public DateTime DateDone { get; set; }

        public void MarkDone()
        {
            HasDone = true;
            DateDone = DateTime.UtcNow;
        }

        public void Reopen()
        {
            HasDone = false;
            DateOpened = DateTime.UtcNow;
        }

        public override bool Equals(object obj)
        {
            if (ReferenceEquals(this, obj))
            {
                return true;
            }

            if (obj == null)
            {
                return false;
            }

            if (obj is TaskItem taskItem &&
                Guid.Equals(Id, taskItem.Id))
            {
                return true;
            }

            return false;
        }

        public override string ToString()
        {
            return Name;
        }

        public override int GetHashCode()
        {
            var hash = Id.GetHashCode();
            if (!string.IsNullOrEmpty(Name))
            {
                hash ^= Name.GetHashCode();
            }

            return hash ^ HasDone.GetHashCode() ^ DateOpened.GetHashCode() ^ DateDone.GetHashCode();
        }
    }
}
