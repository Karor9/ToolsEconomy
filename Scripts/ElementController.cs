using Godot;
using System;
using System.Linq;

public partial class ElementController : Panel
{
    [Export] Control[] LineEdits;

    void Pressed(InputEvent @event, int id)
    {
        if(@event.IsActionPressed("LMB") && Globals.Instace.CurrentToolState == Enums.ToolState.EditingNode)
        {
            LineEdit le = (LineEdit)Globals.Instace.CurrFocus;
            if(le is not null)
            {
                ElementController ec = (ElementController)le.GetParent().GetParent();
                ec.LostFocus();
            }

            LineEdits[id].Visible = true;
            le = (LineEdit)LineEdits[id].GetChild(0);
            le.GrabFocus();
            le.CaretColumn = le.Text.Length;
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
            
            if(le.Name == "NameInput")
                ((NameInputController)le).SaveEdits(this);
            else if(le.Name == "CountInput")
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

    public override void _PhysicsProcess(double delta)
    {
        if(Input.IsActionPressed("LMB") 
        && Globals.Instace.CurrentToolState == Enums.ToolState.MoveNode
        && Globals.Instace.CurrFocus == this)
        {
            MoveNode();
        }
    }


    void MoveNode()
    {
        Control control = Globals.Instace.CurrFocus;
        if(control is null)
            return;
        Vector2 newPos = GetGlobalMousePosition();
        control.Position = newPos - (control.Size/2);
    }



    public void LostFocus()
    {
        Control item = Globals.Instace.CurrFocus;
        // Control parent = (Control)item.GetParent().GetParent();
        ((Control)item.GetParent()).Visible = false;
        ((LineEdit)item).ReleaseFocus();
        Globals.Instace.CurrFocus = null;
    }

    void IsObstructed(bool obs)
    {
        Globals.Instace.Obstructed = obs;
        Utils.Print("pink", "Mouse obstructed:" + Globals.Instace.Obstructed);
    }

}
