Shader "Custom/SnowFloorGapsShader"
{
    Properties
    {
        _MainTex ("Main Texture", 2D) = "white" {}
        _SecondaryTex ("Secondary Texture", 2D) = "white" {}
        _Scale ("Tiling Scale", Vector) = (1, 1, 1, 1)
        _Threshold ("Luminance Threshold", Float) = 0.5
        _Add ("Luminance Add", Float) = 0.0
        _SecondaryMultiplier ("Secondary Luminance Multiplier", Float) = 1.0
        _MainColor ("Main Color", Color) = (1, 1, 1, 1)
        _EdgeColor ("Edge Color", Color) = (0.5, 0.7, 1.0, 1)
        _KernelDist ("Kernel Distance", Float) = 1.0
    }
    SubShader
    {
        Tags { "RenderType"="Transparent" "Queue"="Transparent" }
        Blend SrcAlpha OneMinusSrcAlpha
        ZWrite Off
        LOD 100


        Pass
        {
            CGPROGRAM
            #pragma vertex vert
            #pragma fragment frag
            #include "UnityCG.cginc"

            struct appdata_t
            {
                float4 vertex : POSITION;
                float2 uv : TEXCOORD0;
            };

            struct v2f
            {
                float2 uv : TEXCOORD0;
                float4 vertex : SV_POSITION;
            };

            sampler2D _MainTex;
            sampler2D _SecondaryTex;
            float4 _MainTex_ST;
            float4 _SecondaryTex_ST;
            float _Threshold;
            float _Add;
            float _SecondaryMultiplier;
            float4 _MainColor;
            float4 _EdgeColor;
            float2 _Scale;
            float _KernelDist;

            v2f vert (appdata_t v)
            {
                v2f o;
                o.vertex = UnityObjectToClipPos(v.vertex);

                // Ensure UV tiling with scale applied
                o.uv = v.uv * _Scale.xy;
                return o;
            }

            fixed4 frag (v2f i) : SV_Target
            {
                // Modulo ensures tiling without clamping
                float2 tiledUV = frac(i.uv);

                // Calculate luminance for main texture
                float3 texColor = tex2D(_MainTex, tiledUV).rgb;
                float luminance = dot(texColor, float3(0.299, 0.587, 0.114));

                // Calculate luminance for secondary texture
                float3 secondaryTexColor = tex2D(_SecondaryTex, tiledUV).rgb;
                float secondaryLuminance = dot(secondaryTexColor, float3(0.299, 0.587, 0.114));

                // Combine luminance values
                float combinedLuminance = luminance + _Add + (secondaryLuminance * _SecondaryMultiplier);

                // Check kernel fragment (right neighbor)
                float2 kernelOffset = float2(_KernelDist, -_KernelDist) * _Scale.xy;
                float2 kernelUV = frac(tiledUV + kernelOffset);

                float3 kernelTexColor = tex2D(_MainTex, kernelUV).rgb;
                float kernelLuminance = dot(kernelTexColor, float3(0.299, 0.587, 0.114));

                float3 kernelSecondaryTexColor = tex2D(_SecondaryTex, kernelUV).rgb;
                float kernelSecondaryLuminance = dot(kernelSecondaryTexColor, float3(0.299, 0.587, 0.114));

                float combinedKernelLuminance = kernelLuminance + _Add + (kernelSecondaryLuminance * _SecondaryMultiplier);

                if (combinedLuminance > _Threshold)
                {
                    if (combinedKernelLuminance <= _Threshold)
                    {
                        return _EdgeColor; // Edge fragment
                    }
                    return _MainColor; // Main fragment
                }
                return float4(0, 0, 0, 0); // Transparent
            }
            ENDCG
        }
    }
    FallBack "Unlit/Transparent"
}
