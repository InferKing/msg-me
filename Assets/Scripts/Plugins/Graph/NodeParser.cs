using System.Linq;
using System.Collections.Generic;
using UnityEngine;

using XNode;

public class NodeParser : MonoBehaviour, IInitializer
{
    [SerializeField] private DialogueGraph _graph;
    private EventBus _bus;
    private Coroutine _parser;
    public void Initialize()
    {
        _bus = ServiceLocator.Current.Get<EventBus>();
        _bus.Subscribe<EndingGameSignal>(OnGameEnding);
        _bus.Subscribe<NextNodeSignal>(OnNextNode);
        _bus.Subscribe<NodeFinishedTextSignal>(ParseNode);
        _bus.Subscribe<ChoosingPathParsedSignal>(OnChoosingPathParsed);
        ParseNodes();
    }
    private void ParseNodes()
    {
        foreach (BaseNode node in _graph.nodes.Cast<BaseNode>())
        {
            node.Initialize();
            if (node is StartNode temp)
            {
                _graph.current = node;
                temp.Implement();
            }
        }
        ParseNode(new NodeFinishedTextSignal());
    }
    private void ParseNode(NodeFinishedTextSignal signal)
    {
        BaseNode node = _graph.current;
        switch (node)
        {
            case StartNode temp:
                NextNode(Constants.Output);
                ParseNode(new NodeFinishedTextSignal());
                break;
            case EndNode temp:
                break;
            case DialogueNode temp:
                _bus.Invoke(new NodeParsedDataSignal(temp.data));
                break;
            case ChoosingPathNode temp:
                temp.StartParseCondition();
                break;
        }
    }
    private void NextNode(string fieldName)
    {
        foreach (NodePort p in _graph.current.Ports)
        {
            if (p.fieldName == fieldName)
            {
                List<NodePort> ports = p.GetConnections();
                if (ports.Count == 1)
                {
                    _graph.current = ports[0].node as BaseNode;
                    return;
                }
                throw new System.Exception($"The number of ports {ports.Count} " +
                        $"does not correspond to the number of outputs to be processed.");

            }
        }
    }
    private void NextNode(string fieldName, bool data) 
    {
        foreach (NodePort p in _graph.current.Ports)
        {
            if (p.fieldName == fieldName)
            {
                List<NodePort> ports = p.GetConnections();
                if (ports.Count == 0)
                {
                    throw new System.Exception($"The number of ports {ports.Count} " +
                        $"does not correspond to the number of outputs to be processed.");
                }
                _graph.current = data ? ports[0].node as BaseNode : ports[1].node as BaseNode;
                return;
            }
        }
    }
    private void OnNextNode(NextNodeSignal signal)
    {
        NextNode(Constants.Output);
        ParseNode(new NodeFinishedTextSignal());
    }
    private void OnChoosingPathParsed(ChoosingPathParsedSignal signal)
    {
        NextNode(Constants.Output, signal.data);
        ParseNode(new NodeFinishedTextSignal());
    }
    private void OnGameEnding(EndingGameSignal signal)
    {

    }
    private void OnDisable()
    {
        if (_bus == null) return;
        _bus.Unsubscribe<EndingGameSignal>(OnGameEnding);
        _bus.Unsubscribe<NextNodeSignal>(OnNextNode);
        _bus.Unsubscribe<NodeFinishedTextSignal>(ParseNode);
        _bus.Unsubscribe<ChoosingPathParsedSignal>(OnChoosingPathParsed);
    }
}
