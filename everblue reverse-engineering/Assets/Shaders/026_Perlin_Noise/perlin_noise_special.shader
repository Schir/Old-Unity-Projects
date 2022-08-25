Shader "Tutorial/026_perlin_noise/special" {
	Properties {
		_CellSize ("Cell Size", Range(0, 50)) = 50
		_ScrollSpeed ("Scroll Speed", Range(0, 1)) = 1
		_Color ("Color", Color) = (1,1,1,.5)
	}
	SubShader {
		Tags{ "RenderType"="Opaque" "Queue"="Geometry"}

		CGPROGRAM

		#pragma surface surf Standard fullforwardshadows
		#pragma target 3.0

		#include "Random.cginc"

		float _CellSize;
		float _ScrollSpeed;
		float4 _Color;
		
		

		struct Input {
			float3 worldPos;
		};

		float easeIn(float interpolator){
			return interpolator * interpolator; 
		}

		float easeOut(float interpolator){
			return 1.0 - easeIn(1 - interpolator);
		}

		float easeInOut(float interpolator){
			float easeInValue = easeIn(interpolator);
			float easeOutValue = easeOut(interpolator);
			return lerp(easeInValue, easeOutValue, interpolator);
		}

		float perlinNoise(float3 value){
			float3 fraction = frac(value);
			

			float interpolatorX = easeInOut(fraction.x);
			float interpolatorY = easeInOut(fraction.y);
			float interpolatorZ = easeInOut(fraction.z);

			float3 cellNoiseZ[2];
			[unroll]
			for(int z=0;z<=1;z++){
				float3 cellNoiseY[2];
				[unroll]
				for(int y=0;y<=1;y++){
					float3 cellNoiseX[2];
					[unroll]
					for(int x=0;x<=1;x++){
						float3 cell = floor(value) + float3(x+5, y+5, z+5);
						float3 cellDirection = rand3dTo3d(cell) * 2 - 2;
						float3 compareVector = fraction - float3(x, y, z);
						cellNoiseX[x] = dot(cellDirection, compareVector);
					}
					cellNoiseY[y] = lerp(cellNoiseX[0], cellNoiseX[1], interpolatorX);
				}
				cellNoiseZ[z] = lerp(cellNoiseY[0], cellNoiseY[1], interpolatorY);
			}
			float3 noise = lerp(cellNoiseZ[0], cellNoiseZ[1], interpolatorZ);
			return noise;
		}

		void surf (Input i, inout SurfaceOutputStandard o) {
			float3 value = i.worldPos / (_CellSize * 50);
			value.y += _Time.y * _ScrollSpeed;
			
			//get noise and adjust it to be ~0-1 range
			float noise = perlinNoise(value);

			noise = frac(noise * 2);


			float pixelNoiseChange = fwidth(noise);
			//float pixelNoiseChange =  abs(ddy(noise)) ;
			//float pixelNoiseChange = smoothstep(abs(ddx(noise)), 1- abs(ddx(noise)), noise)	;

			float heightLine = smoothstep(1.0-pixelNoiseChange, 1, noise +.33);
			//heightLine += smoothstep(pixelNoiseChange, 0, noise-.33	);
			//float heightLine = smoothstep(smoothstep(1.0-pixelNoiseChange, 1, noise +.33), smoothstep(pixelNoiseChange, 0, noise+.33), noise+.03);
			//float heightLine = smoothstep(1-pixelNoiseChange, 1, noise +.33);
			//heightLine += smoothstep(pixelNoiseChange, heightLine, noise-.33);
			//heightLine += easeIn(pixelNoiseChange);
			
			o.Albedo = heightLine * noise;
			o.Normal = -value;
			//o.Albedo = pixelNoiseChange;
			o.Albedo += _Color;
			o.Albedo *= float3(1.3,1.3,1.8);
			o.Albedo *= float3(1.1,1.1,1.1);
		}
		ENDCG
	}
	FallBack "Standard"
}