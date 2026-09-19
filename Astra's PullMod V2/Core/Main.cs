using UnityEngine;
using UnityEngine.InputSystem;
using System.Collections.Generic;
using Astras_PullMod_V2.Core.GUIHelpers;
using Astras_PullMod_V2.Core.MainSystems;
using Astras_PullMod_V2.Core.PresetSystem;
using static Astras_PullMod_V2.Core.GUIHelpers.GlobalStyles;

namespace Astras_PullMod_V2.Core;

public class Main : MonoBehaviour
{
    // Yes cuz no GlobalSettings.cs
    private Rect Window = new(155, 155, 400, 430);
    private bool Open;

    private readonly string[] Tabs = { "Pull", "Speed", "Settings", "Presets" };
    private int CurrentTab;

    private bool Advanced;

    private List<string> Presets = new();
    private string PresetName = "Preset";
    private Vector2 PresetScroll;

    private void OnGUI()
    {
        EnsureLoad();

        if (!Open)
            return;

        Window.width = 400f;
        Window.height = 430f;

        Window = GUILayout.Window(987421, Window, UIM, "Astra's PullMod V2", WindowStyle);
    }

    private void Update()
    {
        if (Keyboard.current != null && Keyboard.current.pKey.wasPressedThisFrame)
            Open = !Open;
    }

    private void FixedUpdate()
    {
        PullSystem.Update();
        SpeedSystem.Update();
    }

    private void Start()
    {
        PresetManager.Init();
        Presets = PresetManager.GetPresets();
    }

    // ui stuff now

    private void UIM(int i)
    {
        CurrentTab = GUILayout.Toolbar(CurrentTab, Tabs, TabStyle);
        GUILayout.Space(10f);
        switch (CurrentTab)
        {
            case 0:
                Pull();
                break;

            case 1:
                Speed();
                break;

            case 2:
                Settings();
                break;

            case 3:
                PresetMenu();
                break;
        }

        GUILayout.FlexibleSpace();
        if (GUILayout.Button("Close", ButtonStyle))
            Open = !Open;

        GUI.DragWindow();
    }

    private void Pull()
    {
        PullSystem.Enabled = GUILayout.Toggle(PullSystem.Enabled, "Pull Mod");

        GUILayout.Space(5f);
        GUILayout.Label("Mode");

        int mode = (int)PullSystem.Mode;
        mode = GUILayout.Toolbar(mode, new[] { "Stable", "Dynamic", "Strong" }, TabStyle);
        PullSystem.Mode = (PullSystem.PullMode)mode;

        GUILayout.Space(10f);

        GUILayout.Label($"Pull Power: {PullSystem.PullPower:F3}");
        PullSystem.PullPower = GUILayout.HorizontalSlider(PullSystem.PullPower, 0.001f, 1f, SliderStyle, SliderThumbStyle);

        GUILayout.Label($"Uphill Power: {PullSystem.UpHillPower:F3}");
        PullSystem.UpHillPower = GUILayout.HorizontalSlider(PullSystem.UpHillPower, 0.001f, 0.5f, SliderStyle, SliderThumbStyle);

        GUILayout.Space(5f);
        GUILayout.Label("Presets");

        GUILayout.BeginHorizontal();

        if (GUILayout.Button("Speed", ButtonStyle))
            PullPresets.Speed();

        if (GUILayout.Button("Legit", ButtonStyle))
            PullPresets.Legit();

        if (GUILayout.Button("Strong", ButtonStyle))
            PullPresets.Strong();

        GUILayout.EndHorizontal();

        GUILayout.Space(5f);

        if (GUILayout.Button(Advanced ? "Advanced ▲" : "Advanced ▼", ButtonStyle))
            Advanced = !Advanced;

        if (!Advanced)
            return;

        GUILayout.Space(5f);

        GUILayout.Label($"Momentum: {PullSystem.Momentum:F4}");
        PullSystem.Momentum = GUILayout.HorizontalSlider(PullSystem.Momentum, 0f, 0.05f, SliderStyle, SliderThumbStyle);

        GUILayout.Label($"Max Pull: {PullSystem.MaxPull:F3}");
        PullSystem.MaxPull = GUILayout.HorizontalSlider(PullSystem.MaxPull, 0.01f, 1f, SliderStyle, SliderThumbStyle);

        PullSystem.ClampVelocity = GUILayout.Toggle(PullSystem.ClampVelocity, "Velocity Clamp");

        if (PullSystem.ClampVelocity)
        {
            GUILayout.Label($"Max Velocity: {PullSystem.MaxVelocity:F1}");
            PullSystem.MaxVelocity = GUILayout.HorizontalSlider(PullSystem.MaxVelocity, 5f, 150f, SliderStyle, SliderThumbStyle);
        }

        GUILayout.Space(5f);

        if (GUILayout.Button("Reset Pull", ButtonStyle))
            PullSystem.ResetPull();
    }

