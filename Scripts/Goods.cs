using Godot;

public partial class Goods : Resource
{
    public string Name;
    public double Count;

    public Goods(string name, double count)
    {
        Name = name;
        Count = count;
    }

    public Goods() : this("EmptyName", 0d) {}
}