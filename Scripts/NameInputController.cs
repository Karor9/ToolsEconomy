using Godot;
using System;
using System.ComponentModel;

public partial class NameInputController : InputController
{
    [Export] RichTextLabel richTextLabel;
    public override void SaveEdits(ElementController ec)
    {
        base.SaveEdits(ec);
        g.Name = newValue;
        ((RichTextLabel)ec.GetChild(1)).Text = g.Name;
    }

    public override void SaveEdits(GeneratorController ec)
    {
        base.SaveEdits(ec);
    }


    public void OnLeaveFocusText()
    {
        richTextLabel.Text = Text;
        // Visible = false;
        Node node = GetParent().GetParent();
        if(node is ElementController ec)
            SaveEdits(ec);
        else if(node is GeneratorController gc)
            SaveEdits(gc);
    }
    
    public void OnEnterFocus()
    {
        Text = richTextLabel.Text;
        Visible = true;
    }

}
