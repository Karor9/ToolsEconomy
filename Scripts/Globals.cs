using Godot;
using Godot.Collections;

public partial class Globals : Node
{
    //Exported scenes - nodes, generators, lines;
    [Export] public PackedScene Node;


    [Export] public Control Nodes;

    //Resources
    [Export] Array<int> Goods = new Array<int>();

    public static Globals Instance {get; private set;}
    public Enums.ToolState CurrentState;

    public int ClickedId = -1;
    public bool IsObstructed = false;

    public override void _Ready()
    {
        Instance = this;
    }

    public void SetNextID(int id)
    {
        Goods.Add(id);
    }

}