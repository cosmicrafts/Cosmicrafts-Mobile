Shader "Custom/FogOfWar"
{
    Properties
    {
        _MainTex ("Base (RGB)", 2D) = "white" {}
        _FogColor ("Fog Color", Color) = (0, 0, 0, 1) // Fog color (black)
        _PlayerPos ("Player Position", Vector) = (0, 0, 0, 0) // Default player position
        _RevealRadius ("Reveal Radius", float) = 5.0 // Radius of revealed area
    }
    SubShader
    {
        Tags { "RenderType"="Opaque" }
        LOD 100

        Pass
        {
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

            sampler2D _MainTex;
            float4 _PlayerPos;
            float _RevealRadius;
            float4 _FogColor;

            v2f vert (appdata v)
            {
                v2f o;
                o.vertex = UnityObjectToClipPos(v.vertex);
                o.uv = v.uv;
                return o;
            }

            fixed4 frag (v2f i) : SV_Target
            {
                float2 worldPos = i.uv.xy * _ScreenParams.xy;
                float distanceFromPlayer = distance(worldPos, _PlayerPos.xy);

                // Calculate the alpha value based on the distance from the player
                float alpha = smoothstep(_RevealRadius, _RevealRadius - 1.0, distanceFromPlayer);

                // Mix between the texture and fog color based on the alpha
                fixed4 color = tex2D(_MainTex, i.uv);
                return lerp(_FogColor, color, alpha);
            }
            ENDCG
        }
    }
    FallBack "Diffuse"
}
