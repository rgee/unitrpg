using UnityEngine;

public class PhaseTextFlyByCommand {
    public Vector2 center;
    public float moveTime;
    public Vector2 offscreen;
    public float pause;

    public PhaseTextFlyByCommand(Vector2 center, Vector2 offscreen, float moveTime, float pause) {
        this.center = center;
        this.offscreen = offscreen;
        this.moveTime = moveTime;
        this.pause = pause;
    }
}