using Godot;
using System;
using System.Linq;
using Godot.Collections;
using System.Threading.Tasks;

public partial class ElementsController : Control
{
    [Export] Control Parent;
    [Export] Control ParentGenerator;
    [Export] Node ParentCrafting;
    [ExportGroup("Nodes")]
    [Export] PackedScene Node;
    [Export] PackedScene Generator;
    [Export] PackedScene Crafting;

    public override void _Ready()
    {
        Globals.Instance.GoodParent = Parent;
    }

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
                case Enums.ToolState.AddingCrafting:
                    CreateCraftingNode();
                    break;
            }
        }
        if(@event.IsActionPressed("Spawn"))
        {
            _ = HandleSpawnAsync(async () => {
                await SpawnGeneratorDots(1f);
                await CraftingDots(1f);
                await CraftGoods(1f);
            });
        }
    }

    async Task HandleSpawnAsync(Func<Task> action)
    {
        await action();
    }

    async Task CraftGoods(float time)
    {
        foreach (Node item in ParentCrafting.GetChildren())
        {
            CraftingController cc = (CraftingController)item;
            if(cc.currentTranstactions <= 0)
                continue;
            int[] targetGoods = cc.OutputRecipe.Keys.ToArray();
            foreach (Node node in item.GetChild(5).GetChildren())
            {
                Line2D line = (Line2D)node;
                Node dotParent = line.GetChild(0);
                Dot dot = (Dot)Globals.Instance.Dot.Instantiate();
                dot.Init(line.Points[0], line.Points[1], line, 0, Colors.Yellow, false);
                dotParent.AddChild(dot);
            }

            foreach (int good in targetGoods)
            {
                Globals.Instance.Goods[good].AddValue(cc.OutputRecipe[good]);
            }
            cc.currentTranstactions -= 1;
        }        
        if(ParentCrafting.GetChildCount() <= 0)
            time = time / 1000f;
        await Utils.DelaySeconds(time, this);
    }

    async Task CraftingDots(float time)
    {
        foreach (Node item in ParentCrafting.GetChildren())
        {
            CraftingController cc = (CraftingController)item;
            bool haveGoods = true;
            int[] requiredGoods = cc.Recipe.Keys.ToArray();
            foreach (int good in requiredGoods)
            {
                if(Globals.Instance.Goods[good].Count < cc.Recipe[good])
                {
                    haveGoods = false;
                    break;
                }
            }
            if(!haveGoods)
                continue;
            
            foreach (Node node in item.GetChild(4).GetChildren())
            {
                Line2D line = (Line2D)node;
                Node dotParent = line.GetChild(0);
                Dot dot = (Dot)Globals.Instance.Dot.Instantiate();
                dot.Init(line.Points[0], line.Points[1], line, 0, Colors.LimeGreen, false);
                dotParent.AddChild(dot);
            }

            foreach (int good in requiredGoods)
            {
                Globals.Instance.Goods[good].AddValue(-1*cc.Recipe[good]);
            }
            cc.currentTranstactions += 1;
            // GD.Print(cc.Recipe.Keys);
        }
        if(ParentCrafting.GetChildCount() <= 0)
            time = time / 1000f;
        await Utils.DelaySeconds(time, this);
    }

    private async Task SpawnGeneratorDots(float time)
    {
        foreach (Node item in ParentGenerator.GetChildren())
        {
            Node node = item.GetChild(6);
            GeneratorController gc = (GeneratorController)item;
            if(node.GetChildCount() <= 0)
                continue;
            foreach (Node item2 in node.GetChildren())
            {
                Line2D line2D = (Line2D)item2;
                Node dotParent = line2D.GetChild(0);
                Dot dot = (Dot)Globals.Instance.Dot.Instantiate();
                dot.Init(line2D.Points[0], line2D.Points[1], line2D, gc.GetValueToAdd());
                dotParent.AddChild(dot);
            }
        }
        if(ParentGenerator.GetChildCount() <= 0)
            time = time / 1000f;
        await Utils.DelaySeconds(time, this);
    }

    private void CreateCraftingNode()
    {
        Node node = Crafting.Instantiate();
        CraftingController cc = (CraftingController)node;

        SetupEvents(cc);

        ParentCrafting.AddChild(cc);
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
        Arrow line = (Arrow)Globals.Instance.Arrow.Instantiate();
        GeneratorController gc = (GeneratorController)Globals.Instance.CurrFocus;
        line.Parent = gc;
        line.Child = nodeProp;
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
                    nameInput.SaveEdits(element);
                }
                else if (le is CountInputController countInput)
                {
                    countInput.SaveEdits(element);
                }
            }
            if(grandParent is GeneratorController node)
            {
                if (le is NameInputController nameInput)
                {
                    nameInput.SaveEdits(node);
                }
                else if (le is CountInputController countInput)
                {
                    countInput.SaveEdits(node);
                } else
                {;
                    node.LostFocusPanel();
                }
                
            }
        }
        if(Globals.Instance.CurrFocus is ChanceController ce)
        {
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
