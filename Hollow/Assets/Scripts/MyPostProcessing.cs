using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MyPostProcessing : MonoBehaviour
{
    public Material postprocessing;

    private void OnRenderImage(RenderTexture source, RenderTexture destination)
    {
        Graphics.Blit(source, destination, postprocessing);
    }
}
