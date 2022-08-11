public class Experience{
    public float[] State { get; private set; }
    public int Action { get; private set; }
    public float Reward { get; private set; }
    public float[] NextState { get; private set; }
    public Experience(float[] state, int action, float reward, float[] nextState){
        this.State = state;
        this.Action = action;
        this.Reward = reward;
        this.NextState = nextState;
    }
}