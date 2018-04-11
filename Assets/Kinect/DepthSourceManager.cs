using UnityEngine;
using System.Collections;
using Windows.Kinect;
using System;
public class DepthSourceManager : MonoBehaviour
{   
    private KinectSensor _Sensor;
	private DepthFrameReader _Reader;
	private CoordinateMapper _Mapper;
	public Texture2D _DepthTexture;
	public Texture2D _UVTexture;
	private ushort[] _sData;
	
	private byte[] _Data;
	
	private int _DepthWidth;
	public int GetDepthWidth()
	{
		return _DepthWidth;
	}
	
	private int _DepthHeight;
	public int GetDepthHeight()
	{
		return _DepthHeight;
	}

    void Awake () 
	{
		
		_Sensor = KinectSensor.GetDefault();
		
		if (_Sensor != null) 
        {
			_Reader = _Sensor.DepthFrameSource.OpenReader();

			var frameDesc = _Sensor.DepthFrameSource.FrameDescription;

			_DepthWidth = frameDesc.Width;
			_DepthHeight = frameDesc.Height;

			// Use ARGB4444 as there's no handier 16 bit texture format readily available
			_DepthTexture = new Texture2D(_DepthWidth, _DepthHeight, TextureFormat.ARGB4444, false,true);
			_UVTexture = new Texture2D(_DepthWidth, _DepthHeight, TextureFormat.RGBA32, false,true);
			_Data = new byte[_Sensor.DepthFrameSource.FrameDescription.LengthInPixels * _Sensor.DepthFrameSource.FrameDescription.BytesPerPixel];
			_sData = new ushort[_Sensor.DepthFrameSource.FrameDescription.LengthInPixels];
			_Mapper = _Sensor.CoordinateMapper;
		}
	}

	unsafe void Update () 
    {
        if (_Reader != null)
        {
        	var frame = _Reader.AcquireLatestFrame();

        	if (frame != null)
        	{
				
					
				fixed (byte* pData = _Data)
				{

					frame.CopyFrameDataToIntPtr(new System.IntPtr(pData), (uint)_Data.Length);
					
				}
				frame.CopyFrameDataToArray(_sData);
				_DepthTexture.LoadRawTextureData(_Data);
				ColorSpacePoint[] colorSpace = new ColorSpacePoint[_sData.Length];
				_Mapper.MapDepthFrameToColorSpace(_sData, colorSpace);
				Color32[] pixels = _UVTexture.GetPixels32();
				for (int i = 0; i < colorSpace.Length; i++)
				{

					
					
					pixels[i].r = (byte)((colorSpace[i].X / 1920)*255); //ranges from 0 to 255
					float remainderX = colorSpace[i].X / 1920 * 255 - pixels[i].r;
					pixels[i].b = (byte)(remainderX*255); 
					pixels[i].g = (byte)((colorSpace[i].Y / 1080)*255);
					float remainderY = colorSpace[i].Y / 1080 * 255 - pixels[i].g;
					pixels[i].a = (byte)(remainderY*255);

				}

				_UVTexture.SetPixels32(pixels);
				_UVTexture.Apply();
				_DepthTexture.Apply();
				

				frame.Dispose();
				
        		
        		frame = null;

				
			}
        }
    }
    
	void OnApplicationQuit()
	{
		if (_Reader != null)
		{
			_Reader.Dispose();
			_Reader = null;
		}
		
		if (_Sensor != null)
		{
			if (_Sensor.IsOpen)
			{
				_Sensor.Close();
			}
			
			_Sensor = null;
		}
    }
}
