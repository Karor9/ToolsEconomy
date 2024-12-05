using Godot;
using Godot.Collections;

public partial class NodeProp : Panel
{
    [Export] public Array<int> Parents = new Array<int>();
    [Export] public Array<int> Childs = new Array<int>();

    public int ID;

    public override void _Ready()
    {
        Button b = GetChild(1) as Button;
        b.Pressed += () => OnClick();
    }

    void OnClick()
    {
        if(Globals.Instance.ClickedId == -1)
        {
            Globals.Instance.ClickedId = ID;
        } else if (Globals.Instance.ClickedId > -1)
        {
            Parents.Add(Globals.Instance.ClickedId);
            Globals.Instance.Nodes[Globals.Instance.ClickedId].Childs.Add(ID);

            Node node = Globals.Instance.Edge.Instantiate();
            Vector2 s = Globals.Instance.Nodes[Globals.Instance.ClickedId].Position + (Globals.Instance.Nodes[Globals.Instance.ClickedId].Size / 2);
            Vector2 e = Position + (Size/2);
            Line2D line = node as Line2D;
            line.Points = new Vector2[] {s, e};
            Globals.Instance.EdgesContainer.AddChild(line);
            Globals.Instance.ClickedId = -1;
        }
    }
}
