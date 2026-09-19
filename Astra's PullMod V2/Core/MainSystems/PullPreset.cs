namespace Astras_PullMod_V2.Core.MainSystems; // yeah yeah yeah not really apart of "MainSystems" but it just fits here.

public static class PullPresets
{
    public static void Speed()
    {
        PullSystem.PullPower = 0.025f;
        PullSystem.UpHillPower = 0.020f;
        PullSystem.Momentum = 0.0005f;
        PullSystem.MaxPull = 0.050f;
        PullSystem.Mode = PullSystem.PullMode.Stable;
    }

    public static void Legit()
    {
        PullSystem.PullPower = 0.070f;
        PullSystem.UpHillPower = 0.065f;
        PullSystem.Momentum = 0.001f;
        PullSystem.MaxPull = 0.090f;
        PullSystem.Mode = PullSystem.PullMode.Dynamic;
    }

    public static void Strong()
    {
        PullSystem.PullPower = 0.100f;
        PullSystem.UpHillPower = 0.075f;
        PullSystem.Momentum = 0.002f;
        PullSystem.MaxPull = 0.150f;
        PullSystem.Mode = PullSystem.PullMode.Strong;
    }
}
