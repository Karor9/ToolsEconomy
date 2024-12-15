using Godot;

public partial class Tools : Control
{
    [Export] PackedScene Node;
    public override void _Input(InputEvent @event)
    {
        if(@event.IsActionPressed("RMB"))
        {
            Node node = Node.Instantiate();
            NodeProp n = node as NodeProp;
            n.ID = GetChildren().Count;
            n.Position = GetGlobalMousePosition();
            AddChild(n);
            Globals.Instance.Nodes.Add(n);
        }
    }
}
