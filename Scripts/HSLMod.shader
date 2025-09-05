Shader "Unlit/HSLTint" {
    Properties {
        _MainTex ("Diffuse", 2D) = "white" {}
        _Tint ("Color Tint", Color) = (1,1,1,1)
        _HueShift ("Hue Shift (Degrees)", Float) = 0
        _Saturation ("Saturation Scale", Float) = 1
        _Luminance ("Luminance Scale", Float) = 1
    }
    SubShader {
        Tags { "RenderType"="Transparent" "Queue"="Transparent" }
        LOD 100

        Pass {
            Blend SrcAlpha OneMinusSrcAlpha
            ZWrite Off
            CGPROGRAM
            #pragma vertex vert
            #pragma fragment frag
            #include "UnityCG.cginc"

            sampler2D _MainTex;
            float4 _MainTex_ST;
            float4 _Tint;
            float _HueShift;
            float _Saturation;
            float _Luminance;

            struct appdata {
                float4 vertex : POSITION;
                float2 uv : TEXCOORD0;
            };

            struct v2f {
                float2 uv : TEXCOORD0;
                float4 vertex : SV_POSITION;
            };

            float3 RGBToHSL(float3 c) {
                float maxc = max(c.r, max(c.g, c.b));
                float minc = min(c.r, min(c.g, c.b));
                float l = (maxc + minc) * 0.5;
                float s = 0;
                float h = 0;

                if (maxc != minc) {
                    float d = maxc - minc;
                    s = l < 0.5 ? d / (maxc + minc) : d / (2.0 - maxc - minc);
                    if (maxc == c.r) h = (c.g - c.b) / d + (c.g < c.b ? 6.0 : 0.0);
                    else if (maxc == c.g) h = (c.b - c.r) / d + 2.0;
                    else h = (c.r - c.g) / d + 4.0;
                    h /= 6.0;
                }
                return float3(h, s, l);
            }

            float HueToRGB(float p, float q, float t) {
                if (t < 0.0) t += 1.0;
                if (t > 1.0) t -= 1.0;
                if (t < 1.0/6.0) return p + (q - p) * 6.0 * t;
                if (t < 1.0/2.0) return q;
                if (t < 2.0/3.0) return p + (q - p) * (2.0/3.0 - t) * 6.0;
                return p;
            }

            float3 HSLToRGB(float3 hsl) {
                float r, g, b;
                float h = hsl.x;
                float s = hsl.y;
                float l = hsl.z;

                if (s == 0.0) {
                    r = g = b = l;
                } else {
                    float q = l < 0.5 ? l * (1.0 + s) : l + s - l * s;
                    float p = 2.0 * l - q;
                    r = HueToRGB(p, q, h + 1.0/3.0);
                    g = HueToRGB(p, q, h);
                    b = HueToRGB(p, q, h - 1.0/3.0);
                }
                return float3(r, g, b);
            }

            v2f vert (appdata v) {
                v2f o;
                o.vertex = UnityObjectToClipPos(v.vertex);
                o.uv = TRANSFORM_TEX(v.uv, _MainTex);
                return o;
            }

            fixed4 frag (v2f i) : SV_Target {
                float4 col = tex2D(_MainTex, i.uv);
                col.rgb *= _Tint.rgb;

                float3 hsl = RGBToHSL(col.rgb);
                hsl.x = frac(hsl.x + _HueShift / 360.0);
                hsl.y *= _Saturation;
                hsl.z *= _Luminance;
                col.rgb = HSLToRGB(hsl);

                return col;
            }
            ENDCG
        }
    }
}
