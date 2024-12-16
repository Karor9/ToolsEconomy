using Godot;
using Godot.Collections;

public partial class UIControler : Control
{
    [Export] Button Open;
    [Export] Button Close;
    [Export] Panel UIPanel;
    [Export] Control Nodes;
    [Export] Array<Button> UIButtons = new Array<Button>();

    public override void _Ready()
    {
        Open.Visible = false;
        Close.Visible = true;
        UIPanel.Visible = true;

        for (int i = 0; i < UIButtons.Count; i++)
        {
            int x = i + 1;
            UIButtons[i].Pressed += () => ChangeToolState(x);
        }
    }

    void ChangeState()
    {
        GD.Print("state");
        Open.Visible = !Open.Visible;
        Close.Visible = !Close.Visible;
        UIPanel.Visible = !UIPanel.Visible;
    }

    void ChangeToolState(int id)
    {
        if(Nodes.GetChildCount() > 0 && Globals.Instance.CurrentState == Globals.ToolState.AddingNode)
        {
            Node n = Nodes.GetChild(Nodes.GetChildCount() - 1);
            n.QueueFree();
        }
        Globals.Instance.CurrentState = (Globals.ToolState)id;
        for (int i = 0; i < UIButtons.Count; i++)
        {
            if(i + 1 == id)
                UIButtons[i].Disabled = true;
            else if(UIButtons[i].Disabled)
                UIButtons[i].Disabled = false;
        }
    }
}
