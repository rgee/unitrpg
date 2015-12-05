using System;
using System.Runtime.Serialization;

namespace Contexts.Global.Models {
    [Serializable]
    public class SavedBattleState : ISerializable {
        public void GetObjectData(SerializationInfo info, StreamingContext context) {
        }
    }
}