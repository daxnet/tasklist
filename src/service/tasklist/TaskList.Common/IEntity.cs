using System;
using System.Collections.Generic;
using System.Text;

namespace TaskList.Common
{
    public interface IEntity
    {
        Guid Id { get; }
    }
}
