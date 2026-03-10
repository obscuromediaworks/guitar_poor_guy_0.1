namespace GuitarPoorGuy.Gameplay.Input
{
    public interface ILaneInputSource
    {
        bool WasLanePressedThisFrame(int laneIndex);
    }
}
