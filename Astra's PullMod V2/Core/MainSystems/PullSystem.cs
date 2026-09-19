using Astras_PullMod_V2.Core.GUIHelpers;
using GorillaLocomotion;
using UnityEngine;

namespace Astras_PullMod_V2.Core.MainSystems;

public static class PullSystem
{
    public enum PullMode { Stable, Dynamic, Strong }
    public enum HandMode { Both, Left, Right }
    public enum ActivationMode { Release, Touch, Hold }

    public static bool Enabled;
    public static float PullPower = 0.025f;
    public static float UpHillPower = 0.020f;
    public static float Momentum = 0.001f;
    public static float MaxPull = 0.070f;
    public static bool ClampVelocity = true;
    public static float MaxVelocity = 15f;

    public static PullMode Mode = PullMode.Dynamic;
    public static HandMode Hand = HandMode.Both;
    public static ActivationMode Activation = ActivationMode.Release;

    private static bool LastLeftTouch, LastRightTouch;
    private static float ReleaseTime, TouchTime;

    private const float ReleaseWindow = 0.10f;
    private const float TouchLinger = 0.06f;

    public static void Update()
    {
        bool leftTouch = GTPlayer.Instance.IsHandTouching(true);
        bool rightTouch = GTPlayer.Instance.IsHandTouching(false);
        bool leftRelease = !leftTouch && LastLeftTouch;
        bool rightRelease = !rightTouch && LastRightTouch;

        if (CheckHand(leftRelease, rightRelease))
            ReleaseTime = ReleaseWindow;

        if (CheckHand(leftTouch, rightTouch))
            TouchTime = TouchLinger;

        ReleaseTime = Mathf.Max(0f, ReleaseTime - Time.fixedDeltaTime);
        TouchTime = Mathf.Max(0f, TouchTime - Time.fixedDeltaTime);

        bool activate = Activation switch
        {
            ActivationMode.Release => ReleaseTime > 0f,
            ActivationMode.Touch => CheckHand(leftTouch, rightTouch) || TouchTime > 0f,
            ActivationMode.Hold => true,
            _ => false
        };

        LastLeftTouch = leftTouch;
        LastRightTouch = rightTouch;

        if (!Enabled || !InputSelector.Pressed || !activate)
            return;

        switch (Mode)
        {
            case PullMode.Stable: Stable(); break;
            case PullMode.Dynamic: Dynamic(); break;
            case PullMode.Strong: Strong(); break;
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
        Vector3 pull = new(vel.x * PullPower, vel.y * UpHillPower, vel.z * PullPower);
        GTPlayer.Instance.transform.position += Vector3.ClampMagnitude(pull, MaxPull);
    }

    private static void Dynamic()
    {
        Vector3 vel = Velocity();
        float speed = vel.magnitude;
        float power = Mathf.Clamp(PullPower + speed * Momentum, PullPower, PullPower * 8f);

        Vector3 pull = new(vel.x * power, vel.y * UpHillPower, vel.z * power);
        float momentumBoost = Mathf.Clamp01(speed / Mathf.Max(MaxVelocity, 0.01f));

        pull *= Mathf.Lerp(0.85f, 1.5f, momentumBoost);
        GTPlayer.Instance.transform.position += Vector3.ClampMagnitude(pull, MaxPull);
    }

    private static void Strong()
    {
        Rigidbody rb = GorillaTagger.Instance.rigidbody;
        Vector3 vel = Velocity();
        float speed = vel.magnitude;
        float power = Mathf.Clamp(PullPower + speed * Momentum * 2f, PullPower, PullPower * 12f);
        Vector3 direction = speed > 0.01f ? vel.normalized : Vector3.zero;

        Vector3 pull = new(vel.x * power, vel.y * UpHillPower * 1.15f, vel.z * power);
        pull += direction * (Momentum * speed * 0.5f);
        pull = Vector3.ClampMagnitude(pull, MaxPull * 1.5f);

        GTPlayer.Instance.transform.position += pull;

        Vector3 velocityBoost = new(vel.x * Momentum * 0.5f, Mathf.Max(0f, vel.y) * Momentum * 0.2f, vel.z * Momentum * 0.5f);
        rb.linearVelocity += velocityBoost;
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
        ReleaseTime = TouchTime = 0f;
    }
}