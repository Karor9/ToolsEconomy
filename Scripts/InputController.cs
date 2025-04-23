using Godot;


public partial class InputController : LineEdit
{
    protected Goods g;
    protected string newValue;
    public virtual void SaveEdits(ElementController ec)
    {
        newValue = Text;
        int id = int.Parse(ec.Name);
        g = Globals.Instance.Goods[id];
        ec.LostFocus();
    }

    public virtual void SaveEdits(GeneratorController ec)
    {
        newValue = Text;
        ec.LostFocus();
    }
}