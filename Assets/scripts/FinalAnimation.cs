using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FinalAnimation : MonoBehaviour
{
    private int timer = 0;

    [SerializeField] private int animationDelay = 0; 

    [Header("target objects (must have mesh renderer and material)")]
    [SerializeField] private List<GameObject> objectsToGlitch = new List<GameObject>(); 

    [Header("glitch material (use constant glitch ONLY!!)")]
    [SerializeField] private Material glitchMaterial; 

    [Header("timing Settings")]
    [SerializeField] private float minInterval = 0.1f; 
    [SerializeField] private float maxInterval = 0.5f; 
    [SerializeField] private float minGlitchDuration = 0.05f; 
    [SerializeField] private float maxGlitchDuration = 0.2f; 

    [Header("count settings, controls how many objects are glitching")]
    [SerializeField] private int minPerGlitch = 1; 
    [SerializeField] private int maxPerGlitch = 3; 

    private Dictionary<GameObject, Material> originalMaterials = new Dictionary<GameObject, Material>(); // For tracking original materials
    private Dictionary<GameObject, MeshRenderer> objectRenderers = new Dictionary<GameObject, MeshRenderer>(); // For quick access to MeshRenderer components
    private Coroutine glitchRoutine;
    private bool hasStartedGlitching = false;

    public bool readyForFinalAnim = false;

    void Start()
    {
        // Initialize materials and components for each object
        foreach (var obj in objectsToGlitch)
        {
            //first pass: is it NOT null
            if (obj == null)
            {
                //skip
                continue;
            }
            //then does it have a mesh renderer?
            MeshRenderer renderer = obj.GetComponent<MeshRenderer>();
            if (renderer == null)
            {
                continue;
            }
            //what abt a mat
            Material originalMat = renderer.material;
            if (originalMat == null)
            {
                continue;
            }

            originalMaterials[obj] = originalMat; //assign value to key
            objectRenderers[obj] = renderer;
        }

        if (objectRenderers.Count == 0)
        {
            Debug.LogError("No valid objects initialized for glitching! Check your objectsToGlitch list.");
        }
    }

    void Update()
    {
        
        if (!hasStartedGlitching && readyForFinalAnim)
        {
            StartGlitching();
            hasStartedGlitching = true;
        }
    }

    public void StartGlitching()
    {
        if (glitchRoutine != null) StopCoroutine(glitchRoutine);
        glitchRoutine = StartCoroutine(GlitchLoop());
    }

    public void StopGlitching()
    {
        if (glitchRoutine != null) StopCoroutine(glitchRoutine);

        // Revert all to original materials
        foreach (var kvp in objectRenderers)
        {
            if (originalMaterials.TryGetValue(kvp.Key, out Material originalMat))
            {
                kvp.Value.material = originalMat;
                Debug.Log($"Reverted {kvp.Key.name} to original material.");
            }
        }
    }

    private IEnumerator GlitchLoop()
    {
        while (true)
        {
            // Wait for random interval
            yield return new WaitForSeconds(Random.Range(minInterval, maxInterval)); 
            /*
             * Notes on yield for later:
             * yield is used inside coroutines to pause execution and hand off control back to unity.
             * its like saying stop, run update and stuff, and then come back to me later
             */

            // Pick how many to glitch
            int count = Random.Range(minPerGlitch, maxPerGlitch + 1);

            // Shuffle the list to randomly select unique objects (avoids duplicates in one event)
            List<GameObject> shuffled = new List<GameObject>(objectsToGlitch);
            shuffled.Sort((a, b) => Random.Range(-1, 2)); // Simple shuffle

            for (int i = 0; i < count && i < shuffled.Count; i++)
            {
                GameObject obj = shuffled[i];
                if (objectRenderers.TryGetValue(obj, out MeshRenderer renderer) &&
                    originalMaterials.TryGetValue(obj, out Material originalMat))
                {
                    StartCoroutine(ApplyGlitch(renderer, originalMat));
                }
                else
                {
                    //somehhing is up, missing component/material
                }
            }
        }
    }

    private IEnumerator ApplyGlitch(MeshRenderer renderer, Material originalMat)
    /*
     * use timer to set material to glitch material for a bit, and then set it back.
     */
    {
        if (renderer == null || glitchMaterial == null)
        {
            yield break;
        }
        
        renderer.material = glitchMaterial;
        yield return new WaitForSeconds(Random.Range(minGlitchDuration, maxGlitchDuration));
        renderer.material = originalMat;
    }

    void OnApplicationQuit() //or else it won't reset, had same issue for screen glitch
    {
        StopGlitching();
    }
}

