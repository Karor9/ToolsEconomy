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

        panel.MouseEntered += () => Obstructed(true);
        Open.MouseEntered += () => Obstructed(true);
        Close.MouseEntered += () => Obstructed(true);
        Simulate.MouseEntered += () => Obstructed(true);
    
        panel.MouseExited += () => Obstructed(false);
        Open.MouseExited += () => Obstructed(false);
        Close.MouseExited += () => Obstructed(false);
        Simulate.MouseExited += () => Obstructed(false);
    
        UIButtons = UIPanel.GetChild(0).GetChild(0).GetChildren();

        for (int i = 0; i < UIButtons.Count; i++)
        {
            int x = i + 1;
            if(x >= Enum.GetNames(typeof(Enums.ToolState)).Length)
                break;
            Button button = UIButtons[i] as Button;
            button.Pressed += () => ChangeToolState(x);
            button.MouseEntered += () => Obstructed(true);
            button.MouseExited += () => Obstructed(false);
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
    
    public void Obstructed(bool obstructed)
    {
        Globals.Instance.IsObstructed = obstructed;
    }
}
