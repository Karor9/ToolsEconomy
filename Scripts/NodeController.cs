using Godot;

public partial class NodeController : Panel
{
    public virtual void MoveNode()
    {
        Control control = Globals.Instace.CurrFocus;
        if(control is null)
            return;
        Vector2 newPos = GetGlobalMousePosition();
        control.Position = newPos - (control.Size/2);
    }

    public virtual void LostFocus()
    {
        Control item = Globals.Instace.CurrFocus;
        // Control parent = (Control)item.GetParent().GetParent();
        ((Control)item.GetParent()).Visible = false;
        ((LineEdit)item).ReleaseFocus();
        Globals.Instace.CurrFocus = null;
    }
}