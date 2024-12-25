using Godot;

public static class Utils
{
    public static void Obstructed(bool obstructed)
    {
        Globals.Instance.IsObstructed = obstructed;
    }
}