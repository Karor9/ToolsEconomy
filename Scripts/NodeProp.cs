using Godot;
using Godot.Collections;
using System;
using System.Reflection;

public partial class NodeProp : Control
{
    public int ID;
    public double Count;
    public string Text;


    [Export] public Array<int> Parents = new Array<int>();
    [Export] public Array<int> Childs = new Array<int>();
    [Export] public Array<int> StartLine = new Array<int>();
    [Export] public Array<int> EndLine = new Array<int>();

    public override void _Ready()
    {
        Name = ID.ToString();
        //Grab LineEdit and add signal to save changes
        Control edit = GetChild(3) as Control;
        edit.FocusExited += () => SaveEditName();

        //Check is mouse is on Button (Node)
        Button button = GetChild(GetChildCount() - 1) as Button;

        button.MouseEntered += () => Obstructed(true);
        button.MouseExited += () => Obstructed(false);
        // GD.Print(button.IsConnected(SignalName.MouseEntered, Callable.From(() => Obstructed(true))));

        //Set Texts on Node
        SetCountText();
        SetNameText();
    }
    public void Obstructed(bool obstructed)
    {
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

    public void SetNameText()
    {
        RichTextLabel name = GetChild(1) as RichTextLabel;
        LineEdit lineEdit = GetChild(3) as LineEdit;
        name.Text = "[center]" + Text;
        lineEdit.Text = Text;
    }

    public void SetCountText()
    {
        RichTextLabel count = GetChild(2) as RichTextLabel;
        count.Text = "[center]" + Math.Round(Count, 2).ToString();
    }

    

    public virtual void OnClick()
    {
        switch(Globals.Instance.CurrentState)
        {
            case Enums.ToolState.MoveNode:
                Globals.Instance.ClickedId = ID;
                break;
        }
    }

    private void AddLine()
    {
        if(ID == Globals.Instance.ClickedId)
            return;
    }

}