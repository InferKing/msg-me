using System.Collections.Generic;
using UnityEngine;

public class PathNode : BaseNode, INodeRealizer
{
    [Input] public int input;
    [Output] public int output;
    public List<PathNodeData> path;
    public void Implement()
    {
        
    }
}
