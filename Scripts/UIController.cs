using Godot;
using System;
using static Enums;

public partial class UIController : Control
{
    [ExportGroup("UIElements")]
    [Export] Panel UIPanel;

    [ExportGroup("OpenCloseButtons")]
    [Export] Button openButton;
    [Export] Button closeButton;

    void OpenClose(bool open)
    {
        openButton.Visible = !open;
        closeButton.Visible = open;
        UIPanel.Visible = open;
    }

    void SetToolState(int state)
    {
        Globals.Instace.CurrentToolState = (ToolState)state;
        Utils.Print("#008000", Globals.Instace.CurrentToolState.ToString());
        GD.PrintRich("[color=#]"+ Globals.Instace.CurrentToolState);
    }

    void IsObstructed(bool state)
    {
        Globals.Instace.Obstructed = state;
        Utils.Print("pink", "Mouse obstructed:" + Globals.Instace.Obstructed);
    }
}
