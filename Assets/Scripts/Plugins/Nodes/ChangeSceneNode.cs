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
