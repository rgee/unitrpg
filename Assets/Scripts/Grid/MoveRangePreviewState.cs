using System.Collections.Generic;

public class MoveRangePreviewState {
    public enum PreviewType {
        OFF,
        RANGE_WITH_OBSTACLES,
        RANGE_WITHOUT_OBSTACLES
    }

    public static readonly List<PreviewType> TogglableTypes = new List<PreviewType> {
        PreviewType.OFF,
        PreviewType.RANGE_WITH_OBSTACLES,
        PreviewType.RANGE_WITHOUT_OBSTACLES
    };

    public string SelectionName;
    public PreviewType Type;

    public PreviewType GetNextType() {
        var currentIdx = TogglableTypes.IndexOf(Type);
        return TogglableTypes[(currentIdx + 1)%TogglableTypes.Count];
    }
}