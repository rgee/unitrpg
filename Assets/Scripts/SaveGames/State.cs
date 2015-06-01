using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;

namespace SaveGames {
    [Serializable]
    public class State : ISerializable {
        public DateTime SavedOn;
        public ulong SecondsPlayed;
        public int Chapter;
        public List<Character> Characters;

        public State() {
        }

        protected State(SerializationInfo info, StreamingContext ctx) {
            Chapter = info.GetInt32("chapter");
            SecondsPlayed = info.GetUInt64("secondsPlayed");
            Characters = (List<Character>) info.GetValue("characters", typeof (List<Character>));
            SavedOn = info.GetDateTime("savedOn");
        }

        protected virtual void GetObjectData(SerializationInfo info, StreamingContext context) {
            info.AddValue("chapter", Chapter);
            info.AddValue("secondsPlayed", SecondsPlayed);
            info.AddValue("characters", Characters);
            info.AddValue("savedOn", SavedOn);
        }

        void ISerializable.GetObjectData(SerializationInfo info, StreamingContext context) {
            GetObjectData(info, context);
        }
    }
}
