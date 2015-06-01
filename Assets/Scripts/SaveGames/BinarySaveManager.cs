using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Runtime.Serialization;
using System.Runtime.Serialization.Formatters.Binary;
using System.Text;
using SaveGames;

namespace SaveGames {
    public class BinarySaveManager : ISaver, ILoader {
        public List<State> GetAll(string path) {
            var files = from fileName in Directory.GetFiles(path)
                        where fileName.EndsWith(".sav")
                        select fileName;

            var states = from fileName in files
                         select Load(fileName);

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
            return Type.GetType(String.Format("{0}, {1}", typeName, assemblyName));
        }
    }
}
