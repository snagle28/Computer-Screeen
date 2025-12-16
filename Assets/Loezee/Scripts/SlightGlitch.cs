using UnityEngine;
using UnityEngine.UI;

public class SlightGlitch : MonoBehaviour
{
    public Material screenMat;
    public Button glitchButton;
    public Button glitchOff;

    [Header("Shader Properties (must match Shader Graph Reference exactly)")]
    public string glitchOnProperty = "_glitchON";
    public string flickerScaleXProperty = "_FlickerScaleX";

    [Header("Flicker Settings")]
    [Tooltip("Gradient Noise Scale X in the Flickering block.")]
    public float flickerScaleX = 50f;

    void Start()
    {
        if (glitchButton != null) glitchButton.onClick.AddListener(TriggerGlitch);
        if (glitchOff != null) glitchOff.onClick.AddListener(StopGlitch);

        ApplyFlickerX();

        screenMat.SetFloat(glitchOnProperty, 1f);
    }

    void OnValidate()
    {
        ApplyFlickerX();
    }

    void ApplyFlickerX()
    {
        if (screenMat == null) return;
        screenMat.SetFloat(flickerScaleXProperty, flickerScaleX);
    }

    void TriggerGlitch()
    {
        if (screenMat == null) return;
        screenMat.SetFloat(glitchOnProperty, 1f);
    }

    void StopGlitch()
    {
        if (screenMat == null) return;
        screenMat.SetFloat(glitchOnProperty, 0f);
    }

    void OnApplicationQuit()
    {
        StopGlitch();
    }
}