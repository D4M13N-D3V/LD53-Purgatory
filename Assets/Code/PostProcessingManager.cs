using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PostProcessingManager : MonoBehaviour
{
    [SerializeField]
    List<GameObject> postProcessingPresets;

    Camera mainCamera;

    void Start()
    {
        mainCamera = Camera.main;
    }

    public void SetPostProcessing(int index)
    {
        if (index < 0 || index >= postProcessingPresets.Count)
            return;

        foreach (var postProcessingPreset in postProcessingPresets)
        {
            postProcessingPreset.SetActive(false);
        }

        postProcessingPresets[index].SetActive(true);
    }
}
