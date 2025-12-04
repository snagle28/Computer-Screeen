using System.Collections.Generic;
using UnityEngine;

public class FinalAnimation : MonoBehaviour
{
    private int timer = 0;

    public int animationDelay;
    public List <GameObject> objectsToGltch;
    [SerializeField] private int TimerMinimum;
    [SerializeField] private int TimerMaximum;

    public Material GlitchMaterial;
    
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        timer++;
        if (timer >= animationDelay)
        {
            UltimateEnvironmentGlitch();
        }
        
    }

    void UltimateEnvironmentGlitch()
    {
        foreach (GameObject obj in objectsToGltch)
        {
            
            Material myMat = obj.GetComponent<MeshRenderer>().material;
            Material ogMat = myMat;
            
            var WaitTimerMax = Random.Range(TimerMinimum, TimerMaximum);
            var WaitTimer = 0;
            var GlitchTimerMax = Random.Range(TimerMinimum, TimerMaximum);
            var GlitchTimer = 0;
            
            WaitTimer++;
            print(WaitTimer);
            if (WaitTimer >= WaitTimerMax)
            {
                GlitchTimer++;
                if (GlitchTimer <= GlitchTimerMax)
                {
                    print("trying to set material to glitch material");
                    myMat = GlitchMaterial;
                }
                else
                {
                    myMat = ogMat;
                }
            }

        }
    }
}
