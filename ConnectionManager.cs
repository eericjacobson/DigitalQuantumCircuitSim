using Godot;
using System;
using System.Linq.Expressions;

public partial class ConnectionManager : Control
{
    static public bool AttemptingConnection = false;

    static private ConnectionPoint _connectionStart;
    static private ConnectionPoint _connectionEnd;

    static public void OnConnectionPointHovered(ConnectionPoint point) {
        GD.Print("pointhovered");
        if (!IsValidConnection(point)) { return; }
        if (!AttemptingConnection) { return; }
        _connectionEnd = point;
        GD.Print("hoverpointset");
    }

    static public void SetConnectionStart(ConnectionPoint connectionPoint) {
        GD.Print("setconstart");
        _connectionStart = connectionPoint;
        _connectionEnd = null;
        AttemptingConnection = true;
    }

    static public void CancelConnection() {
        GD.Print("cancel");
        _connectionStart = null;
        _connectionEnd = null;
        AttemptingConnection = false;
    }

    static public bool IsValidConnection(ConnectionPoint attemptedConnectionEnd) {
        return (_connectionStart?.ParentNode != attemptedConnectionEnd?.ParentNode) && _connectionStart != attemptedConnectionEnd;
    }

    public override void _Process(double delta)
    {
        QueueRedraw();
        if (Input.IsActionJustPressed("ui_cancel")) {
            CancelConnection();
        }
    }


    public override void _Draw()
    {
        //GD.Print("AC" + AttemptingConnection, "CS" + _connectionStart, "CE" + _connectionEnd);
        if (AttemptingConnection) {
            DrawLine(_connectionStart.MidPoint, _connectionEnd == null ? GetTree().Root.GetMousePosition() : _connectionEnd.MidPoint, Colors.DarkRed, 50);
        }
    }

}
