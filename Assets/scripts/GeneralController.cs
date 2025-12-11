using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Serialization;
using UnityEngine.UI;

public class GeneralController : MonoBehaviour
{
    
    
    public SwitchText2 pairSwapScript;
    public SwitchText tplSwapScript;
    
    
    [Header("All SwitchText scripts")]
    public List<SwitchText> tplSwapScripts;

    [FormerlySerializedAs("tplSwapScripts")] [Header("All SwitchText2 scripts")]
    public List<SwitchText2> pairSwapScripts;

    // This can be used to know if everything is complete
    public bool allPuzzlesComplete = false;

    [Header("Put the invisible button for the next page here. Must be inactive.")]
    public Button nextPageButton;

    [Header("Put the GameObject that you want to cover the button here. It should have an image or sprite component attached.")]
    public GameObject buttonConcealer;
    
    public AudioSource audioSource;
    public AudioClip allPuzzlesSolved;

    private bool hasPlayedSound = false;
    // Update is called once per frame
    void Update()
    {
        if (DoneAllPuzzles() == true)
        {
            if (!hasPlayedSound)
            {
                audioSource.PlayOneShot(allPuzzlesSolved);
                hasPlayedSound = true;
            }
            Image myImg = nextPageButton.GetComponent<Image>();
            GameObject buttonOBj = nextPageButton.gameObject;
            buttonOBj.SetActive(true);
            buttonConcealer.SetActive(false);

        }
        
    }


    public bool DoneAllPuzzles()
    {
        
        bool pairsDone = true;
        bool tplsDone = true;
        
        foreach (SwitchText2 pairSwapScript in pairSwapScripts)
        {
            if (!pairSwapScript.puzzleDone)
            {
                pairsDone = false;
                break;
            }
        }

        if(tplSwapScript != null)
        {
           foreach (SwitchText tplSwapScript in tplSwapScripts)
           {
               if (!tplSwapScript.finalSwapDone)
               {
                   tplsDone = false;
                   break;
               }
           }
        }

        print("pairs done: " + pairsDone);
        print("tpls done: " + tplsDone);
        if (pairsDone && tplsDone)
        {
            return true;
        }
        else
        {
            return false;
        }
        
    }
    
    
}
