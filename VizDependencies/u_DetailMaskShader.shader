Shader "Custom/DetailMaskShader"
{
    Properties
    {
        _Color ("Base Color", Color) = (1, 1, 1, 1)
        _LightColor ("Light Color", Color) = (1, 1, 1, 1)
        _DetailMap ("Detail Map", 2D) = "white" {}
        _DetailColor ("Detail Color", Color) = (1, 1, 1, 1)
        _MaskMap ("Mask Map (R channel)", 2D) = "white" {}
        _DetailTiling ("Detail Tiling (XY)", Vector) = (1, 1, 0, 0)
        _MaskTiling ("Mask Tiling (XY)", Vector) = (1, 1, 0, 0)
        [Toggle] _UseMaskMap ("Use Mask Map", Float) = 1
        _MaskThreshold ("Mask Threshold", Range(0, 1)) = 0.5
    }

    SubShader
    {
        Tags { "RenderType"="Transparent" "Queue"="Transparent" }
        LOD 100

        Pass
        {
            Blend SrcAlpha OneMinusSrcAlpha
            ZWrite Off

            CGPROGRAM
            #pragma vertex vert
            #pragma fragment frag
            #include "UnityCG.cginc"

            struct appdata
            {
                float4 vertex : POSITION;
                float2 uv : TEXCOORD0;
            };

            struct v2f
            {
                float2 uv : TEXCOORD0;
                float4 vertex : SV_POSITION;
            };

            float4 _Color;
            float4 _LightColor;
            sampler2D _DetailMap;
            float4 _DetailColor;
            sampler2D _MaskMap;
            float4 _DetailTiling;
            float4 _MaskTiling;
            float _UseMaskMap;
            float _MaskThreshold;

            v2f vert (appdata v)
            {
                v2f o;
                o.vertex = UnityObjectToClipPos(v.vertex);
                o.uv = v.uv;
                return o;
            }

            fixed4 frag (v2f i) : SV_Target
            {
                // Sample detail map with tiling (using frac for repeat)
                float2 detailUV = frac(i.uv * _DetailTiling.xy);
                float4 detailColor = tex2D(_DetailMap, detailUV) * _DetailColor;

                // Start with base color and lighting
                float4 finalColor = _Color;
                finalColor.rgb *= _LightColor.rgb;

                // Handle masking and blending
                if (_UseMaskMap > 0.5)
                {
                    // Sample mask with tiling
                    float2 maskUV = frac(i.uv * _MaskTiling.xy);
                    float maskR = tex2D(_MaskMap, maskUV).r;
                    
                    if (maskR > _MaskThreshold)
                    {
                        // Full detail above threshold
                        finalColor.rgb = lerp(finalColor.rgb, detailColor.rgb, detailColor.a);
                    }
                    else
                    {
                        // Below threshold - blend using mask R value
                        float blendFactor = detailColor.a * .5;
                        finalColor.rgb = lerp(finalColor.rgb, detailColor.rgb, blendFactor);
                    }
                }
                else
                {
                    // No mask, full detail
                    finalColor.rgb = lerp(finalColor.rgb, detailColor.rgb, detailColor.a);
                }

                // Preserve base alpha
                finalColor.a = _Color.a;

                return finalColor;
            }
            ENDCG
        }
    }
}
