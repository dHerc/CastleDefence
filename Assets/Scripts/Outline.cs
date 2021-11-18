using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Outline : MonoBehaviour
{
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

		var rt = RenderTexture.GetTemporary(src.width, src.height, 0, RenderTextureFormat.R8);
		cam.targetTexture = rt;

		outlineMaterial.SetTexture("_SceneTex", src);
		outlineMaterial.SetColor("_Color", color);
		Graphics.Blit(rt, dst, outlineMaterial);

		RenderTexture.ReleaseTemporary(rt);
	}
}