public struct Hit {
    public readonly bool Crit;
    public readonly int Damage;
    public readonly bool Glanced;
    public readonly bool Missed;

    public Hit(int damage, bool glanced, bool crit, bool missed) {
        Damage = damage;
        Glanced = glanced;
        Crit = crit;
        Missed = missed;
    }
}