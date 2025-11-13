using UnityEngine;

public class DragText : MonoBehaviour
{
    public DragText partner;
    public GameManager manager;

    private Vector3 startPos;
    private Vector3 grabOffset; //makes grabbing the text boxes look smoother
    private bool isDragging;

    void OnMouseDown()
    {
        isDragging = true;
        startPos = transform.position;
        
        //convert the mouse screen coords to world space so we can position the object properly in the space
        Vector3 mouseWorld = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        grabOffset = transform.position - new Vector3(mouseWorld.x, mouseWorld.y, transform.position.z);
    }

    void OnMouseDrag()
    {
        if (!isDragging) return; //don't do anything if not dragging

        //continuously update the pos to follow the mouse
        Vector3 mouseWorld = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        Vector3 target = new Vector3(mouseWorld.x, mouseWorld.y, startPos.z) + grabOffset;
        transform.position = target;
    }

    void OnMouseUp()
    {
        isDragging = false; //no more dragging when no mouse

        //check if texts are overlapping
        //if they do overlap, then initialpos
        if (partner && GetComponent<Collider2D>().bounds.Intersects(partner.GetComponent<Collider2D>().bounds))
        {
            // swap
            Vector3 temp = partner.transform.position;
            partner.transform.position = startPos;
            transform.position = temp;

            manager.TextsPossiblySwapped();
        }
        else
        {
            transform.position = startPos; //snap back if not colliding and swapping
        }
    }
}


