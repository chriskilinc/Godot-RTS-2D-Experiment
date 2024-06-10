using Godot;
using System;
using System.Collections.Generic;
using System.Linq;

public partial class World : Node2D
{
    Node _unitsNode;
    Godot.Collections.Array<Node> _unitsEntities;

    public override void _Ready()
    {
        // Called every time the node is added to the scene.
        _unitsNode = GetNode<Node>("%Units");
        _unitsEntities = _unitsNode.GetChildren();
        GD.Print("Spawned Units: " + _unitsEntities);

        // add event listener for area selected
        GetNode<Camera>("%Camera").Connect("AreaSelected", Callable.From((Camera camera) => OnAreaSelected(camera)));
    }

    public override void _Process(double delta)
    {
        // Called every frame. Delta is the elapsed time since the previous frame.
    }

    public void OnAreaSelected(Camera camera)
    {
        var start = camera._startV;
        var end = camera._endV;

        Godot.Collections.Array<Vector2> area = new Godot.Collections.Array<Vector2>
        {
            new Vector2(Math.Min(start.X, end.X), Math.Min(start.Y, end.Y)),
            new Vector2(Math.Max(start.X, end.X), Math.Max(start.Y, end.Y))
        };

        _unitsEntities.ToList().ForEach(unit =>
        {
            SetUnitSelection(unit, false);
        });

        var units = GetUnitsInArea(area);
        units.ToList().ForEach(unit =>
        {
            SetUnitSelection(unit, true);
        });
    }

    public Node[] GetUnitsInArea(Godot.Collections.Array<Vector2> area)
    {
        var units = _unitsEntities.OfType<CharacterBody2D>()
        .Where(unit => unit.Position.X > area[0].X && unit.Position.X < area[1].X && unit.Position.Y > area[0].Y && unit.Position.Y < area[1].Y)
        .ToArray();

        return units;

        // var units = _unitsEntities.Where(entity =>
        // {
        //     if (entity is CharacterBody2D)
        //     {
        //         GD.Print("Entity is CharacterBody2D");
        //         var unit = (CharacterBody2D)entity;

        //         if (unit.Position.X > area[0].X && unit.Position.X < area[1].X)
        //         {
        //             if (unit.Position.Y > area[0].Y && unit.Position.Y < area[1].Y)
        //             {
        //                 return true;
        //             }
        //         }
        //     }

        //     return false;
        // });
    }

    private static void SetUnitSelection(Node unit, bool selected)
    {
        if (unit is CharacterBody2D)
        {
            // TODO: cast into a custom parent class called "Unit" and call a method "SetSelected"
            var character = (CharacterBody2D)unit;
            if (character.HasMethod("SetSelected"))
            {
                character.Call("SetSelected", selected);
            }
        }
    }
}
