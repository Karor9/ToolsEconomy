using Godot;
using Godot.Collections;
using System;

public partial class ResourceNode : NodeProp
{
    public override void _Ready()
    {
        base._Ready();
        Button button = GetChild(5) as Button;
        button.Pressed += () => OnClick();
    }

    public override void OnClick()
    {
        switch(Globals.Instance.CurrentState)
        {
            case Enums.ToolState.MoveNode:
                Globals.Instance.ClickedId = ID;
                break;
            case Enums.ToolState.AddingLine:
                if(Globals.Instance.ClickedId <= -1)
                {
                    Globals.Instance.ClickedId = ID;
                    Control control = GetChild(4) as Control;
                    control.Visible = true;
                } else
                {
                    AddLine();
                }
                break;
        }
    }

    private void AddLine()
    {
        int id = Globals.Instance.ClickedId;
        if(ID == id)
            return;

        Parents.Add(id);
        ResourceNode resourceNode = Globals.Instance.Nodes.GetNode(new NodePath(id.ToString())) as ResourceNode;
        resourceNode.Childs.Add(ID);
        Node node = Globals.Instance.Edge.Instantiate();
        Vector2 start = resourceNode.Position + (resourceNode.Size / 2);
        Vector2 end = Position + (Size/2);

        Control control = resourceNode.GetChild(4) as Control;
        control.Visible = false;

        LineProp line = node as LineProp;
        line.Points = new Vector2[] {start, end};
        RichTextLabel text = node.GetChild(0) as RichTextLabel;
        text.Position = (start + end)/2 - (text.Size/20);
        text.RotationDegrees = 0;
        line.ID = Globals.Instance.Edges.GetChildCount();

        line.Name = line.ID.ToString();
        EndLine.Add(line.ID);
        resourceNode.StartLine.Add(line.ID);
        Globals.Instance.Edges.AddChild(line);

        Globals.Instance.ClickedId = -1;
    }
}