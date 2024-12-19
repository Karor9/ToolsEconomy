using Godot;
using Godot.Collections;

public partial class Globals : Node
{
    public enum ToolState
    {
        None,
        AddingNode,
        EditingNode,
        AddingLine,
        MoveNode,
        EditingLine,
        AddingGenerator
    }
    public static Globals Instance {get; private set;}
    public Array<NodeProp> Nodes = new Array<NodeProp>();
    public Array<LineProp> Lines = new Array<LineProp>();
    public int ClickedId = -1;
    public ToolState CurrentState;
    public Control EditedNode = null;

    [Export] public PackedScene Edge;
    [Export] public Node EdgesContainer;

    public override void _Ready()
    {
        Instance = this;
    }


}
