using Godot;
using System;

public partial class NameInputController : InputController
{
    public override void SaveEdits(ElementController ec)
    {
        base.SaveEdits(ec);
        g.Name = newValue;
        ((RichTextLabel)ec.GetChild(0)).Text = g.Name;
    }

}
