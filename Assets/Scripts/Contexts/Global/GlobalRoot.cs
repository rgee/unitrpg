using Contexts.Global.Models;
using Newtonsoft.Json.Linq;
using strange.extensions.context.impl;
using UnityEngine;

namespace Contexts.Global {
    public class GlobalRoot : ContextView {
        // JSON asset representing the entire game data tree......
        public TextAsset GameConfiguration;

        void Awake() {

            var stopwatch = System.Diagnostics.Stopwatch.StartNew();
            var gameJSON = JObject.Parse(GameConfiguration.text);
            var game = Game.CreateFromJson(gameJSON);
            stopwatch.Stop();
            
            Debug.LogFormat("Deserializing Game object took {0}ms", stopwatch.ElapsedMilliseconds);
            context = new GlobalContext(this);
        } 
    }
}