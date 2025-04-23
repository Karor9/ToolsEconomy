using Godot;
using System;
using Godot.Collections;

public partial class SaveRecipe : Button
{
    [Export] VBoxContainer GoodsContainer;

    void OnClick()
    {
        if(GoodsContainer.GetChildCount() <= 1)
            return;
        Dictionary<int, double> recipe = new Dictionary<int, double>();
        foreach (Control item in GoodsContainer.GetChildren())
        {
            if(item.Name == "MainPanel")
                continue;
            CraftingGoodsController cgc = (CraftingGoodsController)item;
            if(!cgc.IsInRecipe())
                continue;
            double count = cgc.GetRequired();
            GD.Print("Count ", count);
            if(count <= 0)
                continue;
            recipe.Add(cgc.GetGoodsId(), count);
            GD.Print(recipe.Keys, "keys rolling");
        }

        if(Globals.Instance.CurrFocus is CraftingController cc)
        {
            bool x = cc.SetNewRecipe(recipe);
            if(x)
            {
                Control c = (Control)GoodsContainer.GetParent().GetParent();
                c.Visible = false;
            }
        }
    }
}
