Shader "Custom/URPLitGateClip"
{
    Properties
    {
        [Header(Base Textures)]
        _BaseColor ("Base Color", Color) = (1, 1, 1, 1)
        _MainTex ("Main Texture", 2D) = "white" {}
        _NoiseTex ("Noise Texture (Grayscale)", 2D) = "white" {}
        _NoiseStrength ("Noise Strength", Range(0, 1)) = 0.05
        
        [Header(Material Settings)]
        _Smoothness ("Smoothness", Range(0, 1)) = 0.5
        
        [Header(World Clip Settings)]
        _ClipPosition ("Clip Position (World)", Vector) = (0, 0, 0, 1)
        _ClipNormal ("Clip Normal (World)", Vector) = (0, 0, 1, 0)
        
        [Header(Glow Settings)]
        [HDR] _GlowColor ("Glow Color", Color) = (1, 1, 1, 1)
        _GlowWidth ("Glow Width", Range(0.001, 0.5)) = 0.05
        _GlowIntensity ("Glow Intensity", Float) = 2.0
    }
    SubShader
    {
        Tags 
        { 
            "RenderType" = "Opaque" 
            "Queue" = "Geometry"
            "RenderPipeline" = "UniversalPipeline"
        }
        
        Cull Off

        Pass
        {
            Name "ForwardLit"
            Tags { "LightMode" = "UniversalForward" }

            HLSLPROGRAM
            #pragma vertex vert
            #pragma fragment frag
            #pragma target 3.0

            #pragma multi_compile _ _MAIN_LIGHT_SHADOWS _MAIN_LIGHT_SHADOWS_CASCADE _MAIN_LIGHT_SHADOWS_SCREEN
            #pragma multi_compile _ _ADDITIONAL_LIGHTS_VERTEX _ADDITIONAL_LIGHTS
            #pragma multi_compile_fragment _ _ADDITIONAL_LIGHT_SHADOWS
            #pragma multi_compile_fragment _ _SHADOWS_SOFT
            #pragma multi_compile_fog

            #include "Packages/com.unity.render-pipelines.universal/ShaderLibrary/Core.hlsl"
            #include "Packages/com.unity.render-pipelines.universal/ShaderLibrary/Lighting.hlsl"

            struct Attributes
            {
                float4 positionOS   : POSITION;
                float3 normalOS     : NORMAL;
                float2 uv           : TEXCOORD0;
            };

            struct Varyings
            {
                float4 positionCS   : SV_POSITION;
                float2 uv           : TEXCOORD0;
                float3 positionWS   : TEXCOORD1;
                float3 normalWS     : NORMAL;
                float4 shadowCoord  : TEXCOORD4;
                float  fogFactor    : TEXCOORD5;
            };

            TEXTURE2D(_MainTex);
            SAMPLER(sampler_MainTex);
            TEXTURE2D(_NoiseTex);
            SAMPLER(sampler_NoiseTex);

            CBUFFER_START(UnityPerMaterial)
                float4 _BaseColor;
                float4 _MainTex_ST;
                float4 _NoiseTex_ST;
                float4 _ClipPosition;
                float4 _ClipNormal;
                float _NoiseStrength;
                float4 _GlowColor;
                float _GlowWidth;
                float _GlowIntensity;
                float _Smoothness;
            CBUFFER_END

            Varyings vert(Attributes input)
            {
                Varyings output;
                VertexPositionInputs posInputs = GetVertexPositionInputs(input.positionOS.xyz);
                output.positionCS = posInputs.positionCS;
                output.positionWS = posInputs.positionWS;
                
                VertexNormalInputs normalInputs = GetVertexNormalInputs(input.normalOS, float4(1, 1, 1, 1));
                output.normalWS = normalInputs.normalWS;
                
                output.uv = input.uv * _MainTex_ST.xy + _MainTex_ST.zw;
                output.shadowCoord = GetShadowCoord(posInputs);
                output.fogFactor = ComputeFogFactor(posInputs.positionCS.z);
                
                return output;
            }

            half4 frag(Varyings input) : SV_Target
            {
                // 1. Sample Noise
                float2 noiseUV = input.uv * _NoiseTex_ST.xy + _NoiseTex_ST.zw;
                float noise = SAMPLE_TEXTURE2D(_NoiseTex, sampler_NoiseTex, noiseUV).r;
                
                // 2. Tính khoảng cách từ pixel hiện tại tới mặt phẳng cắt (World Space)
                float3 normClipDir = normalize(_ClipNormal.xyz);
                float dist = dot(input.positionWS - _ClipPosition.xyz, normClipDir);
                
                // 3. Kết hợp khoảng cách và noise để làm mấp mô vệt cắt
                float clipFactor = dist + (noise - 0.5) * _NoiseStrength;
                
                // 4. Cắt bỏ pixel nằm ở phía bên kia mặt phẳng (clipFactor < 0)
                clip(clipFactor);
                
                // 5. Màu cơ bản & Ánh sáng URP Lit
                half4 albedo = SAMPLE_TEXTURE2D(_MainTex, sampler_MainTex, input.uv) * _BaseColor;
                Light mainLight = GetMainLight(input.shadowCoord);
                
                half3 N = normalize(input.normalWS);
                half3 L = normalize(mainLight.direction);
                half NdotL = saturate(dot(N, L));
                
                half3 ambient = SampleSH(N) * albedo.rgb;
                half3 diffuse = mainLight.color * NdotL * mainLight.shadowAttenuation;
                
                half3 additionalDiffuse = 0;
                int additionalLightsCount = GetAdditionalLightsCount();
                for (int i = 0; i < additionalLightsCount; ++i)
                {
                    Light light = GetAdditionalLight(i, input.positionWS);
                    half3 addL = normalize(light.direction);
                    half addNdotL = saturate(dot(N, addL));
                    additionalDiffuse += light.color * addNdotL * light.distanceAttenuation * light.shadowAttenuation;
                }
                
                half3 litColor = (ambient + diffuse + additionalDiffuse) * albedo.rgb;
                
                // 6. Tạo viền phát sáng (Glow) ngay tại mặt cắt
                if (clipFactor < _GlowWidth)
                {
                    float glowLerp = 1.0 - (clipFactor / _GlowWidth);
                    half4 glow = _GlowColor * _GlowIntensity * glowLerp;
                    litColor = lerp(litColor, glow.rgb, glowLerp);
                }
                
                half4 finalColor = half4(litColor, albedo.a);
                finalColor.rgb = MixFog(finalColor.rgb, input.fogFactor);
                
                return finalColor;
            }
            ENDHLSL
        }
        
        // Pass ShadowCaster để bóng đổ cũng bị cắt khớp với Gate
        Pass
        {
            Name "ShadowCaster"
            Tags { "LightMode" = "ShadowCaster" }

            ZWrite On
            ZTest LEqual
            ColorMask 0
            Cull Off

            HLSLPROGRAM
            #pragma vertex vert
            #pragma fragment frag
            #pragma multi_compile_shadowcaster
            #pragma target 2.0

            #include "Packages/com.unity.render-pipelines.universal/ShaderLibrary/Core.hlsl"
            #include "Packages/com.unity.render-pipelines.universal/ShaderLibrary/Shadows.hlsl"

            struct Attributes
            {
                float4 positionOS   : POSITION;
                float3 normalOS     : NORMAL;
                float2 uv           : TEXCOORD0;
            };

            struct Varyings
            {
                float4 positionCS   : SV_POSITION;
                float2 uv           : TEXCOORD0;
                float3 positionWS   : TEXCOORD1;
            };

            TEXTURE2D(_NoiseTex);
            SAMPLER(sampler_NoiseTex);

            CBUFFER_START(UnityPerMaterial)
                float4 _NoiseTex_ST;
                float4 _ClipPosition;
                float4 _ClipNormal;
                float _NoiseStrength;
            CBUFFER_END

            Varyings vert(Attributes input)
            {
                Varyings output;
                VertexPositionInputs posInputs = GetVertexPositionInputs(input.positionOS.xyz);
                VertexNormalInputs normalInputs = GetVertexNormalInputs(input.normalOS, float4(1, 1, 1, 1));
                
                float3 positionWS = posInputs.positionWS;
                float4 positionCS = TransformWorldToHClip(ApplyShadowBias(positionWS, normalInputs.normalWS, _MainLightPosition.xyz));
                
                #if UNITY_REVERSED_Z
                    positionCS.z = min(positionCS.z, positionCS.w * UNITY_NEAR_CLIP_VALUE);
                #else
                    positionCS.z = max(positionCS.z, positionCS.w * UNITY_NEAR_CLIP_VALUE);
                #endif

                output.positionCS = positionCS;
                output.uv = input.uv;
                output.positionWS = positionWS;
                return output;
            }

            half4 frag(Varyings input) : SV_Target
            {
                float2 noiseUV = input.uv * _NoiseTex_ST.xy + _NoiseTex_ST.zw;
                float noise = SAMPLE_TEXTURE2D(_NoiseTex, sampler_NoiseTex, noiseUV).r;
                
                float3 normClipDir = normalize(_ClipNormal.xyz);
                float dist = dot(input.positionWS - _ClipPosition.xyz, normClipDir);
                float clipFactor = dist + (noise - 0.5) * _NoiseStrength;
                
                clip(clipFactor);
                return 0;
            }
            ENDHLSL
        }
    }
}
