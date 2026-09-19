using Astras_PullMod_V2.Libraries; 

namespace Astras_PullMod_V2.Core.GUIHelpers;

public static class InputSelector
{
    public static string[] InputNames =
  {
        "Right Grab",
        "Left Grab",
        "Right Trigger",
        "Left Trigger",
        "RightJoyStick (Hold)",
        "LeftJoyStick (Hold)",
        "A Button",
        "B Button",
        "X Button",
        "Y Button"
    };

    public static Func<bool>[] Inputs =
    {
        () => InputLib.RightGrab,
        () => InputLib.LeftGrab,
        () => InputLib.RightTrigger,
        () => InputLib.LeftTrigger,
        () => InputLib.RightJoystickClick,
        () => InputLib.LeftJoystickClick,
        () => InputLib.RightControllerAButton,
        () => InputLib.RightControllerBButton,
        () => InputLib.LeftControllerXButton,
        () => InputLib.LeftControllerYButton
    };

    public static int SelectedIndex = 0;

    public static bool Pressed => Inputs[SelectedIndex]?.Invoke() ?? false;
}
