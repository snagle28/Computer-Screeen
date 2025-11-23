using UnityEngine;
using UnityEngine.UI;

public class AnimatorTrigger : MonoBehaviour
{
    public GameObject page0;
    public GameObject page1;
    public Animator animator;

    
    public Material staticMaterial;
    
    Image page0Image;
    
    int timer = 0;

    void Start()
    {
        page0Image = page0.GetComponent<Image>();
    }
        
    public void SetActiveStuff()
    {
        
            page0.SetActive(false);
            page1.SetActive(true);
        
    }
    
    public void setMaterial()
    {
        page0Image.material = staticMaterial;
    }

    void Update()
    {
        AnimatorStateInfo stateInfo = animator.GetCurrentAnimatorStateInfo(0);
        
        if (stateInfo.IsName("movingOut") && stateInfo.normalizedTime >= 3.5f)
        {
            SetActiveStuff();
        }
    }
}
