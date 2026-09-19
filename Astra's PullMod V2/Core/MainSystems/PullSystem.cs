using Astras_PullMod_V2.Core.GUIHelpers;
using GorillaLocomotion;
using UnityEngine;

namespace Astras_PullMod_V2.Core.MainSystems;

public static class PullSystem
{
    public enum PullMode
    {
        Stable,
        Dynamic,
        Strong
    }

    public enum HandMode
    {
        Both,
        Left,
        Right
    }

    public enum ActivationMode
    {
        Release,
        Touch,
        Hold
    }

    public static bool Enabled = false;

    public static float PullPower = 0.025f;
    public static float UpHillPower = 0.020f;

    public static float Momentum = 0.001f;
    public static float MaxPull = 0.070f;

    public static bool ClampVelocity = true;
    public static float MaxVelocity = 15f;

    public static PullMode Mode = PullMode.Dynamic;
    public static HandMode Hand = HandMode.Both;
    public static ActivationMode Activation = ActivationMode.Release;

    private static bool LastLeftTouch;
    private static bool LastRightTouch;

    public static void Update()
    {
        bool leftTouch = GTPlayer.Instance.IsHandTouching(true);
        bool rightTouch = GTPlayer.Instance.IsHandTouching(false);

        bool leftRelease = !leftTouch && LastLeftTouch;
        bool rightRelease = !rightTouch && LastRightTouch;

        bool Activate = Activation switch
        {
            ActivationMode.Release => CheckHand(leftRelease, rightRelease),
            ActivationMode.Touch => CheckHand(leftTouch, rightTouch),
            ActivationMode.Hold => true,
            _ => false
        };

        LastLeftTouch = leftTouch;
        LastRightTouch = rightTouch;

        if (!Enabled || !InputSelector.Pressed || !Activate)
            return;

        switch (Mode)
        {
            case PullMode.Stable:
                Stable();
                break;

            case PullMode.Dynamic:
                Dynamic();
                break;

            case PullMode.Strong:
                Strong();
                break;
        }
    }

    private static bool CheckHand(bool left, bool right)
    {
        return Hand switch
        {
            HandMode.Left => left,
            HandMode.Right => right,
            HandMode.Both => left || right,
            _ => false
        };
    }

    private static Vector3 Velocity()
    {
        Vector3 vel = GorillaTagger.Instance.rigidbody.linearVelocity;

        if (ClampVelocity)
            vel = Vector3.ClampMagnitude(vel, MaxVelocity);

        return vel;
    }

    private static void Stable()
    {
        Vector3 vel = Velocity();
        Vector3 Pull = new Vector3(vel.x * PullPower, vel.y * UpHillPower, vel.z * PullPower);
        GTPlayer.Instance.transform.position += Vector3.ClampMagnitude(Pull, MaxPull);
    }

    private static void Dynamic()
    {
        Vector3 vel = Velocity();
        float power = PullPower + vel.magnitude * Momentum;
        power = Mathf.Clamp(power, PullPower, MaxPull);
        Vector3 Pull = new Vector3(vel.x * power, vel.y * UpHillPower, vel.z * power);
        GTPlayer.Instance.transform.position += Vector3.ClampMagnitude(Pull, MaxPull);
    }
    private static void Strong()
    {
        Vector3 vel = Velocity();
        float power = PullPower + vel.magnitude * Momentum * 1.5f;
        power = Mathf.Clamp(power, PullPower, MaxPull);
        Vector3 Pull = new Vector3(vel.x * power, vel.y * UpHillPower, vel.z * power);
        GTPlayer.Instance.transform.position += Vector3.ClampMagnitude(Pull, MaxPull * 1.35f);
    }

    public static void ResetPull()
    {
        PullPower = 0.025f;
        UpHillPower = 0.020f;
        Momentum = 0.001f;
        MaxPull = 0.070f;
        ClampVelocity = true;
        MaxVelocity = 15f;
        Mode = PullMode.Dynamic;
    }

    public static void Reset()
    {
        Enabled = false;
        ResetPull();
        Hand = HandMode.Both;
        Activation = ActivationMode.Release;
    }
}
