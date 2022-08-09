using System;
using System.Collections;
using System.Collections.Generic;

public class Matrix
{
    public int Rows { get; private set; }
    public int Columns {get; private set; }
    public float[,] Data { get; private set; }

    public Matrix(int rows, int cols){
        this.Rows = rows;
        this.Columns = cols;
        this.Data = new float[rows, cols];
    }

    public static Matrix FromArray(float[] arr){
        Matrix m = new Matrix(arr.Length, 1);    
        for(int i=0; i<arr.Length; i++){
            m.Data[i, 0] = arr[i];
        }

        return m;
    }

    public float[] ToArray(){
        List<float> arr = new List<float>();

        for(int i=0; i<Rows; i++){
            for(int j=0; j<Columns; j++){
                arr.Add(Data[i, j]);
            }
        }

        return arr.ToArray();
    }

    public void Randomize(){
        for(int i=0; i<Rows; i++){
            for(int j=0; j<Columns; j++){
                Data[i, j] = UnityEngine.Random.Range(-1f, 1f);
            }
        }
    }

    public static Matrix operator +(Matrix m1, Matrix m2){
        if(m1.Rows != m2.Rows || m1.Columns != m2.Columns){
            throw new System.Exception("Matrices must be of the same size");
        }
        Matrix m = new Matrix(m1.Rows, m1.Columns);
        for(int i=0; i<m1.Rows; i++){
            for(int j=0; j<m1.Columns; j++){
                m.Data[i, j] = m1.Data[i, j] + m2.Data[i, j];
            }
        }
        return m;
    }

    public static Matrix operator +(Matrix m1, float f){
        Matrix m = new Matrix(m1.Rows, m1.Columns);
        for(int i=0; i<m1.Rows; i++){
            for(int j=0; j<m1.Columns; j++){
                m.Data[i, j] = m1.Data[i, j] + f;
            }
        }
        return m;
    }
    
    public static Matrix operator -(Matrix m1, Matrix m2){
        if(m1.Rows != m2.Rows || m1.Columns != m2.Columns){
            throw new System.Exception("Matrices must be of the same size");
        }
        Matrix m = new Matrix(m1.Rows, m1.Columns);
        for(int i=0; i<m1.Rows; i++){
            for(int j=0; j<m1.Columns; j++){
                m.Data[i, j] = m1.Data[i, j] - m2.Data[i, j];
            }
        }
        return m;
    }

    public static Matrix operator -(Matrix m1, float f){
        Matrix m = new Matrix(m1.Rows, m1.Columns);
        for(int i=0; i<m1.Rows; i++){
            for(int j=0; j<m1.Columns; j++){
                m.Data[i, j] = m1.Data[i, j] - f;
            }
        }
        return m;
    }

    public static Matrix Transpose(Matrix m){
        Matrix t = new Matrix(m.Columns, m.Rows);
        for(int i=0; i<m.Rows; i++){
            for(int j=0; j<m.Columns; j++){
                t.Data[j, i] = m.Data[i, j];
            }
        }
        return t;
    }

    public static Matrix operator *(Matrix m1, Matrix m2){
        if(m1.Columns != m2.Rows){
            throw new System.Exception("Matrices must be of compatible sizes");
        }
        Matrix m = new Matrix(m1.Rows, m2.Columns);
        for(int i=0; i<m1.Rows; i++){
            for(int j=0; j<m2.Columns; j++){
                for(int k=0; k<m1.Columns; k++){
                    m.Data[i, j] += m1.Data[i, k] * m2.Data[k, j];
                }
            }
        }
        return m;
    }

    public static Matrix operator *(Matrix m1, float f){
        Matrix m = new Matrix(m1.Rows, m1.Columns);
        for(int i=0; i<m1.Rows; i++){
            for(int j=0; j<m1.Columns; j++){
                m.Data[i, j] = m1.Data[i, j] * f;
            }
        }
        return m;
    }

    public static Matrix Hadamard(Matrix m1, Matrix m2){
        if(m1.Rows != m2.Rows || m1.Columns != m2.Columns){
            throw new System.Exception("Matrices must be of the same size");
        }
        Matrix m = new Matrix(m1.Rows, m1.Columns);
        for(int i=0; i<m1.Rows; i++){
            for(int j=0; j<m1.Columns; j++){
                m.Data[i, j] = m1.Data[i, j] * m2.Data[i, j];
            }
        }
        return m;
    }

    public static Matrix Map(Matrix m, System.Func<float, float> f){
        Matrix n = new Matrix(m.Rows, m.Columns);
        for(int i=0; i<m.Rows; i++){
            for(int j=0; j<m.Columns; j++){
                n.Data[i, j] = f(m.Data[i, j]);
            }
        }
        return n;
    }

    public void Print(){
        string s = "";
        for(int i=0; i<Rows; i++){
            for(int j=0; j<Columns; j++){
                s += Data[i, j] + " ";
            }
            s += "\n";
        } 

        Console.Write(s);
    }
}