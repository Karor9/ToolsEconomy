using System;
using Godot;
using Godot.Collections;

public partial class NodeProp : Panel
{
    [Export] public Array<int> Parents = new Array<int>();
    [Export] public Array<int> Childs = new Array<int>();
    bool secondPressTime = false;

    public int ID;

    public override void _Ready()
    {
        Button b = GetChild(2) as Button;
        b.Pressed += () => OnClick();
    }


    void AddLine()
    {
        if(ID == Globals.Instance.ClickedId)
            return;
        Parents.Add(Globals.Instance.ClickedId);
        Globals.Instance.Nodes[Globals.Instance.ClickedId].Childs.Add(ID);
        Node node = Globals.Instance.Edge.Instantiate();
        Vector2 s = Globals.Instance.Nodes[Globals.Instance.ClickedId].Position + (Globals.Instance.Nodes[Globals.Instance.ClickedId].Size / 2);
        Vector2 e = Position + (Size/2);
        Line2D line = node as Line2D;
        line.Points = new Vector2[] {s, e};
        RichTextLabel text = node.GetChild(0) as RichTextLabel;
        text.Position = (s + e)/2;
        text.RotationDegrees = 0;
        Globals.Instance.EdgesContainer.AddChild(line);
        Globals.Instance.ClickedId = -1;
    }

    void OnClick()
    {
        GD.Print(Globals.Instance.CurrentState);
        if(Globals.Instance.CurrentState != Globals.ToolState.AddingLine)
            return;
        if(Globals.Instance.ClickedId == -1)
        {
            Globals.Instance.ClickedId = ID;
        } else if (Globals.Instance.ClickedId > -1)
        {
            AddLine();
        }
    }

    // public override void _Input(InputEvent @event)
    // {
    //     if(@event is InputEventMouseButton e && e.ButtonIndex == MouseButton.Left && e.DoubleClick)
    //     {
    //         Globals.Instance.ClickedId = -1;
    //         GD.Print(ID);
    //     }
    // }
}
