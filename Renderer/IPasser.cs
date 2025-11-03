using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ConsoleRenderer {
    public abstract class Passer {

        public string name = string.Empty;

        public virtual void Init () { }
        public virtual void Pass () { }

    }
}
