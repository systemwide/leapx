Shader "Custom/KinectPointCloudShader" 
{
	Properties 
	{
		_WorldScale ("WorldScale", Float) = 1.0
			
	}

	SubShader 
	{
		Pass
		{
			Tags { "RenderType"="Opaque" }
			LOD 200
		
			CGPROGRAM
				#pragma target 5.0
				#include "UnityCG.cginc" 
		static const int DepthWidth = 512;
	static const int DepthHeight = 424;

	static const float MillimetersToMetersScale = 1.0 / 1000.0;

	static const float2 DepthWidthHeight = float2(DepthWidth, DepthHeight);
	static const float2 DepthHalfWidthHeight = DepthWidthHeight / 2.0;

	static const float SensorHorizontalFOVDegrees = 70.6;
	static const float XYSpread = tan(radians(SensorHorizontalFOVDegrees) * 0.5) / (DepthWidth * 0.5);

	static const float MinDepthMM = 300.0;
	static const float MaxDepthMM = 8000.0;

	// vertex offsets for building a quad from a depth pixel
	static const float4 quadOffsets[4] =
	{
		float4(1.0, 0.0, 0, 0),
		float4(0.0, 0.0, 0, 0),
		float4(1.0, 1.0, 0, 0),
		float4(0.0, 1.0, 0, 0),
	};

	// texture lookup offsets for loading current and nearby depth pixels
	static const int3 textureOffsets4Samples[4] =
	{
		int3(1, 0, 0),
		int3(0, 0, 0),
		int3(1, 1, 0),
		int3(0, 1, 0),
	};

	struct EMPTY_INPUT
	{
	};
	float DepthFromPacked4444(float4 packedDepth)
	{
		return packedDepth.r * 255 + packedDepth.a * 255 * 256;
	}


	EMPTY_INPUT VS_Empty()
	{
		return (EMPTY_INPUT)0;
	}
				#pragma vertex VS_Empty
				#pragma geometry GS_Main			
				#pragma fragment frag				
				float4x4 _ModelViewMatrix;
				float _WorldScale;
				Texture2D _DepthTex;
				Texture2D _UVTex;
				sampler2D _ColorTex;
				struct POSCOLOR_INPUT
				{
					float4	pos		: POSITION;
					float2  uv		: TEXCOORD0;
				};
				[maxvertexcount(4)]
				void GS_Main(point EMPTY_INPUT p[1], uint primID : SV_PrimitiveID, inout TriangleStream<POSCOLOR_INPUT> triStream)
				{
					POSCOLOR_INPUT output;							
					
					int x = DepthWidth - primID % DepthWidth;
					int y = primID / DepthWidth;
					int3 textureCoordinates = int3(x, y, 0);
					int3 textureCoordinates3 = int3(x + 1, y, 0);
					int3 textureCoordinates2 = int3(x, y, 0);
					int3 textureCoordinates1 = int3(x + 1, y + 1, 0);
					int3 textureCoordinates0 = int3(x, y + 1, 0);
										
					float depth = DepthFromPacked4444(_DepthTex.Load(textureCoordinates));
					
					// don't output quads for pixels with invalid depth data
					if (depth < MinDepthMM || depth > MaxDepthMM)
					{
						//return;
					}					
					
					// color based on depth
					//output.color.rgb = 1.0 - ((depth - MinDepthMM) / (MaxDepthMM - MinDepthMM));
					//output.color.a = 1.0;					
					
					// convert to meters and scale to world				
					float worldScaledDepth = depth * MillimetersToMetersScale * _WorldScale;

					// Invert Y (0 Y is the top of the texture, but Y increases in the 3D world as you go upwards)				
					float4 worldPos = float4(textureCoordinates.x, DepthHeight - textureCoordinates.y, worldScaledDepth, 1.0);
					
					// Center coordinates such that 0,0 is the center in the world					
					worldPos.xy -= DepthHalfWidthHeight;	
					worldPos.x = -worldPos.x;
					
					// Data is projected onto the 2D sensor, but knowing the depth and field of view we can reconstruct world space position
					worldPos.xy *= XYSpread * worldScaledDepth;		

					//float4 camPos = worldPos;
					//camPos.y = -1 * camPos.y;
					float4 temp = mul(_ModelViewMatrix, worldPos);
					
					float4 viewPos = mul(UNITY_MATRIX_V, temp);
					float4 uvData = _UVTex.Load(textureCoordinates);
					float4 uvDataTest[4];
					uvDataTest[0] = _UVTex.Load(textureCoordinates0);
					uvDataTest[1] = _UVTex.Load(textureCoordinates1);
					uvDataTest[2] = _UVTex.Load(textureCoordinates2);
					uvDataTest[3] = _UVTex.Load(textureCoordinates3);

					for (int i = 0; i < 4; ++i)
					{
						// expand vertices in view space so that they're always the same size no matter what direction you're looking at them from
						float4 viewPosExpanded = viewPos + (quadOffsets[i] * XYSpread * worldScaledDepth);
						output.pos = mul(UNITY_MATRIX_P, viewPosExpanded);
						output.uv = float2(uvDataTest[i].r + uvDataTest[i].g / 255, uvDataTest[i].b + uvDataTest[i].a / 255);
						//output.uv = float2(uvData.r, uvData.g);
						triStream.Append(output);

					}
				}
				float4 frag(POSCOLOR_INPUT input) : COLOR
				{
					return tex2D(_ColorTex,input.uv);
				}
				
				
			ENDCG
		}
	} 
}
