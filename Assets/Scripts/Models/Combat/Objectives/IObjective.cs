namespace Models.Combat.Objectives {
    public interface IObjective {
        bool IsFailed(IOldBattle battle);
        bool IsComplete(IOldBattle battle);
    }
}