using Godot;

public partial class NodeController : Panel
{
    public virtual void MoveNode()
    {
        Control control = Globals.Instance.CurrFocus;
        if(control is null)
            return;
        Vector2 newPos = GetGlobalMousePosition();
        control.Position = newPos - (control.Size/2);
    }

    public virtual void LostFocus()
    {
        Control item = Globals.Instance.CurrFocus;
        if(item is null)
            return;
        // Control parent = (Control)item.GetParent().GetParent();
        ((Control)item.GetParent()).Visible = false;
        ((LineEdit)item).ReleaseFocus();
        Globals.Instance.CurrFocus = null;
    }
}