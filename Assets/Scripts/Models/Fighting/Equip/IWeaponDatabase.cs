namespace Models.Fighting.Equip {
    public interface IWeaponDatabase {
        Weapon GetByName(string name);
    }
}