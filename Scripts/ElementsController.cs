using Godot;
using System;

public partial class ElementsController : Control
{
    [Export] Control Parent;
    [ExportGroup("Nodes")]
    [Export] PackedScene Node;
    [Export] PackedScene Generator;
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
                case Enums.ToolState.EditingNode:
                    SaveEdits();
                    break;
                case Enums.ToolState.AddingGenerator:
                    CreateGenerator();
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
        Count.Text = Utils.FormatNumbers(good.Count);

        Vector2 size = nodeProp.Size;
        nodeProp.Position = GetGlobalMousePosition() - (size/2);
        nodeProp.MouseEntered += () => IsObstructed(true);
        nodeProp.MouseExited += () => IsObstructed(false);

        foreach (Control item in nodeProp.GetChildren())
        {
            item.MouseEntered += () => IsObstructed(true);
            item.MouseExited += () => IsObstructed(false);
        }
        Parent.AddChild(nodeProp);
    }

    void SaveEdits()
    {
        LineEdit le = (LineEdit)Globals.Instace.CurrFocus;
        if(Globals.Instace.CurrFocus is null)
            return;
        if(le.Name == "NameInput")
            ((NameInputController)le).SaveEdits((ElementController)le.GetParent().GetParent());
        else if(le.Name == "CountInput")
            ((CountInputController)le).SaveEdits((ElementController)le.GetParent().GetParent());
    }

    void CreateGenerator()
    {
        Node node = Generator.Instantiate();
        Panel nodeProp = (Panel)node;
        
        return;
    }
}
