using Godot;
using System;

public partial class Arrow : Line2D
{
    public Panel Parent;
    public Panel Child;

    public override void _ExitTree()
    {
        ClearElementConnections(Parent);
        ClearElementConnections(Child);

    }

    void ClearElementConnections(object obj)
    {
        if (obj is ElementController ec)
        {
            Utils.RemoveNulls(ec.InLine);
            Utils.RemoveNulls(ec.OutLine);
        }

        if(obj is CraftingController cc)
        {
            Utils.RemoveNulls(cc.InLine);
            Utils.RemoveNulls(cc.OutLine);
        }
    }

}
