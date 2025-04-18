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
                    CreateNode();
                    break;
            }
        }

    }

    void IsObstructed(bool state)
    {
        Globals.Instace.Obstructed = state;
    }

    void CreateNode()
    {
        Node node = Node.Instantiate();
        Panel nodeProp = (Panel)node;

        int id = Utils.GetNextID();
        Goods good = new Goods("New Goods Name", 0d, nodeProp);
        Utils.AddToGoodsArray(id, good);
        nodeProp.Name = id.ToString();
                
        RichTextLabel GoodName = (RichTextLabel)nodeProp.GetChild(0);
        GoodName.Text = good.Name;
                
        RichTextLabel Count = (RichTextLabel)nodeProp.GetChild(1);
        Count.Text = string.Format("{0:0.00}",Math.Round(good.Count, 3));

        Vector2 size = nodeProp.Size;
        nodeProp.Position = GetGlobalMousePosition() - (size/2);
        nodeProp.MouseEntered += () => IsObstructed(true);
        nodeProp.MouseExited += () => IsObstructed(false);
        Parent.AddChild(nodeProp);
    }

}