    private void Speed()
    {
        SpeedSystem.Enabled = GUILayout.Toggle(SpeedSystem.Enabled, "Speed Boost");
        GUILayout.Space(10f);
        GUILayout.Label($"Jump Speed: {SpeedSystem.Speed:F1}");
        SpeedSystem.Speed = GUILayout.HorizontalSlider(SpeedSystem.Speed, 1f, 15f, SliderStyle, SliderThumbStyle);
        GUILayout.Label($"Jump Multiplier: {SpeedSystem.Multiplier:F2}");
        SpeedSystem.Multiplier = GUILayout.HorizontalSlider(SpeedSystem.Multiplier, 1f, 3f, SliderStyle, SliderThumbStyle);
        GUILayout.Space(5f);
        SpeedSystem.OnlyWithPull = GUILayout.Toggle(SpeedSystem.OnlyWithPull, "Only With Pull Mod");
        GUILayout.Space(10f);
        GUILayout.BeginHorizontal();
        if (GUILayout.Button("Normal", ButtonStyle))
            SpeedSystem.Normal();

        if (GUILayout.Button("Boost", ButtonStyle))
            SpeedSystem.Boost();

        if (GUILayout.Button("Strong", ButtonStyle))
            SpeedSystem.Strong();

        GUILayout.EndHorizontal();
        GUILayout.Space(10f);
        if (GUILayout.Button("Reset Speed", ButtonStyle))
            SpeedSystem.Reset();
    }

    private void Settings()
    {
        GUILayout.Label("Input");
        InputSelector.SelectedIndex = MenuLib.Dropdown(
            "pullmod_v2_input",
            InputSelector.InputNames,
            InputSelector.SelectedIndex,
            GUILayout.Width(200)
        );
        GUILayout.Label($"Current Input: {InputSelector.InputNames[InputSelector.SelectedIndex]}");
        GUILayout.Space(10f);
        GUILayout.Label("Hand");
        int hand = (int)PullSystem.Hand;
        hand = GUILayout.Toolbar(hand, new[] { "Both", "Left", "Right" }, TabStyle);
        PullSystem.Hand = (PullSystem.HandMode)hand;
        GUILayout.Space(10f);
        GUILayout.Label("Activation");
        int activation = (int)PullSystem.Activation;
        activation = GUILayout.Toolbar(activation, new[] { "Release", "Touch", "Hold" }, TabStyle);
        PullSystem.Activation = (PullSystem.ActivationMode)activation;
        GUILayout.Space(10f);
        if (GUILayout.Button("Reset Settings", ButtonStyle))
        {
            PullSystem.Reset();
            SpeedSystem.Reset();
            InputSelector.SelectedIndex = 0;
        }
    }

    private void PresetMenu()
    {
        GUILayout.Label("Preset Name");
        PresetName = GUILayout.TextField(PresetName);
        GUILayout.Space(5f);
        GUILayout.BeginHorizontal();
        if (GUILayout.Button("Save", ButtonStyle))
        {
            PresetManager.SavePreset(PresetName);
            Presets = PresetManager.GetPresets();
        }
        if (GUILayout.Button("Refresh", ButtonStyle))
            Presets = PresetManager.GetPresets();

        GUILayout.EndHorizontal();
        GUILayout.Space(10f);
        PresetScroll = GUILayout.BeginScrollView(PresetScroll);
        foreach (string preset in Presets)
        {
            GUILayout.BeginHorizontal();
            GUILayout.Label(preset);
            if (GUILayout.Button("Load", ButtonStyle, GUILayout.Width(60)))
                PresetManager.LoadPreset(preset);

            if (GUILayout.Button("Delete", ButtonStyle, GUILayout.Width(60)))
            {
                PresetManager.DeletePreset(preset);
                Presets = PresetManager.GetPresets();

                GUILayout.EndHorizontal();
                break;
            }
            GUILayout.EndHorizontal();
        }

        GUILayout.EndScrollView();
    }
}