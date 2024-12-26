using Godot;
using System;
using System.Threading;

public partial class LineProp : Line2D
{
    double val = 1d;
    public int ID;
    public override void _Ready()
    {
        RichTextLabel label = GetChild(0) as RichTextLabel;
        label.Text = "[center]" + Math.Round(val, 2).ToString();
        LineEdit lineEdit = GetChild(1) as LineEdit;
        lineEdit.FocusExited += () => SetNameText();
    }

    public override void _Process(double delta)
    {
        base._Process(delta);
    }

    private void SetNameText(bool show = false)
    {
        RichTextLabel name = GetChild(0) as RichTextLabel;
        LineEdit lineEdit = GetChild(1) as LineEdit;
        name.Visible = !show;
        lineEdit.Visible = show;
        if(!show)
            name.Text = lineEdit.Text;
        else
            lineEdit.Position = name.Position;
        lineEdit.Text = name.Text;
    }
}