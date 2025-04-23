using System;
using Godot;
using Godot.Collections;
using static Enums;

public partial class UIController : Control
{
    [ExportGroup("UIElements")]
    [Export] Panel UIPanel;
    [Export] Panel SecondaryPanel;
    [Export] RichTextLabel CurrentState;
    [Export] Camera2D Camera;
    [Export] VBoxContainer CraftingInputContainer;
    [Export] Node CraftingParent;

    [ExportGroup("OpenCloseButtons")]
    [Export] Button openButton;
    [Export] Button closeButton;
    [Export] Array<Control> CategoryButtons = new Array<Control>();

    public override void _Ready()
    {
        SetState();
        Globals.Instance.Camera = Camera;
        Globals.Instance.CraftingInputContainer = CraftingInputContainer;
    }

    void SetState()
    {
        CurrentState.Text = Globals.Instance.CurrentToolState.ToString();
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
        Globals.Instance.CurrentToolState = (ToolState)state;
        if(Globals.Instance.CurrentToolState == ToolState.MoveNode)
            ChangeStateCrafting(false);
        else
            ChangeStateCrafting(true);
        Utils.Print("#008000", Globals.Instance.CurrentToolState.ToString());
        CloseSecondary();
        SetState();
        LostFocusOnAction();
    }

    private void ChangeStateCrafting(bool v)
    {
        foreach (Control item in CraftingParent.GetChildren())
        {
            CraftingController cc = (CraftingController)item;
            cc.MoveButtonSetup(v);
        }
    }


    void LostFocusOnAction()
    {
        if(Globals.Instance.CurrFocus is null)
            return;
        if (Globals.Instance.CurrFocus is LineEdit le && Globals.Instance.CurrFocus is not GeneratorController)
        {
            if (le.Name == "NameInput" && le is NameInputController nameInput)
            {
                nameInput.SaveEdits((ElementController)le.GetParent().GetParent());
            }
            else if (le.Name == "CountInput" && le is CountInputController countInput)
            {
                countInput.SaveEdits((ElementController)le.GetParent().GetParent());
            }
        }
        else if (Globals.Instance.CurrFocus is GeneratorController ge)
        {
            ge.LostFocusPanel();
        } else if(Globals.Instance.CurrFocus is ChanceController sb)
        {
            sb.Save();
        }

    }

    void IsObstructed(bool state)
    {
        Globals.Instance.Obstructed = state;
        Utils.Print("pink", "Mouse obstructed:" + Globals.Instance.Obstructed);
    }
}
