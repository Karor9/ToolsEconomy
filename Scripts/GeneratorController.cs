using Godot;
using System;

public partial class GeneratorController : NodeController
{
    [Export] TextureRect Dot;
    [Export] public LineEdit LineEdit;
    void ShowGrabbedFocus(bool vis, Color color)
    {
        Globals.Instace.CurrFocus = this;
        Dot.Visible = vis;
        Dot.SelfModulate = color;
    }

    void Pressed(InputEvent @event)
    {
        if(@event.IsActionPressed("LMB") && Globals.Instace.CurrentToolState == Enums.ToolState.EditingNode)
        {
            LineEdit le = (LineEdit)Globals.Instace.CurrFocus;
            if(le is not null)
            {
                GeneratorController ec = (GeneratorController)le.GetParent().GetParent();
                ec.LostFocus();
            }
            le = LineEdit;
            le.GrabFocus();
            le.CaretColumn = le.Text.Length;
            ((Control)le.GetParent()).Visible = true;
            Globals.Instace.CurrFocus = le;
        }
        if(@event.IsActionPressed("LMB") &&
        Globals.Instace.CurrentToolState == Enums.ToolState.MoveNode)
        {
            Globals.Instace.CurrFocus = this;
        }
    }

    public override void _Input(InputEvent @event)
    {
        if(@event.IsActionPressed("AcceptTextEdit") 
        && Globals.Instace.CurrentToolState == Enums.ToolState.EditingNode
        && Globals.Instace.CurrFocus != null)
        {
            if(Globals.Instace.CurrFocus.GetParent().GetParent() != this)
                return;
            LineEdit le = (LineEdit)Globals.Instace.CurrFocus;
            
            Utils.Print("yellow", le);
            ((CountInputController)le).SaveEdits(this);

        }

        if(Input.IsActionPressed("LMB") 
        && Globals.Instace.CurrentToolState == Enums.ToolState.MoveNode
        && Globals.Instace.CurrFocus == this)
        {
            SelfModulate = new Color(1, 1, 1, 0.5f);
        }

        if(@event.IsActionReleased("LMB") 
        && Globals.Instace.CurrentToolState == Enums.ToolState.MoveNode
        && Globals.Instace.CurrFocus == this)
        {
            SelfModulate = new Color(1,1,1,1);
            Globals.Instace.CurrFocus = null;
        }
    }

    public override void LostFocus()
    {
        base.LostFocus();
    }

    public void LostFocusPanel()
    {
        Control item = Globals.Instace.CurrFocus;
        item.ReleaseFocus();
        Globals.Instace.CurrFocus = null;
        ((TextureRect)GetChild(4)).Visible = false;
    }

    public override void _PhysicsProcess(double delta)
    {
        if(Input.IsActionPressed("LMB") 
        && Globals.Instace.CurrentToolState == Enums.ToolState.MoveNode
        && Globals.Instace.CurrFocus == this)
        {
            MoveNode();
        }
    }

    public override void MoveNode()
    {
        base.MoveNode();
    }

}
