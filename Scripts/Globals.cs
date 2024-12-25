using Godot;
using Godot.Collections;

public partial class Globals : Node
{
    public static Globals Instance {get; private set;}
    public Enums.ToolState CurrentState;

    public int ClickedId = -1;
    public bool IsObstructed;

    public override void _Ready()
    {
        Instance = this;
    }
}