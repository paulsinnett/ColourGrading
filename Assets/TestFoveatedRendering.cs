using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TestFoveatedRendering : MonoBehaviour
{
    bool applyingFFV = false;

    void Update()
    {
        if (OVRInput.GetDown(OVRInput.Button.PrimaryIndexTrigger))
        {
            applyingFFV = !applyingFFV;
            OVRManager.fixedFoveatedRenderingLevel =
                applyingFFV?
                    OVRManager.FixedFoveatedRenderingLevel.High :
                    OVRManager.FixedFoveatedRenderingLevel.Off;
        }
    }
}
