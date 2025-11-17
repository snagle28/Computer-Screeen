using UnityEngine;
using System.Collections.Generic;
using UnityEngine.EventSystems;
using UnityEngine.UI;




public class InputRelaySink : MonoBehaviour
{
    [SerializeField] private RectTransform CanvasTransform;
    GraphicRaycaster raycaster; //graphic raycaster needed for UI raycasts
    
    //list to keep track of draggable objects
    List<GameObject> DragTargets = new List<GameObject>(); 
    
    //persistend pointer data and currently active drag target
    private PointerEventData perPointer = null; //persistent pointer data
    private GameObject activeDT = null; //active drag target
    
    /*
     * Keep last mouse position to calcualte the change in the mouse's movement.
     * this is only needed because we want the slider to keep moving down even if the
     * mouse exits the slider's bounds while dragging.
     */
    
    private Vector2 lastMousePos = Vector2.zero;
    void Start()
    {
        raycaster = GetComponent<GraphicRaycaster>();
    }
    

    public void OnCursorInput(Vector2 normalisedPosition)
    {
        //STEP 1: CALCULATE MOUSE POSITION IN CANVAS SPACE//
        Vector3 mousePosition = new Vector3(CanvasTransform.sizeDelta.x * normalisedPosition.x,
            CanvasTransform.sizeDelta.y * normalisedPosition.y,
            0f);
        
        Vector2 mousePos = new Vector2(mousePosition.x, mousePosition.y);
        
        //STEP 2: CHECK MOUSE BUTTONS//

        bool sendMouseDown = Input.GetMouseButtonDown(0);
        bool sendMouseUp = Input.GetMouseButtonUp(0);
        bool isMouseDown = Input.GetMouseButton(0);
        
        //STEP 3: ENSURE PERSISTENT POINTER DATA EXISTS WHEN NEEDED//
        if (sendMouseDown)
        {
            perPointer = new PointerEventData(EventSystem.current);
            perPointer.position = mousePos;
            perPointer.pressPosition = mousePos;
            perPointer.delta = Vector2.zero;
            perPointer.button = PointerEventData.InputButton.Left;
        }
        else if (perPointer == null)
        {
            //we still create a temporary pointer for raycasting when nothing is active
            perPointer = new PointerEventData(EventSystem.current);
            perPointer.position = mousePos;
        }
        
        //STEP 4: COMPUTE DELTA//
        Vector2 delta = mousePos - lastMousePos;
        if (sendMouseDown)
        {
            delta = Vector2.zero; //avoid jump on initial press
        }
        perPointer.position = mousePos;
        perPointer.delta = delta;
        
        //STEP 5: RAYCAST TO SEE WHAT WE'RE OVER THIS FRME//
        List<RaycastResult> results = new List<RaycastResult>();
        raycaster.Raycast(perPointer, results);
        
        //STEP 6: PICKK A TARGET AND INITIALIZE DRAG STATE//
        if (sendMouseDown)
        {
            if (results.Count > 0)
            {
                var hit = results[0];
                GameObject hitGO = hit.gameObject;
                
                perPointer.pointerPressRaycast = hit;
                perPointer.pointerCurrentRaycast = hit;
                
                ExecuteEvents.Execute(hitGO, perPointer, ExecuteEvents.pointerDownHandler);
                
                perPointer.pointerPress = hitGO;
                perPointer.pointerDrag = hitGO;
                perPointer.pressPosition = mousePos;
                
                bool began = ExecuteEvents.Execute(hitGO, perPointer, ExecuteEvents.beginDragHandler);
                if (began || hitGO.GetComponentInParent<Slider>() != null)
                {
                    activeDT = hitGO;
                    if (!DragTargets.Contains(hitGO))
                    {
                        DragTargets.Add(hitGO);
                    }
                    
                    var slider = hitGO.GetComponentInParent<Slider>();
                    if (slider != null)
                    {
                        slider.OnInitializePotentialDrag(perPointer);
                    }
                }
            }
            else
            {
                perPointer.pointerPress = null;
                perPointer.pointerDrag = null;
            }
        }
        
        
        //STEP 7: END DRAG & CLEANUP//
        if (isMouseDown && activeDT != null)
        {
            perPointer.dragging = true;
            
            ExecuteEvents.Execute(activeDT, perPointer, ExecuteEvents.dragHandler);
            
            var activeSlider = activeDT.GetComponentInParent<Slider>();
            if (activeSlider != null)
            {
                activeSlider.OnDrag(perPointer);
            }
        }
        else
        {
            foreach (var result in results)
            {
                PointerEventData tempDat = new PointerEventData(EventSystem.current);
                tempDat.position = mousePos;
                tempDat.pointerCurrentRaycast = tempDat.pointerPressRaycast = result;
                tempDat.button = isMouseDown ? PointerEventData.InputButton.Left : PointerEventData.InputButton.Left;
                var slider = result.gameObject.GetComponentInParent<Slider>();
                
                
                if (sendMouseDown)
                {
                    if (ExecuteEvents.Execute(result.gameObject, tempDat, ExecuteEvents.pointerDownHandler))
                        break;
                }
                else if (sendMouseUp)
                {
                    bool didRun = ExecuteEvents.Execute(result.gameObject, tempDat, ExecuteEvents.pointerUpHandler);
                    didRun |= ExecuteEvents.Execute(result.gameObject, tempDat, ExecuteEvents.pointerClickHandler);
                    if (didRun) break;
                }
            }
        }
        
        
        //STEP 8: MOUSE UP: END DRAG & CLEANUP//
        if (sendMouseUp)
        {
            if (activeDT != null)
            {
                ExecuteEvents.Execute(activeDT, perPointer, ExecuteEvents.endDragHandler);
                
                if (perPointer.pointerPress != null)
                {
                    ExecuteEvents.Execute(perPointer.pointerPress, perPointer, ExecuteEvents.pointerUpHandler);
                    ExecuteEvents.Execute(perPointer.pointerPress, perPointer, ExecuteEvents.pointerClickHandler);
                }
            }
            
            foreach (var t in DragTargets)
            {
                if (t != activeDT)
                {
                    ExecuteEvents.Execute(t, perPointer, ExecuteEvents.endDragHandler);
                }
            }
            
            DragTargets.Clear();
            activeDT = null;
            
            perPointer = null;
            lastMousePos = Vector2.zero;
        }
        else
        {
            lastMousePos = Vector2.zero;
        }
    }
}

