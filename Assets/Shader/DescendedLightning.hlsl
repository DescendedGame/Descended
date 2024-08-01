#ifndef CUSTOM_LIGHTING_INCLUDED
#define CUSTOM_LIGHTING_INCLUDED


void CalculateMainLight_float(float3 WorldPos, out float3 Direction, out float3 Color, out half DistanceAtten, out half ShadowAtten) {
#ifdef SHADERGRAPH_PREVIEW
    Direction = half3(-0.5, -0.5, 0);
    Color = 1;
    DistanceAtten = 1;
    ShadowAtten = 1;
#else
#if SHADOWS_SCREEN
    half4 clipPos = TransformWorldToHClip(WorldPos);
    half4 shadowCoord = ComputeScreenPos(clipPos);
#else
    half4 shadowCoord = TransformWorldToShadowCoord(WorldPos);
#endif
    Light mainLight = GetMainLight(shadowCoord);
    Direction = mainLight.direction;
    Color = mainLight.color;
    DistanceAtten = mainLight.distanceAttenuation;
    ShadowAtten = mainLight.shadowAttenuation;
#endif
}

void AddAdditionalLights_float(float Porosity, float Ambience, float AmbShade, float Roughness, float3 WorldPosition, float3 WorldNormal, float3 WorldView,
    float MainDiffuse, float MainSpecular, float3 MainColor, float4 WaterAtt,
    out float3 Diffuse, out float3 Specular, out float3 Ambient) {
    
	Diffuse = 0;
	Specular = 0;
    Ambient = 0;


#if defined(SHADOWS_SHADOWMASK) && defined(LIGHTMAP_ON)
    half4 shadowMask = inputData.shadowMask;
#elif !defined (LIGHTMAP_ON)
    half4 shadowMask = unity_ProbesOcclusion;
#else
    half4 shadowMask = half4(1, 1, 1, 1);
#endif

#ifndef SHADERGRAPH_PREVIEW
    
int pixelLightCount = GetAdditionalLightsCount();

    for (int i = 0; i < pixelLightCount; i++) {

		
        Light light = GetAdditionalLight(i, WorldPosition, shadowMask);


        half NdotL = saturate(dot(normalize(WorldNormal), normalize(light.direction)));
        //half thisDiffuse = light.distanceAttenuation * NdotL;

        half shadowAtt = light.shadowAttenuation * 2 * ceil(NdotL-0.1);
		shadowAtt = saturate(floor(shadowAtt * 2));


		float3 t_color = light.color;
		float t_distance1 = length(light.direction);
		float t_distance2 = length(WorldView);

		float atten = light.distanceAttenuation;

		float3 amb = t_color * Ambience * MainColor /** saturate(AmbShade + shadowAtt)*/ * light.lightDistance;
		Ambient += amb* ceil(atten);
		Ambient += atten * Porosity * t_color * MainColor;

		float3 absorb2 = float3(pow(0.5, t_distance2/WaterAtt.x),pow(0.5, t_distance2/WaterAtt.y),pow(0.5, t_distance2/WaterAtt.z));


		//Ambient *= absorb2;
		


		t_color.r *= pow(0.5, t_distance1/WaterAtt.x);
		t_color.g *= pow(0.5, t_distance1/WaterAtt.y);
		t_color.b *= pow(0.5, t_distance1/WaterAtt.z);

		

        float3 diffu = shadowAtt  *t_color * saturate(ceil((NdotL)-0.25)+0.6) *atten * MainDiffuse * MainColor;
		Diffuse += diffu;
		//Diffuse*= absorb2;
        
		


		half lookangle = ((1 - dot(normalize(WorldView), normalize(light.direction))) / 2);
		float3  spec = shadowAtt * (t_color * atten) *  ceil(saturate(dot(reflect(normalize(light.direction), WorldNormal), -normalize(WorldView))-Roughness)) * MainSpecular;
		Specular += spec;
        //Specular*=absorb2;

		//ADD WATERATT AFTER!

    }
    
#endif

}

