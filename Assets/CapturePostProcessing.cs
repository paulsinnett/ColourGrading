using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEditor;
using UnityEngine;

public class CapturePostProcessing : MonoBehaviour
{
#if UNITY_EDITOR
    [ContextMenu("Export To PNG")]
    void ExportFrameToPng()
    {
        string path = EditorUtility.SaveFilePanel("Export PNG...", "", "Frame", "png");

        if (string.IsNullOrEmpty(path))
            return;

        EditorUtility.DisplayProgressBar("Export PNG", "Rendering...", 0f);

        var camera = GetComponent<Camera>();
        var w = camera.pixelWidth;
        var h = camera.pixelHeight;

        var texOut = new Texture2D(w, h, TextureFormat.RGBAFloat, false, true);
        var target = RenderTexture.GetTemporary(w, h, 24, RenderTextureFormat.ARGBFloat, RenderTextureReadWrite.Linear);

        var lastActive = RenderTexture.active;
        var lastTargetSet = camera.targetTexture;

        camera.targetTexture = target;
        camera.Render();
        camera.targetTexture = lastTargetSet;

        EditorUtility.DisplayProgressBar("Export PNG", "Reading...", 0.25f);

        RenderTexture.active = target;
        texOut.ReadPixels(new Rect(0, 0, w, h), 0, 0);
        texOut.Apply();
        RenderTexture.active = lastActive;

        EditorUtility.DisplayProgressBar("Export PNG", "Encoding...", 0.5f);

        var bytes = ImageConversion.EncodeToPNG(texOut);

        EditorUtility.DisplayProgressBar("Export PNG", "Saving...", 0.75f);

        File.WriteAllBytes(path, bytes);

        EditorUtility.ClearProgressBar();
        AssetDatabase.Refresh();

        RenderTexture.ReleaseTemporary(target);
        DestroyImmediate(texOut);
    }
#endif
}