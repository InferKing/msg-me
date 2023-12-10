using System.Collections;
using System.Collections.Generic;
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
