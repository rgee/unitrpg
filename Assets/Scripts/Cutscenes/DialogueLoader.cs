using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;
using UnityEngine;

public class DialogueLoader {
    private static readonly string DIALOGUE_RESOURCE_ROOT = "Dialogue";

    public static TextAsset Load(string path) {
        var fullPath = Path.Combine(DIALOGUE_RESOURCE_ROOT, path);
        return Resources.Load(fullPath) as TextAsset;
    }
}
