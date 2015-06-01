using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using SaveGames;
using UnityEngine;

public class SaveGameList : MonoBehaviour {
    public GameObject SavePrefab;

    private List<State> _saves;
    private BinarySaveManager _saveManager;
    private List<GameObject> _saveBubbles = new List<GameObject>();
    private GameObject _topRow;
    private GameObject _bottomRow;

    void Start() {
        var test = new State {
            Chapter = 2,
            SecondsPlayed = 1000
        };


        _topRow = transform.FindChild("Rows/Row 1").gameObject;
        _bottomRow = transform.FindChild("Rows/Row 2").gameObject;

        _saveManager = new BinarySaveManager();
        _saveManager.Save(test, "test.sav");
        _saves = _saveManager.GetAll(Directory.GetCurrentDirectory()).Take(6).ToList();

        foreach (var state in _saves) {
            var instance = Instantiate(SavePrefab);
            var bubble = instance.GetComponent<SaveGameBubble>();
            bubble.State = state;
            _saveBubbles.Add(instance);
        }

        for (var i = 0; i < Math.Min(3, _saveBubbles.Count); i++) {
           _saveBubbles[i].transform.SetParent(_topRow.transform);
        }

        for (var i = 3; i < Math.Min(6, _saveBubbles.Count); i++) {
           _saveBubbles[i].transform.SetParent(_bottomRow.transform);
        }
    }
}
