using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ColourGrading : MonoBehaviour
{
	public Material colourGrading;

	void OnRenderImage(RenderTexture source, RenderTexture destination)
	{
		// Copy the source Render Texture to the destination,
		// applying the material along the way.
		Graphics.Blit(source, destination, colourGrading);
	}

	void Start()
	{
		Texture2D texture = colourGrading.GetTexture("_LUT") as Texture2D;
		Color colour = texture.GetPixel(0, 0);
		Debug.Log(colour);
	}
}