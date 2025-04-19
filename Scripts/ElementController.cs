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
        && Globals.Instace.CurrFocus.GetParent().GetParent() == this)
        {
            LineEdit le = (LineEdit)Globals.Instace.CurrFocus;
            Utils.Print("yellow", le.Name);
            string newValue = le.Text;
            int id = int.Parse(Name);
            Goods g = Globals.Instace.Goods[id];
            ((ElementController)le.GetParent().GetParent()).LostFocus();
            if(le.Name == "NameInput")
            {
                g.Name = newValue;
                ((RichTextLabel)GetChild(0)).Text = g.Name;
            } else if(le.Name == "CountInput")
            {
                if(double.TryParse(newValue, out double parsed))
                    g.Count = parsed;
                ((RichTextLabel)GetChild(1)).Text = Utils.FormatNumbers(g.Count);
            }
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



    void LostFocus()
    {
        Control item = Globals.Instace.CurrFocus;
        // Control parent = (Control)item.GetParent().GetParent();
        ((Control)item.GetParent()).Visible = false;
        ((LineEdit)item).ReleaseFocus();
        Globals.Instace.CurrFocus = null;
    }

}
