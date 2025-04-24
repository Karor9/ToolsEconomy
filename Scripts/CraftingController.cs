using System;
using Godot;
using Godot.Collections;

public partial class CraftingController : NodeController
{
    [Export] public Array<Line2D> InLine;
    [Export] public Array<Line2D> OutLine;

    [Export] Button SetRecipe;
    [Export] Button ShowRecipe;

    [Export] public Dictionary<int, double> Recipe = new Dictionary<int, double>();
    [Export] public Dictionary<int, double> OutputRecipe = new Dictionary<int, double>();

    [Export] Node ArrowIn;
    [Export] Node ArrowOut;

    [Export] bool InputMode = true;
    public void SetupCrafting()
    {
        if(Globals.Instance.CraftingInputContainer is null)
            return;
        if(Globals.Instance.Goods.Keys.Count <= 0)
            return;
        ((Control)Globals.Instance.CraftingInputContainer.GetParent().GetParent()).Visible = true;
        SpawnGoods();
        return;
    }

    public void MoveButtonSetup(bool val)
    {
        SetRecipe.Visible = val;
        ShowRecipe.Visible = val;
    }

    public override void _Ready()
    {
        Utils.Print("green", "Goods Count: " + Globals.Instance.Goods.Keys.Count);
    }

    public bool SetNewRecipe(Dictionary<int, double> newRecipe)
    {
        if(InputMode)
        {
            Recipe.Clear();
            Recipe = newRecipe;
            SpawnArrows();
            SpawnGoods("Output");
            InputMode = false;
            return false;
        } else
        {
            OutputRecipe.Clear();
            OutputRecipe = newRecipe;
            SpawnArrows();
            InputMode = true;

            return true;
        }
    }

    void SpawnGoods(string newText = "Input")
    {
        Node parent = Globals.Instance.CraftingInputContainer;
        foreach (Node item in parent.GetChildren())
        {
            if(item.Name != "MainPanel") //TBD workaround
                item.QueueFree();
            else
            {
                RichTextLabel rtl = (RichTextLabel)item.GetChild(1);
                rtl.Text = newText;
            }

        }
        foreach (int item in Globals.Instance.Goods.Keys)
        {
            Node node = Globals.Instance.Crafting.Instantiate();
            CraftingGoodsController cgc = (CraftingGoodsController)node;
            cgc.Init(item);
            parent.AddChild(cgc);
        }
        Globals.Instance.CurrFocus = this;
    }

    void SpawnArrows()
    {
        if(InputMode)
        {
            foreach (int item in Recipe.Keys)
            {
                Node node = Globals.Instance.Arrow.Instantiate();
                ElementController ec = (ElementController)Globals.Instance.Goods[item].Element;
                Arrow line = (Arrow)node;

                line.Parent = ec;
                line.Child = this;

                line.GetChild(0).QueueFree();
                Vector2 fp = Utils.GetEdgePoints(ec, this);
                Vector2 ep = Utils.GetEdgePoints(this, ec);
                line.Points = [fp, ep];

                ec.OutLine.Add(line);
                InLine.Add(line);

                Utils.DrawArrow(line, fp, ep);
                line.Name = item.ToString();
                ArrowIn.AddChild(line);
            }
        } else
        {
            foreach (int item in OutputRecipe.Keys)
            {
                Node node = Globals.Instance.Arrow.Instantiate();
                ElementController ec = (ElementController)Globals.Instance.Goods[item].Element;
                Arrow line = (Arrow)node;

                line.Parent = this;
                line.Child = ec;

                line.GetChild(0).QueueFree();
                Vector2 fp = Utils.GetEdgePoints(ec, this);
                Vector2 ep = Utils.GetEdgePoints(this, ec);
                line.Points = [ep, fp];
            
                ec.InLine.Add(line);
                OutLine.Add(line);

                Utils.DrawArrow(line, ep, fp);
                line.Name = item.ToString();
                ArrowOut.AddChild(line);
            }
        }
    }

    public override void MoveNode()
    {
        base.MoveNode();

        foreach (Node item in InLine)
        {
            Arrow line = (Arrow)item;
            Panel element = (Panel)line.GetParent().GetParent();
            Utils.RedrawArrow(line.Parent, line.Child, line);
        }
        foreach (Node item in OutLine)
        {
            Arrow line = (Arrow)item;
            Panel element = (Panel)line.GetParent().GetParent();
            Utils.RedrawArrow(line.Parent, line.Child, line);
        }
    }

    public override void _PhysicsProcess(double delta)
    {
        if(Input.IsActionPressed("LMB") 
        && Globals.Instance.CurrentToolState == Enums.ToolState.MoveNode
        && Globals.Instance.CurrFocus == this)
        {
            MoveNode();
        }
    }

    public override void _Input(InputEvent @event)
    {
        if(Input.IsActionPressed("LMB") 
        && Globals.Instance.CurrentToolState == Enums.ToolState.MoveNode
        && Globals.Instance.CurrFocus == this)
        {
            SelfModulate = new Color(1, 1, 1, 0.5f);
        }

        if(@event.IsActionReleased("LMB") 
        && Globals.Instance.CurrentToolState == Enums.ToolState.MoveNode
        && Globals.Instance.CurrFocus == this)
        {
            SelfModulate = new Color(1,1,1,1);
            Globals.Instance.CurrFocus = null;
        }
    }

    void Pressed(InputEvent @event)
    {
        if(@event.IsActionPressed("LMB") &&
        Globals.Instance.CurrentToolState == Enums.ToolState.MoveNode)
        {
            Globals.Instance.CurrFocus = this;
        }
    }



}
