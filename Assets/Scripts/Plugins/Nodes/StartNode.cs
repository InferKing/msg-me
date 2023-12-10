public class StartNode : BaseNode, INodeRealizer
{
    [Output] public int output;
    public void Implement()
    {
        _bus.Invoke(new StartGameSignal());
    }
}
