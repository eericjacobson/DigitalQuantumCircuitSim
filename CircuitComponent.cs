using Godot;
using System;

[GlobalClass]
public partial class CircuitComponent : DraggablePanel
{


    public CircuitComponent(int qubitsOut) {
        SetOutgoingConnections(qubitsOut);
        Size = new Vector2(400, 400);
    }

    private void SetOutgoingConnections(int qubitsOut) {
        VBoxContainer rightContainer = new VBoxContainer();
        rightContainer.SetAnchorsPreset(LayoutPreset.RightWide);
        rightContainer.Alignment = BoxContainer.AlignmentMode.Center;
        for (int i = 0; i < qubitsOut; i++) {
            ConnectionPoint c = new(this)
            {
                CustomMinimumSize = new Vector2(100, 100),
                Modulate = Colors.AliceBlue
            };
            rightContainer.AddChild(c);
        }
        AddChild(rightContainer);
    }
}
