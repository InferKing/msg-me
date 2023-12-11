using XNode;

[NodeWidth(304)]
public class BaseNode : Node, IInitializer
{
    protected EventBus _bus;
    public void Initialize()
    {
        _bus = ServiceLocator.Current.Get<EventBus>();
    }
}
