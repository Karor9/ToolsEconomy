using Godot;
using System;

public partial class ElementsController : Control
{
    [Export] Control Parent;
    [Export] Control ParentGenerator;
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
                    if(Globals.Instace.CurrFocus is not null)
                        CreateGenerateNode();
                    else
                        CreateGenerator();
                    break;
            }
        }

    }

    void IsObstructed(bool state)
    {
        Globals.Instace.Obstructed = state;
    }

    void CreateGenerateNode()
    {
        if(Globals.Instace.Obstructed)
            return;
        Panel nodeProp = CreateNode();
        Line2D line = (Line2D)Globals.Instace.Arrow.Instantiate();
        
        GeneratorController gc = (GeneratorController)Globals.Instace.CurrFocus;
        Node node = gc.GetChild(5);
        Vector2 fp = gc.Position + (gc.Size/2);
        Vector2 ep = nodeProp.Position + (nodeProp.Size/2);
        line.Points = [fp, ep];
        line.Name = nodeProp.Name;
        NodePath nodePath = new NodePath(nodeProp.Name);
        if(node.GetNodeOrNull(nodePath) is not null)
            return;
        node.AddChild(line);
        ElementController ec = (ElementController)nodeProp;
        ec.InLine.Add(line);
        gc.LostFocusPanel();
    }

    Panel CreateNode()
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
        SetupEvents(nodeProp);
        Parent.AddChild(nodeProp);
        return nodeProp;
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
        if(Globals.Instace.Obstructed)
            return;
        Node node = Generator.Instantiate();
        Panel nodeProp = (Panel)node;
        nodeProp.Name = "Generator " + ParentGenerator.GetChildCount().ToString();
        RichTextLabel Count = (RichTextLabel)nodeProp.GetChild(1);
        Count.Text = Utils.FormatNumbers(1);

        nodeProp = SetupEvents(nodeProp);
        ParentGenerator.AddChild(nodeProp);
        nodeProp.GrabFocus();
        
        return;
    }

    Panel SetupEvents(Panel nodeProp)
    {
        Vector2 size = nodeProp.Size;
        nodeProp.Position = GetGlobalMousePosition() - (size/2);
        nodeProp.MouseEntered += () => IsObstructed(true);
        nodeProp.MouseExited += () => IsObstructed(false);
        return nodeProp;
    }
}
