using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.Events;

public class InputSource : MonoBehaviour
{
    
    [SerializeField] private float RaycastDistance = 15f;
    [SerializeField] private LayerMask RaycastMask = ~0;
    [SerializeField] UnityEvent<Vector2> OnCursorInput = new UnityEvent<Vector2>();
    [SerializeField] UnityEvent OnCursorExit = new UnityEvent();
    
    
    private bool wasHittingLastFrame = false;

    // last canvas coordinates b4 leaving canvas
    private Vector2 lastCanvasCoor;
    
    private bool isDragging = false;

    // tracking mouse movement
    private Vector2 lastMousePos;
    private bool hasLastMousePos = false;

    // how much UV moves per pixel of mouse movement 
    private Vector2 uvPerPixel = Vector2.zero;
    private bool hasCalibration = false; //true when we compute uvPerPixel. NEED THIS 
    //b4 we simulate moevemnt
    
    public AudioSource audioSource;
    public AudioClip quacksound;

    public AudioClip clickSound;

    void Start()
    {
        // initialize lastMousePos so first delta isn’t huge
        lastMousePos = Input.mousePosition;
        hasLastMousePos = true;
    }

    // Update is called once per frame
    private Vector2 mouseDelta;
     void Update()
    {
        /*
         * ADDED THESE VARS to FIX THE FACT THAT U CANT DRAG OFF SCREEN
         */
        Vector2 mousePos = Input.mousePosition;
        if (hasLastMousePos)
        {
            mouseDelta = mousePos - lastMousePos;
        }
        else
        {
            mouseDelta = Vector2.zero;
        }
        
        lastMousePos = mousePos;
        hasLastMousePos = true;

        bool mouseHeld = Input.GetMouseButton(0);
        
        /*
         * 
         */

        Ray mouseRay = Camera.main.ScreenPointToRay(mousePos);
        RaycastHit hitResult;

        if (Physics.Raycast(mouseRay, out hitResult, RaycastDistance, RaycastMask, QueryTriggerInteraction.Ignore))
        {
            if (hitResult.collider.gameObject == gameObject)
            {
                if (Input.GetMouseButtonDown(0))
                {
                    audioSource.PlayOneShot(clickSound);
                }
                //so this gets our coordinates on the object that we hit.
                //has calibration is true when we compute
                //uvPerPixel from mouse movement (uv -> mouseDelta)
                Vector2 canvasCoor = hitResult.textureCoord;

                if (hasCalibration == false && mouseDelta.sqrMagnitude > 0.0001f) //checking for like the most subtle possible change
                {
                    hasCalibration = true;
                }

                if (mouseHeld && lastCanvasCoor != Vector2.zero && mouseDelta.sqrMagnitude > 0.0001f)
                {
                    Vector2 canvasChange = canvasCoor - lastCanvasCoor;

                    // avoid division by zero
                    if (Mathf.Abs(mouseDelta.x) > 0.001f)
                    {
                        uvPerPixel.x = canvasChange.x / mouseDelta.x;
                    }

                    if (Mathf.Abs(mouseDelta.y) > 0.001f)
                    {
                        uvPerPixel.y = canvasChange.y / mouseDelta.y;
                    }
                    
                    hasCalibration = true; //just in case bc im not taking chances
                }

                lastCanvasCoor = canvasCoor;

                if (mouseHeld)
                {
                    isDragging = true;
                }

                OnCursorInput.Invoke(canvasCoor);
                wasHittingLastFrame = true;
                return;
            }
            else
            {
                // other obj
                GoOffCanvas(mouseHeld, mouseDelta);
                Debug.Log(hitResult.collider.gameObject.name);
                if ((hitResult.collider.gameObject.tag == "duck") && Input.GetMouseButtonDown(0))
                {
                    audioSource.PlayOneShot(quacksound);
                }
                return;
            }
        }
        else
        {
            // nothing hit at all
            GoOffCanvas(mouseHeld, mouseDelta);
        }
    }
    private void GoOffCanvas(bool mouseHeld, Vector2 mouseDelta)
    {
        if (isDragging && mouseHeld && hasCalibration)
        {
            
            Vector2 uvMove = new Vector2(mouseDelta.x * uvPerPixel.x,
                mouseDelta.y * uvPerPixel.y
            );

            lastCanvasCoor += uvMove;
            lastCanvasCoor.x = Mathf.Clamp01(lastCanvasCoor.x);
            lastCanvasCoor.y = Mathf.Clamp01(lastCanvasCoor.y);

            OnCursorInput.Invoke(lastCanvasCoor);
            // do NOT treat this as an exit while dragging
            wasHittingLastFrame = true;
        }
        else
        {
            // not dragging or mouse released: normal exit behaviour
            if (wasHittingLastFrame)
            {
                OnCursorExit.Invoke();
                wasHittingLastFrame = false;
            }

            // stop dragging if mouse button not held
            if (!mouseHeld)
                isDragging = false;
        }

        // NEW: Detect release during off-screen sim and force exit
        if (isDragging && !mouseHeld && !wasHittingLastFrame)
        {
            print("should stop dragging now");
            OnCursorExit.Invoke();  // Ensure cleanup on off-screen release
            isDragging = false;
        }
    }
}
