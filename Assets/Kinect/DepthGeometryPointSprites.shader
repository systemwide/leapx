Shader "Custom/DepthGeometryPointSprites" 
{
	Properties
	{
		_WorldScale("WorldScale", Float) = 1.0
		_TexSpread("TexSpread", Float) = .01
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
				#include "KinectCommon.cginc"
				#pragma vertex VS_Empty
				#pragma geometry GS_Main			
				#pragma fragment frag				
				float4x4 _ModelViewMatrix;
				float _WorldScale;
				float _TexSpread;
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
				    int3 textureCoordinates = int3(x,y, 0);			
					int3 textureCoordinates3 = int3(x+1, y, 0);
					int3 textureCoordinates2 = int3(x, y, 0);
					int3 textureCoordinates1 = int3(x+1, y+1, 0);
					int3 textureCoordinates0 = int3(x, y+1, 0);
										
					float depth = DepthFromPacked4444(_DepthTex.Load(textureCoordinates));
					
					// don't output quads for pixels with invalid depth data
					if (depth < MinDepthMM || depth > MaxDepthMM)
					{
						return;
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
						output.uv = float2(uvDataTest[i].r+uvDataTest[i].b/255, uvDataTest[i].g+uvDataTest[i].a/255);
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
