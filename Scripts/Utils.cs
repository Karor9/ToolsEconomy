using System;
using System.Formats.Tar;
using System.Linq;
using Godot;

public static class Utils
{
    public static void Print(string message)
    {
        GD.Print(message);
    }

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
        if(Globals.Instance.Goods.Count <= 0)
            return 0;
        return Globals.Instance.Goods.Keys.Max() + 1;
    }

    public static void AddToGoodsArray(int id, Goods goods)
    {
        if(!Globals.Instance.Goods.ContainsKey(id))
            Globals.Instance.Goods.Add(id, goods);
        //TBD
    }

    public static string FormatNumbers(float val)
    {
        return string.Format("{0:0.000}",Math.Round(val, 3));
    }
    public static string FormatNumbers(double val)
    {
        return string.Format("{0:0.000}",Math.Round(val, 3));
    }
    public static string FormatNumbers(int val)
    {
        return string.Format("{0:0.000}",Math.Round((decimal)val, 3));
    }

    public static string FormatPercentage(double val)
    {
        return string.Format("{0:0.00}%", Math.Round(val, 2));
    }

    public static Vector2 GetEdgePoints(Panel panel, Panel targetPanel)
    {
        Vector2 target = targetPanel.Position + (targetPanel.Size/2);
        Vector2 center = panel.Position + (panel.Size/2);
        Vector2 dir = (target-center).Normalized();

        float dx = panel.Size.X / 2;
        float dy = panel.Size.Y / 2;

        float px = center.X + dx * Mathf.Sign(dir.X);
        float py = center.Y + dx * Mathf.Sign(dir.Y);

        float tx = dx / Mathf.Abs(dir.X);
        float ty = dy / Mathf.Abs(dir.Y);

        if(tx < ty)
            return center + dir * tx;
        else
            return center + dir * ty;
    }

    // public static Vector2 GetEdgePoint(Panel panel, Vector2 target)
    // {
    //     Vector2 center = panel.Position + (panel.Size/2);
    //     Vector2 dir = (target-center).Normalized();

    //     float dx = panel.Size.X / 2;
    //     float dy = panel.Size.Y / 2;

    //     float px = center.X + dx * Mathf.Sign(dir.X);
    //     float py = center.Y + dx * Mathf.Sign(dir.Y);

    //     float tx = dx / Mathf.Abs(dir.X);
    //     float ty = dy / Mathf.Abs(dir.Y);

    //     if(tx < ty)
    //         return center + dir * tx;
    //     else
    //         return center + dir * ty;
    // }

    public static void DrawArrow(Line2D line, Vector2 tail, Vector2 tip)
    {

        float angleCorr = 0.4f;
        float arrowSize = 10f;
        float angle = (tail-tip).Angle();

        Vector2 leftWing = tip + new Vector2(Mathf.Cos(angle + angleCorr), Mathf.Sin(angle + angleCorr)) * arrowSize;
        Vector2 rightWing = tip + new Vector2(Mathf.Cos(angle - angleCorr), Mathf.Sin(angle - angleCorr)) * arrowSize;

        line.AddPoint(leftWing);
        line.AddPoint(rightWing);
        line.AddPoint(tip);
    }

    public static void RedrawArrow(Panel p1, Panel p2, Line2D line2D)
    {
        Vector2 tail = GetEdgePoints(p1, p2);
        Vector2 tip = GetEdgePoints(p2, p1);
        line2D.Points = [tail, tip];
        ChanceController node = (ChanceController)line2D.GetChild(0);
        CalculateChangeBlockPos(line2D, node);
        DrawArrow(line2D, tail, tip);
    }

    public static void SpawnChangeBlock(Line2D line)
    {
        ChanceController node = (ChanceController)Globals.Instance.ChangeBlock.Instantiate();
        CalculateChangeBlockPos(line, node);
        line.AddChild(node);
    }

    static void CalculateChangeBlockPos(Line2D line, ChanceController node)
    {
        node.Position = (line.Points[0] + line.Points[1]) / 2;
        node.Position = new Vector2(node.Position.X, node.Position.Y-node.Size.Y/2);
    }

}