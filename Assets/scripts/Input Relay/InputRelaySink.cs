using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.EventSystems;
using UnityEngine.UI;


public class InputRelaySink : MonoBehaviour
{
    [SerializeField] private RectTransform CanvasTransform;
    GraphicRaycaster raycaster;
    
    List<GameObject> DragTargets = new List<GameObject>(); //make list of objects that can
    //receive the drag event
    
    
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        raycaster = GetComponent<GraphicRaycaster>();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void OnCursorInput(Vector2 normalisedPosition)
    {
        //calculate the mouse position in canvas space 
        Vector3 mousePosition = new Vector3(CanvasTransform.sizeDelta.x * normalisedPosition.x,
            CanvasTransform.sizeDelta.y * normalisedPosition.y, 
            0f);
        //Debug.Log(mousePosition); 
        
        //construct a pointer event
        PointerEventData mouseEvent = new PointerEventData(EventSystem.current);
        mouseEvent.position = mousePosition;

        bool sendMouseDown = Input.GetMouseButtonDown(0);
        bool sendMouseUp = Input.GetMouseButtonUp(0);
        bool isMouseDown = Input.GetMouseButton(0);
        
        
        //send through end drag events as needed
        if (sendMouseUp)
        {
            foreach (var target in DragTargets)
            {
                ExecuteEvents.Execute(target,mouseEvent, ExecuteEvents.endDragHandler);
            }
            DragTargets.Clear();
        }
        
        //performing a raycast using the graphics raycaster
        List<RaycastResult> results = new List<RaycastResult>();
        raycaster.Raycast(mouseEvent, results);
        
        //process the raycast results
        foreach (var result in results)
        {
            //setup the new event data
            PointerEventData eventData = new PointerEventData(EventSystem.current);
            eventData.position = mousePosition;
            eventData.pointerCurrentRaycast = eventData.pointerPressRaycast = result;

            //is the mouse down
            if (isMouseDown)
            {
                eventData.button = PointerEventData.InputButton.Left;
                
                
            }

            //potentioally new drag targets?
            if (sendMouseDown)
            {
                DragTargets.Add(result.gameObject);
                ExecuteEvents.Execute(result.gameObject, eventData, ExecuteEvents.beginDragHandler);
            } //need to update drag target
            else if (DragTargets.Contains(result.gameObject))
            {
                eventData.dragging = true;
                ExecuteEvents.Execute(result.gameObject, eventData, ExecuteEvents.dragHandler);
            }
            
            Debug.Log(result.gameObject.name);
            if (sendMouseDown)
            {
                ExecuteEvents.Execute(result.gameObject, 
                    eventData, 
                    ExecuteEvents.pointerDownHandler);
            }
            else if (sendMouseUp)
            {
                ExecuteEvents.Execute(result.gameObject, 
                    eventData, 
                    ExecuteEvents.pointerUpHandler);
                ExecuteEvents.Execute(result.gameObject, 
                    eventData, 
                    ExecuteEvents.pointerClickHandler);
            }
        }
    }
}
