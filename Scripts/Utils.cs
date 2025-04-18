using System;
using System.Linq;
using Godot;

public static class Utils
{
    public static void Print(string color, string message)
    {
        GD.PrintRich("[color="+color+"]"+message);
    }

    public static void Print(string color, Variant message)
    {
        GD.PrintRich("[color="+color+"]"+message);
    }

    public static int GetNextID()
    {
        if(Globals.Instace.Goods.Count <= 0)
            return 0;
        return Globals.Instace.Goods.Keys.Max() + 1;
    }

    public static void AddToGoodsArray(int id, Goods goods)
    {
        if(!Globals.Instace.Goods.ContainsKey(id))
            Globals.Instace.Goods.Add(id, goods);
        //TBD
    }
}