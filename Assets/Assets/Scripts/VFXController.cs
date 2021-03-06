﻿using UnityEngine;
using System.Collections;

public class VFXController : MonoBehaviour
{

    public float intensity;
    private Material material;

    // Creates a private material used to the effect
    void Awake()
    {
        material = new Material(Shader.Find("PAW/BWDiffuse"));
    }

    // Postprocess the image
    void OnRenderImage(RenderTexture source, RenderTexture destination)
    {
		//intensity =  Mathf.Abs(Mathf.Sin(Time.time * .2f) );

        material.SetFloat("_bwBlend",intensity);
        Graphics.Blit(source, destination, material);
    }
}