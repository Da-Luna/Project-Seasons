Shader "TLSShader/BurningPaper"
{
    Properties {
        _BaseMap ("Base Map", 2D) = "white" {}
        _BaseColor ("Base Color", Color) = (1,1,1,1)
        _EffectColor("Effect Color", Color) = (1,0,0,1) // Standardeffektfarbe ist Rot
        _AlphaClip ("Alpha Clip", Range(0, 1)) = 0
        _Metallic ("Metallic", Range(0, 1)) = 0.0
        _DissolveTex ("Dissolution Texture", 2D) = "gray" {}
        _Progress ("Progress", Range(0, 1.1)) = 0
    }

    SubShader {
        Tags { "Queue"="Geometry" }
        Pass {
            Blend SrcAlpha OneMinusSrcAlpha // Additive blending
            CGPROGRAM
            #pragma vertex vert
            #pragma fragment frag
            #pragma multi_compile_instancing
            #include "UnityCG.cginc"

            struct appdata {
                float4 vertex : POSITION;
                float2 uv : TEXCOORD0;
            };

            struct v2f {
                float2 uv : TEXCOORD0;
                UNITY_FOG_COORDS(1)
                float4 vertex : SV_POSITION;
            };

            sampler2D _BaseMap;
            float4 _BaseColor;
            float4 _EffectColor;
            float _Metallic;

            v2f vert(appdata v) {
                v2f o;
                o.vertex = UnityObjectToClipPos(v.vertex);
                o.uv = v.uv;
                UNITY_TRANSFER_FOG(o,o.vertex);
                return o;
            }

            sampler2D _DissolveTex;
            float _Progress;
            float _AlphaClip;

            fixed4 frag(v2f i) : SV_Target {
                fixed4 baseColor = tex2D(_BaseMap, i.uv) * _BaseColor;
                fixed4 effectColor = _EffectColor;

                // Berechne die Glätte (Metallic-Effekt) nur für die Basisfarbe
                baseColor.rgb *= _Metallic;

                fixed val = 1 - tex2D(_DissolveTex, i.uv).r;
                if (val < (1 - _Progress) - 0.04 || baseColor.a < _AlphaClip) {
                    discard;
                }

                bool b = val < (1 - _Progress);
                // Effektfarbe über Basisfarbe legen
                fixed4 finalColor = lerp(baseColor, baseColor + effectColor, b);
                return finalColor;
            }
            ENDCG
        }
    }
    FallBack "Diffuse"
}
