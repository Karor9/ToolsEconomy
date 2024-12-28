using Godot;
using System;
using System.Threading;

public partial class LineProp : Line2D
{
    public double Val = 1d;
    public int ID;
    public bool Editable = true;
    float MaxAngle = 0.03f;
    public override void _Ready()
    {
        RichTextLabel label = GetChild(0) as RichTextLabel;
        label.Text = "[center]" + Math.Round(Val, 2).ToString();
        LineEdit lineEdit = GetChild(1) as LineEdit;
        lineEdit.FocusExited += () => SetNameText();
    }

    public override void _Process(double delta)
    {
        if(Input.IsActionJustReleased("LMB"))
        {
            if(Points.Length > 0)
            {
                float pos1 = Points[0].AngleToPoint(Points[1]) - Points[0].AngleToPoint(GetGlobalMousePosition());
                float pos2 = Points[1].AngleToPoint(Points[0]) - Points[1].AngleToPoint(GetGlobalMousePosition());
                bool onLine = pos1 > -MaxAngle && pos1 < MaxAngle && pos2 > -MaxAngle && pos2 < MaxAngle;
                if(onLine)
                    LineClick();
                // else if(!onLine && Globals.Instance.EditedNode == GetChild(1))
                // {
                //     Globals.Instance.EditedNode.ReleaseFocus();
                //     Globals.Instance.EditedNode = null;
                // }
            }
        }
    }

    private void LineClick()
    {
        if(Globals.Instance.CurrentState == Enums.ToolState.EditingLine && Editable)
        {
            RichTextLabel label = GetChild(0) as RichTextLabel;
            LineEdit lineEdit = GetChild(1) as LineEdit;

            SetNameText(true);

            lineEdit.CaretColumn = lineEdit.Text.Length;
            lineEdit.GrabFocus();
            Globals.Instance.EditedNode = lineEdit;
        }
    }

    private void SetNameText(bool show = false)
    {
        RichTextLabel name = GetChild(0) as RichTextLabel;
        LineEdit lineEdit = GetChild(1) as LineEdit;
        name.Visible = !show;
        lineEdit.Visible = show;
        if(!show)
            name.Text = "[center]" + lineEdit.Text;
        else
            lineEdit.Position = name.Position;
        lineEdit.Text = name.Text.Remove(0,8);
    }
}