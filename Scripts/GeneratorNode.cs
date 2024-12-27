using Godot;
using Godot.Collections;
using System;

public partial class GeneratorNode : NodeProp
{
    public override void _Ready()
    {
        Count = 10d;
        base._Ready();
        Button button = GetChild(GetChildCount() - 1) as Button;
        button.Pressed += () => OnClick();
    }

    public override void OnClick()
    {
        switch(Globals.Instance.CurrentState)
        {
            case Enums.ToolState.MoveNode:
                Globals.Instance.ClickedId = ID;
                break;
            case Enums.ToolState.AddingGenerator:
                if(StartLine.Count > 0)
                    break;
                int id = Globals.Instance.ClickedId;
                if(id > -1)
                {
                    Node n = Globals.Instance.Edge.Instantiate();

                    Vector2 start = Position + (Size/2);
                    NodeProp nodeProp = Globals.Instance.GetNode(id) as NodeProp;
                    Control control = nodeProp.GetChild(4) as Control;
                    control.Visible = false;
                    Vector2 end = nodeProp.Position + (nodeProp.Size/2);

                    LineProp line = n as LineProp;
                    line.Editable = false;
                    line.Gradient = null;
                    line.DefaultColor = Colors.Bisque;
                    line.Val = 10d;
                    int LineId = Globals.Instance.Edges.GetChildCount();
                    line.Name = LineId.ToString();
                    line.Points = new Vector2[] {start, end};
                    Globals.Instance.Edges.AddChild(line);
                    
                    StartLine.Add(LineId);
                    nodeProp.EndLine.Add(LineId);
                    Globals.Instance.ClickedId = -1;
                    break;
                }
                break;
        }
    }
}