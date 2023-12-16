using System.Collections.Generic;

public class ShowPathButtonsSignal
{
    public readonly List<PathNodeData> data;
    public ShowPathButtonsSignal(List<PathNodeData> data)
    {
        this.data = data;
    }
}
