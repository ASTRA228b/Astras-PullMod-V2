using BepInEx;
using UnityEngine;
using Astras_PullMod_V2.Core.GUIHelpers;
using Astras_PullMod_V2.Core.MainSystems;

namespace Astras_PullMod_V2.Core.PresetSystem;

public static class PresetManager
{
    public static string PresetFolder => Path.Combine(Paths.ConfigPath, "Astra's PullMod V2", "Presets");

    public static void Init()
    {
        if (!Directory.Exists(PresetFolder))
            Directory.CreateDirectory(PresetFolder);
    }

    public static void SavePreset(string name)
    {
        if (string.IsNullOrWhiteSpace(name))
            return;

        Init();

        PullPresetData preset = new()
        {
            Name = name,

            PullEnabled = PullSystem.Enabled,
            PullPower = PullSystem.PullPower,
            UpHillPower = PullSystem.UpHillPower,
            Momentum = PullSystem.Momentum,
            MaxPull = PullSystem.MaxPull,
            ClampVelocity = PullSystem.ClampVelocity,
            MaxVelocity = PullSystem.MaxVelocity,

            PullMode = (int)PullSystem.Mode,
            HandMode = (int)PullSystem.Hand,
            ActivationMode = (int)PullSystem.Activation,

            SpeedEnabled = SpeedSystem.Enabled,
            SpeedOnlyWithPull = SpeedSystem.OnlyWithPull,
            Speed = SpeedSystem.Speed,
            Multiplier = SpeedSystem.Multiplier,

            InputIndex = InputSelector.SelectedIndex
        };

        string json = JsonUtility.ToJson(preset, true);
        string path = Path.Combine(PresetFolder, SafeName(name) + ".json");

        File.WriteAllText(path, json);
    }

    public static void LoadPreset(string name)
    {
        Init();

        string path = Path.Combine(PresetFolder, SafeName(name) + ".json");

        if (!File.Exists(path))
            return;

        string json = File.ReadAllText(path);
        PullPresetData preset = JsonUtility.FromJson<PullPresetData>(json);

        if (preset == null)
            return;

        PullSystem.Enabled = preset.PullEnabled;
        PullSystem.PullPower = preset.PullPower;
        PullSystem.UpHillPower = preset.UpHillPower;
        PullSystem.Momentum = preset.Momentum;
        PullSystem.MaxPull = preset.MaxPull;
        PullSystem.ClampVelocity = preset.ClampVelocity;
        PullSystem.MaxVelocity = preset.MaxVelocity;

        PullSystem.Mode = (PullSystem.PullMode)preset.PullMode;
        PullSystem.Hand = (PullSystem.HandMode)preset.HandMode;
        PullSystem.Activation = (PullSystem.ActivationMode)preset.ActivationMode;

        SpeedSystem.Enabled = preset.SpeedEnabled;
        SpeedSystem.OnlyWithPull = preset.SpeedOnlyWithPull;
        SpeedSystem.Speed = preset.Speed;
        SpeedSystem.Multiplier = preset.Multiplier;

        if (preset.InputIndex >= 0 && preset.InputIndex < InputSelector.InputNames.Length)
            InputSelector.SelectedIndex = preset.InputIndex;
    }

    public static void DeletePreset(string name)
    {
        string path = Path.Combine(PresetFolder, SafeName(name) + ".json");

        if (File.Exists(path))
            File.Delete(path);
    }

    public static List<string> GetPresets()
    {
        Init();

        List<string> presets = new();

        foreach (string file in Directory.GetFiles(PresetFolder, "*.json"))
            presets.Add(Path.GetFileNameWithoutExtension(file));

        return presets;
    }

    private static string SafeName(string name)
    {
        foreach (char c in Path.GetInvalidFileNameChars())
            name = name.Replace(c.ToString(), "");

        return name.Trim();
    }

}