using UnityEngine;
using UnityEngine.UI;
/*
 * In order to set this up: have two button objects with this script attached.
 * Assign each other as partners in the inspector.
 * Assign the display images to each button (one active, one inactive).
 * Assign one button as the controller.
 */

public class SwitchText2 : MonoBehaviour
{
    //assign a partner to each text
    public SwitchText2 partner;

    [Header("Images for this pair")] 
    public GameObject displayImage1;
    public GameObject displayImage2; //starts inactive

    public bool isController;

    private Button button;
    private bool isSelected = false;

    public bool puzzleDone = false;

    //static = shared by the pair so image only switches once
    private static bool imageSwitched = false;

    void Awake()
    {
        button = GetComponent<Button>();
    }
    //
    // void Update()
    // {
    //     //swap when 2 are selected and enter is pressed
    //     if (isSelected && partner.isSelected)
    //     {
    //         if (Input.GetKeyDown(KeyCode.Return))
    //         {
    //             SwapPositions();
    //         }
    //     }
    // }

    public void OnClick()
    {
        // toggle selection state
        isSelected = !isSelected;
        SetSelected();
        if (isSelected && partner != null && partner.isSelected)
        {
            SwapPositions();
        }
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

         // Swap the visible sprite between the two display Image components (only once if desired)
        if (!imageSwitched && displayImage1 != null && displayImage2 != null && isController)
        {
            if (displayImage1.activeSelf)
            {
                displayImage1.SetActive(false);
            }
            else
            {
                displayImage1.SetActive(true);
            }

            if (!displayImage2.activeSelf)
            {
                displayImage2.SetActive(true);
            }
            else
            {
                displayImage2.SetActive(false);
            }
           // imageSwitched = true;
           puzzleDone = true;
        }
    }

    private void SetSelected()
    {
        // Highlight yellow when selected, white when not
        Image img = button.GetComponent<Image>();
        if (isSelected)
            img.color = new Color(1f, 1f, 0.6f, 1f);
        else
            img.color = Color.white;
    }
}