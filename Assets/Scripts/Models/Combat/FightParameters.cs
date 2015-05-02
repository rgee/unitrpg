public struct FightParameters {
    public readonly int CritChance;
    public readonly int Damage;
    public readonly int GlanceChance;
    public readonly int HitChance;
    public readonly int Hits;

    public FightParameters(int damage, int hits, int hitChance, int critChance, int glanceChance) {
        Damage = damage;
        Hits = hits;
        HitChance = hitChance;
        CritChance = critChance;
        GlanceChance = glanceChance;
    }
}