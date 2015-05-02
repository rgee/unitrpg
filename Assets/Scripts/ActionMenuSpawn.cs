using UnityEngine;

public class ActionMenuSpawn : MonoBehaviour {
    public GameObject actionMenuType;
    public int maxWedges = 8;

    public void spawnMenu(Vector2 position, MenuItem[] items) {
        var totalWedges = 0;
        foreach (var item in items) {
            totalWedges += item.wedges;
        }

        if (totalWedges > maxWedges) {
            // blow up?
        }
    }

    public struct MenuItem {
        public string label;
        public string name;
        public int wedges;
    }
}