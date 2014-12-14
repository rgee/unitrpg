using UnityEngine;
using System.IO;
using System.Collections;
using System.Collections.Generic;
using System.Runtime.Serialization.Formatters.Binary;

[System.Serializable]
public class SaveGame {
	public static SaveGame current = new SaveGame();

	public int chapter;

	public Dictionary<string, Models.Character> characters = new Dictionary<string, Models.Character>();

	public static List<SaveGame> saveGames = new List<SaveGame>();

	public static void Select (int index) {
		SaveGame.current = saveGames[index];
	}

	public static void Save() {
		saveGames.Add(SaveGame.current);
		BinaryFormatter formatter = new BinaryFormatter();

		string path = Application.persistentDataPath + "/saveGames.gd";
		Debug.Log ("Saving to " + path);

		FileStream file = File.Create(path);
		formatter.Serialize(file, SaveGame.saveGames);
		file.Close();
	}

	public static void Load() {
		string path = Application.persistentDataPath + "/saveGames.gd";
		Debug.Log ("Loading save files from " + path);
		if (File.Exists (path)) {
			BinaryFormatter formatter = new BinaryFormatter();
			FileStream file = File.Open(path, FileMode.Open);
			SaveGame.saveGames = (List<SaveGame>)formatter.Deserialize(file);
			file.Close();
		}
	}

	public Models.Character getByName(string name) {
		if (SaveGame.current.characters.ContainsKey(name)) {
			return SaveGame.current.characters[name];
		}

		return (Models.Character)Resources.Load("Characters/" + name);
	}
}	
