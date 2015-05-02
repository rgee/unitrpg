using UnityEngine;

public abstract class Objective : MonoBehaviour {
    public abstract bool IsComplete();
    public abstract bool IsFailed();
}