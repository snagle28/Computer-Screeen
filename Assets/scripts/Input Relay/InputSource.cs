using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.Events;

public class InputSource : MonoBehaviour
{
    
    [SerializeField] private float RaycastDistance = 15f;
    [SerializeField] private LayerMask RaycastMask = ~0;
    [SerializeField] UnityEvent<Vector2> OnCursorInput = new UnityEvent<Vector2>();
    
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        //Debug.Log(Input.mousePosition);
        //retrieve a ray based on mouse position
        Ray mouseRay = Camera.main.ScreenPointToRay(Input.mousePosition);

        //raycast to find what we need to hit
        RaycastHit hitResult;
        if (Physics.Raycast(mouseRay, out hitResult, RaycastDistance, RaycastMask, QueryTriggerInteraction.Ignore))
        {
            //ignore if that object is not us
            //if (hitResult.collider.gameObject != gameObject) return;
            if (hitResult.collider.gameObject != gameObject) return;
            //Debug.Log($"Hit {hitResult.collider.gameObject.name} at {hitResult.textureCoord}");
            
            OnCursorInput.Invoke(hitResult.textureCoord);
            //Debug.Log(hitResult.textureCoord); 
            //testing new repo
            
        } 
        
    }
}
