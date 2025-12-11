using UnityEngine;

public class AudioManager : MonoBehaviour
{
    
    public AudioSource audioSource;

    public AudioClip clickSound;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetMouseButtonDown(0))
        {
            audioSource.PlayOneShot(clickSound);
        }
    }
}
