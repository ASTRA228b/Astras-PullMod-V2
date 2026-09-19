namespace Astras_PullMod_V2.Core.PresetSystem;

[Serializable]
public class PullPresetData
{
    public string Name = "";

    public bool PullEnabled;
    public float PullPower;
    public float UpHillPower;
    public float Momentum;
    public float MaxPull;
    public bool ClampVelocity;
    public float MaxVelocity;

    public int PullMode;
    public int HandMode;
    public int ActivationMode;

    public bool SpeedEnabled;
    public bool SpeedOnlyWithPull;
    public float Speed;
    public float Multiplier;

    public int InputIndex;
}