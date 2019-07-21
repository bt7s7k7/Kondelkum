using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WorldSwitcher : MonoBehaviour {
	public Material worldAWindowMaterial;
	public Material worldBWindowMaterial;
	public Camera playerCamera;
	public Camera worldACamera;
	public Camera worldBCamera;

	public string worldALayer = "WorldA";
	public string worldBLayer = "WorldB";
	public bool isInBWorld = false;


	[B.MethodButton("Switch")]
	public void Switch() {
		var isLayer = LayerMask.NameToLayer(isInBWorld ? worldALayer : worldBLayer);
		gameObject.layer = isLayer;
		playerCamera.cullingMask = isInBWorld ? worldACamera.cullingMask : worldBCamera.cullingMask;
		isInBWorld = !isInBWorld;
	}

	public GameObject player;

	public Vector2Int prevScreenSize;
	public RenderTexture wordlARender;
	public RenderTexture worldBRender;
	public float renderScale = 1;

	private void Update() {
		var newScreenSize = new Vector2Int(Screen.width, Screen.height);
		if (newScreenSize != prevScreenSize) {
			ResetRenderTextures();
			prevScreenSize = newScreenSize;
		}
	}

	[B.MethodButton("Reset render texures")]
	public void ResetRenderTextures() {
		Debug.Log("Reseting render textures");
		var screenSize = new Vector2Int(Screen.width, Screen.height);
		if (wordlARender) wordlARender.Release();
		if (worldBRender) worldBRender.Release();

		wordlARender = new RenderTexture(screenSize.x, screenSize.y, 24, RenderTextureFormat.ARGBFloat);
		worldBRender = new RenderTexture(screenSize.x, screenSize.y, 24, RenderTextureFormat.ARGBFloat);

		wordlARender.Create();
		worldBRender.Create();

		worldAWindowMaterial.mainTexture = worldBRender;
		worldBWindowMaterial.mainTexture = wordlARender;

		worldACamera.targetTexture = wordlARender;
		worldBCamera.targetTexture = worldBRender;
	}
}
