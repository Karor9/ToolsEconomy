using Godot;


public partial class InputController : LineEdit
{
    protected Goods g;
    protected string newValue;
    public virtual void SaveEdits(ElementController ec)
    {
        Utils.Print("yellow", Name);
        newValue = Text;
        int id = int.Parse(ec.Name);
        g = Globals.Instace.Goods[id];
        ec.LostFocus();
    }

    public virtual void SaveEdits(GeneratorController ec)
    {
        Utils.Print("yellow", Name);
        newValue = Text;
        ec.LostFocus();
    }
}