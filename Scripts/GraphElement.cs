using Godot;
using Godot.Collections;
using System;

public partial class GraphElement : Panel
{
    public int ID;
    public double Count;
    public string Text;
    public Array<int> LinesStart = new Array<int>();
    public Array<int> LinesStop = new Array<int>();

    public override void _Ready()
    {
        Control edit = GetChild(3) as Control;
        edit.FocusExited += () => SaveEditName();
        SetCountText();
        SetNameText();
    }

    void SaveEditName()
    {
        LineEdit lineEdit = GetChild(3) as LineEdit;
        lineEdit.Visible = false;

        RichTextLabel name = GetChild(1) as RichTextLabel;
        name.Visible = true;
        Text = lineEdit.Text;
        SetNameText();
    }

    public void SetCountText()
    {
        RichTextLabel count = GetChild(2) as RichTextLabel;
        count.Text = Math.Round(Count, 2).ToString();
    }

    public void SetNameText()
    {
        RichTextLabel name = GetChild(1) as RichTextLabel;
        LineEdit lineEdit = GetChild(3) as LineEdit;
        name.Text = Text;
        lineEdit.Text = Text;
    }
}