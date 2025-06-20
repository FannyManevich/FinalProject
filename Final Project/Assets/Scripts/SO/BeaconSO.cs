using UnityEngine;

[CreateAssetMenu(fileName = "Input", menuName = "SO/Beacon", order = 4)]
public class BeaconSO : ScriptableObject
{
    public InputChannel inputChannel;
    public GameStateChannel gameStateChannel;
}
