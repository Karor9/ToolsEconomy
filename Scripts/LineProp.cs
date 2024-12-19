using Godot;
using System;

public partial class LineProp : Line2D
{
    double val = 1d;
    public int ID;

    public override void _Ready()
    {
        RichTextLabel label = GetChild(0) as RichTextLabel;
        label.Text = Math.Round(val, 2).ToString();
        LineEdit le = GetChild(1) as LineEdit;
        le.FocusExited += () => SetNameText();
    }

    public override void _Process(double delta)
    {
        if(Input.IsActionJustReleased("LMB"))
            if(Points.Length > 0)
                {
                    float pos1 = Points[0].AngleToPoint(Points[1]) - Points[0].AngleToPoint(GetGlobalMousePosition());
                    float pos2 = Points[1].AngleToPoint(Points[0]) - Points[1].AngleToPoint(GetGlobalMousePosition());
                    if(pos1 > -0.02f && pos1 < 0.02f && pos2 > -0.02f && pos2 < 0.02f)
                        LineClick();
                }

                // GD.Print(Points[0].AngleToPoint(Points[1]) - Points[0].AngleToPoint(GetGlobalMousePosition()));
                // if(Points[0].AngleToPoint(Points[1]) + 0.1f >= Points[0].AngleToPoint(GetGlobalMousePosition()) &&
                // Points[0].AngleToPoint(Points[1]) - 0.1f >= Points[0].AngleToPoint(GetGlobalMousePosition()))
                //     GD.Print("Linia");
            // GD.Print(Points[0].AngleToPoint(Points[1]));
            // GD.Print(Points[1].AngleToPoint(Points[0]));
            // GD.Print(Points[0].AngleToPoint(GetGlobalMousePosition()));

    }

    void LineClick ()
    {
        if(Globals.Instance.CurrentState == Globals.ToolState.EditingLine)
        {
            RichTextLabel label = GetChild(0) as RichTextLabel;
            LineEdit lineEdit = GetChild(1) as LineEdit;

            SetNameText(true);

            lineEdit.CaretColumn = lineEdit.Text.Length;
            lineEdit.GrabFocus();
            Globals.Instance.EditedNode = lineEdit;
        }
    }


    public void SetNameText(bool show = false)
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
