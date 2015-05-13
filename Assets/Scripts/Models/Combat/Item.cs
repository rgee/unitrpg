using UnityEngine;

namespace Models.Combat.Inventory {
    public class Item : ScriptableObject {
        public string Name;

        public Item(string name) {
            Name = name;
        }
    }
}
