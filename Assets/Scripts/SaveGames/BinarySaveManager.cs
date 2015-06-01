using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Runtime.Serialization;
using System.Runtime.Serialization.Formatters.Binary;

namespace SaveGames {
    public class BinarySaveManager : ISaver, ILoader {
        public static State CurrentState;

        public List<State> GetAll(string path) {
            var files = from fileName in Directory.GetFiles(path)
                        where fileName.EndsWith(".sav")
                        select fileName;

            var states = (from fileName in files
                          select Load(fileName))
                .OrderBy((state) => state.SavedOn);
                         

            return new List<State>(states);
        }

        public State Load(string path) {
            var state = new State();
            var stream = File.Open(path, FileMode.Open);
            var binaryFormatter = new BinaryFormatter {Binder = new VersionDeserializationBinder()};
            state = (State)binaryFormatter.Deserialize(stream);
            stream.Close();

            return state;

        }

        public void Save(State state, string path) {
            var stream = File.Open(path, FileMode.Create);
            var binaryFormatter = new BinaryFormatter {Binder = new VersionDeserializationBinder()};
            state.SavedOn = new DateTime();
            binaryFormatter.Serialize(stream, state);
            stream.Close();
        }
    }

    class VersionDeserializationBinder : SerializationBinder {
        public override Type BindToType(string assemblyName, string typeName) {
            if (string.IsNullOrEmpty(assemblyName) || string.IsNullOrEmpty(typeName)) {
                return null;
            }

            assemblyName = Assembly.GetExecutingAssembly().FullName;
            return Type.GetType(string.Format("{0}, {1}", typeName, assemblyName));
        }
    }
}
