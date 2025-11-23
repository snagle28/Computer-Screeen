// using UnityEngine;
// using UnityEngine.UI;
// using UnityEngine.EventSystems;
//
// public class SwitchText : MonoBehaviour
// {
//     //can be set in only one of the texts (textA1) that's the controller
//     [Header("Controller Settings")]
//     public bool isController = false; 
//
//     [Header("Text objects")]
//     public SwitchText text1;
//     public SwitchText text2;
//     public SwitchText text3;
//
//     [Header("Images")]
//     public SpriteRenderer image1;
//     public SpriteRenderer image2;
//     public SpriteRenderer image3;
//
//     private Button button;
//     //is the text selected and highlighted?
//     private bool isSelected = false;
//
//     //static = shared across all instances of SwitchText
//     private static SwitchText controller;
//
//
//     //selectedA = 1st selected text
//     //selectedB = 2nd selected text
//     private SwitchText selectedA;
//     private SwitchText selectedB;
//
//     // 0 = image1 
//     // 1 = image2 
//     // 2 = image3
//     //imageStage is tracking which image is visible
//     private int imageStage = 0;
//
//     //is image1 switched with image2 yet?
//     //not yet
//     private bool baseSwapDone = false;
//
//     void Awake()
//     {
//         button = GetComponent<Button>();
//
//         if (isController)
//         {
//             //give textA1 the controller
//             controller = this;
//             //start with image1
//             imageStage = 0;
//             //textA1 has not switched with textA2 yet
//             baseSwapDone = false;
//             //imageA1 on and imageA2-3 off
//             UpdateImagesSwitch();
//         }
//     }
//
//     void Update()
//     {
//         //if ur not the controller, don't do anything
//         //only the controller will listen for "enter" and swap
//         if (!isController) return;
//
//         //press enter = swap
//         if (Input.GetKeyDown(KeyCode.Return))
//         {
//             TrySwapSelected();
//         }
//     }
//
//     void OnClick()
//     {
//         //tells controller that this text has been clicked
//         controller.ToggleSelect(this);
//     }
//
//     private void ToggleSelect(SwitchText selected)
//     {
//         //no need to do anything to text3 if the base swap (text 1 and text2) are not switched yet
//         if (!baseSwapDone && selected == text3)
//         {
//             return;
//         }
//
//         //if either selectedA or selectedB has a text, then when they're clicked again, unselect it
//         if (selectedA == selected)
//         {
//             selected.SetSelected(false);
//             selectedA = null;
//             return;
//         }
//         if (selectedB == selected)
//         {
//             selected.SetSelected(false);
//             selectedB = null;
//             return;
//         }
//
//         ///if both selectedA and selectedB have a text, then when player clicks the 3rd text, return so nothing happens 
//         //prevents 3 different texts from being swapped (confusing)
//         if (selectedA != null && selectedB != null)
//         {
//             return;
//         }
//
//         //if selectedA and selectedB are empty, then players can click the texts
//         if (selectedA == null)
//         {
//             selectedA = selected;
//             selected.SetSelected(true);
//         }
//         else if (selectedB == null && selectedA != selected)
//         {
//             selectedB = selected;
//             selected.SetSelected(true);
//         }
//     }
//
//     private void TrySwapSelected()
//     {
//         //no swap if 2 texts arent selected
//         if (selectedA == null || selectedB == null)
//         {
//             return;
//         }
//
//         //which texts are selected?
//         bool hasText1 = (selectedA == text1 || selectedB == text1);
//         bool hasText2 = (selectedA == text2 || selectedB == text2);
//         bool hasText3 = (selectedA == text3 || selectedB == text3);
//
//         //is baseSwapDone true yet?
//         //baseSwapDone is asking if text1 and text2 are swapped yet
//         if (!baseSwapDone)
//         {
//             //only text1 and text2 are included, not text3
//             if (hasText1 && hasText2 && !hasText3)
//             {
//                 //swap the texts
//                 DoSwap(selectedA, selectedB);
//                 //baseSwapDone is now true
//                 baseSwapDone = true;
//                 //now swap to image2 because text1 and text2 are now swapped
//                 SwitchImage1ToImage2();  
//             }
//
//             return;
//         }
//
//         //after baseSwapDone is true, all texts can be selected and swapped in pairs
//         bool includes12 = hasText1 && hasText2 && !hasText3; //text 1 and 2
//         bool includes13 = hasText1 && hasText3 && !hasText2; //text 1 and 3
//         bool includes23 = hasText2 && hasText3 && !hasText1; //text 2 and 3
//
//         if (includes12)
//         {
//             //no image change
//             DoSwap(selectedA, selectedB);
//         }
//         else if (includes13)
//         {
//             //if images 1 and 3 are swapped, then switch image 2 to image 3
//             DoSwap(selectedA, selectedB);
//             SwitchImage2toImage3();
//         }
//         else if (includes23)
//         {
//             //no image change
//             DoSwap(selectedA, selectedB);
//         }
//     }
//
//     private void DoSwap(SwitchText a, SwitchText b)
//     {
//         //swap the positions
//         //pos is selectedA position
//         Vector3 pos = a.transform.position;
//         //switch selectedA pos with selectedB pos
//         a.transform.position = b.transform.position;
//         b.transform.position = pos;
//
//         //clear the highlight/selection after the switch
//         ClearSelection();
//     }
//
//     private void SwitchImage1ToImage2()
//     {
//         //if it's on image1, switch to image2
//         if (imageStage == 0)
//         {
//             imageStage = 1;
//             //update sprite renderer
//             UpdateImagesSwitch();
//         }
//     }
//
//     private void SwitchImage2toImage3()
//     {
//         //if it's on image2, switch to image3
//         if (imageStage == 1)
//         {
//             imageStage = 2;
//             //update sprite renderer
//             UpdateImagesSwitch();
//         }
//     }
//
//     //makes sure only 1 image is on, rest is off
//     //imageStage can only be 0,1,2 (only one of the images), so when one image is on, only that one is true
//     private void UpdateImagesSwitch()
//     {
//         image1.enabled = (imageStage == 0);
//         image2.enabled = (imageStage == 1);
//         image3.enabled = (imageStage == 2);
//     }
//
//     private void ClearSelection()
//     {
//         //if selected/clicked on, then unselect if clicked on again
//         if (selectedA != null) selectedA.SetSelected(false);
//         if (selectedB != null) selectedB.SetSelected(false);
//
//         selectedA = null;
//         selectedB = null;
//     }
//
//     public void SetSelected(bool selected)
//     {
//         isSelected = selected;
//
//         //if selected, then highlight yellow, if not, then make it white
//         if (isSelected)
//         {
//             sr.color = new Color(1f, 1f, 0.6f, 1f);
//         }
//         else
//         {
//             sr.color = Color.white;
//         }
//     }
// }
//

