using Godot;

public partial class Tools : Control
{
    [Export] PackedScene Node;
    public override void _Input(InputEvent @event)
    {
        if(@event.IsActionPressed("LMB"))
        {
            switch(Globals.Instance.CurrentState)
            {
                case Globals.ToolState.AddingNode:
                    Node node = Node.Instantiate();
                    NodeProp n = node as NodeProp;
                    n.ID = GetChildren().Count;
                    n.Text = "New Resource";
                    Vector2 size = n.Size;
                    n.Position = GetGlobalMousePosition() - (size/2);
                    AddChild(n);
                    Globals.Instance.Nodes.Add(n);
                    break;
                case Globals.ToolState.EditingNode:
                    if(Globals.Instance.EditedNode == null)
                        break;
                    Globals.Instance.EditedNode.ReleaseFocus();
                    Globals.Instance.EditedNode = null;
                    break;
                case Globals.ToolState.MoveNode:
                    GD.Print("Start");
                    break;
            }
        }
        
        if(@event.IsActionReleased("LMB"))
        {
            switch(Globals.Instance.CurrentState)
            {
                case Globals.ToolState.MoveNode:
                    if(Globals.Instance.ClickedId < 0)
                        break;
                    Vector2 NewPos = GetGlobalMousePosition();
                    Control control = Globals.Instance.Nodes[Globals.Instance.ClickedId];
                    control.Position = NewPos - (control.Size/2);
                    Globals.Instance.ClickedId = -1;
                    break;
            }
        }
    }
}
