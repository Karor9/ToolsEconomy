using System;
using Godot;
using Godot.Collections;

public partial class NodeProp : GraphElement
{
    [Export] public Array<int> Parents = new Array<int>();
    [Export] public Array<int> Childs = new Array<int>();
    bool secondPressTime = false;

    public override void _Ready()
    {
        base._Ready();
        Button b = GetChild(GetChildCount() - 1) as Button;
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

        LineProp line = node as LineProp;
        line.Points = new Vector2[] {s, e};
        RichTextLabel text = node.GetChild(0) as RichTextLabel;
        text.Position = (s + e)/2 - (text.Size/20);
        text.RotationDegrees = 0;
        line.ID = Globals.Instance.EdgesContainer.GetChildCount();
        
        LinesStop.Add(line.ID);
        Globals.Instance.Nodes[Globals.Instance.ClickedId].LinesStart.Add(line.ID); 
        Globals.Instance.Lines.Add(line);
        Globals.Instance.EdgesContainer.AddChild(line);

        
        Globals.Instance.ClickedId = -1;
    }

    void OnClick()
    {
        switch(Globals.Instance.CurrentState)
        {
            case Globals.ToolState.AddingLine:
                if(Globals.Instance.ClickedId == -1)
                {
                    Globals.Instance.ClickedId = ID;
                } else if (Globals.Instance.ClickedId > -1)
                {
                    AddLine();
                }
                break;
            case Globals.ToolState.EditingNode:
                LineEdit lineEdit = GetChild(3) as LineEdit;
                lineEdit.Visible = true;

                RichTextLabel name = GetChild(1) as RichTextLabel;
                name.Visible = false;

                lineEdit.CaretColumn = lineEdit.Text.Length;
                lineEdit.GrabFocus();
                Globals.Instance.EditedNode = lineEdit;
                break;
            case Globals.ToolState.MoveNode:
                Globals.Instance.ClickedId = ID;
                break;

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
