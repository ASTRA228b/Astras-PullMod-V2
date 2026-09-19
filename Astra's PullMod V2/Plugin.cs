using BepInEx;
using UnityEngine;
using Astras_PullMod_V2.Core;
using Astras_PullMod_V2.Stuff;

namespace Astras_PullMod_V2.Plugin;

[BepInPlugin(Constantss.GUID, Constantss.Name, Constantss.Version)]
public class Plugin : BaseUnityPlugin
{
    void Awake()
    {
        GameObject Plugin = new GameObject(Constantss.ObjectName);
        Plugin.AddComponent<Main>();
        DontDestroyOnLoad(Plugin);
    }
}