using UnityEngine;
using UnityEngine.UI;

public class phoneCall : MonoBehaviour
{
    public Button startCall;
    public AudioClip callSound;
    public AudioSource audioSource;
    
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
        //CREEPY GLITCH EFFECT
        // if (startCall.onClick.GetPersistentEventCount() > 0)
        // {
        //     audioSource.PlayOneShot(callSound);
        // }
    }

    public void playSound()
    {
        audioSource.PlayOneShot(callSound);
    }
}
