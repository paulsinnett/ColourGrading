using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ColourGrading : MonoBehaviour
{
	public Texture2D nullLUT;
	public Texture2D colourGradingLUT;
	public Material colourGrading;

	void OnRenderImage(RenderTexture source, RenderTexture destination)
	{
		// Copy the source Render Texture to the destination,
		// applying the material along the way.
		Graphics.Blit(source, destination, colourGrading);
	}

	void Update()
	{
		if (Input.GetKeyDown(KeyCode.Space) ||
			OVRInput.GetDown(OVRInput.Button.PrimaryIndexTrigger))
		{
			if (colourGrading.GetTexture("_LUT") == nullLUT)
			{
				Debug.LogFormat(
					"Switching to texture {0}",
					colourGradingLUT);

				colourGrading.SetTexture("_LUT", colourGradingLUT);
			}
			else
			{
				Debug.LogFormat(
					"Switching to texture {0}",
					nullLUT);

				colourGrading.SetTexture("_LUT", nullLUT);
			}
		}
	}
}