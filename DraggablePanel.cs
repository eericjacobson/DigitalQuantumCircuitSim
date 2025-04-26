using Godot;

[GlobalClass]
public partial class DraggablePanel : Panel
{
    private bool _hovered = false;
    private Vector2 _dragOffset;
    private bool _isDragging = false;

    public DraggablePanel() {
        MouseEntered += () => _hovered = true;
        MouseExited += () => _hovered = false;
    }

    public override void _Process(double delta)
    {
        if (!_hovered && !_isDragging) { return; }

        if (!Input.IsMouseButtonPressed(MouseButton.Left)) {
            _isDragging = false;
            return;
        }

        Vector2 mousePosition = GetTree().Root.GetMousePosition();
        if (!_isDragging) {
            _isDragging = true;
            _dragOffset = GlobalPosition - mousePosition;
        }
        
        GlobalPosition = mousePosition + _dragOffset;
    }
}