void ToonShading_float(in float3 Normal, in float RampSmoothness, in float3 ClipSpacePosition, in float3 WorldPosition, in float4 RampTinting,
in float RampOffset, out float3 RampOutput, out float3 Direction)
{
	#ifdef SHADERGRAPH_PREVIEW
		RampOutput = float3(0.5,0.5,0);
		Direction = float3(0.5,0.5,0);
	#else
		#if SHADOWS_SCREEN
			half4 shadowCoord = ComputeScreenPos(ClipSpacePosition);
		#else
			half4 shadowCoord = TransformWorldToShadowCoord(WorldPosition);
		#endif 

		#if _MAIN_LIGHT_SHADOWS_CASCADE || _MAIN_LIGHT_SHADOWS
			Light light = GetMainLight(shadowCoord);
		#else
			Light light = GetMainLight();
		#endif

		half dotProduct = dot(Normal, light.direction) * 0.5 + 0.5;
		
		half ramp = smoothstep(RampOffset, RampOffset+ RampSmoothness, dotProduct);
		ramp *= light.shadowAttenuation;
		RampOutput = light.color * (ramp + RampTinting) ;
		Direction = light.direction;
	#endif

}