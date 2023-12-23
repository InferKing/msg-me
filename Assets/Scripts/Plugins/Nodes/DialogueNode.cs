using UnityEngine;

public class DialogueNode : BaseNode, INodeRealizer
{
    [Input] public int input;
    [Output] public int output;
    public NodeData data;
    public void Implement()
    {
        
    }
}
