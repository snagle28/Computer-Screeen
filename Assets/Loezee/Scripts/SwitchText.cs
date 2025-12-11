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
    
    public bool finalSwapDone = false;

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
        if (isController)
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
        if (finalSwapDone) return; // can't switch after you're done
        
        if (imageStage == 2)
            return;
        
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
            if (baseSwapDone)
            {
                return;  // Skip this swap to prevent revert
            }
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
            checkFinal();
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

    public void checkFinal()
    {
        if (imageStage == 2)
        {
            Debug.Log("Final swap done");
            finalSwapDone = true;
        }
    }
}
