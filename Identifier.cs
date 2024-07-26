using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Game
{
    public class Identifier
    {
        public Guid Id { get; protected set; } = Guid.NewGuid();
    }
}
