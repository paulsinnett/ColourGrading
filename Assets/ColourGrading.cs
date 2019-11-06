﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[ExecuteInEditMode]
public class ColourGrading : MonoBehaviour
{
    Queue<Texture3D> queue = new Queue<Texture3D>();
    public float transitionTime = 1.0f;
    float interpolation = 0.0f;
    Texture3D defaultGrading3D;
    Texture previous;

    public void SetColourGrading(Texture3D grading)
    {
        queue.Enqueue(grading);
    }

    void Update()
    {
        interpolation += Time.deltaTime;
        if (interpolation > transitionTime)
        {
            interpolation = transitionTime;
            if (queue.Count > 0)
            {
                Texture3D next = queue.Dequeue();

                Texture current =
                    Shader.GetGlobalTexture("_ColourGradingLUT1");

                if (next != current)
                {
                    interpolation = 0.0f;
                    Shader.SetGlobalTexture("_ColourGradingLUT0", current);
                    Shader.SetGlobalTexture("_ColourGradingLUT1", next);
                }
            }
        }

        Shader.SetGlobalFloat(
            "_ColourGradingFade",
            interpolation / transitionTime);
    }

    void Reset()
    {
        CleanUp();
        CreateDefaultGrading();
    }

    void Awake()
    {
        CreateDefaultGrading();
    }

    void CreateDefaultGrading()
    {
        Debug.Assert(previous == null);
        previous = Shader.GetGlobalTexture("_ColourGradingLUT1");

        Texture2D defaultGrading2D =
            Resources.Load<Texture2D>("NullGrading");

        Debug.Assert(defaultGrading2D != null);
        defaultGrading3D = CreateGrading3D(defaultGrading2D);
        InitialiseColourGrading(defaultGrading3D);
    }

    public Texture3D GetDefaultGrading()
    {
        return defaultGrading3D;
    }

    void OnDestroy()
    {
        CleanUp();
    }

    void CleanUp()
    {
        if (previous != null)
        {
            Shader.SetGlobalTexture("_ColourGradingLUT0", previous);
            Shader.SetGlobalTexture("_ColourGradingLUT1", previous);
            previous = null;
        }
        if (Application.isPlaying && defaultGrading3D != null)
        {
            Destroy(defaultGrading3D);
            defaultGrading3D = null;
        }
    }

    public void InitialiseColourGrading(Texture3D grading)
    {
        if (grading == null)
        {
            grading = defaultGrading3D;
        }
        Debug.Assert(grading != null);
        Shader.SetGlobalTexture("_ColourGradingLUT0", grading);
        Shader.SetGlobalTexture("_ColourGradingLUT1", grading);
        Debug.LogFormat("Set colour grading to {0}", grading);
    }

    public static Texture3D CreateGrading3D(Texture2D grading2D)
    {
        Debug.Assert(grading2D != null);
        Debug.Assert(grading2D.width == 256);
        Debug.Assert(grading2D.height == 16);
        Debug.Assert(grading2D.wrapMode == TextureWrapMode.Clamp);
        Debug.Assert(grading2D.filterMode == FilterMode.Bilinear);
        Debug.Assert(grading2D.isReadable);

        Texture3D grading3D =
            new Texture3D(16, 16, 16, TextureFormat.ARGB32, false);

        grading3D.SetPixels(grading2D.GetPixels());
        grading3D.filterMode = FilterMode.Bilinear;
        grading3D.wrapMode = TextureWrapMode.Clamp;
        grading3D.Apply(false, true);
        grading3D.name = grading2D.name;
        return grading3D;
    }
}