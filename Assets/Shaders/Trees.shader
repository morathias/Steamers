Shader "Vegetation/Trees" {
	//===================================================================================================================
	Properties {
		_Color ("Color", Color) = (1,1,1,1)
		_MainTex ("Diffuse (RGB)", 2D) = "white" {}
		[Normal]
		_NormalTex("Normal Map", 2D) = "white"{}
		_Glossiness("Smoothness", 2D) = "black"{}
		_OcclusionPow("Ambient Occlusion Power", Range(0,1)) = 1
		_AlphaCutout ("Alpha Cutout", Range(0,1)) = 0.5

		_ScatteringTex("SubSurface Scattering Map", 2D) = "white"{}
		_ScatteringColor("SubSurface Scattering Color", Color) = (1, 1, 1, 1)
		_ScatteringDistortion("Subsurface Scattering Distortion", Float) = 1
		_ScatteringPower("Subsurface Scattering Power", Float) = 1

		[HideInInspector]
		_Wind("Wind", Vector) = (0,0,0,0)
		_BendScale("Wind Bend Scale", Range(0.1, 3)) = 1
		[HideInInspector]
		_WindSpeed("Wind Speed", Vector) = (0,0,0,0)
		_BranchAmplitud("Branch Amplitud", Float) = 0.05
		_LeafAmplitud("Leaf Amplitud", Float) = 0.05
	}
	//===================================================================================================================
	SubShader {
		Tags{ "Queue" = "Geometry" "RenderType" = "Opaque"}
		LOD 200
		
		Cull Off
		//------------------------------------------------------------------------------------------------------------
		CGPROGRAM
		
		#pragma surface surf Standard fullforwardshadows vertex:vert addshadow
		#pragma target 3.0
		//------------------------------------------------------------------------------------------------------------
		struct Input {
			float2 uv_MainTex;
			float3 viewDir;
			float4 color: Color;
		};
		//------------------------------------------------------------------------------------------------------------
		sampler2D _MainTex;
		sampler2D _Glossiness;
		sampler2D _NormalTex;
		fixed4 _Color;
		float _OcclusionPow;
		float _AlphaCutout;

		sampler2D _ScatteringTex;
		fixed4 _ScatteringColor;
		float _ScatteringPower, _ScatteringDistortion;

		float3 _Wind;
		float2 _WindSpeed;
		float _BendScale, _BranchAmplitud, _LeafAmplitud;
		//------------------------------------------------------------------------------------------------------------
		float4 SmoothCurve(float4 x) {
			return x * x *(3.0 - 2.0 * x);
		}

		float4 TriangleWave(float4 x) {
			return abs(frac(x + 0.5) * 2.0 - 1.0);
		}

		float4 SmoothTriangleWave(float4 x) {
			return SmoothCurve(TriangleWave(x));
		}
		//------------------------------------------------------------------------------------------------------------
		void vert(inout appdata_full inputVertex){;
			float3 vPos = mul(unity_ObjectToWorld, inputVertex.vertex).xyz;
			//-------------------------Main Bending-------------------------------------------------------------------
			float3 objectPos = mul(unity_ObjectToWorld, float4(0.0, 0.0, 0.0, 1.0)).xyz;
			vPos -= objectPos;

			float relativeLength = length(vPos);
			float bendFactor = vPos.y * (_BendScale * 0.01);

			bendFactor += 1.0;
			bendFactor *= bendFactor;
			bendFactor = bendFactor * bendFactor - bendFactor;

			/*_Wind.x = sin(_Wind.x * _Time.x);
			_Wind.z = cos(_Wind.z * _Time.y);*/

			float3 newVertexPos = vPos;
			newVertexPos.xz += _Wind.xz * bendFactor;

			vPos.xyz = normalize(newVertexPos.xyz) * relativeLength;
			//-----------------------Detail Bending-------------------------------------------------------------------
			float windStrength = length(_WindSpeed);

			float objectPhase = dot(objectPos.xyz, 1);

			float branchPhase = inputVertex.color.g;
			branchPhase += objectPhase;
			
			float vertexPhase = dot(vPos.xyz, 1 + branchPhase);

			float2 vertexWavesInterpolation = _Time.y + float2(vertexPhase, branchPhase);
			float4 vertexWaves = (frac(vertexWavesInterpolation.xxyy *
								 float4(1.975, 0.793, 0.375, 0.193)) *
								 2.0 - 1.0) * .1 * .2;
			vertexWaves = SmoothTriangleWave(vertexWaves);
			float2 WavesSum = vertexWaves.xz + vertexWaves.yw;

			vPos.xzy += WavesSum.xxy * float3(inputVertex.color.r * _LeafAmplitud * windStrength
											  * mul(unity_ObjectToWorld, inputVertex.normal).xy,
											  (1 - inputVertex.color.b) * _BranchAmplitud * windStrength);
			//---------------------------Output-----------------------------------------------------------------------
			inputVertex.vertex = mul(unity_WorldToObject, vPos);
		}
		//------------------------------------------------------------------------------------------------------------
		void surf(Input IN, inout SurfaceOutputStandard o) {
			//-----------------------------------Subsurface Scattering------------------------------------------------
			fixed atten = 1;
			fixed4 scatteringThickness = tex2D(_ScatteringTex, IN.uv_MainTex);

			half3 lightDir = normalize(mul(unity_LightPosition[0], UNITY_MATRIX_IT_MV).xyz);
			//half3 lightDir = normalize(_WorldSpaceLightPos0);

			half3 translucentLightDir = lightDir + o.Normal * _ScatteringDistortion;
			float translucentDot = pow(max(0, dot(normalize(IN.viewDir), -translucentLightDir)), _ScatteringPower) * 1.5f;
			fixed3 translucentLight = (atten * 2) * (translucentDot) * scatteringThickness.rgb * _ScatteringColor.rgb;

			fixed4 color = tex2D(_MainTex, IN.uv_MainTex) * _Color;

			fixed3 translucentColor = color + (_LightColor0.rgb * translucentLight);
			//-------------------------------------------Output-------------------------------------------------------
			o.Albedo = translucentColor;
			o.Smoothness = tex2D(_Glossiness, IN.uv_MainTex);
			o.Normal = UnpackNormal(tex2D(_NormalTex, IN.uv_MainTex));
			fixed occlusionPower = (1 - IN.color.a) * _OcclusionPow;
			o.Occlusion = IN.color.a + occlusionPower;
			o.Metallic = 0;

			if (color.a < _AlphaCutout)
				discard;
			else
				o.Alpha = color.a;
		}
		ENDCG
	}
	//===================================================================================================================
	FallBack "Transparent/Cutout/Diffuse"
}
