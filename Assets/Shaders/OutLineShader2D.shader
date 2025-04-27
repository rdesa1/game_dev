Shader "Custom/SpriteOutline"
{
    Properties
    {
        _MainTex("Sprite Texture", 2D) = "white" {}
        _OutlineColor("Outline Color", Color) = (0,0,0,1)
        _OutlineThickness("Outline Thickness", Range(0, 10)) = 1
    }
    SubShader
    {
        Tags
        {
            "RenderType"="Opaque"
            "Queue"="Transparent"
        }
        LOD 100

        Pass
        {
            Cull Off
            ZWrite Off
            Blend SrcAlpha OneMinusSrcAlpha

            CGPROGRAM
            #pragma vertex vert
            #pragma fragment frag
            #pragma multi_compile _ ETC1_EXTERNAL_ALPHA // Important for SpriteRenderer batching

            #include "UnityCG.cginc"

            struct appdata_t
            {
                float4 vertex : POSITION;
                float2 texcoord : TEXCOORD0;
            };

            struct v2f
            {
                float4 vertex : SV_POSITION;
                float2 texcoord : TEXCOORD0;
            };

            sampler2D _MainTex;
            float4 _MainTex_ST;
            fixed4 _OutlineColor;
            float _OutlineThickness;
            fixed4 _RendererColor; // this will be populated by SpriteRenderer.color!

            v2f vert(appdata_t IN)
            {
                v2f OUT;
                OUT.vertex = UnityObjectToClipPos(IN.vertex);
                OUT.texcoord = TRANSFORM_TEX(IN.texcoord, _MainTex);
                return OUT;
            }

            fixed4 frag(v2f IN) : SV_Target
            {
                float2 pixelSize = _OutlineThickness * float2(ddx(IN.texcoord.x), ddy(IN.texcoord.y));
                
                // sample surrounding texels
                fixed4 col = tex2D(_MainTex, IN.texcoord) * _RendererColor; // Multiply by the correct color
                fixed alpha = col.a;

                alpha += tex2D(_MainTex, IN.texcoord + float2(pixelSize.x, 0)).a;
                alpha += tex2D(_MainTex, IN.texcoord + float2(-pixelSize.x, 0)).a;
                alpha += tex2D(_MainTex, IN.texcoord + float2(0, pixelSize.y)).a;
                alpha += tex2D(_MainTex, IN.texcoord + float2(0, -pixelSize.y)).a;

                // outline if any neighboring alpha pixel exists
                if (col.a == 0 && alpha > 0)
                {
                    return _OutlineColor;
                }
                else
                {
                    return col;
                }
            }
            ENDCG
        }
    }
}
