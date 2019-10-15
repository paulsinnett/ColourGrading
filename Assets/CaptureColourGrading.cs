using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;

public class CaptureColourGrading : MonoBehaviour
{
    [ContextMenu("Capture Texture")]
    void Capture()
    {
        Camera camera = GetComponent<Camera>();
        camera.Render();

        Texture2D image = new Texture2D(camera.targetTexture.width, camera.targetTexture.height);
        image.ReadPixels(new Rect(0, 0, camera.targetTexture.width, camera.targetTexture.height), 0, 0);
        image.Apply();
        File.WriteAllBytes("CapturedLUT.png", image.EncodeToPNG());
    }
}
