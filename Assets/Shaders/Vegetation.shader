// Upgrade NOTE: commented out 'float4 unity_LightmapST', a built-in variable
// Upgrade NOTE: commented out 'sampler2D unity_Lightmap', a built-in variable
// Upgrade NOTE: replaced tex2D unity_Lightmap with UNITY_SAMPLE_TEX2D

Shader "Unlit/Vegetation (Transparent)"
{
    Properties
    {
        _MainTex ("Texture", 2D) = "white" {}
        _Cutoff ("Alpha cutoff", Range(0,1)) = 0.5

        _WindDirection("Wind Direction", Vector) = (1,0,0,0)
        _WindSpeed("Wind Speed", Float) = 1
        _WindScale("Wind Scale", Float) = 2
    }
    SubShader
    {
        Tags {
             "RenderType"="TransparentCutout"
             "Queue" = "AlphaTest"
             "IgnoreProjector" = "True"
             }
        LOD 100
        Cull Off
        //ZWrite Off
        //Blend SrcAlpha OneMinusSrcAlpha

        Pass
        {  
            Tags{"LightMode" = "ForwardBase"}
            CGPROGRAM
            
            #pragma vertex vert
            #pragma fragment frag Standard
            // make fog work
            #pragma multi_compile_fog

            #pragma multi_compile_instancing
            #pragma multi_compile_fwdbase
            #pragma fragmentoption ARB_fog_exp2
			#pragma fragmentoption ARB_precision_hint_fastest

            #pragma target 3.0

            //#define _SURFACE_TYPE_TRANSPARENT 1
            //#define _BLENDMODE_ALPHA 1

            #include "UnityCG.cginc"
            #include "AutoLight.cginc"
            #include "UnityStandardUtils.cginc"
            
            struct appdata
            {
                float4 vertex : POSITION;
                float2 uv : TEXCOORD0;
                float2 uv2: TEXCOORD1;
                UNITY_VERTEX_INPUT_INSTANCE_ID
                float4 color : COLOR;
                float3 normal: NORMAL;
            };

            struct v2f
            {
                float2 uv : TEXCOORD0;
                float2 uv2: TEXCOORD1;
                UNITY_FOG_COORDS(6)
                float4 pos : SV_POSITION;
                float4 color : COLOR;
                float3 worldNormal : TEXCOORD3;
                float3 worldPos : TEXCOORD2;
                LIGHTING_COORDS(4,5)
            };

            sampler2D _MainTex;
            float4 _MainTex_ST;

            // sampler2D unity_Lightmap;
            // float4 unity_LightmapST; 

            float2 _WindDirection;
            float _WindScale;
            float _WindSpeed;
            fixed _Cutoff;

            void Unity_Normalize_float2(float2 In, out float2 Out)
            {
                Out = normalize(In);
            }
        
            void Unity_Multiply_float (float2 A, float2 B, out float2 Out)
            {
                Out = A * B;
            }
        
            void Unity_TilingAndOffset_float(float2 UV, float2 Tiling, float2 Offset, out float2 Out)
            {
                Out = UV * Tiling + Offset;
            }
        
            void Unity_Subtract_float(float A, float B, out float Out)
            {
                Out = A - B;
            }
        
            void Unity_Add_float(float A, float B, out float Out)
            {
                Out = A + B;
            }
        
            void Unity_Combine_float(float R, float G, float B, float A, out float4 RGBA, out float3 RGB, out float2 RG)
            {
                RGBA = float4(R, G, B, A);
                RGB = float3(R, G, B);
                RG = float2(R, G);
            }
        
            void Unity_Lerp_float3(float3 A, float3 B, float3 T, out float3 Out)
            {
                Out = lerp(A, B, T);
            }
        
            void Unity_Subtract_float3(float3 A, float3 B, out float3 Out)
            {
                Out = A - B;
            }

            float2 unity_gradientNoise_dir(float2 p)
            {
                // Permutation and hashing used in webgl-nosie goo.gl/pX7HtC
                p = p % 289;
                float x = (34 * p.x + 1) * p.x % 289 + p.y;
                x = (34 * x + 1) * x % 289;
                x = frac(x / 41) * 2 - 1;
                return normalize(float2(x - floor(x + 0.5), abs(x) - 0.5));
            }

                
            float unity_gradientNoise(float2 p)
            {
                float2 ip = floor(p);
                float2 fp = frac(p);
                float d00 = dot(unity_gradientNoise_dir(ip), fp);
                float d01 = dot(unity_gradientNoise_dir(ip + float2(0, 1)), fp - float2(0, 1));
                float d10 = dot(unity_gradientNoise_dir(ip + float2(1, 0)), fp - float2(1, 0));
                float d11 = dot(unity_gradientNoise_dir(ip + float2(1, 1)), fp - float2(1, 1));
                fp = fp * fp * fp * (fp * (fp * 6 - 15) + 10);
                return lerp(lerp(d00, d01, fp.y), lerp(d10, d11, fp.y), fp.x);
            }

            void Unity_GradientNoise_float(float2 UV, float Scale, out float Out)
            { Out = unity_gradientNoise(UV * Scale) + 0.5; }

            v2f vert (appdata v)
            {
                v2f o;
                UNITY_SETUP_INSTANCE_ID(v);

                float2 windDirection = _WindDirection;
                windDirection = normalize(windDirection);

                float windSpeed = _WindSpeed;
                float2 windDirectionFinal = windDirection * windSpeed;

                float2 offset = _Time.y.xx * windDirectionFinal;
                float3 objectToWorldPos = mul(unity_ObjectToWorld, v.vertex).xyz;
                float3 viewSpace = UnityObjectToViewPos(v.vertex).xyz;
                o.worldPos = objectToWorldPos;

                float2 tiligOut;
                Unity_TilingAndOffset_float(viewSpace.xy, float2(1,1), offset, tiligOut);

                float windScale = _WindScale;
                float gradientNoise;
                Unity_GradientNoise_float(tiligOut, windScale, gradientNoise);
                float swayStrength = gradientNoise - 0.5f;

                float swayFinal = swayStrength + objectToWorldPos.x;

                float3 swayTransform = float3(swayFinal, objectToWorldPos.y, objectToWorldPos.z);
                
                float3 swayLockInY;
                Unity_Lerp_float3(objectToWorldPos, swayTransform, v.vertex.y, swayLockInY);

                o.pos =  mul(UNITY_MATRIX_VP, float4(swayLockInY, v.vertex.w));
                
                
                o.color = float4(swayStrength + 0.8, swayStrength + 0.8, swayStrength + 0.8, 1);
                
                o.uv = TRANSFORM_TEX(v.uv, _MainTex);
                o.uv2 = v.uv2 * unity_LightmapST.xy + unity_LightmapST.zw;
                o.worldNormal = UnityObjectToWorldNormal(v.normal);
                UNITY_TRANSFER_FOG(o,o.vertex);
                TRANSFER_VERTEX_TO_FRAGMENT(o);
                return o;
            }

            fixed4 frag (v2f i) : SV_Target
            {
                // sample the texture
                half3 lm = DecodeLightmap(UNITY_SAMPLE_TEX2D(unity_Lightmap, i.uv2));
                fixed4 col = tex2D(_MainTex, i.uv);
                //col.rgb *= lm + 0.2f;
                clip(col.a - _Cutoff);
                //col *= i.color;
                float shadow = LIGHT_ATTENUATION(i);
                half3 ambient = ShadeSHPerPixel(i.worldNormal, half3(0,0,0), i.worldPos);
                
                float shadowMask = 1 - shadow;
                float3 ambientShadow = ambient * shadowMask ;
                float3 shadowFinal = ambientShadow - (1 - shadowMask);

                col.xyz = ambient;
                
                fixed4 test = float4(clamp(shadowFinal, 0, ambient) , 1);
                // apply fog
                UNITY_APPLY_FOG(i.fogCoord, col);
                //return test;
                return col;
            }

            ENDCG
        }
    }
}
