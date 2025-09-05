Shader "UI/AlphaMaskedGleam"
{
    Properties
    {
        _Color ("Color", Color) = (1,1,1,1)
        _MainTex ("Main Texture", 2D) = "white" {}
        _GleamTex ("Gleam Texture", 2D) = "white" {}
        _GleamOffset ("Gleam Offset", Float) = 0.0
        _GleamOffsetV ("Gleam OffsetV", Float) = 0.0
        _GleamSpeed ("Gleam Speed", Float) = 1.0
        _GleamIntensity ("Gleam Intensity", Range(0, 1)) = 0.5

        _PaddingLeft ("Padding Left", Range(0,1)) = 0
        _PaddingRight ("Padding Right", Range(0,1)) = 0
        _PaddingTop ("Padding Top", Range(0,1)) = 0
        _PaddingBottom ("Padding Bottom", Range(0,1)) = 0
    }

    SubShader
    {
        Tags { "Queue" = "Transparent" "IgnoreProjector" = "True" "RenderType" = "Transparent" }
        LOD 100

        Pass
        {
            Blend SrcAlpha OneMinusSrcAlpha
            ZWrite Off
            Cull Off

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

            sampler2D _MainTex;
            float4 _MainTex_ST;
            fixed4 _Color;

            sampler2D _GleamTex;
            float _GleamSpeed;
            float _GleamIntensity;
            float _GleamOffset;
            float _GleamOffsetV;

            float _PaddingLeft;
            float _PaddingRight;
            float _PaddingTop;
            float _PaddingBottom;

            v2f vert (appdata_t v)
            {
                v2f o;
                o.vertex = UnityObjectToClipPos(v.vertex);
                o.uv = TRANSFORM_TEX(v.uv, _MainTex);
                return o;
            }

            fixed4 frag (v2f i) : SV_Target
            {
                float2 uv = i.uv;

                // Padding clipping
                bool insideX = uv.x >= _PaddingLeft && uv.x <= (1.0 - _PaddingRight);
                bool insideY = uv.y >= _PaddingBottom && uv.y <= (1.0 - _PaddingTop);
                if (!(insideX && insideY)) return fixed4(0, 0, 0, 0); // fully clipped

                fixed4 baseCol = tex2D(_MainTex, uv);
                float2 gleamUV = uv + float2(_GleamOffset, _GleamOffsetV);
                fixed4 gleamCol = tex2D(_GleamTex, gleamUV);

                float mask = baseCol.a * gleamCol.a;
                fixed3 gleam = gleamCol.rgb * _GleamIntensity;

                fixed4 finalCol = baseCol;
                finalCol.rgb += gleam * mask;

                return finalCol * _Color;
            }
            ENDCG
        }
    }
}
