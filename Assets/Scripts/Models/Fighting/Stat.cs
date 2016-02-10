namespace Models.Fighting {
    public class Stat {
        public virtual int Value { get; private set; } 
        public StatType Type { get; protected set; }
        
        public Stat(int value, StatType type) {
            Value = value;
            Type = type;
        }
        
        public Stat() { }
    }
}