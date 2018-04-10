using UnityEngine;
using System;
using System.Collections;
using System.Runtime.InteropServices;


public class KinectStreamingPlugin : MonoBehaviour
{
	[DllImport("KinectClientPlugin")]
	private static extern void SetDepthTextureFromUnity(System.IntPtr texture, int w, int h);
	[DllImport("KinectClientPlugin")]
	private static extern void SetColorTextureFromUnity(System.IntPtr texture, int w, int h);
	[DllImport("KinectClientPlugin")]
	private static extern void SetUVTextureFromUnity(System.IntPtr texture, int w, int h);
	[DllImport("KinectClientPlugin")]
	private static extern IntPtr GetRenderEventFunc();
	[DllImport("KinectClientPlugin")]
	private static extern void StartStreaming(string address, string port);
	
	public Texture2D texDepth;
	public Texture2D texColor;
	public Texture2D texUV;
	void Awake()
	{
		CreateTexturesAndPassToPlugin();
		StartStreaming("localhost","27115");

	}
	IEnumerator Start()
	{
		Debug.Log("here");
		
		yield return StartCoroutine("CallPluginAtEndOfFrames");

	}

	void OnApplicationQuit()
	{
		
	}
	private void CreateTexturesAndPassToPlugin()
	{

		// Create depth texture
		texDepth = new Texture2D(512, 424, TextureFormat.ARGB32, false,true);
		
		// Set point filtering just so we can see the pixels clearly
		texDepth.filterMode = FilterMode.Point;
		// Call Apply() so it's actually uploaded to the GPU
		texDepth.Apply();

		// Create depth texture
		texColor = new Texture2D(960, 540, TextureFormat.ARGB32, false,false);
		// Set point filtering just so we can see the pixels clearly
		texColor.filterMode = FilterMode.Point;
		// Call Apply() so it's actually uploaded to the GPU
		texColor.Apply();

		// Create depth texture
		texUV = new Texture2D(512, 424, TextureFormat.ARGB32, false,true);
		// Set point filtering just so we can see the pixels clearly
		texUV.filterMode = FilterMode.Point;
		// Call Apply() so it's actually uploaded to the GPU
		texUV.Apply();

		// Pass texture pointer to the plugin
		SetDepthTextureFromUnity(texDepth.GetNativeTexturePtr(), texDepth.width, texDepth.height);
		SetColorTextureFromUnity(texColor.GetNativeTexturePtr(), texColor.width, texColor.height);
		SetUVTextureFromUnity(texUV.GetNativeTexturePtr(), texUV.width, texUV.height);
	}

	private IEnumerator CallPluginAtEndOfFrames()
	{
		while (true)
		{
			
			
			// Wait until all frame rendering is done
			yield return new WaitForEndOfFrame();
			// Issue a plugin event with arbitrary integer identifier.
			// The plugin can distinguish between different
			// things it needs to do based on this ID.
			// For our simple plugin, it does not matter which ID we pass here.
			GL.IssuePluginEvent(GetRenderEventFunc(), 1);
		}
	}


}
