using System;
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
    [Export] public Camera2D Camera;

    [ExportGroup("Scenes")]
    [Export] public PackedScene Arrow;
    [Export] public PackedScene ChangeBlock;
    [Export] public PackedScene Dot;
    [Export] public PackedScene Crafting;

    [ExportGroup("Elements")]
    [Export] public VBoxContainer CraftingInputContainer;
    

    public override void _Ready()
    {
        Instance = this;
    }
}
