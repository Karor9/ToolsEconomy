using Godot;
using System;

public partial class NameInputController : InputController
{
    [Export] RichTextLabel richTextLabel;
    public override void SaveEdits(ElementController ec)
    {
        base.SaveEdits(ec);
        g.Name = newValue;
        ((RichTextLabel)ec.GetChild(1)).Text = g.Name;
    }

    public void OnLeaveFocusText()
    {
        richTextLabel.Text = Text;
        // Visible = false;
        SaveEdits((ElementController)GetParent().GetParent());
    }
    
    public void OnEnterFocus()
    {
        Text = richTextLabel.Text;
        Visible = true;
    }

}
