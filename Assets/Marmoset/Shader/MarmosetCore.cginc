// Marmoset Skyshop
// Copyright 2013 Marmoset LLC
// http://marmoset.co

#ifndef MARMOSET_CORE_CGINC
#define MARMOSET_CORE_CGINC

#define INV_2PI 0.15915494309189533576888376337251

//Color-correction
half3 toLinearApprox3(half3 c){ return c*c; }
half3 toLinear3(half3 c)      { return pow(c,2.2); }
half  toLinearFast1(half  c)  { half  c2 = c*c; return dot(half2(0.7532,0.2468),half2(c2,c*c2)); }
half3 toLinearFast3(half3 c)  { half3 c2 = c*c; return 0.7532*c2 + 0.2468*c*c2; }

half  toGammaApprox1(half c)	  { return sqrt(c); }
half3 toGammaApprox3(half3 c) { return sqrt(c); }
half  toGamma1(half c)		  { return pow(c,0.454545); }
half3 toGamma3(half3 c)		  { return pow(c,0.454545); }
half  toGammaFast1(half c)	{
	c = 1.0 - c;
	half c2 = c*c;
	half3 c16 = half3(c2*c2,c2,c);	//^4
	c16.x *= c16.x;					//^8
	c16.x *= c16.x;					//^16
	c16 = half3(1.0,1.0,1.0)-c16;
	return dot(half3(0.326999,0.249006,0.423995),c16);
}
half3 toGammaFast3(half3 c) {
	half3 one = half3(1.0,1.0,1.0);
	c = one - c;
	half3 c2 = c*c;
	half3 c16 = c2*c2;	//^4
	c16 *= c16;			//^8
	c16 *= c16;			//^16
	return  0.326999*(one-c16) + 0.249006*(one-c2) + 0.423995*(one-c);
}

float3 mulVec3( float4x4 m, float3 v ) {
	return m[0].xyz*v.x + (m[1].xyz*v.y + (m[2].xyz*v.z));
}

float3 mulVec3( float3x3 m, float3 v ) {
	return m[0].xyz*v.x + (m[1].xyz*v.y + (m[2].xyz*v.z));
}

float3 mulPoint3( float4x4 m, float3 p ) {
	return m[0].xyz*p.x + (m[1].xyz*p.y + (m[2].xyz*p.z + m[3].xyz));
}

 //TODO: transpose of mulVec: vec3( dot(m[0].xyz,v), ... );

half3 fromRGBM(half4 c)  {
	#ifdef MARMO_LINEAR
	//RGB is pulled to linear space by sRGB sampling, alpha must be in linear space also before use
	return c.rgb * toLinearFast1(c.a);
	#else 
	//leave RGB*A in gamma space, gamma correction is disabled
	return c.rgb * c.a;
	#endif
}

half3 diffCubeLookup(samplerCUBE diffCube, float3 worldNormal) {
	half4 diff = texCUBE(diffCube, worldNormal);
	return fromRGBM(diff);
}

half3 specCubeLookup(samplerCUBE specCube, float3 worldRefl) {
	half4 spec = texCUBE(specCube, worldRefl);
	return fromRGBM(spec);
}

half3 glossCubeLookup(samplerCUBE specCube, float3 worldRefl, float glossLod) {
#ifdef MARMO_BIAS_GLOSS
	half4 lookup = half4(worldRefl,glossLod);
	half4 spec = texCUBEbias(specCube, lookup);
#else
	half4 lookup = half4(worldRefl,glossLod);
	half4 spec = texCUBElod(specCube, lookup);
#endif
	return fromRGBM(spec);
}

half glossLOD(half glossMap, half shininess) {
	glossMap = 1.0-glossMap;
	glossMap = 1.0-glossMap*glossMap;
	return 7.0 + glossMap - shininess*glossMap;
}

half glossExponent(half glossLod) {
	return exp2(8.0-glossLod);
}

//returns 1/spec. function integral
float specEnergyScalar(float gloss) {
	return gloss*INV_2PI + 2.0*INV_2PI;
}

half splineFresnel(float3 N, float3 E, half specIntensity, half fresnel) {
	half factor = 1.0-saturate(dot(N,E));
	half factor3 = factor*factor*factor;
	
	//a spline between 1, factor, and factor^3
	half3 p = half3(1.0, factor, factor3);
	half2 t = half2(1.0-fresnel,fresnel);
	p.x = dot(p.xy,t);
	p.y = dot(p.yz,t);
	factor = 0.05  + 0.95 * dot(p.xy,t);
	factor *= specIntensity;
	#ifndef MARMO_LINEAR
	//if gamma correction is disabled, fresnel*specInt needs to be applied in gamma-space
	factor = toGammaApprox1(factor);
	#endif
	return factor;
}

half fastFresnel(float3 N, float3 E, half specIntensity, half fresnel) {
	#ifdef MARMO_LINEAR
		half factor = 1.0-saturate(dot(N,E));
		factor *= factor*factor;
		factor = 0.05 + factor*0.95;
		return specIntensity * lerp(1.0, factor, fresnel);
	#else
		half factor = 1.0-saturate(dot(N,E));
		//factor *= factor*factor;
		factor *= 0.5 + 0.5*factor;
		factor = 0.15 + factor*0.85;
		factor = specIntensity * lerp(1.0, factor, fresnel);
		return factor;
	#endif
}

float3 skyProject(float4x4 skyMatrix, float4x4 invSkyMatrix, float3 worldPos, float3 R) {
	#if MARMO_BOX_PROJECTION
		//box projection happens in sky-space
		#ifdef MARMO_SKY_ROTATION
			R = mulVec3(skyMatrix,R).xyz; //HACK: mulVec3 is mul(transpose(_sky))
		#endif
		float3 invR = 1.0/R;
		
		float4 P;
		P.xyz = worldPos;
		P.w = 1.0;
		#ifdef MARMO_SKY_ROTATION
			P.xyz = mul(invSkyMatrix,P).xyz;
		#else
			P.xyz -= skyMatrix[3].xyz;
		#endif
		
		float4 rbmax = float4(0.0,0.0,0.0,0.0);
		float4 rbmin = float4(0.0,0.0,0.0,0.0);
		rbmax.xyz =  _SkySize - P.xyz;
		rbmin.xyz = -_SkySize - P.xyz;
		float3 rbminmax = (R>0.0) ? rbmax.xyz : rbmin.xyz;
		rbminmax *= invR;
		
		float fa = min(min(rbminmax.x, rbminmax.y), rbminmax.z);
		R = P.xyz + R*fa;
		//R is in sky space
		return R;
	#else
		#ifdef MARMO_SKY_ROTATION
			R = mulVec3(skyMatrix,R);
		#endif
		return R;
	#endif
}

float4 HDRtoRGBM(float4 color) {
	float toLinear = 2.2;
	float toSRGB = 1.0/2.2;
	#ifdef MARMO_LINEAR
		color.rgb = pow(color.rgb, toSRGB);
	#endif
	color *= 1.0/6.0;
	float m = max(max(color.r,color.g),color.b);
	m = saturate(m);
	m = ceil(m*255.0)/255.0;
	
	if( m > 0.0 ) {
		float inv_m = 1.0/m;
		color.rgb = saturate(color.rgb*inv_m);
		color.a = m;
	} else {
		color = half4(0.0,0.0,0.0,0.0);
	}
	
	#ifdef MARMO_LINEAR
		//output gets converted to sRGB by gamma correction, premptively undo it
		color.rgb = pow(color.rgb, toLinear);
	#endif
	return color;
}
#endif