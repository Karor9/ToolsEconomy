using Godot;
using Godot.Collections;
using System;
using System.Reflection;

public partial class NodeProp : Control
{
    public int ID;
    public double Count;
    public string Text;


    public override void _Ready()
    {
        Name = ID.ToString();
        //Grab LineEdit and add signal to save changes
        Control edit = GetChild(3) as Control;
        edit.FocusExited += () => SaveEditName();

        //Check is mouse is on Button (Node)
        Button button = GetChild(5) as Button;

        button.Pressed += () => OnClick();
        button.MouseEntered += () => Obstructed(true);
        button.MouseExited += () => Obstructed(false);
        // GD.Print(button.IsConnected(SignalName.MouseEntered, Callable.From(() => Obstructed(true))));

        //Set Texts on Node
        SetCountText();
        SetNameText();
    }
    public void Obstructed(bool obstructed)
    {
        GD.Print(obstructed);
        Globals.Instance.IsObstructed = obstructed;
    }

    void SaveEditName()
    {
        //Disable editing Name
        LineEdit lineEdit = GetChild(3) as LineEdit;
        lineEdit.Visible = false;

        //Get Text component
        RichTextLabel name = GetChild(1) as RichTextLabel;
        name.Visible = true;
        //Update text;
        Text = lineEdit.Text;
        //Update displayed text;
        SetNameText();
    }

    void SetNameText()
    {
        RichTextLabel name = GetChild(1) as RichTextLabel;
        LineEdit lineEdit = GetChild(3) as LineEdit;
        name.Text = Text;
        lineEdit.Text = Text;
    }

    void SetCountText()
    {
        RichTextLabel count = GetChild(2) as RichTextLabel;
        count.Text = Math.Round(Count, 2).ToString();
    }

    

    public void OnClick()
    {
        GD.Print(ID);
        switch(Globals.Instance.CurrentState)
        {
            case Enums.ToolState.MoveNode:
                Globals.Instance.ClickedId = ID;
                break;
        }
    }
}