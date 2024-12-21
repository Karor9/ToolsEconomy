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

    
        Control c = Globals.Instance.Nodes[Globals.Instance.ClickedId].GetChild(4) as Control;
        c.Visible = false;

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
                if(Globals.Instance.ClickedId <= -1)
                {
                    Globals.Instance.ClickedId = ID;
                    Control c = GetChild(4) as Control;
                    c.Visible = true;
                } else 
                {
                    AddLine();
                } 
                break;
            case Globals.ToolState.AddingGenerator:
                if(Globals.Instance.ClickedId <= -1)
                {
                    Globals.Instance.ClickedId = ID;
                }
                Generator g = Globals.Instance.FreshGenerator;
                if(Globals.Instance.FreshGenerator == null)
                    break;
                Line2D line2D = new Line2D();
                line2D.DefaultColor = Colors.Blue;
                line2D.Points = new Vector2[] {g.Position + (g.Size/2), Globals.Instance.Nodes[Globals.Instance.ClickedId].Position + (Globals.Instance.Nodes[Globals.Instance.ClickedId].Size/2)};
                Globals.Instance.EdgesGeneratorContainer.AddChild(line2D);
                Globals.Instance.FreshGenerator = null;
                Globals.Instance.ClickedId = -1;
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
