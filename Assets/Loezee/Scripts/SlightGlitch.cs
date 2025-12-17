using UnityEngine;
using UnityEngine.UI;

public class SlightGlitch : MonoBehaviour
{
    public Material screenMat;
    public Button glitchButton;
    public Button glitchOff;

    [Header("Shader Property References")]
    public string glitchOnProperty = "_glitchON";
    public string flickerScaleXProperty = "_FlickerScaleX";
    public string glitchStrengthProperty = "_glitchStrength";

    [Header("Flicker Timing")]
    public float flickerScaleX = 0.08f;

    [Header("Glitch Intensity")]
    [Range(0f, 1f)]
    public float glitchStrength = 0.25f;

    void Start()
    {
        if (glitchButton != null) glitchButton.onClick.AddListener(TriggerGlitch);
        if (glitchOff != null) glitchOff.onClick.AddListener(StopGlitch);

        ApplyParams();
        screenMat.SetFloat(glitchOnProperty, 1f);
    }

    void OnValidate()
    {
        ApplyParams();
    }

    void ApplyParams()
    {
        if (screenMat == null) return;

        screenMat.SetFloat(flickerScaleXProperty, flickerScaleX);
        screenMat.SetFloat(glitchStrengthProperty, glitchStrength);
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