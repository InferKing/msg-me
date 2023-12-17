using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ChangeSceneNode : BaseNode, INodeRealizer
{
    [Input] public int input;
    [Output] public int output;
    public Sprite scene;
    public void Implement()
    {
        
    }
}
