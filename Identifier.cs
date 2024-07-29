using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Game
{
    public abstract class Identifier
    {
        public Guid Id { get; protected set; } = Guid.NewGuid();
    }
}
