using Godot;
using Godot.Collections;
using System;
using System.Linq;

public partial class ElementController : NodeController
{
    [Export] Control[] LineEdits;
    [Export] public Array<Line2D> InLine;
    [Export] public Array<Line2D> OutLine;

    void Pressed(InputEvent @event, int id)
    {
        if(@event.IsActionPressed("LMB") && Globals.Instance.CurrentToolState == Enums.ToolState.EditingNode)
        {
            LineEdit le = (LineEdit)Globals.Instance.CurrFocus;
            if(le is not null)
            {
                ElementController ec = (ElementController)le.GetParent().GetParent();
                ec.LostFocus();
            }

            LineEdits[id].Visible = true;
            le = (LineEdit)LineEdits[id].GetChild(0);
            le.GrabFocus();
            le.CaretColumn = le.Text.Length;
            Globals.Instance.CurrFocus = le;
        }
        if(@event.IsActionPressed("LMB") &&
        Globals.Instance.CurrentToolState == Enums.ToolState.MoveNode)
        {
            Globals.Instance.CurrFocus = this;
        }

        if(Input.IsActionPressed("LMB")
        && Globals.Instance.CurrentToolState == Enums.ToolState.AddingGenerator
        && Globals.Instance.CurrFocus != null)
        {
            GeneratorController gc = (GeneratorController)Globals.Instance.CurrFocus;
            if(gc is null)
            {
                Utils.Print("red", "Generator IS NULL");
                return;
            }
            Node node = gc.GetChild(6);

            Line2D line2D = (Line2D)Globals.Instance.Arrow.Instantiate();
            // Vector2 fp = gc.Position + (gc.Size/2);
            // Vector2 ep = Position + (Size/2);
            Vector2 fp = Utils.GetEdgePoint(gc, this);
            Vector2 ep = Utils.GetEdgePoint(this, gc);
            line2D.Points = [fp, ep];
            Utils.DrawArrow(line2D, fp, ep);
            line2D.Name = Name;



            NodePath nodePath = new NodePath(Name);
            if(node.GetNodeOrNull(nodePath) is not null)
                return;
            node.AddChild(line2D);
            InLine.Add(line2D);
            gc.LostFocusPanel();
            Utils.Print("green", "Connect");
            //TBD
        }
    }

    public override void _Input(InputEvent @event)
    {
        if(@event.IsActionPressed("AcceptTextEdit") 
        && Globals.Instance.CurrentToolState == Enums.ToolState.EditingNode
        && Globals.Instance.CurrFocus != null)
        {
            if(Globals.Instance.CurrFocus.GetParent().GetParent() != this)
                return;
            LineEdit le = (LineEdit)Globals.Instance.CurrFocus;
            
            if(le.Name == "NameInput")
                ((NameInputController)le).SaveEdits(this);
            else if(le.Name == "CountInput")
                ((CountInputController)le).SaveEdits(this);

        }

        if(Input.IsActionPressed("LMB") 
        && Globals.Instance.CurrentToolState == Enums.ToolState.MoveNode
        && Globals.Instance.CurrFocus == this)
        {
            SelfModulate = new Color(1, 1, 1, 0.5f);
        }

        if(@event.IsActionReleased("LMB") 
        && Globals.Instance.CurrentToolState == Enums.ToolState.MoveNode
        && Globals.Instance.CurrFocus == this)
        {
            SelfModulate = new Color(1,1,1,1);
            Globals.Instance.CurrFocus = null;
        }
    }

    public override void _PhysicsProcess(double delta)
    {
        if(Input.IsActionPressed("LMB") 
        && Globals.Instance.CurrentToolState == Enums.ToolState.MoveNode
        && Globals.Instance.CurrFocus == this)
        {
            MoveNode();
        }
    }

    public override void MoveNode()
    {
        base.MoveNode();
    }

    public override void LostFocus()
    {
        base.LostFocus();
    }


    void IsObstructed(bool obs)
    {
        Globals.Instance.Obstructed = obs;
        Utils.Print("pink", "Mouse obstructed:" + Globals.Instance.Obstructed);
    }

}
