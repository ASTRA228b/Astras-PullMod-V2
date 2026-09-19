using GorillaLocomotion;

namespace Astras_PullMod_V2.Core.MainSystems;

public static class SpeedSystem
{
    public static bool Enabled;
    public static bool OnlyWithPull;

    public static float Speed = 8.5f;
    public static float Multiplier = 1.5f;

    public static void Update()
    {
        if (!Enabled)
            return;

        if (OnlyWithPull && !PullSystem.Enabled)
            return;

        GTPlayer.Instance.maxJumpSpeed = Speed;
        GTPlayer.Instance.jumpMultiplier = Multiplier;
    }

    public static void Normal()
    {
        Speed = 6.5f;
        Multiplier = 1.1f;
    }

    public static void Boost()
    {
        Speed = 8.5f;
        Multiplier = 1.5f;
    }

    public static void Strong()
    {
        Speed = 10f;
        Multiplier = 1.8f;
    }

    public static void Reset()
    {
        Enabled = false;
        OnlyWithPull = false;

        Speed = 8.5f;
        Multiplier = 1.5f;
    }
}