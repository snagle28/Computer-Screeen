using UnityEngine;
using UnityEngine.UI;

public class GlitchController : MonoBehaviour
{
    public Material screenMat;
    public Button glitchButton;

    void Start()
    {
        //add listener tells the button to call the function when clicked 
        glitchButton.onClick.AddListener(TriggerGlitch);
    }
    
    void TriggerGlitch()
    {
        screenMat.SetFloat("_glitchON", 1f);
    }

    void StopGlitch()
    {
        screenMat.SetFloat("_glitchON", 0f);
    }

    void OnMouseExit()
    {
        
    }
    
}
