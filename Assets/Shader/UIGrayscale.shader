Shader "UI/Grayscale"
{
    Properties
    {
        _MainTex ("Texture", 2D) = "white" {}
        _Color ("Tint", Color) = (1,1,1,1)
        _Saturation ("Saturation", Range(0,1)) = 1 // 1 = warna penuh, 0 = abu-abu total
        _Brightness ("Brightness (saat tidak aktif)", Range(0.3,1)) = 1
    }
    SubShader
    {
        Tags { "Queue"="Transparent" "IgnoreProjector"="True" "RenderType"="Transparent" "PreviewType"="Plane" "CanUseSpriteAtlas"="True" }
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
                float4 vertex : POSITION;
                float2 texcoord : TEXCOORD0;
                float4 color : COLOR;
            };

            struct v2f
            {
                float4 vertex : SV_POSITION;
                float2 texcoord : TEXCOORD0;
                float4 color : COLOR;
            };

            sampler2D _MainTex;
            fixed4 _Color;
            float _Saturation;
            float _Brightness;

            v2f vert (appdata_t v)
            {
                v2f o;
                o.vertex = UnityObjectToClipPos(v.vertex);
                o.texcoord = v.texcoord;
                o.color = v.color * _Color;
                return o;
            }

            fixed4 frag (v2f i) : SV_Target
            {
                fixed4 col = tex2D(_MainTex, i.texcoord) * i.color;

                // Hitung nilai abu-abu (luminance) dari warna asli
                float gray = dot(col.rgb, float3(0.299, 0.587, 0.114));

                // Interpolasi antara abu-abu penuh dan warna asli berdasarkan _Saturation
                col.rgb = lerp(fixed3(gray, gray, gray), col.rgb, _Saturation);

                // Redupkan sedikit saat tidak jadi speaker aktif
                col.rgb *= _Brightness;

                return col;
            }
            ENDCG
        }
    }
}
