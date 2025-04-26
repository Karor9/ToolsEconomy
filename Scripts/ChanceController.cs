using Godot;
using System;
using System.ComponentModel.DataAnnotations;

public partial class ChanceController : Panel
{
    [Export] Button button;
    [Export] RichTextLabel label;
    [Export] SpinBox input;

    void OnClick()
    {
        if(Globals.Instance.CurrentToolState == Enums.ToolState.EditingNode &&
        Globals.Instance.CurrFocus is null)
        {
            Globals.Instance.CurrFocus = this;
            input.GrabFocus();
            input.Visible = true;   
            label.Visible = false;
        }
    }

    public double GetChance()
    {
        return double.Parse(label.Text.Replace("%", ""))/100d;
    }

    public void Save()
    {
        if(Globals.Instance.CurrFocus == this)
            return;
        label.Text = Utils.FormatPercentage(input.Value);
        input.Visible = false;
        label.Visible = true;
        Globals.Instance.CurrFocus = null;
        input.ReleaseFocus();
    }
    public void SaveMouse()
    {
        if(Globals.Instance.Obstructed)
            return;
        label.Text = Utils.FormatPercentage(input.Value);
        input.Visible = false;
        label.Visible = true;
        Globals.Instance.CurrFocus = null;
        input.ReleaseFocus();
    }

    void IsObstructed(bool obs)
    {
        Globals.Instance.Obstructed = obs;
        Utils.Print("pink", "Mouse obstructed:" + Globals.Instance.Obstructed);
    }

    public override void _Input(InputEvent @event)
    {
        if(@event.IsActionPressed("AcceptTextEdit") &&
        Globals.Instance.CurrFocus == this)
        {
            Save();
            input.ReleaseFocus();
        }
        if(@event.IsActionPressed("PickAll") &&
        Globals.Instance.CurrFocus == this)
        {
            LineEdit le = input.GetLineEdit();
            le.GrabFocus();
            le.Select(0, le.Text.Length - 1);
        }

    }

    public override void _Ready()
    {
        label.Text = Utils.FormatPercentage(input.Value);
    }

}
