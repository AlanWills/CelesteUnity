Shader "Celeste/Sprite Outline"
{
    Properties
    {
        [PerRendererData] _MainTex ("Sprite Texture", 2D) = "white" {}
        _Color ("Tint", Color) = (1,1,1,1)
        
        [Header(Outline Settings)]
        _OutlineColour ("Outline Colour", Color) = (1, 1, 1, 1)
        _OutlineWidth ("Outline Width", Range(0, 10)) = 1
    }

    SubShader
    {
        Tags
        { 
            "Queue"="Transparent" 
            "IgnoreProjector"="True" 
            "RenderType"="Transparent" 
            "PreviewType"="Plane"
            "CanUseSpriteAtlas"="True"
        }

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

            struct appdata_t
            {
                float4 vertex   : POSITION;
                float4 color    : COLOR;
                float2 texcoord : TEXCOORD0;
            };

            struct v2f
            {
                float4 vertex   : SV_POSITION;
                fixed4 color    : COLOR;
                float2 texcoord : TEXCOORD0;
            };

            fixed4 _Color;
            sampler2D _MainTex;
            float4 _MainTex_TexelSize; // Automatically populated by Unity
            
            fixed4 _OutlineColour;
            float _OutlineWidth;

            v2f vert(appdata_t IN)
            {
                v2f OUT;
                OUT.vertex = UnityObjectToClipPos(IN.vertex);
                OUT.texcoord = IN.texcoord;
                OUT.color = IN.color * _Color;
                return OUT;
            }

            fixed4 frag(v2f IN) : SV_Target
            {
                // Sample the original pixel
                fixed4 c = tex2D(_MainTex, IN.texcoord) * IN.color;

                // If the pixel is mostly transparent, we check neighbors for an outline
                if (c.a < 0.1)
                {
                    float w = _OutlineWidth;
                    float tx = _MainTex_TexelSize.x * w;
                    float ty = _MainTex_TexelSize.y * w;

                    // Check the 8 surrounding neighboring pixels (Up, Down, Left, Right + Diagonals)
                    float alpha = 0;
                    alpha = max(alpha, tex2D(_MainTex, IN.texcoord + float2(0, ty)).a);
                    alpha = max(alpha, tex2D(_MainTex, IN.texcoord + float2(0, -ty)).a);
                    alpha = max(alpha, tex2D(_MainTex, IN.texcoord + float2(tx, 0)).a);
                    alpha = max(alpha, tex2D(_MainTex, IN.texcoord + float2(-tx, 0)).a);
                    
                    alpha = max(alpha, tex2D(_MainTex, IN.texcoord + float2(tx, ty)).a);
                    alpha = max(alpha, tex2D(_MainTex, IN.texcoord + float2(-tx, ty)).a);
                    alpha = max(alpha, tex2D(_MainTex, IN.texcoord + float2(tx, -ty)).a);
                    alpha = max(alpha, tex2D(_MainTex, IN.texcoord + float2(-tx, -ty)).a);

                    // If any neighbor has alpha, this pixel becomes part of the outline
                    if (alpha > 0.1)
                    {
                        // Return the outline color with its own alpha intact
                        return fixed4(_OutlineColour.rgb, _OutlineColour.a * alpha);
                    }
                }

                return c;
            }
            ENDCG
        }
    }
}