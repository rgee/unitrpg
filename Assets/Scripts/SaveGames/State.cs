using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;

namespace SaveGames {
    [Serializable]
    public class State : ISerializable {
        public State() {
        }

        public State(SerializationInfo info, StreamingContext ctx) {
            
        }

        public void GetObjectData(SerializationInfo info, StreamingContext context) {

        }
    }
}
