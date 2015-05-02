namespace Models.Combat.Objectives {
    public interface IObjective {
        bool IsFailed(IBattle battle);
        bool IsComplete(IBattle battle);
    }
}