// Ignore Spelling: NPC

namespace Assets.Scripts.Managers
{
    public enum RegisterState
    {
        Free,
        WaitingForPlayer,
        WaitingForPlant,
        Processing
    }
    public enum PlayerState
    {
        Moving,
        HoldPlant,
        InRestock,
        InRegister
    }
    public enum NPC_State
    {
        WalkToFlower,
        WalkToLine,
        Wait,
        InLine,
        Exit
    }
    public enum GameState
    {
        MainMenu,
        Playing,
        Panels,
        Dialogue,
        EndShift
    }
}