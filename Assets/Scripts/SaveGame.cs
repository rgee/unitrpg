using System;
using System.Collections.Generic;
using System.IO;
using System.Runtime.Serialization.Formatters.Binary;
using Models;
using UnityEngine;

[Serializable]
public class SaveGame {
    public static SaveGame current = new SaveGame();
    // All other save games.
    public static List<SaveGame> saveGames = new List<SaveGame>();
    // Which chapter the player is on
    public int chapter;
    // All the characters by name
    public Dictionary<string, Character> characters = new Dictionary<string, Character>();

    public static void Select(int index) {
        current = saveGames[index];
    }

    public static void Save() {
        saveGames.Add(current);
        var formatter = new BinaryFormatter();

        var path = Application.persistentDataPath + "/saveGames.gd";
        Debug.Log("Saving to " + path);

        var file = File.Create(path);
        formatter.Serialize(file, saveGames);
        file.Close();
    }

    public static void Load() {
        var path = Application.persistentDataPath + "/saveGames.gd";
        Debug.Log("Loading save files from " + path);
        if (File.Exists(path)) {
            var formatter = new BinaryFormatter();
            var file = File.Open(path, FileMode.Open);
            saveGames = (List<SaveGame>) formatter.Deserialize(file);
            file.Close();
        }
    }

    public Character getByName(string name) {
        if (current.characters.ContainsKey(name)) {
            return current.characters[name];
        }

        return (Character) Resources.Load("Characters/" + name);
    }
}