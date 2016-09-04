using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;

namespace Assets.Scripts.Contexts.Global.Models {
    public class TurnEventConfiguration {
        public readonly int Turn;
        public readonly string EventName;

        public TurnEventConfiguration(int turn, string eventName) {
            Turn = turn;
            EventName = eventName;
        }
    }
}
