using Godot;
using System;

public partial class GoblinFighter : CharacterBody2D
{
    [Export]
    bool _selected = false;

    [Export]
    Panel _box;

    public override void _Ready()
    {
        // Called every time the node is added to the scene.
        _box = GetNode<Panel>("%Box");
    }

    public void SetSelected(bool value)
    {
        _box.Visible = value;
    }
}
