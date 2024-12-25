using Godot;
using Godot.Collections;
using System;

public partial class UIController : Control
{
    [Export] Button Open;
    [Export] Button Close;
    [Export] Button Simulate;

    [Export] Control UIPanel;

    Array<Node> UIButtons = new Array<Node>();

    public override void _Ready()
    {
        SetupUIButtons();
    }

    private void SetupUIButtons()
    {
        Open.Visible = false;
        Close.Visible = true;
        UIPanel.Visible = true;

        Control panel = UIPanel.GetChild(0) as Control;

        panel.MouseEntered += () => Utils.Obstructed(true);
        Open.MouseEntered += () => Utils.Obstructed(true);
        Close.MouseEntered += () => Utils.Obstructed(true);
        Simulate.MouseEntered += () => Utils.Obstructed(true);
    
        panel.MouseExited += () => Utils.Obstructed(false);
        Open.MouseExited += () => Utils.Obstructed(false);
        Close.MouseExited += () => Utils.Obstructed(false);
        Simulate.MouseExited += () => Utils.Obstructed(false);
    
        UIButtons = UIPanel.GetChild(0).GetChild(0).GetChildren();

        for (int i = 0; i < UIButtons.Count; i++)
        {
            int x = i + 1;
            if(x >= Enum.GetNames(typeof(Enums.ToolState)).Length)
                break;
            Button button = UIButtons[i] as Button;
            button.Pressed += () => ChangeToolState(x);
            button.MouseEntered += () => Utils.Obstructed(true);
            button.MouseExited += () => Utils.Obstructed(false);
        }
    }

    private void ChangeToolState(int x)
    {
        Globals.Instance.CurrentState = (Enums.ToolState)x;

        for (int i = 0; i < UIButtons.Count; i++)
        {
            Button button = UIButtons[i] as Button;
            if(i + 1 == x)
                button.Disabled = true;
            else if(button.Disabled)
                button.Disabled = false;
        }
    }

    void ChangeState()
    {
        Open.Visible = !Open.Visible;
        Close.Visible = !Close.Visible;
        UIPanel.Visible = !UIPanel.Visible;
    }
}
