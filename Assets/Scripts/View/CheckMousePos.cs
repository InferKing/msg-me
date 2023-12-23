using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class CheckMousePos : MonoBehaviour
{
    private PointerEventData _pointerEventData;
    private GraphicRaycaster _graphicRaycaster;
    private void Start()
    {
        _graphicRaycaster = FindObjectOfType<GraphicRaycaster>();
    }
    public bool IsUnderButton()
    {
        _pointerEventData = new PointerEventData(EventSystem.current);
        _pointerEventData.position = Input.mousePosition;
        List<RaycastResult> results = new List<RaycastResult>();
        _graphicRaycaster.Raycast(_pointerEventData, results);
        return results.Exists((item) => item.gameObject.CompareTag("UIButton"));
    }
}
