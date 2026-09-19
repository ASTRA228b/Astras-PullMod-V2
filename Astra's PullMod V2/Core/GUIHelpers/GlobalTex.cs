using UnityEngine;

namespace Astras_PullMod_V2.Core.GUIHelpers;

public static class GlobalTex
{
    public static Texture2D MakeTex(int W, int H, Color C)
    {
        Texture2D Yes = new(W, H);
        Yes.SetPixel(0, 0, C);
        Yes.Apply();
        return Yes;
    } // GlobalTex.MakeTex(1, 1, COLOR);
}