using System;

namespace Models.Combat {
    [Flags]
    public enum CombatActionType {
        Move = 1 << 0,
        Fight = 1 << 1,
        Item = 1 << 2,
        Trade = 1 << 3,
        Talk = 1 << 4,
        Wait = 1 << 5,
        Brace = 1 << 6,
        Cover = 1 << 7,
        Attack = 1 << 8,
        Use = 1 << 9
    }
}