using UnityEngine;
using static Astras_PullMod_V2.Core.GUIHelpers.GlobalStyles;

namespace Astras_PullMod_V2.Core.GUIHelpers;

public static class MenuLib
{
    private static Dictionary<string, bool> dropdownStates = new Dictionary<string, bool>();

    public static int Dropdown(string id, string[] options, int selectedIndex, params GUILayoutOption[] layout)
    {
        if (!dropdownStates.ContainsKey(id))
            dropdownStates[id] = false;


        if (GUILayout.Button(options[selectedIndex], mainButton, layout))
        {
            dropdownStates[id] = !dropdownStates[id];
        }


        if (dropdownStates[id])
        {
            GUILayout.BeginVertical(boxStyle);

            for (int i = 0; i < options.Length; i++)
            {
                if (GUILayout.Button(options[i], optionButton))
                {
                    selectedIndex = i;
                    dropdownStates[id] = false;
                }
            }

            GUILayout.EndVertical();
        }

        return selectedIndex;
    }
}