void VVolFog_float(float Random, float3 WorldPosition, float3 WorldView, float4 WaterAtt, float4 FogCol,
	out float3 Color, out float Cut){

	Color = 0;
	Cut = 1;

#if defined(SHADOWS_SHADOWMASK) && defined(LIGHTMAP_ON)
	half4 shadowMask = inputData.shadowMask;
#elif !defined (LIGHTMAP_ON)
	half4 shadowMask = unity_ProbesOcclusion;
#else
	half4 shadowMask = half4(1, 1, 1, 1);
#endif

#ifndef SHADERGRAPH_PREVIEW
	
	int pixelLightCount = GetAdditionalLightsCount();

	Color = 0;

	for (int i = 0; i < pixelLightCount; i++) {


		Light light = GetAdditionalLight(i, WorldPosition, shadowMask);

		half t_dist = length(WorldView);
		float3 t_dir = normalize(WorldView);

		static int t_max = 3;

		half t_step = (t_dist-1) / t_max;
		//Random = 1;
		half newrand = Random;

		newrand *= t_step;

		float3 supercolor = float3(0, 0, 0);

		half distgone = newrand;
		half p_distgone = 0;
		half t_rand = 0;

		//[unroll]
		for (int j = 0; j < t_max; j++)
		{

			Light light2 = GetAdditionalLight(i, WorldPosition + t_dir * distgone, shadowMask);
			t_rand = distgone - p_distgone;
			p_distgone = distgone;
			distgone = clamp((distgone + t_step), 0, t_dist);
			//distgone = clamp(distgone, 0, t_dist);

			
			float3 t_col = light2.color * t_rand *  (light2.distanceAttenuation * (light2.shadowAttenuation) * 0.5 + light2.lightDistance)/* +  0.2*sqrt(log10(10*(1-0.9*(1-light2.distanceAttenuation))))*/;
			t_col.r *= pow(0.5, (t_dist - distgone + length(light2.direction))/WaterAtt.x);
			t_col.g *= pow(0.5, (t_dist - distgone + length(light2.direction))/WaterAtt.y);
			t_col.b *= pow(0.5, (t_dist - distgone + length(light2.direction))/WaterAtt.z);

			supercolor += t_col/** FogCol.w* FogCol.xyz*/;

		}

		//if(length(light.direction) < 10 && length(light.direction) > 9)
		//{
		//	Cut = 0;
		//}
		//else
		//{
		//discard;
		Color += supercolor  * ceil(light.distanceAttenuation) * 0.001 /** ((t_dist-1)/(newrand+(t_dist-1)))*/;
		//}
		
	}

#endif

}


void VLightSelect_float(float3 worldPos, float3 objPos,
	out float index){

	index = 0;

//#if defined(SHADOWS_SHADOWMASK) && defined(LIGHTMAP_ON)
//	half4 shadowMask = inputData.shadowMask;
//#elif !defined (LIGHTMAP_ON)
//	half4 shadowMask = unity_ProbesOcclusion;
//#else
//	half4 shadowMask = half4(1, 1, 1, 1);
//#endif

#ifndef SHADERGRAPH_PREVIEW
	
		int pixelLightCount = GetAdditionalLightsCount();
		half val;
		index = 0;

		Light light = GetAdditionalLight(1, worldPos/*, shadowMask*/);
		val = ceil(saturate(length(worldPos - (objPos-light.direction)) -0.01));
		val = 1-val;
		index +=  saturate(pixelLightCount-1) *1*val;

		light = GetAdditionalLight(2, worldPos/*, shadowMask*/);
		val = ceil(saturate(length(worldPos - (objPos-light.direction)) -0.01));
		val = 1-val;
		index += saturate(pixelLightCount-2) *2*val;

		light = GetAdditionalLight(3, worldPos/*, shadowMask*/);
		val = ceil(saturate(length(worldPos - (objPos-light.direction)) -0.01));
		val = 1-val;
		index += saturate(pixelLightCount-3) *3*val;

		
		light = GetAdditionalLight(4, worldPos/*, shadowMask*/);
		val = ceil(saturate(length(worldPos - (objPos-light.direction)) -0.01));
		val = 1-val;
		index += saturate(pixelLightCount-4) *4*val;

		light = GetAdditionalLight(5, worldPos/*, shadowMask*/);
		val = ceil(saturate(length(worldPos - (objPos-light.direction)) -0.01));
		val = 1-val;
		index += saturate(pixelLightCount-5) *5*val;

		light = GetAdditionalLight(6, worldPos/*, shadowMask*/);
		val = ceil(saturate(length(worldPos - (objPos-light.direction)) -0.01));
		val = 1-val;
		index += saturate(pixelLightCount-6) *6*val;

		light = GetAdditionalLight(7, worldPos/*, shadowMask*/);
		val = ceil(saturate(length(worldPos - (objPos-light.direction)) -0.01));
		val = 1-val;
		index += saturate(pixelLightCount-7) *7*val;
		


		//if(length(worldPos - (objPos-light.direction)) < 0.01)
		//{
		

		//}


#endif

}


