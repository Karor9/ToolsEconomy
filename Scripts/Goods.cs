using Godot;

public partial class Goods : Resource
{
    [Export] public string Name;
    [Export] public double Count;
    public Control Element;

    public Goods(string name, double count, Control control)
    {
        Name = name;
        Count = count;
        Element = control;
    }

    public Goods() : this("EmptyName", 0d, null) {}
}