using Godot;
using Godot.Collections;
using System;
using System.Linq;

public partial class ElementController : NodeController
{
    [Export] Control[] LineEdits;
    [Export] public Array<Arrow> InLine;
    [Export] public Array<Arrow> OutLine;

    [Export] RichTextLabel GoodsName;
    [Export] RichTextLabel GoodsCount;


    void Pressed(InputEvent @event, int id)
    {
        if(@event.IsActionPressed("LMB") && Globals.Instance.CurrentToolState == Enums.ToolState.EditingNode)
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
            GD.Print(id);
            LineEdits[id].Visible = true;

            if (LineEdits[id].GetChild(0) is LineEdit newLe)
            {
                newLe.GrabFocus();
                newLe.CaretColumn = newLe.Text.Length;
                Globals.Instance.CurrFocus = newLe;
            }
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

            Arrow line2D = (Arrow)Globals.Instance.Arrow.Instantiate();
            // Vector2 fp = gc.Position + (gc.Size/2);
            // Vector2 ep = Position + (Size/2);
            Vector2 fp = Utils.GetEdgePoints(gc, this);
            Vector2 ep = Utils.GetEdgePoints(this, gc);
            line2D.Points = [fp, ep];
            line2D.Parent = gc;
            line2D.Child = this;

            Utils.SpawnChangeBlock(line2D);
            Utils.DrawArrow(line2D, fp, ep);
            line2D.Name = Name;



            NodePath nodePath = new NodePath(Name);
            if(node.GetNodeOrNull(nodePath) is not null)
                return;
            node.AddChild(line2D);

            InLine.Add(line2D);
            gc.LostFocusPanel();
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
            if(Globals.Instance.CurrFocus is LineEdit le)
            {
                if(le.Name == "NameInput")
                    ((NameInputController)le).SaveEdits(this);
                else if(le.Name == "CountInput")
                    ((CountInputController)le).SaveEdits(this);
            } else
            {
                ReleaseFocus();
                Globals.Instance.CurrFocus = null;
            }
        }
    }

    //POST SELF CODE REVIEW

    void IsObstructed(bool obs)
    {
        Globals.Instance.Obstructed = obs;
        Utils.Print("pink", "Mouse obstructed:" + Globals.Instance.Obstructed);
    }

    // public override void LostFocus()
    // {
    //     base.LostFocus();
    // }

    public override void MoveNode()
    {
        base.MoveNode();
        foreach (Node item in this.InLine)
        {
            Line2D line = (Line2D)item;
            Panel element = (Panel)line.GetParent().GetParent();
            Utils.RedrawArrow(element, this, line);
        }
        foreach (Node item in this.OutLine)
        {
            Line2D line = (Line2D)item;
            Panel element = (Panel)line.GetParent().GetParent();
            Utils.RedrawArrow(this, element, line);
        }
    }

    
    public void UpdateText()
    {
        int id = int.Parse(Name);
        GoodsName.Text = Globals.Instance.Goods[id].Name;
        GoodsCount.Text = Utils.FormatNumbers(Globals.Instance.Goods[id].Count);
    }
}
