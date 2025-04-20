using Godot;
using Godot.Collections;
using static Enums;

public partial class UIController : Control
{
    [ExportGroup("UIElements")]
    [Export] Panel UIPanel;
    [Export] Panel SecondaryPanel;
    [Export] RichTextLabel CurrentState;

    [ExportGroup("OpenCloseButtons")]
    [Export] Button openButton;
    [Export] Button closeButton;
    [Export] Array<Control> CategoryButtons = new Array<Control>();

    public override void _Ready()
    {
        SetState();
    }

    void SetState()
    {
        CurrentState.Text = Globals.Instace.CurrentToolState.ToString();
    }


    void OpenClose(bool open)
    {
        openButton.Visible = !open;
        closeButton.Visible = open;
        UIPanel.Visible = open;
    }

    void OpenSecondary(int category)
    {
        SecondaryPanel.Visible = true;
        for (int i = 0; i < CategoryButtons.Count; i++)
        {
            if(category == i)
                CategoryButtons[i].Visible = true;
            else
                CategoryButtons[i].Visible = false;
        }
    }

    void CloseSecondary()
    {
        SecondaryPanel.Visible = false;
    }

    void SetToolState(int state)
    {
        Globals.Instace.CurrentToolState = (ToolState)state;
        Utils.Print("#008000", Globals.Instace.CurrentToolState.ToString());
        GD.PrintRich("[color=#]"+ Globals.Instace.CurrentToolState);
        CloseSecondary();
        SetState();
        LostFocusOnAction();
    }

    void LostFocusOnAction()
    {
        if(Globals.Instace.CurrFocus is null)
            return;
        LineEdit le = (LineEdit)Globals.Instace.CurrFocus; 
        if(le.Name == "NameInput")
            ((NameInputController)le).SaveEdits((ElementController)le.GetParent().GetParent());
        else if(le.Name == "CountInput")
            ((CountInputController)le).SaveEdits((ElementController)le.GetParent().GetParent());
    }

    void IsObstructed(bool state)
    {
        Globals.Instace.Obstructed = state;
        Utils.Print("pink", "Mouse obstructed:" + Globals.Instace.Obstructed);
    }
}
