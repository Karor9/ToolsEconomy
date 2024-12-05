using Godot;

public partial class Tools : CanvasLayer
{
    [Export] PackedScene Node;
    public override void _Input(InputEvent @event)
    {
        if(@event.IsActionPressed("RMB"))
        {
            Node node = Node.Instantiate();
            NodeProp n = node as NodeProp;
            n.ID = GetChildren().Count;
            n.Position = GetViewport().GetMousePosition();
            AddChild(n);
            Globals.Instance.Nodes.Add(n);
        }
    }
}
