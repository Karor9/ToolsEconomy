using Godot;

public partial class Tools : Control
{
    [Export] PackedScene Node;
    public override void _Input(InputEvent @event)
    {
        if(@event.IsActionPressed("LMB") && Globals.Instance.CurrentState == Globals.ToolState.AddingNode)
        {
            GD.Print("added");
            Node node = Node.Instantiate();
            NodeProp n = node as NodeProp;
            n.ID = GetChildren().Count;
            Vector2 size = n.Size;
            n.Position = GetGlobalMousePosition() - (size/2);
            AddChild(n);
            Globals.Instance.Nodes.Add(n);
        }
    }
}
