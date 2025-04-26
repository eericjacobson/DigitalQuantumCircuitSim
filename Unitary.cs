using Godot;
using System;
using System.Collections.Generic;
using System.Numerics;

public class PermutationUnitary : Unitary
{
    public PermutationUnitary(int size, int[] targetQubits)
    {
        int dim = 1 << size;
        Complex[,] data = new Complex[dim, dim];

        // Build full permutation: targetQubits first, then all others
        List<int> fullPermutation = new List<int>(targetQubits);
        for (int i = 0; i < size; i++) {
            if (!fullPermutation.Contains(i)) {
                fullPermutation.Add(i);
            }
        }

        // For each basis state |i⟩, figure out where it goes under permutation
        for (int i = 0; i < dim; i++) {
            int permutedIndex = 0;

            for (int j = 0; j < size; j++) {
                int originalQubitIndex = fullPermutation[j];
                int bit = (i >> (size - 1 - originalQubitIndex)) & 1;
                permutedIndex |= (bit << (size - 1 - j));
            }

            data[permutedIndex, i] = Complex.One;
        }

        Matrix = new Matrix(data);
    }
}

public class Matrix
{
    public int Rows;
    public int Columns;

    public Complex[,] Values;

    public void Print() {
        GD.Print("[");

        for (int row = 0; row < Rows; row++) {
            String toPrint = "";
            for (int column = 0; column < Columns; column++) {
                toPrint += Values[row, column].ToString() + "    ";
            }
            GD.Print(toPrint);
        }

        GD.Print("]");
    }

    // Kronecker
    public static Matrix operator &(Matrix a, Matrix b) {
        Matrix matrix = new Matrix(a.Rows * b.Rows, a.Columns * b.Columns);
        for (int rowA = 0; rowA < a.Rows; rowA++) {
            for (int columnA = 0; columnA < a.Columns; columnA++) {
                for (int rowB = 0; rowB < b.Rows; rowB++) {
                    for (int columnB = 0; columnB < b.Columns; columnB++) {
                        matrix.Values[rowA * b.Rows + rowB, columnA * b.Columns + columnB] = a.Values[rowA, columnA] * b.Values[rowB, columnB];
                    }    
                } 
            }    
        }

        return matrix;
    }

    // Matrix Multiplication
    public static Matrix operator *(Matrix a, Matrix b) {
        if (a.Columns != b.Rows) { throw new Exception("Missmatching matrix dimensions! " + a.Columns + " " + b.Rows); }

        Matrix matrix = new Matrix(a.Rows, b.Columns);

        for (int row = 0; row < a.Rows; row++) {
            for (int column = 0; column < b.Columns; column++) {
                for (int k = 0; k < a.Columns; k++) {
                    matrix.Values[row, column] += a.Values[row, k] * b.Values[k, column];
                }
            }
        }
        return matrix;
    }

    public Matrix(int rows, int columns) {
        Rows = rows;
        Columns = columns;
        Values = new Complex[Rows, Columns];
    }

    public Matrix(Complex[,] values) {
        Values = values;
        Rows = Values.GetLength(0);
        Columns = Values.GetLength(1);
    }

    public Matrix Transpose() {
        Matrix matrix = new Matrix(Columns, Rows);

        for (int row = 0; row < matrix.Rows; row++) {
            for (int column = 0; column < matrix.Columns; column++) {
                matrix.Values[row, column] = Values[column, row];
            }
        }
        return matrix;
    }

    public Matrix ApplyOperationToElements(Func<Complex, Complex> func) {
        for (int row = 0; row < Values.Length; row++) {
            for (int column = 0; column < Values.Length; column++) {
                Values[row, column] = func(Values[row, column]);
            }
        }
        return this;
    }

}

public class Unitary
{
    protected bool IsIdentity;
    public Matrix Matrix;
    //public Unitary Dagger => new Unitary(Matrix.ApplyOperationToElements(Complex.Conjugate).Transpose());

    public Unitary(Matrix matrix) {
        if (matrix.Rows != matrix.Columns) { throw new Exception("Unitary matrix must be n*n in dimension"); }
        else {
            Matrix = matrix;
        }
    }

    public Unitary(int n, bool isIdentity)
    {
        IsIdentity = isIdentity;
        if (isIdentity) {
            Complex[,] matrix = new Complex[n,n];
            for (int i = 0; i < n; i++) {
                matrix[i,i] = new Complex(1, 0);
            }
            Matrix = new Matrix(matrix);
        } else {
            Matrix = new Matrix(n, n);
        }
    }

    public Unitary() {
        Matrix = new Matrix(new Complex[,] {{1}});
    }

    public static Unitary operator *(Unitary a, Unitary b) {
        //if (a is Identity) { return b; }
        //if (b is Identity) { return a;}
        //if (a == b.Dagger) { return new Identity(); }

        return new Unitary(a.Matrix * b.Matrix);
    }

    public static Unitary operator &(Unitary a, Unitary b) {
        return new Unitary(a.Matrix & b.Matrix);
    }

    public static QuantumRegister operator *(Unitary a, QuantumRegister b) {
        if (a.Matrix.Columns != b.Amplitudes.Rows) { throw new Exception("Unitary size " + a.Matrix.Columns + " does not match Register " + b.Amplitudes.Rows); }
        b.Amplitudes = a.Matrix * b.Amplitudes;
        return b;
    }

    public void Print() {
        Matrix.Print();
    }

    public Unitary Transpose() {
        return new Unitary(Matrix.Transpose());
    }
}
