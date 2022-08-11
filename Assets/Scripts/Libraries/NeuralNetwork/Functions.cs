using System;

public class Functions{
    public static float MSELoss(Matrix input, Matrix target){
        if(input.Rows != target.Rows || input.Columns != target.Columns){
            throw new Exception("Input and target must have the same dimensions");
        }
        return Matrix.Mean((input - target)^2);
    }
}