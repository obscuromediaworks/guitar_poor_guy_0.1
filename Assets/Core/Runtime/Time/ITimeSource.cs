namespace GuitarPoorGuy.Core.Time
{
    public interface ITimeSource
    {
        double SongTimeMs { get; }
        void Reset(double startTimeMs = 0);
        void Tick(double deltaTimeSeconds);
    }
}
