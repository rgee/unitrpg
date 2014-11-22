using UnityEngine;
using System.Collections;

public class ActionMenuSpawn : MonoBehaviour {

	public GameObject actionMenuType;

	public int maxWedges = 8;

	public struct MenuItem {
		public string label;
		public string name;
		public int wedges;
	}

	public void spawnMenu(Vector2 position, MenuItem[] items) {
		int totalWedges = 0;
		foreach (MenuItem item in items) {
			totalWedges += item.wedges;
		}

		if (totalWedges > maxWedges) {
			// blow up?
		}


	}
}
