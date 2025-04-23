using Godot;
using System;
using System.Security.Cryptography;

public partial class CraftingGoodsController : Panel
{
    [Export] RichTextLabel Label;
    [Export] SpinBox Count;
    [Export] TextureButton Yay;
    [Export] TextureButton Nay;
    int id = 0;
    double requiredCount = 0;

    public void Init(int id)
    {
        this.id = id;
        Name = id.ToString();
    }

    public override void _Ready()
    {
        if(!Globals.Instance.Goods.Keys.Contains(id))
            return;
        Label.Text = Globals.Instance.Goods[id].Name;
        Count.Value = double.Parse(Count.GetLineEdit().Text.Replace(".",","));
    }

    void OnClickYay()
    {
        Yay.Visible = false;
        Nay.Visible = true;
        requiredCount = double.Parse(Count.GetLineEdit().Text.Replace(".",","));
        Count.GetLineEdit().Editable = true;

    }

    void OnClickNay()
    {
        Nay.Visible = false;
        Yay.Visible = true;
        requiredCount = 0;
        Count.GetLineEdit().Text = Utils.FormatNumbers(requiredCount);
        Count.GetLineEdit().Editable = false;
    }

}
