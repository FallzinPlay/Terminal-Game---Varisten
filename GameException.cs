using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Game
{
    internal class GameException : ApplicationException
    {
        public GameException(string messageName) : base(messageName) { }
    }
}
