// Upgrade NOTE: replaced 'mul(UNITY_MATRIX_MVP,*)' with 'UnityObjectToClipPos(*)'

// Upgrade NOTE: replaced 'mul(UNITY_MATRIX_MVP,*)' with 'UnityObjectToClipPos(*)'

Shader "Custom/grass_pbr"
{
    Properties
    {
        _Color ("Color", Color) = (1,1,1,1)
        _MainTex ("Albedo (RGBA)", 2D) = "white" {}
        _Glossiness ("Smoothness", Range(0,1)) = 0.5
        _Metallic ("Metallic", Range(0,1)) = 0.0
        _Cutoff("Base Alpha cutoff", Range(0,.9)) = .5
        _WindDirection("Wind Direction", Vector) = (1,0,0,0)
        _WindSpeed("Wind Speed", Float) = 1
        _WindScale("Wind Scale", Float) = 2
        _PlayerPos("Player position", Vector) = (0,0,0,0)
    }
    SubShader
    {
        Tags {
             "RenderType"="TransparentCutout"
             "Queue" = "AlphaTest"
             "IgnoreProjector" = "True"
             "ForceNoShadowCasting"="True"
             }
        LOD 200
        Cull Off
        ZWrite On

        CGPROGRAM
        // Physically based Standard lighting model, and enable shadows on all light types
        #pragma surface surf Standard alphatest:_Cutoff vertex:vert

        // Use shader model 3.0 target, to get nicer looking lighting
        #pragma target 3.0

        sampler2D _MainTex;

        struct Input
        {
            float2 uv_MainTex;
            half3 color:COLOR;
        };

        half _Glossiness;
        half _Metallic;
        fixed4 _Color;
        float2 _WindDirection;
        float _WindScale;
        float _WindSpeed;
        float3 _PlayerPos;

        // Add instancing support for this shader. You need to check 'Enable Instancing' on materials that use the shader.
        // See https://docs.unity3d.com/Manual/GPUInstancing.html for more information about instancing.
        // #pragma instancing_options assumeuniformscaling
        UNITY_INSTANCING_BUFFER_START(Props)
            // put more per-instance properties here
        UNITY_INSTANCING_BUFFER_END(Props)

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

        void vert(inout appdata_full v){
            float2 windDirection = _WindDirection;
            windDirection = normalize(windDirection);

            float windSpeed = _WindSpeed;
            float2 windDirectionFinal = windDirection * windSpeed;

            float2 offset = _Time.y.xx * windDirectionFinal;
            float3 objectToWorldPos = mul(unity_ObjectToWorld, v.vertex).xyz;

            float3 dist = distance(_PlayerPos, objectToWorldPos);
            float3 circle = 1 - saturate(dist/ 1);
            float3 sphereDist = objectToWorldPos - _PlayerPos ;
            sphereDist -= circle;
            
            float3 viewSpace = UnityObjectToViewPos(v.vertex).xyz;

            float2 tiligOut;
            Unity_TilingAndOffset_float(viewSpace.xy, float2(1,1), offset, tiligOut);
            objectToWorldPos -= mul(unity_ObjectToWorld, float4(0.0, 0.0, 0.0, 1.0)).xyz;

            float windScale = _WindScale;
            float gradientNoise;
            Unity_GradientNoise_float(tiligOut, windScale, gradientNoise);
            float swayStrength = gradientNoise - 0.5f;
            clamp(swayStrength, -0.01, 0.01);

            float swayFinal = swayStrength + objectToWorldPos.x;

            float3 swayTransform = float3(swayFinal, objectToWorldPos.y, objectToWorldPos.z);
            
            float3 swayLockInY;
            Unity_Lerp_float3(objectToWorldPos, swayTransform, v.vertex.y, swayLockInY);
            
            v.vertex = mul(unity_WorldToObject, swayLockInY);
            v.vertex.xz += (circle.xz * -sphereDist.xz) * 0.65;
            half shadowTint = clamp(gradientNoise, 0.6, 1);
            shadowTint += 0.4;
            v.color = half4(shadowTint, shadowTint, shadowTint, 0);
        }

        void surf (Input IN, inout SurfaceOutputStandard o)
        {
            // Albedo comes from a texture tinted by color
            fixed4 c = tex2D (_MainTex, IN.uv_MainTex) * _Color;
            o.Albedo = c * IN.color;
            // Metallic and smoothness come from slider variables
            o.Metallic = _Metallic;
            o.Smoothness = _Glossiness;
            o.Alpha = c.a;
        }
        ENDCG
    }
    FallBack "Diffuse"
}
