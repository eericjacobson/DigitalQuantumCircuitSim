using System.Numerics;
using Godot;

public partial class Control2 : Godot.Control
{
    public override void _EnterTree()
    {
        QuantumRegister q = new QuantumRegister(2);
        
        Unitary CX = new Unitary(new Matrix(new Complex[,] {{1,0,0,0}, {0,1,0,0}, {0,0,0,1}, {0,0,1,0}}));

        Unitary X = new Unitary(new Matrix(new Complex[,] { {0, 1}, {1, 0} }));

        Unitary I2 = new Unitary(2, true);

        double sqrt2 = 1;
        Unitary H = new Unitary(new Matrix(new Complex[,] { {1 / sqrt2, 1 / sqrt2 }, {1 / sqrt2, -1 / sqrt2 } }));
        
        QuantumCircuit Hadamard = new QuantumCircuit(H);

        //((RyT & I2) * CX * (H & I2) * q).Print();

        new PermutationUnitary(1, [0]).Print();

        ComponentConnection componentConnection = new ComponentConnection(Hadamard, [0]);
        ComponentConnection componentConnection2 = new ComponentConnection(Hadamard, [2]);

        QuantumCircuit qc = new QuantumCircuit(4, [componentConnection, componentConnection2]);

        qc.Execute().Print();
    }

    public override void _Ready()
    {
        QuantumCircuit qc = new QuantumCircuit(4, []);
    }

}
