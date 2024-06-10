using Godot;
using System;
using System.IO;

public partial class Camera : Camera2D
{
    Vector2 _mousePosition = new Vector2();
    Vector2 _mousePositionGlobal = new Vector2();
    Vector2 _start = new Vector2();
    Vector2 _startV = new Vector2();
    Vector2 _end = new Vector2();
    Vector2 _endV = new Vector2();
    bool _isDragging = false;

    Panel _selectionPanel;

    // signal
    [Signal]
    public delegate void AreaSelectedEventHandler(Vector2 position);

    [Signal]
    public delegate void StartMoveSelectionEventHandler(Vector2 position);



    public override void _Ready()
    {
        // Called every time the node is added to the scene.
        _selectionPanel = GetNode<Panel>("%SelectionPanel");
        _selectionPanel.Visible = false;

    }

    public override void _Process(double delta)
    {
        if (Input.IsActionJustPressed("mouse_left"))
        {
            _start = _mousePositionGlobal;
            _startV = _mousePosition;
            _isDragging = true;
        }

        // Called every frame. Delta is the elapsed time since the previous frame.
        if (_isDragging)
        {
            _end = _mousePositionGlobal;
            _endV = _mousePosition;
            DrawArea();
        }

        if (Input.IsActionJustReleased("mouse_left"))
        {
            if (_startV.DistanceTo(_mousePosition) > 20)
            {
                _end = _mousePositionGlobal;
                _endV = _mousePosition;
                _isDragging = false;
                DrawArea(false);
                // EmitSignal(nameof(AreaSelectedEventHandler));
            }
            else
            {
                _end = _start;
                _isDragging = false;
                DrawArea(false);
            }
        }
    }

    // input event
    public override void _Input(InputEvent @event)
    {
        if (@event is InputEventMouse mouseButton)
        {
            _mousePosition = mouseButton.Position;
            _mousePositionGlobal = GetGlobalMousePosition();
        }
    }

    public void DrawArea(bool s = true)
    {
        _selectionPanel.Size = new Vector2(MathF.Abs(_startV.X - _endV.X), MathF.Abs(_startV.Y - _endV.Y));
        var position = new Vector2
        {
            X = MathF.Min(_startV.X, _endV.X),
            Y = MathF.Min(_startV.Y, _endV.Y)
        };
        _selectionPanel.Position = position;
        _selectionPanel.Size *= s ? 1 : 0;
        _selectionPanel.Visible = s;
    }
}
