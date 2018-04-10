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
	float4( 1.0, 0.0, 0, 0),			
	float4( 0.0, 0.0, 0, 0),
	float4( 1.0, 1.0, 0, 0),
	float4( 0.0, 1.0, 0, 0),
};

// texture lookup offsets for loading current and nearby depth pixels
static const float2 textureOffsets[4] =
{
	float2(1, 0),
	float2(0, 0),
	float2(1, 1),
	float2(0, 1),
};				

struct EMPTY_INPUT
{
};


float DepthFromPacked4444(float4 packedDepth)
{
	// convert from [0,1] to [0,15]
	packedDepth *= 15.01f;
	
	// truncate to an int
	int4 rounded = (int4)packedDepth;				
	
	return rounded.w * 4096 + rounded.x * 256 + rounded.y * 16 + rounded.z;				
}

/*
float DepthFromPacked4444(float4 packedDepth)
{
	return packedDepth.r * 255 + packedDepth.a * 255 * 256;
}
*/


EMPTY_INPUT VS_Empty()
{
	return (EMPTY_INPUT)0;
}
