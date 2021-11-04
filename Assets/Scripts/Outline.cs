using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Outline : MonoBehaviour
{
	public Shader DrawAsSolidColor;
	public Shader OutlineShader;
	Material outlineMaterial;
	public Color color;
	Camera cam;

	void Start()
	{
		outlineMaterial = new Material(OutlineShader);
		cam = new GameObject().AddComponent<Camera>();
	}

	void OnRenderImage(RenderTexture src, RenderTexture dst)
	{
		cam.CopyFrom(Camera.current);
		cam.backgroundColor = Color.black;
		cam.clearFlags = CameraClearFlags.Color;

		cam.cullingMask = 1 << LayerMask.NameToLayer("Outline");

		var rt = RenderTexture.GetTemporary(src.width, src.height);
		cam.targetTexture = rt;

		//cam.RenderWithShader(DrawAsSolidColor, "");

		outlineMaterial.SetTexture("_SceneTex", src);
		outlineMaterial.SetColor("_Color", color);
		Graphics.Blit(rt, dst, outlineMaterial);

		RenderTexture.ReleaseTemporary(rt);
	}
}