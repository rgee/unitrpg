namespace Models.Fighting.Maps.Configuration {
    public interface IMapConfigRepository {
        MapConfig GetConfigByMapName(string mapName);
    }
}