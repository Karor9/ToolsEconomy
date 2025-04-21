using Godot;
using Godot.Collections;
using static Enums;

public partial class Globals : Node
{
    public static Globals Instance = null;
    public ToolState CurrentToolState = ToolState.None;
    public bool Obstructed = false;
    [Export] public Control CurrFocus = null;
    [Export] public Dictionary<int, Goods> Goods = new Dictionary<int, Goods>();
    

    [ExportGroup("Scenes")]
    [Export] public PackedScene Arrow;
    public override void _Ready()
    {
        Instance = this;
    }

}
