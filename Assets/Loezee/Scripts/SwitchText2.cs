using UnityEngine;

public class SwitchText2 : MonoBehaviour
{
    //assign a partner to each text
    public SwitchText2 partner;  

    [Header("Images for this pair")]
    public SpriteRenderer image4;   
    public SpriteRenderer image5;   

    private SpriteRenderer sr;
    private bool isSelected = false;

    //static = shared by the pair so image only switches once
    private static bool imageSwitched = false;

    void Awake()
    {
        sr = GetComponent<SpriteRenderer>();
    }

    void Update()
    {
        //swap when 2 are selected and enter is pressed
        if (isSelected && partner.isSelected)
        {
            if (Input.GetKeyDown(KeyCode.Return))
            {
                SwapPositions();
            }
        }
    }

    void OnMouseDown()
    {
        // toggle selection state
        isSelected = !isSelected;
        SetSelected();
    }

    private void SwapPositions()
    {
        //swap positions
        Vector3 pos = transform.position;
        transform.position = partner.transform.position;
        partner.transform.position = pos;

        //reset the selection
        isSelected = false;
        partner.isSelected = false;

        //turn the color back to white/unselected
        SetSelected();
        partner.SetSelected();

        //just switch imageB1 and imageB2 once
        if (!imageSwitched && image4 != null && image5 != null)
        {
            image4.enabled = false;
            image5.enabled = true;
            imageSwitched = true;
        }
    }

    private void SetSelected()
    {
        //yellow for highlights, white for unselected
        if (isSelected)
        {
            sr.color = new Color(1f, 1f, 0.6f, 1f);  
        }
        else
        {
            sr.color = Color.white;    
        }
    }
}