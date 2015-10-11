using Models;
using UnityEngine;

public class DialoguePortraitView : MonoBehaviour {
    public DialogueActors ActorDatabase;
    public string ActorName;

    private GameObject _currentActorPortrait;

    public void SetActor(string name, EmotionType emotion) {
        var actor = ActorDatabase.FindByName(name);
        var portrait = actor.FindPortraitByEmotion(emotion);
        AttachGameObject(Instantiate(portrait.Prefab));
        ActorName = name;
    }

    private void AttachGameObject(GameObject instantiatedGameObject) {
        if (_currentActorPortrait != null) {
            Destroy(_currentActorPortrait);
        }

        instantiatedGameObject.transform.SetParent(transform);
        instantiatedGameObject.transform.localScale = new Vector3(0.0005f, 0.0005f, 1f);
        instantiatedGameObject.transform.localPosition = new Vector3(0.341f, -0.522f, -0.9f);
        _currentActorPortrait = instantiatedGameObject;
    }
}