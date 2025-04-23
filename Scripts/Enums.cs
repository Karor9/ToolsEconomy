using Godot;
using System;

public static partial class Enums
{
    public enum ToolState
    {
        None, //0
        AddingNode, //1
        EditingNode, //2
        AddingLine, //3
        MoveNode, //4
        EditingLine, //5
        AddingGenerator, //6
        AddingCrafting
    }
}