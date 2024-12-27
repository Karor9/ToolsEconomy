using Godot;
using Godot.Collections;
using System;

public partial class ResourceNode : NodeProp
{
    public override void _Ready()
    {
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
            case Enums.ToolState.AddingGenerator:
                if(Globals.Instance.ClickedId <= -1 && Globals.Instance.LastGenerator == null)
                {
                    Globals.Instance.ClickedId = ID;
                    Control control = GetChild(4) as Control;
                    control.Visible = true;
                } else if(Globals.Instance.LastGenerator != null)
                {
                    GeneratorNode generator = Globals.Instance.LastGenerator;
                    Globals.Instance.LastGenerator = null;
                    Control c = generator.GetChild(4) as Control;
                    c.Visible = false;
                    Vector2 start = generator.Position + (generator.Size / 2);
                    Vector2 end = Position + (Size/2);

                    Node node = Globals.Instance.Edge.Instantiate();
                    LineProp line = node as LineProp;
                    line.Points = new Vector2[] {start, end};
                    line.DefaultColor = Colors.Bisque;
                    line.Gradient = null;
                    int id = Globals.Instance.Edges.GetChildCount();
                    line.Name = id.ToString();
                    line.ID = id;
                    line.Val = 10d;
                    line.Editable = false;
                    generator.StartLine.Add(id);
                    EndLine.Add(id);

                    Globals.Instance.Edges.AddChild(line);
                    Globals.Instance.ClickedId = -1;
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
        ResourceNode resourceNode = Globals.Instance.GetNode(id) as ResourceNode;
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