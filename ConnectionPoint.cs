using Godot;
using System;
using System.Runtime.Serialization;

[GlobalClass]
public partial class ConnectionPoint : Panel
{
    private bool _hovered = false;
    public Vector2 MidPoint => GlobalPosition + Size * 0.5f;
    public Node ParentNode;

    public ConnectionPoint(Node parentNode) {
        ParentNode = parentNode;
        MouseEntered += () => { _hovered = true; ConnectionManager.OnConnectionPointHovered(this); };
        MouseExited += () => { _hovered = false; ConnectionManager.OnConnectionPointHovered(null); };
    }

    public override void _Process(double delta)
    {
        if (_hovered && Input.IsActionJustPressed("click")) {
            ConnectionManager.SetConnectionStart(this);
        }
    }

}
