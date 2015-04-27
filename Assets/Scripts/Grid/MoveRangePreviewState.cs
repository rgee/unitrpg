using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine.UI;

public class MoveRangePreviewState {
    public enum PreviewType {
        OFF,
        RANGE_WITH_OBSTACLES,
        RANGE_WITHOUT_OBSTACLES
    }

    public static readonly List<PreviewType> TogglableTypes = new List<PreviewType>
    {
        PreviewType.OFF,
        PreviewType.RANGE_WITH_OBSTACLES,
        PreviewType.RANGE_WITHOUT_OBSTACLES
    };

    public PreviewType Type;
    public string SelectionName;

    public PreviewType GetNextType() {
        int currentIdx = TogglableTypes.IndexOf(Type);
        return TogglableTypes[(currentIdx + 1) % TogglableTypes.Count];
    }
}
