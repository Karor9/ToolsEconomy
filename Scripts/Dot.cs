using Godot;
using System;

public partial class Dot : CharacterBody2D
{
    Vector2 start;
    Vector2 end;
    Vector2 chancePoint;
    bool? chancePassed = null;
    float baseSpeed = 2f;
    float minDist = 10f;
    float totalDistance;
    float speed = 0f;
    ChanceController myChance;
    int goodsIdDest;
    double valueToAdd;
    bool chanceBlockActive;

    public void Init(Vector2 s, Vector2 e, Line2D line, double add = 0, Color? color = null, bool chanceBlock = true)
    {
        start = s;
        end = e;
        chanceBlockActive = chanceBlock;
        if(chanceBlockActive)
            myChance = (ChanceController)line.GetChild(1);
        goodsIdDest = int.Parse(line.Name);
        valueToAdd = add;
        if(color != null)
            ((Sprite2D)GetChild(1)).SelfModulate = (Color)color;
    }

    public override void _Ready()
    {
        Position = start;
        chancePoint = (start + end) / 2;

        totalDistance = start.DistanceTo(chancePoint) + chancePoint.DistanceTo(end);
        speed = totalDistance * baseSpeed;
    }

    public override void _PhysicsProcess(double delta)
    {
        if(chanceBlockActive)
        {
            if(chancePassed is null)
            {
                Vector2 dir = (chancePoint - Position).Normalized();
                Velocity = dir * speed;
                MoveAndSlide();
                if(Position.DistanceTo(chancePoint) < minDist)
                    chancePassed = GD.Randf() <= myChance.GetChance();
            } else if ((bool)chancePassed)
            {
                Vector2 dir = (end - Position).Normalized();
                Velocity = dir * speed;
                MoveAndSlide();

                if(Position.DistanceTo(end) < minDist)
                {
                    if(valueToAdd != 0)
                        Globals.Instance.Goods[goodsIdDest].AddValue(valueToAdd);

                    QueueFree();
                }
                    
            } else
            {
                QueueFree();
            }
        } else
        {
            Vector2 dir = (end - Position).Normalized();
            Velocity = dir * speed * 2;
            MoveAndSlide();
            if(Position.DistanceTo(end) < minDist)
                QueueFree();
        }
    } 



}
