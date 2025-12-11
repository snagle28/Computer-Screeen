using UnityEngine;
using UnityEngine.UI;

public class errorAnimation : MonoBehaviour
{
    public Animator animator;
    public string triggerName = "PlayErrorAnimation";

    [SerializeField] private Button page10button;
    
    public AudioSource audioSource;
    public AudioClip errorSound;

    void Start()
    {
        if (page10button != null)
        {
            page10button.onClick.AddListener(PlayAnimation);
        }

        if (animator == null)
        {
            animator = GetComponent<Animator>();
        }


        animator.enabled = false; 
    }


    void PlayAnimation()
    {
        animator.enabled = true;
        animator.SetTrigger(triggerName);
        audioSource.PlayOneShot(errorSound);
    }
}