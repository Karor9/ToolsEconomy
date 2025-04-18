using Godot;
using System;

public partial class ElementsController : Control
{
    [Export] Control Parent;
    [ExportGroup("Nodes")]
    [Export] PackedScene Node;
    public override void _Input(InputEvent @event)
    {
        if(@event.IsActionPressed("LMB"))
        {
            if(Globals.Instace.Obstructed)
                return;
            switch(Globals.Instace.CurrentToolState)
            {
                case Enums.ToolState.AddingNode:
                    Node node = Node.Instantiate();
                    Panel nodeProp = (Panel)node;

                    int id = Utils.GetNextID();
                    Utils.AddToGoodsArray(id);
                    nodeProp.Name = id.ToString();
                    
                    Vector2 size = nodeProp.Size;
                    nodeProp.Position = GetGlobalMousePosition() - (size/2);
                    nodeProp.MouseEntered += () => IsObstructed(true);
                    nodeProp.MouseExited += () => IsObstructed(false);
                    Parent.AddChild(nodeProp);

                    break;
            }
        }

    }

    void IsObstructed(bool state)
    {
        Globals.Instace.Obstructed = state;
    }

}
