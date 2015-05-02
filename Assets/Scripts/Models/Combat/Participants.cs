public struct Participants {
    public readonly Models.Combat.Unit Attacker;
    public readonly Models.Combat.Unit Defender;

    public Participants(Models.Combat.Unit attacker, Models.Combat.Unit defender) {
        Attacker = attacker;
        Defender = defender;
    }

    public Participants Invert() {
        return new Participants(Defender, Attacker);
    }
}