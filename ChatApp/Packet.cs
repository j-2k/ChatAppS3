using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ChatApp //color is 58
{
    [Serializable] //now can be serialized
    struct Packet //avoid referencing & instead copy to avoid problems with color changing mid sending a colored msg
    {
        public string nickname;
        public string message;
        public ConsoleColor textColor;
    }
}
