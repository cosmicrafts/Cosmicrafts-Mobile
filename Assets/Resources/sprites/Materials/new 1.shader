Shader "Custom/Particles/AdditiveNativeGlow"
{
    Properties
    {
        _MainTex ("Particle Texture", 2D) = "white" {}
        _Threshold ("Black Level Threshold", Range(0,1)) = 0.1
        _GlowIntensity ("Glow Intensity", Range(0,5)) = 1.5
    }
    
    SubShader
    {
        Tags { "Queue"="Transparent" "RenderType"="Transparent" }
        LOD 200
        
        Pass
        {
            ZWrite Off
            Blend SrcAlpha One
            Cull Off
            Lighting Off
            Fog { Mode Off }
            
            CGPROGRAM
            #pragma vertex vert
            #pragma fragment frag
            #include "UnityCG.cginc"

            sampler2D _MainTex;
            float4 _MainTex_ST;
            float _Threshold;
            float _GlowIntensity;

            struct appdata
            {
                float4 vertex : POSITION;
                float2 uv : TEXCOORD0;
                float4 color : COLOR;
            };

            struct v2f
            {
                float4 pos : SV_POSITION;
                float2 uv : TEXCOORD0;
                float4 color : COLOR;
            };

            v2f vert (appdata v)
            {
                v2f o;
                o.pos = UnityObjectToClipPos(v.vertex);
                o.uv = TRANSFORM_TEX(v.uv, _MainTex);
                o.color = v.color;
                return o;
            }

            fixed4 frag (v2f i) : SV_Target
            {
                // Sample texture color
                fixed4 texColor = tex2D(_MainTex, i.uv) * i.color;

                // Black Level Suppression (set black pixels to transparent)
                if (texColor.r < _Threshold && texColor.g < _Threshold && texColor.b < _Threshold)
                {
                    texColor.a = 0;
                }

                // Native Glow Effect - Intensify the brighter colors based on alpha
                float glowFactor = saturate(texColor.a * _GlowIntensity);
                
                // Increase the brightness of the sprite's own colors for a glow effect
                texColor.rgb *= 1.0 + glowFactor;

                return texColor;
            }
            ENDCG
        }
    }
}
