using Godot;
using System;
using System.Diagnostics.Metrics;
using System.Linq;

public partial class GeneratorController : NodeController
{
    [Export] TextureRect Dot;
    [Export] Control[] LineEdits;
    [Export] RichTextLabel CountText;
    [Export] Node LineParent;
    void ShowGrabbedFocus(bool vis, Color color)
    {
        Globals.Instance.CurrFocus = this;
        Dot.Visible = vis;
        Dot.SelfModulate = color;
    }

    void Pressed(InputEvent @event, int id)
    {
        if (@event.IsActionPressed("LMB") && Globals.Instance.CurrentToolState == Enums.ToolState.EditingNode)
        {
            if (Globals.Instance.CurrFocus is LineEdit le)
            {
                var parent = le.GetParent();
                if (parent != null && parent.GetParent() is GeneratorController gc)
                {
                    gc.LostFocus();
                } else if (parent != null && parent.GetParent() is ElementController ec)
                {
                    ec.LostFocus();
                }
            }

            LineEdits[id].Visible = true;

            if (LineEdits[id].GetChild(0) is LineEdit newLe)
            {
                newLe.GrabFocus();
                newLe.CaretColumn = newLe.Text.Length;
                Globals.Instance.CurrFocus = newLe;
            }
        }

        // if (@event.IsActionPressed("LMB") && Globals.Instance.CurrentToolState == Enums.ToolState.EditingNode)
        // {
        //     if (Globals.Instance.CurrFocus is LineEdit previousLe)
        //     {
        //         if (previousLe.GetParent()?.GetParent() is GeneratorController ec)
        //         {
        //             ec.LostFocus();
        //         }
        //     }

        //     LineEdit le = LineEdit; 
        //     le.GrabFocus();
        //     le.CaretColumn = le.Text.Length;
        //     ((Control)le.GetParent()).Visible = true;
        //     Globals.Instance.CurrFocus = le;
        // }
        if(@event.IsActionPressed("LMB") &&
        Globals.Instance.CurrentToolState == Enums.ToolState.MoveNode)
        {
            Globals.Instance.CurrFocus = this;
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
            // Globals.Instance.CurrFocus = null;
            LostFocusPanel();
        }
    }

    public override void LostFocus()
    {
        base.LostFocus();
        ((TextureRect)GetChild(5)).Visible = false;
    }

    public void LostFocusPanel()
    {
        Control item = Globals.Instance.CurrFocus;
        item.ReleaseFocus();
        Globals.Instance.CurrFocus = null;
        if(GetChild(6).GetChildCount() <= 0)
            QueueFree();
        ((TextureRect)GetChild(5)).Visible = false;
        SaveEdits();
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
        foreach (Node item in LineParent.GetChildren())
        {
            Line2D line = (Line2D)item;
            int id = int.Parse(line.Name);
            Panel element = (Panel)Globals.Instance.Goods[id].Element;
            Utils.RedrawArrow(this, element, line);
        }
    }

    public void SaveEdits()
    {
        if(!((Control)GetChild(4)).Visible)
            return;
        Node countText = GetChild(4).GetChild(0);
        
        if(countText is CountInputController cic)
        {
            cic.SaveEdits(this);
        }
    }

    public double GetValueToAdd()
    {
        return double.Parse(CountText.Text);
    }


}
