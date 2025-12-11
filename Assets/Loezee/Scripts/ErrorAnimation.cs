using UnityEngine;
using UnityEngine.UI;  

public class errorAnimation : MonoBehaviour
{
    public Animator animator;  
    public string triggerName = "playError";  

    [SerializeField] private Button page10button;  

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
        Debug.Log("pressed, playing animation");
        animator.enabled = true;
        animator.SetTrigger(triggerName);
    }
}