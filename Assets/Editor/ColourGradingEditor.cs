using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEditor;
using UnityEngine;

[CustomEditor(typeof(ColourGrading))]
public class ColourGradingEditor : Editor
{
    public override void OnInspectorGUI()
    {
        DrawDefaultInspector();

        ColourGrading mp = (ColourGrading) target;
        if (GUILayout.Button("Generate Default LUT"))
        {
            Texture2D lut =
                new Texture2D(256, 16, TextureFormat.RGB24, false, true);

            Color[] pixels = lut.GetPixels();

            for (int r = 0; r < 16; ++r)
            {
                for (int g = 0; g < 16; ++g)
                {
                    for (int b = 0; b < 16; ++b)
                    {
                        pixels[b * 16 + g * 256 + r] =
                            new Color(r / 15.0f, g / 15.0f, b / 15.0f);
                    }
                }
            }

            lut.SetPixels(pixels);
            lut.Apply(false);
            
            string path =
                Path.Combine(Application.dataPath, "NullLUT.png");

            File.WriteAllBytes(path, ImageConversion.EncodeToPNG(lut));
            AssetDatabase.ImportAsset(path);
        }
    }
}