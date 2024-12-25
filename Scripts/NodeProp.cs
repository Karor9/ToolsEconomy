using Godot;
using Godot.Collections;
using System;

public partial class NodeProp : Control
{
    public int ID;
    public double Count;
    public string Text;


    public override void _Ready()
    {
        //Grab LineEdit and add signal to save changes
        Control edit = GetChild(3) as Control;
        edit.FocusExited += () => SaveEditName();

        //Check is mouse is on Button (Node)
        Button button = GetChild(GetChildCount() - 1) as Button;
        button.MouseEntered += () => Utils.Obstructed(true);
        button.MouseExited += () => Utils.Obstructed(false);

        //Set Texts on Node
        SetCountText();
        SetNameText();
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

    

    public virtual void OnClick()
    {
        switch(Globals.Instance.CurrentState)
        {
            case Enums.ToolState.MoveNode:
                Globals.Instance.ClickedId = ID;
                GD.Print(Globals.Instance.ClickedId);
                break;
        }
    }
}