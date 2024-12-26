using Godot;
using System;
using System.Threading;

public partial class Tools : Control
{
    public override void _Input(InputEvent @event)
    {
        if(@event.IsActionReleased("LMB"))
        {
            int id = -1;
            switch(Globals.Instance.CurrentState)
            {
                case Enums.ToolState.AddingNode:
                    if(Globals.Instance.IsObstructed)
                        break;
                    id = Globals.Instance.Nodes.GetChildCount();
                    GD.Print("ID: ", id, " Start");
                    Node node = Globals.Instance.Node.Instantiate();
                    
                    ResourceNode nodeProp = node as ResourceNode;
                    nodeProp.ID = id;
                    Globals.Instance.SetNextID(nodeProp.ID);
                    nodeProp.Text = "New Resource";
                    
                    Vector2 size = nodeProp.Size;
                    nodeProp.Position = GetGlobalMousePosition() - (size/2);
                    Globals.Instance.Nodes.AddChild(nodeProp);
                    break;
                case Enums.ToolState.MoveNode:
                    id = Globals.Instance.ClickedId;
                    if(id < 0)
                        break;
                    Vector2 newPos = GetGlobalMousePosition();
                    Control c = Globals.Instance.Nodes.GetNode(new NodePath(id.ToString())) as Control;
                    
                    c.Modulate = new Color(c.Modulate.R, c.Modulate.G, c.Modulate.B, 1f);
                    c.Position = newPos - (c.Size/2);
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
                int id = Globals.Instance.ClickedId;
                ResourceNode node = Globals.Instance.Nodes.GetNode(new NodePath(id.ToString())) as ResourceNode;
                Vector2 newPos = GetGlobalMousePosition() -  (node.Size/2);
                
                node.Position = newPos;
                node.Modulate = new Color(node.Modulate.R, node.Modulate.G, node.Modulate.B, 0.5f);
            }
        }
    }
}