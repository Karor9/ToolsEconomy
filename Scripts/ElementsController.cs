using Godot;
using System;
using System.Runtime.Serialization;

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
            if(Globals.Instance.Obstructed)
                return;
            switch(Globals.Instance.CurrentToolState)
            {
                case Enums.ToolState.AddingNode:
                    CreateNode();
                    break;
                case Enums.ToolState.EditingNode:
                    SaveEdits();
                    break;
                case Enums.ToolState.AddingGenerator:
                    if(Globals.Instance.CurrFocus is not null)
                        CreateGenerateNode();
                    else
                        CreateGenerator();
                    break;
            }
        }
    }

    void IsObstructed(bool state)
    {
        Globals.Instance.Obstructed = state;
    }

    void CreateGenerateNode()
    {
        if(Globals.Instance.Obstructed)
            return;
        Panel nodeProp = CreateNode();
        Line2D line = (Line2D)Globals.Instance.Arrow.Instantiate();
        
        GeneratorController gc = (GeneratorController)Globals.Instance.CurrFocus;
        Node node = gc.GetChild(6);
        // Vector2 fp = gc.Position + (gc.Size/2);
        // Vector2 ep = nodeProp.Position + (nodeProp.Size/2);

        
        Vector2 fp = Utils.GetEdgePoints(gc, nodeProp);
        Vector2 ep = Utils.GetEdgePoints(nodeProp, gc);
        line.Points = [fp, ep];

        Utils.SpawnChangeBlock(line);

        Utils.DrawArrow(line, fp, ep);
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
                
        RichTextLabel GoodName = (RichTextLabel)nodeProp.GetChild(1);
        GoodName.Text = good.Name;
                
        RichTextLabel Count = (RichTextLabel)nodeProp.GetChild(2);
        Count.Text = Utils.FormatNumbers(good.Count);
        SetupEvents(nodeProp);
        Parent.AddChild(nodeProp);
        return nodeProp;
    }

    void SaveEdits()
    {
        if (Globals.Instance.CurrFocus is LineEdit le)
        {
            Node grandParent = le.GetParent()?.GetParent();

            if (grandParent is ElementController element)
            {
                if (le is NameInputController nameInput)
                {
                    Utils.Print("blue", "NameInputController");
                    nameInput.SaveEdits(element);
                }
                else if (le is CountInputController countInput)
                {
                    Utils.Print("blue", "CountInputController");
                    countInput.SaveEdits(element);
                }
            }
            if(grandParent is GeneratorController node)
            {
                Utils.Print("blue", "GeneratorController");
                node.LostFocusPanel();
            }
            Utils.Print("blue", "LineEdit");
        }
        if(Globals.Instance.CurrFocus is ChanceController ce)
        {
            Utils.Print("blue", "ChanceController");
            ce.SaveMouse();
        }
    }


    void CreateGenerator()
    {
        if(Globals.Instance.Obstructed)
            return;
        Node node = Generator.Instantiate();
        Panel nodeProp = (Panel)node;
        nodeProp.Name = "Generator " + ParentGenerator.GetChildCount().ToString();
        RichTextLabel Count = (RichTextLabel)nodeProp.GetChild(2);
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
