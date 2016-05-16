namespace Models.Fighting.Battle.Objectives {
    public interface IObjective {
        string Description { get; }
        bool IsComplete(IBattle battle);
        bool HasFailed(IBattle battle);
    }
}