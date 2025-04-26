

using System;
using System.Collections.Generic;
using Godot;

public class ComponentConnection {
    public QuantumCircuit QuantumCircuit;
    public int[] AffectedQubits;

    public ComponentConnection(QuantumCircuit quantumCircuit, int[] affectedQubits) {
        QuantumCircuit = quantumCircuit;
        AffectedQubits = affectedQubits;
    }
}

public class QuantumCircuit
{
    public int Size;

    private Unitary unitaryOverride;

    public QuantumRegister InputRegister;

    private bool overrideUnitary = false;
    private ComponentConnection[] connections;

    public QuantumCircuit(int size, ComponentConnection[] connections) {
        Size = size;
        this.connections = connections;
        InputRegister = new QuantumRegister(size);
    }

    public QuantumCircuit(Unitary unitaryOverride) {
        this.unitaryOverride = unitaryOverride;
        overrideUnitary = true;
        Size = (int)Math.Log2(unitaryOverride.Matrix.Rows);
    }

    public Unitary GetUnitary() {
        if (overrideUnitary) { return unitaryOverride; }
        Unitary unitary = new Unitary((int)Math.Pow(2, Size), true);
        foreach (ComponentConnection connection in connections) {
            PermutationUnitary permutationUnitary = new PermutationUnitary(Size, connection.AffectedQubits);
            int remainingQubits = Size - connection.QuantumCircuit.Size;
            Unitary fullUnitary = connection.QuantumCircuit.GetUnitary() & new Unitary((int)Math.Pow(2, remainingQubits), true);
            GD.PrintS(permutationUnitary.Matrix.Rows, permutationUnitary.Matrix.Columns, fullUnitary.Matrix.Rows, fullUnitary.Matrix.Columns, unitary.Matrix.Rows, unitary.Matrix.Columns); 
            unitary = permutationUnitary.Transpose() * fullUnitary * permutationUnitary * unitary;
        }
        return unitary;
    }

    public QuantumRegister Execute() {
        return GetUnitary() * InputRegister;
    }
}