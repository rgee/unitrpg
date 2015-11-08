namespace Contexts.Common.Model.Objectives {
    public interface IObjective {
        ObjectiveType Type { get; } 
        string Description { get; }
    }
}