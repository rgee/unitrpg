namespace Models.Fighting {
    public interface IStat {
        int Value { get; } 
        StatType Type { get; }
    }
}