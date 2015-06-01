using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;

namespace SaveGames {
    [Serializable]
    public class Character : ISerializable {
        public string Name;
        public int Level;

        public Character() {
            
        }
        protected Character(SerializationInfo info, StreamingContext context) {
            Name = info.GetString("name");
            Level = info.GetInt32("level");
        }

        protected virtual void GetObjectData(SerializationInfo info, StreamingContext context) {
            info.AddValue("name", Name);
            info.AddValue("level", Level);
        }

        void ISerializable.GetObjectData(SerializationInfo info, StreamingContext context) {
            GetObjectData(info, context);
        }
    }
}
