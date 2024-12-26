using Godot;
using System;
using System.Threading;

public partial class Tools : Control
{
    [Export] Control Nodes;
    [Export] Control Edges;

    public override void _Ready()
    {
        Globals.Instance.Nodes = Nodes;
        Globals.Instance.Edges = Edges;
    }
    public override void _Input(InputEvent @event)
    {
        if(@event.IsActionPressed("LMB"))
        {
            switch(Globals.Instance.CurrentState)
            {
                case Enums.ToolState.AddingNode:
                    if(Globals.Instance.IsObstructed)
                        break;
                    
                    int id = Nodes.GetChildCount();
                    Node node = Globals.Instance.Node.Instantiate();
                    
                    ResourceNode nodeProp = node as ResourceNode;
                    nodeProp.ID = id;
                    Globals.Instance.SetNextID(nodeProp.ID);
                    nodeProp.Text = "New Resource";
                    
                    Vector2 size = nodeProp.Size;
                    nodeProp.Position = GetGlobalMousePosition() - (size/2);
                    Nodes.AddChild(nodeProp);
                    break;
                case Enums.ToolState.AddingGenerator:
                    if(Globals.Instance.IsObstructed)
                        break;
                    break;
            }
        }
        if(@event.IsActionReleased("LMB"))
        {
            switch(Globals.Instance.CurrentState)
            {
                case Enums.ToolState.MoveNode:
                    
                    MoveNode(1f);

                    Globals.Instance.ClickedId = -1;

                    break;
            }
        }
    }

    public override void _PhysicsProcess(double delta)
    {
        if(Input.IsMouseButtonPressed(MouseButton.Left) && Globals.Instance.CurrentState == Enums.ToolState.MoveNode)
        {
            if(Globals.Instance.ClickedId >= 0)
            {
                MoveNode(0.5f);
            }
        }
    }

    void MoveNode(float alpha)
    {
        int id = Globals.Instance.ClickedId;
        if(id < 0)
            return;
        Vector2 newPos = GetGlobalMousePosition();
        NodeProp c = Globals.Instance.Nodes.GetNode(new NodePath(id.ToString())) as NodeProp;
        
        c.Modulate = new Color(c.Modulate.R, c.Modulate.G, c.Modulate.B, alpha);
        c.Position = newPos - (c.Size/2);
        foreach (int item in c.StartLine)
        {
            LineProp lineProp = Globals.Instance.Edges.GetNode(new NodePath(item.ToString())) as LineProp;
            lineProp.SetPointPosition(0, c.Position + (c.Size/2));
            RichTextLabel text = lineProp.GetChild(0) as RichTextLabel;
            text.Position = (lineProp.Points[0] + lineProp.Points[1])/2 - (text.Size/20);
        }
        foreach (int item in c.EndLine)
        {
            LineProp lineProp = Globals.Instance.Edges.GetNode(new NodePath(item.ToString())) as LineProp;
            lineProp.SetPointPosition(1, c.Position + (c.Size/2));
            RichTextLabel text = lineProp.GetChild(0) as RichTextLabel;
            text.Position = (lineProp.Points[0] + lineProp.Points[1])/2 - (text.Size/20);
        }
    }
}