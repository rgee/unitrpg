using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;

public class DialogueActors : ScriptableObject {
    public List<DialogueActor> Actors = new List<DialogueActor>();

    public DialogueActor FindByName(string name) {
        foreach (var actor in Actors) {
            if (actor.Name == name) {
                return actor;
            }
        }

        throw new ArgumentException("Could not find actor by name " + name);
    }
}
