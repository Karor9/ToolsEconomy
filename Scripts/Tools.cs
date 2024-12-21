using System.Threading;
using Godot;

public partial class Tools : Control
{
    [Export] PackedScene Node;
    [Export] PackedScene Generator;
    [Export] Control Generators;
    public override void _Input(InputEvent @event)
    {
        if(@event.IsActionPressed("LMB"))
        {
            switch(Globals.Instance.CurrentState)
            {
                case Globals.ToolState.AddingNode:
                    if(Globals.Instance.IsObstructed)
                        break;
                    Node node = Node.Instantiate();
                    NodeProp n = node as NodeProp;
                    n.ID = GetChildren().Count;
                    n.Text = "New Resource";
                    Vector2 size = n.Size;
                    n.Position = GetGlobalMousePosition() - (size/2);
                    AddChild(n);
                    Globals.Instance.Nodes.Add(n);
                    break;
                case Globals.ToolState.EditingNode:
                case Globals.ToolState.EditingLine:
                    if(Globals.Instance.EditedNode == null)
                        break;
                    Globals.Instance.EditedNode.ReleaseFocus();
                    Globals.Instance.EditedNode = null;
                    break;
                // case Globals.ToolState.MoveNode:
                //     GD.Print("Start");
                //     break;
                case Globals.ToolState.AddingGenerator:
                    if(Globals.Instance.IsObstructed)
                        break;
                    if(Globals.Instance.ClickedId < 0 && Globals.Instance.FreshGenerator == null)
                    {
                        Node gen = Generator.Instantiate();
                        Generator g = gen as Generator;
                        g.Text = "New Generator";
                        g.ID = Generators.GetChildren().Count;
                        Vector2 genSize = g.Size;
                        g.Position = GetGlobalMousePosition() - (genSize/2);
                        Globals.Instance.FreshGenerator = g;
                        Generators.AddChild(g);
                        break;
                    }
                    else if(Globals.Instance.ClickedId >= 0 && Globals.Instance.FreshGenerator == null)
                    {
                        Node gen = Generator.Instantiate();
                        Generator g = gen as Generator;
                        g.Text = "New Generator";
                        g.ID = Generators.GetChildren().Count;
                        Vector2 genSize = g.Size;
                        g.Position = GetGlobalMousePosition() - (genSize/2);

                        Line2D line2D = new Line2D();
                        line2D.DefaultColor = Colors.Blue;
                        line2D.Points = new Vector2[] {g.Position + (g.Size/2), Globals.Instance.Nodes[Globals.Instance.ClickedId].Position + (Globals.Instance.Nodes[Globals.Instance.ClickedId].Size/2)};
                        Globals.Instance.EdgesGeneratorContainer.AddChild(line2D);

                        Generators.AddChild(g);
                    }
                    break;
            }
        }
        
        if(@event.IsActionReleased("LMB"))
        {
            switch(Globals.Instance.CurrentState)
            {
                case Globals.ToolState.MoveNode:
                    if(Globals.Instance.ClickedId < 0)
                        break;
                    Vector2 NewPos = GetGlobalMousePosition();
                    Control c = Globals.Instance.Nodes[Globals.Instance.ClickedId];
                    c.Modulate = new Color(c.Modulate.R, c.Modulate.G, c.Modulate.B, 1f);
                    c.Position = NewPos - (c.Size/2);
                    Globals.Instance.ClickedId = -1;
                    break;
            }
        }
    }

    public override void _PhysicsProcess(double delta)
    {
        if(Input.IsMouseButtonPressed(MouseButton.Left) && Globals.Instance.CurrentState == Globals.ToolState.MoveNode)
        {
            if(Globals.Instance.ClickedId >= 0 && Globals.Instance.Nodes.Count > Globals.Instance.ClickedId)
            {
                NodeProp c = Globals.Instance.Nodes[Globals.Instance.ClickedId];
                Vector2 newPos = GetGlobalMousePosition() - (c.Size/2);
                c.Position = newPos;
                c.Modulate = new Color(c.Modulate.R, c.Modulate.G, c.Modulate.B, 0.5f);

                foreach (int item in c.LinesStart)
                {
                    LineProp l = Globals.Instance.Lines[item];
                    l.SetPointPosition(0, c.Position + (c.Size/2));
                    RichTextLabel t = l.GetChild(0) as RichTextLabel;
                    t.Position = (l.Points[0] + l.Points[1])/2 - (t.Size/20);
                }

                foreach (int item in c.LinesStop)
                {
                    LineProp l = Globals.Instance.Lines[item];
                    l.SetPointPosition(1, c.Position + (c.Size/2));
                    RichTextLabel t = l.GetChild(0) as RichTextLabel;
                    t.Position = (l.Points[0] + l.Points[1])/2 - (t.Size/20);
                }
            }
        }
    }
}
