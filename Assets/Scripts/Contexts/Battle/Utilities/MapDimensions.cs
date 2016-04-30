namespace Contexts.Battle.Utilities {
    // Just a little value object to get around the single parameter, 
    // command-bound signal limitation.
    public class MapDimensions {
        public readonly int Width;
        public readonly int Height;

        public MapDimensions(int width, int height) {
            Width = width;
            Height = height;
        }
    }
}