using UnityEngine;

namespace Astras_PullMod_V2.Core.GUIHelpers;

public static class GlobalStyles
{
    public static GUIStyle? WindowStyle, ButtonStyle, TabStyle, SelectedTabStyle, SliderStyle, SliderThumbStyle, boxStyle, mainButton, optionButton;
    private static Texture2D? WindowTex, ButtonTex, SliderTex, SliderThumbTex, SelectedTex;

    private static Color WindowColor = new(0.1f, 0.1f, 0.1f, 1f);
    private static Color ButtonColor = new(0.2f, 0.2f, 0.2f, 1f);
    private static Color SliderTrackColor = new(0.15f, 0.15f, 0.15f, 1f);
    private static Color SliderThumbColor = new(0f, 0.6f, 1f, 1f);
    private static Color SelectedColor = new(0.08f, 0.15f, 0.26f, 1f);

    private static bool Loaded;

    public static void Init()
    {
        WindowTex = GlobalTex.MakeTex(1, 1, WindowColor);
        ButtonTex = GlobalTex.MakeTex(1, 1, ButtonColor);
        SliderTex = GlobalTex.MakeTex(1, 1, SliderTrackColor);
        SliderThumbTex = GlobalTex.MakeTex(1, 1, SliderThumbColor);
        SelectedTex = GlobalTex.MakeTex(1, 1, SelectedColor);

        WindowStyle = new GUIStyle(GUI.skin.window);
        ButtonStyle = new GUIStyle(GUI.skin.button);
        TabStyle = new GUIStyle(GUI.skin.button);
        SelectedTabStyle = new GUIStyle(GUI.skin.button);
        SliderStyle = new GUIStyle(GUI.skin.horizontalSlider);
        SliderThumbStyle = new GUIStyle(GUI.skin.horizontalSliderThumb);

        WindowStyle.normal.textColor = Color.white;
        WindowStyle.fontStyle = FontStyle.Normal;

        ButtonStyle.normal.textColor = Color.white;
        ButtonStyle.hover.textColor = Color.blue;
        ButtonStyle.active.textColor = Color.red;
        ButtonStyle.focused.textColor = Color.white;

        TabStyle.normal.textColor = Color.white;
        TabStyle.hover.textColor = Color.blue;
        TabStyle.active.textColor = Color.red;
        TabStyle.focused.textColor = Color.white;
        TabStyle.onNormal.textColor = Color.blue;
        TabStyle.onHover.textColor = Color.blue;
        TabStyle.onActive.textColor = Color.blue;
        TabStyle.onFocused.textColor = Color.blue;

        SelectedTabStyle.normal.textColor = Color.white;
        SelectedTabStyle.hover.textColor = Color.white;
        SelectedTabStyle.active.textColor = Color.white;

        // menulib start
        boxStyle = new GUIStyle(GUI.skin.box); // for menu lib
        mainButton = new GUIStyle(GUI.skin.button);
        optionButton = new GUIStyle(GUI.skin.button);
        mainButton.fontSize = 14;
        mainButton.fixedHeight = 25;
        mainButton.normal.textColor = Color.white;
        mainButton.hover.textColor = Color.blue;
        mainButton.active.textColor = Color.red;
        mainButton.focused.textColor = Color.white;
        mainButton.onNormal.textColor = Color.blue;
        mainButton.onHover.textColor = Color.blue;
        mainButton.onActive.textColor = Color.blue;
        mainButton.onFocused.textColor = Color.blue;

        optionButton.fontSize = 12;
        optionButton.fixedHeight = 22;
        optionButton.normal.textColor = Color.white;
        optionButton.hover.textColor = Color.blue;
        optionButton.active.textColor = Color.red;
        optionButton.focused.textColor = Color.white;
        optionButton.onNormal.textColor = Color.blue;
        optionButton.onHover.textColor = Color.blue;
        optionButton.onActive.textColor = Color.blue;
        optionButton.onFocused.textColor = Color.blue;
        ApplyBackground(mainButton, ButtonTex);
        ApplyBackground(optionButton, ButtonTex);
        // menulib end

        ApplyBackground(WindowStyle, WindowTex);
        ApplyBackground(ButtonStyle, ButtonTex);
        ApplyBackground(TabStyle, ButtonTex);
        ApplyBackground(SelectedTabStyle, SelectedTex);
        ApplyBackground(SliderStyle, SliderTex);
        ApplyBackground(SliderThumbStyle, SliderThumbTex);

        Loaded = true; 
    }

    public static void EnsureLoad()
    {
        if (!Loaded)
            Init();
    }

    public static void ApplyBackground(GUIStyle style, Texture2D tex)
    {
        style.normal.background = tex;
        style.hover.background = tex;
        style.active.background = tex;
        style.focused.background = tex;
        style.onNormal.background = tex;
        style.onHover.background = tex;
        style.onActive.background = tex;
        style.onFocused.background = tex;
    }
}