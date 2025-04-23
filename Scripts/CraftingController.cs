using Godot;
using Godot.Collections;

public partial class CraftingController : NodeController
{
    [Export] public Array<Line2D> InLine;
    [Export] public Array<Line2D> OutLine;

    [Export] Button SetRecipe;
    [Export] Button ShowRecipe;

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

    public override void _Ready()
    {
        Utils.Print("green", "Goods Count: " + Globals.Instance.Goods.Keys.Count);
    }

    void SpawnGoods()
    {
        Node parent = Globals.Instance.CraftingInputContainer;
        foreach (Node item in parent.GetChildren())
        {
            if(item.Name != "MainPanel")
                item.QueueFree();
        }
        foreach (int item in Globals.Instance.Goods.Keys)
        {
            GD.Print(item);
            Node node = Globals.Instance.Crafting.Instantiate();
            CraftingGoodsController cgc = (CraftingGoodsController)node;
            cgc.Init(item);
            parent.AddChild(cgc);
        }
    }

}
