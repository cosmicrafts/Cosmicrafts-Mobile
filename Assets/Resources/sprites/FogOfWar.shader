Shader "Custom/SimpleFogOfWar"
{
    Properties
    {
        _PlayerPos ("Player Position", Vector) = (0, 0, 0, 0)
        _RevealRadius ("Reveal Radius", Float) = 5.0
        _FogColor ("Fog Color", Color) = (0, 0, 0, 1)  // Black color for fog
    }
    SubShader
    {
        Tags { "RenderType"="Opaque" }
        LOD 200

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
                // Calculate distance from player position in screen space
                float2 pixelPos = i.uv * _ScreenParams.xy;
                float dist = distance(pixelPos, _PlayerPos.xy);

                // Determine visibility based on the distance to the player
                float visibility = smoothstep(_RevealRadius, 0.0, dist);

                // Return the fog color where visibility is 1 (completely fogged)
                // and reveal based on visibility
                return lerp(_FogColor, fixed4(1, 1, 1, 1), visibility);
            }
            ENDCG
        }
    }
}
