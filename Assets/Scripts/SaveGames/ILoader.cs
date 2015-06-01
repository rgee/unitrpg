using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace SaveGames {
    interface ILoader {
        State Load(string path);
    }
}
