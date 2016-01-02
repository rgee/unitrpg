using System.Collections;

internal interface AIStrategy {
    IEnumerator act();
    bool Awake { get; set; }
}