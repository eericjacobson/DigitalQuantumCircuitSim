using System.Numerics;

public class QuantumRegister
{
    public Matrix Amplitudes;
    public QuantumRegister(int numQubits) {
        Matrix matrix = new Qubit(false).Amplitudes;

        for (int i = 0; i < numQubits - 1; i++) {
            matrix &= new Qubit(false).Amplitudes;
        }
        Amplitudes = matrix;
    }

    public void Print() {
        Amplitudes.Print();
    }
}

public class Qubit
{
    public Matrix Amplitudes;

    public Qubit(bool basis) {
        Amplitudes = new Matrix(basis ? new Complex[,] { {0}, {1} } : new Complex[,] { {1}, {0} });
    }

    public void Print() {
        Amplitudes.Print();
    }
}
