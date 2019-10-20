using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TestColourGrading : MonoBehaviour
{
    public Texture2D grading2D;
    Texture3D grading3D;
    bool applyingGrading = false;
    ColourGrading colourGrading;

    void Start()
    {
        Debug.Assert(grading2D != null);
        grading3D = ColourGrading.CreateGrading3D(grading2D);
        colourGrading = FindObjectOfType<ColourGrading>();
        Debug.Assert(colourGrading != null);
    }

    void OnDestroy()
    {
        Debug.Assert(grading3D != null);
        Destroy(grading3D);
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Space) ||
            OVRInput.GetDown(
                OVRInput.Button.PrimaryIndexTrigger,
                OVRInput.Controller.LTouch))
        {
            applyingGrading = !applyingGrading;

            colourGrading.SetColourGrading(
                applyingGrading? grading3D : null);
        }
    }
}
