namespace Models.Fighting.Items {
    public interface IItemDatabase {
        IItem GetItemById(string id);
    }
}