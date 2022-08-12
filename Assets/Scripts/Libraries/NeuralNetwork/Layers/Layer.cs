public class Layer{
    public Matrix Weights;
    public Matrix Bias;
    public Matrix Output {get; protected set;}
    public virtual Matrix Forward(Matrix input){
        throw new System.NotImplementedException();
    }

    public virtual Matrix Forward(Matrix input, IActivationFunction activationFunction){
        throw new System.NotImplementedException();
    }

    public virtual void Randomize(){
    }
}