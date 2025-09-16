Shader "Custom/SpriteEmboss"
{
    Properties
    {
        [NoScaleOffset]_MainTex ("Sprite", 2D) = "white" {}
        _Color ("Tint", Color) = (1,1,1,1)

        _AlphaThresh ("Alpha Threshold", Range(0,1)) = 0.1
        _Steps ("Steps (1-24)", Range(1,24)) = 8

        _HLColor ("Highlight Color", Color) = (1,1,0.7,1)
        _HLAmount ("Highlight Amount", Range(0,1)) = 0.8
        _HLStepDelta ("Highlight Step Delta (px, TL)", Vector) = (-1, 1, 0, 0)

        _SHColor ("Shadow Color", Color) = (0,0,0,1)
        _SHAmount ("Shadow Amount", Range(0,1)) = 0.6
        _SHStepDelta ("Shadow Step Delta (px, BR)", Vector) = (1, -1, 0, 0)
    }
    SubShader
    {
        Tags { "Queue"="Transparent" "RenderType"="Transparent" "IgnoreProjector"="True" "CanUseSpriteAtlas"="True" }
        LOD 100
        Cull Off
        Lighting Off
        ZWrite Off
        Blend SrcAlpha OneMinusSrcAlpha

        Pass
        {
            CGPROGRAM
            #pragma vertex vert
            #pragma fragment frag
            #include "UnityCG.cginc"

            sampler2D _MainTex;
            float4 _MainTex_TexelSize; // x=1/w, y=1/h
            fixed4 _Color;

            float _AlphaThresh;
            int _Steps;

            fixed4 _HLColor;
            float _HLAmount;
            float4 _HLStepDelta; // in pixels (x,y)

            fixed4 _SHColor;
            float _SHAmount;
            float4 _SHStepDelta; // in pixels (x,y)

            struct appdata
            {
                float4 vertex : POSITION;
                float2 uv     : TEXCOORD0;
                fixed4 color  : COLOR;
            };

            struct v2f
            {
                float4 pos : SV_POSITION;
                float2 uv  : TEXCOORD0;
                fixed4 color : COLOR;
            };

            v2f vert (appdata v)
            {
                v2f o;
                o.pos = UnityObjectToClipPos(v.vertex);
                o.uv = v.uv;
                o.color = v.color * _Color;
                return o;
            }

            // Returns [0..1] intensity based on how many samples hit alpha < thresh along a direction
            float SampleTransparencyAlong(float2 uv, float2 dirUV, int steps, float alphaThresh)
            {
                // Early out if already off-texture (rare with sprite UVs)
                float hitCount = 0.0;
                float2 cur = uv;
                [loop]
                for (int i = 1; i <= steps; i++)
                {
                    cur += dirUV; // walk one step
                    fixed4 s = tex2D(_MainTex, cur);
                    // if we encounter transparent (below threshold), count it
                    hitCount += (s.a < alphaThresh) ? 1.0 : 0.0;
                }
                return saturate(hitCount / max(1, steps));
            }

            fixed4 frag (v2f i) : SV_Target
            {
                fixed4 baseCol = tex2D(_MainTex, i.uv) * i.color;

                // Convert step deltas (in pixels) to UV space
                float2 hlStepUV = float2(_HLStepDelta.x * _MainTex_TexelSize.x,
                                         _HLStepDelta.y * _MainTex_TexelSize.y);
                float2 shStepUV = float2(_SHStepDelta.x * _MainTex_TexelSize.x,
                                         _SHStepDelta.y * _MainTex_TexelSize.y);

                // How strongly each direction sees "edge to transparency"
                float hlEdge = SampleTransparencyAlong(i.uv, hlStepUV, _Steps, _AlphaThresh);
                float shEdge = SampleTransparencyAlong(i.uv, shStepUV, _Steps, _AlphaThresh);

                // Final blend factors
                float hlFactor = saturate(hlEdge * _HLAmount);
                float shFactor = saturate(shEdge * _SHAmount);

                // Blend highlight & shadow over base (lerp by factors)
                fixed3 col = baseCol.rgb;
                col = lerp(col, _HLColor.rgb, hlFactor);
                col = lerp(col, _SHColor.rgb, shFactor);

                // Preserve sprite alpha
                return fixed4(col, baseCol.a);
            }
            ENDCG
        }
    }
    FallBack "Transparent/Diffuse"
}
