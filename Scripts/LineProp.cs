using Godot;
using System;

public partial class LineProp : Line2D
{
    double val = 1d;

    public override void _Process(double delta)
    {
        if(Input.IsActionJustPressed("LMB"))
            if(Points.Length > 0)
                {
                    float pos1 = Points[0].AngleToPoint(Points[1]) - Points[0].AngleToPoint(GetGlobalMousePosition());
                    float pos2 = Points[1].AngleToPoint(Points[0]) - Points[1].AngleToPoint(GetGlobalMousePosition());
                    if(pos1 > -0.02f && pos1 < 0.02f && pos2 > -0.02f && pos2 < 0.02f)
                        GD.Print("linia");
                }

                // GD.Print(Points[0].AngleToPoint(Points[1]) - Points[0].AngleToPoint(GetGlobalMousePosition()));
                // if(Points[0].AngleToPoint(Points[1]) + 0.1f >= Points[0].AngleToPoint(GetGlobalMousePosition()) &&
                // Points[0].AngleToPoint(Points[1]) - 0.1f >= Points[0].AngleToPoint(GetGlobalMousePosition()))
                //     GD.Print("Linia");
            // GD.Print(Points[0].AngleToPoint(Points[1]));
            // GD.Print(Points[1].AngleToPoint(Points[0]));
            // GD.Print(Points[0].AngleToPoint(GetGlobalMousePosition()));

    }
}
