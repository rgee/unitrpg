using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;

namespace SaveGames {
    interface ISaver {
        void Save(State state, string path);
    }
}
