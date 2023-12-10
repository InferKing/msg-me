public class EndNode : BaseNode, INodeRealizer
{
    [Input] public int input;
    public void Implement()
    {
        _bus.Invoke(new EndingGameSignal());
    }
}
