using UnityEngine;

namespace Models.Combat.Inventory {
    public class Item : ScriptableObject {
        public string Name;
        public int HealAmount;

        public Item(string name, int healAmount) {
            Name = name;
            HealAmount = healAmount;
        }
    }
}
