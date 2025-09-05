Shader "Custom/TreeSnowShader"
{
    Properties
    {
        _SpriteTex ("Sprite Texture", 2D) = "white" {}
        _NoiseTex ("Snow Noise Texture", 2D) = "white" {}
        _SecondaryTex ("Secondary Snow Texture", 2D) = "white" {}

        _Scale ("Sprite Tiling Scale", Vector) = (1, 1, 0, 0)

        _NoiseTex_ST ("Noise Tiling & Offset", Vector) = (1, 1, 0, 0)
        _SecondaryTex_ST ("Secondary Tiling & Offset", Vector) = (1, 1, 0, 0)

        _Threshold ("Luminance Threshold", Float) = 0.5
        _Add ("Luminance Add", Float) = 0.0
        _SecondaryMultiplier ("Secondary Luminance Multiplier", Float) = 1.0

        _MainColor ("Snow Color", Color) = (1, 1, 1, 1)
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

            struct appdata_t {
                float4 vertex : POSITION;
                float2 uv : TEXCOORD0;
            };

            struct v2f {
                float2 uv : TEXCOORD0;
                float4 vertex : SV_POSITION;
            };

            sampler2D _SpriteTex;
            sampler2D _NoiseTex;
            sampler2D _SecondaryTex;

            float4 _Scale;
            float4 _NoiseTex_ST;
            float4 _SecondaryTex_ST;

            float _Threshold;
            float _Add;
            float _SecondaryMultiplier;
            float4 _MainColor;
            float4 _EdgeColor;
            float _KernelDist;

            v2f vert (appdata_t v)
            {
                v2f o;
                o.vertex = UnityObjectToClipPos(v.vertex);
                o.uv = v.uv * _Scale.xy;
                return o;
            }

            fixed4 frag (v2f i) : SV_Target
            {
                float2 baseUV = i.uv;

                float4 sprite = tex2D(_SpriteTex, baseUV);
                if (sprite.a == 0) return float4(0, 0, 0, 0);

                float2 noiseUV = TRANSFORM_TEX(baseUV, _NoiseTex);
                //float2 secondaryUV = TRANSFORM_TEX(baseUV, _SecondaryTex);
                //float2 secondaryUV = TRANSFORM_TEX(baseUV);

                float3 noise = tex2D(_NoiseTex, noiseUV).rgb;
                float3 secondary = tex2D(_SecondaryTex, baseUV).rgb;

                float lum = dot(noise, float3(0.299, 0.587, 0.114)) + _Add + dot(secondary, float3(0.299, 0.587, 0.114)) * _SecondaryMultiplier;

                float2 offset = float2(_KernelDist, -_KernelDist) * _Scale.xy;
                float2 neighborUV = i.uv + offset;

                float2 noiseNeighborUV = TRANSFORM_TEX(neighborUV, _NoiseTex);
                float2 secondaryNeighborUV = TRANSFORM_TEX(neighborUV, _SecondaryTex);

                float3 neighborNoise = tex2D(_NoiseTex, noiseNeighborUV).rgb;
                float3 neighborSecondary = tex2D(_SecondaryTex, secondaryNeighborUV).rgb;

                float neighborLum = dot(neighborNoise, float3(0.299, 0.587, 0.114)) + _Add + dot(neighborSecondary, float3(0.299, 0.587, 0.114)) * _SecondaryMultiplier;

                if (lum > _Threshold)
                {
                    if (neighborLum <= _Threshold)
                        return _EdgeColor;

                    return lerp(sprite, _MainColor, _MainColor.a);
                }

                return sprite;
            }
            ENDCG
        }
    }
    FallBack "Unlit/Transparent"
}
