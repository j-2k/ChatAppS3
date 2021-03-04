using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ChatApp
{
    //now can be serialized
    [Serializable]              //avoid referencing & instead copy to avoid 
    struct Packet               //problems with color changing when sending a colored msg
    {
        public string nickname;
        public string message;
        public ConsoleColor textColor;
    }
}
