using Godot;
using Godot.Collections;

public partial class Globals : Node
{
    //Exported scenes - nodes, generators, lines;
    [Export] public PackedScene Node;
    [Export] public PackedScene Generator;
    [Export] public PackedScene Edge;


    [Export] public Control Nodes;
    [Export] public Control Edges;

    //Resources
    [Export] Array<int> Goods = new Array<int>();

    public static Globals Instance {get; private set;}
    public Enums.ToolState CurrentState;

    [Export] public int ClickedId = -1;
    public bool IsObstructed = false;
    public GeneratorNode LastGenerator = null;

    public override void _Ready()
    {
        Instance = this;
    }

    public void SetNextID(int id)
    {
        Goods.Add(id);
    }

    public Node GetNode(int name)
    {
        return Nodes.GetNode(new NodePath(name.ToString()));
    }

    public Node GetEdge(int name)
    {
        return Edges.GetNode(new NodePath(name.ToString()));
    }

}