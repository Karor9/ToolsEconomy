using Godot;
using Godot.Collections;
using static Enums;

public partial class Globals : Node
{
    public static Globals Instace = null;
    public ToolState CurrentToolState = ToolState.None;
    public bool Obstructed = false;
    
    [Export] public Dictionary<int, Goods> Goods = new Dictionary<int, Goods>();

    public override void _Ready()
    {
        Instace = this;
    }

}
