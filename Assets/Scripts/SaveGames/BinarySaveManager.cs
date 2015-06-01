using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.Serialization;
using System.Runtime.Serialization.Formatters.Binary;
using System.Text;
using SaveGames;

namespace SaveGames {
    public class BinarySaveManager : ISaver, ILoader {
        public State Load(string path) {
            var state = new State();
            var stream = File.Open(path, FileMode.Open);
            var binaryFormatter  = new BinaryFormatter();
            binaryFormatter.Binder = new VersionDeserializationBinder();

            var result = (State)binaryFormatter.Deserialize(stream);
            stream.Close();

            return result;

        }

        public void Save(State state, string path) {
            throw new NotImplementedException();
        }
    }

    class VersionDeserializationBinder : SerializationBinder {
        public override Type BindToType(string assemblyName, string typeName) {
            throw new NotImplementedException();
        }
    }
}
