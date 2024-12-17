using System;
using Godot;
using Godot.Collections;

public partial class NodeProp : Panel
{
    [Export] public Array<int> Parents = new Array<int>();
    [Export] public Array<int> Childs = new Array<int>();
    bool secondPressTime = false;

    public int ID;
    public double Count;
    public string Text;

    public override void _Ready()
    {
        Button b = GetChild(GetChildCount() - 1) as Button;
        b.Pressed += () => OnClick();
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

    void AddLine()
    {
        if(ID == Globals.Instance.ClickedId)
            return;
        Parents.Add(Globals.Instance.ClickedId);
        Globals.Instance.Nodes[Globals.Instance.ClickedId].Childs.Add(ID);
        Node node = Globals.Instance.Edge.Instantiate();
        Vector2 s = Globals.Instance.Nodes[Globals.Instance.ClickedId].Position + (Globals.Instance.Nodes[Globals.Instance.ClickedId].Size / 2);
        Vector2 e = Position + (Size/2);
        Line2D line = node as Line2D;
        line.Points = new Vector2[] {s, e};
        RichTextLabel text = node.GetChild(0) as RichTextLabel;
        text.Position = (s + e)/2;
        text.RotationDegrees = 0;
        Globals.Instance.EdgesContainer.AddChild(line);
        Globals.Instance.ClickedId = -1;
    }

    void OnClick()
    {
        switch(Globals.Instance.CurrentState)
        {
            case Globals.ToolState.AddingLine:
                if(Globals.Instance.ClickedId == -1)
                {
                    Globals.Instance.ClickedId = ID;
                } else if (Globals.Instance.ClickedId > -1)
                {
                    AddLine();
                }
                break;
            case Globals.ToolState.EditingNode:
                LineEdit lineEdit = GetChild(3) as LineEdit;
                lineEdit.Visible = true;

                RichTextLabel name = GetChild(1) as RichTextLabel;
                name.Visible = false;

                lineEdit.CaretColumn = lineEdit.Text.Length;
                lineEdit.GrabFocus();
                Globals.Instance.EditedNode = lineEdit;
                break;
            case Globals.ToolState.MoveNode:
                Globals.Instance.ClickedId = ID;
                break;

        }
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

    // public override void _Input(InputEvent @event)
    // {
    //     if(@event is InputEventMouseButton e && e.ButtonIndex == MouseButton.Left && e.DoubleClick)
    //     {
    //         Globals.Instance.ClickedId = -1;
    //         GD.Print(ID);
    //     }
    // }
}