void VVolFogSphere_float(float3 objPos, float Random, float3 WorldPosition, float3 WorldView, float4 WaterAtt, float4 FogCol, float index,
	out float3 Color) {

	Color = 0;

#if defined(SHADOWS_SHADOWMASK) && defined(LIGHTMAP_ON)
	half4 shadowMask = inputData.shadowMask;
#elif !defined (LIGHTMAP_ON)
	half4 shadowMask = unity_ProbesOcclusion;
#else
	half4 shadowMask = half4(1, 1, 1, 1);
#endif

#ifndef SHADERGRAPH_PREVIEW

	//int pixelLightCount = GetAdditionalLightsCount();


	//for (int i = 0; i < pixelLightCount; i++) {

	

		Light light = GetAdditionalLight(index, WorldPosition, shadowMask);

		half t_dist = length(WorldView);
		float3 t_dir = normalize(WorldView);

		static int t_max = 3;
		

		half t_step = (t_dist-1) / t_max;

		half newrand = Random;

		newrand *= t_step;


		half superfog = 0;
		float3 supercolor = float3(0, 0, 0);

		half distgone = newrand;
		half p_distgone = 0;
		half t_rand = 0;

		for (int j = 0; j < t_max; j++)
		{

			Light light2 = GetAdditionalLight(index, WorldPosition + t_dir * distgone, shadowMask);
			t_rand = distgone - p_distgone;
			p_distgone = distgone;
			distgone = clamp((distgone + t_step), 0, t_dist);
			//distgone = clamp(distgone, 0, t_dist);

			

			
			float3 t_col = light2.color * t_rand *  (light2.distanceAttenuation * (light2.shadowAttenuation) * 0.5 + light2.lightDistance)/* +  0.2*sqrt(log10(10*(1-0.9*(1-light2.distanceAttenuation))))*/;
			//float3 t_col = light2.color * t_rand * light2.distanceAttenuation *  (light2.shadowAttenuation) +  0.3*(log10(1-0.9*(1-light2.distanceAttenuation))+1));
			t_col.r *= pow(0.5, (t_dist - distgone + length(light2.direction))/WaterAtt.x);
			t_col.g *= pow(0.5, (t_dist - distgone+ length(light2.direction))/WaterAtt.y);
			t_col.b *= pow(0.5, (t_dist - distgone+ length(light2.direction))/WaterAtt.z);


			supercolor += t_col;

		}

		//if(length(WorldPosition - (objPos-light.direction)) < 0.01)
		//{
		
			Color += /*saturate(pixelLightCount) **/ supercolor /** FogCol.xyz*/ * ceil(light.distanceAttenuation+0.001) * 0.001 * FogCol.w /** ((t_dist-1)/(newrand+(t_dist-1)))*/;

		//}
		
	//}

#endif

}


void ReflectiveTransparentLights_float(float3 WorldPosition, float3 WorldView, float3 Ambience, float4 WaterAtt,
    out float3 Color) {

    Color = 0;

#if defined(SHADOWS_SHADOWMASK) && defined(LIGHTMAP_ON)
    half4 shadowMask = inputData.shadowMask;
#elif !defined (LIGHTMAP_ON)
    half4 shadowMask = unity_ProbesOcclusion;
#else
    half4 shadowMask = half4(1, 1, 1, 1);
#endif

#ifndef SHADERGRAPH_PREVIEW
    
    int pixelLightCount = GetAdditionalLightsCount();

    for (int i = 0; i < pixelLightCount; ++i) {

        Light light = GetAdditionalLight(i, WorldPosition, shadowMask);

        float3 t_col = light.color* ((ceil(light.distanceAttenuation)*((1.1/(1.0+10*(1-light.distanceAttenuation)))-0.1))*light.shadowAttenuation + Ambience);
		t_col.r *= pow(0.5, (length(light.direction))/WaterAtt.x);
		t_col.g *= pow(0.5, (length(light.direction))/WaterAtt.y);
		t_col.b *= pow(0.5, (length(light.direction))/WaterAtt.z);


        Color += t_col;
   
    }

#endif

}

void BlackCut_float(float3 WorldPosition,
    out float Cut) {

    Cut = 1;

#if defined(SHADOWS_SHADOWMASK) && defined(LIGHTMAP_ON)
    half4 shadowMask = inputData.shadowMask;
#elif !defined (LIGHTMAP_ON)
    half4 shadowMask = unity_ProbesOcclusion;
#else
    half4 shadowMask = half4(1, 1, 1, 1);
#endif

#ifndef SHADERGRAPH_PREVIEW
    
    int pixelLightCount = GetAdditionalLightsCount();

    for (int i = 0; i < pixelLightCount; ++i) {

        Light light = GetAdditionalLight(i, WorldPosition, shadowMask);

		//if(length(light.direction) < 10 && length(light.direction) > 9)
		//{
		//	Cut = 0;
		//}
   
    }

#endif

}

#endif





