//UNITY UI VERSION WITH BUTTONS

using UnityEngine;
using UnityEngine.UI;

public class SwitchText : MonoBehaviour
{
    [Header("Controller Settings")]
    public bool isController = false;

    [Header("Text Objects")]
    public SwitchText text1;
    public SwitchText text2;
    public SwitchText text3;

    [Header("Images (Stages)")]
    public Image image1;
    public Image image2;
    public Image image3;

    // UI button component
    private Button button;

    // Selection state
    public bool isSelected = false;

    // Shared controller reference
    private static SwitchText controller;

    // Clicked selections
    private SwitchText selectedA;
    private SwitchText selectedB;

    // Track which image is active
    private int imageStage = 0;

    // Check if 1 & 2 have swapped already
    private bool baseSwapDone = false;

    void Awake()
    {
        button = GetComponent<Button>();

        if (isController)
        {
            controller = this;
            imageStage = 0;
            baseSwapDone = false;
            UpdateImages();
        }
    }

    // Called by UI Button OnClick()
    public void OnClick()
    {
        controller.ToggleSelect(this);
    }

    void Update()
    {
        if(isController && Input.GetKeyDown(KeyCode.Space))
        {
            TrySwapSelected();
        }
    }

    private void ToggleSelect(SwitchText selected)
    {
        if (!baseSwapDone && selected == text3)
            return;

        if (selectedA == selected)
        {
            selected.SetSelected(false);
            selectedA = null;
            return;
        }

        if (selectedB == selected)
        {
            selected.SetSelected(false);
            selectedB = null;
            return;
        }

        if (selectedA != null && selectedB != null)
            return;

        if (selectedA == null)
        {
            selectedA = selected;
            selected.SetSelected(true);
        }
        else if (selectedB == null && selectedA != selected)
        {
            selectedB = selected;
            selected.SetSelected(true);
        }
    }

    // Called by SWAP button via OnClick() event
    public void TrySwapSelected()
    {
        if (selectedA == null || selectedB == null)
            return;

        bool hasText1 = (selectedA == text1 || selectedB == text1);
        bool hasText2 = (selectedA == text2 || selectedB == text2);
        bool hasText3 = (selectedA == text3 || selectedB == text3);

        // First required swap: 1 ↔ 2
        if (!baseSwapDone)
        {
            if (hasText1 && hasText2 && !hasText3)
            {
                DoSwap(selectedA, selectedB);
                baseSwapDone = true;
                SwitchImage1To2();
                Debug.Log("SWAPPED 1 & 2 — baseSwapDone is now TRUE");
            }
            return;
        }

        bool includes12 = hasText1 && hasText2 && !hasText3;
        bool includes13 = hasText1 && hasText3 && !hasText2;
        bool includes23 = hasText2 && hasText3 && !hasText1;

        if (includes12)
        {
            DoSwap(selectedA, selectedB);
        }
        else if (includes13)
        {
            DoSwap(selectedA, selectedB);
            SwitchImage2To3();
        }
        else if (includes23)
        {
            DoSwap(selectedA, selectedB);
        }
    }

    private void DoSwap(SwitchText a, SwitchText b)
    {
        Vector3 pos = a.transform.position;
        a.transform.position = b.transform.position;
        b.transform.position = pos;
        ClearSelection();
    }

    private void SwitchImage1To2()
    {
        if (imageStage == 0)
        {
            imageStage = 1;
            UpdateImages();
        }
    }

    private void SwitchImage2To3()
    {
        if (imageStage == 1)
        {
            imageStage = 2;
            UpdateImages();
        }
    }

    private void UpdateImages()
    {
        image1.enabled = (imageStage == 0);
        image2.enabled = (imageStage == 1);
        image3.enabled = (imageStage == 2);
    }

    private void ClearSelection()
    {
        if (selectedA != null) selectedA.SetSelected(false);
        if (selectedB != null) selectedB.SetSelected(false);

        selectedA = null;
        selectedB = null;
    }

    public void SetSelected(bool selected)
    {
        isSelected = selected;
        Image img = button.GetComponent<Image>();

        if (isSelected)
            img.color = new Color(1f, 1f, 0.6f, 1f); // Highlight
        else
            img.color = Color.white;
    }
}
