using UnityEngine;
using UnityEngine.UI;

public class errorAnimation : MonoBehaviour
{
    public Animator animator;  
    public string triggerName = "playError";  

    [SerializeField] 
    private Button page10button;  
    [SerializeField]
     private GameObject page10;    
    [SerializeField] 
    private GameObject page11;   
     
    private bool animationPlaying = false;

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

    void Update()
    {
        //checking to see if animation has ended
        if (animationPlaying)
        {
            //what's the current animation state
            AnimatorStateInfo stateInfo = animator.GetCurrentAnimatorStateInfo(0);

            //is the animation finished? if >= 1f, then yes
            if (stateInfo.IsName("errorclip") && stateInfo.normalizedTime >= 1.0f)
            {
                animationPlaying = false;  //stop checking

                //go from page10 to page11
                SwitchPages();
            }
        }
    }

    void PlayAnimation()
    {
        animator.enabled = true;  
        animator.SetTrigger(triggerName);  
        animationPlaying = true;  //start monitoring the animation
    }

    //switch from page10-11
    void SwitchPages()
    {
        page10.SetActive(false);  //disable page10
        page11.SetActive(true);   //activate page11
    }
}