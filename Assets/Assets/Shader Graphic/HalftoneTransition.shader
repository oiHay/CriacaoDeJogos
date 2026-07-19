Shader "Custom/Transition"
{
    Properties
    {
        [PerRendererData] _MainTex ("Sprite Texture", 2D) = "white" {}
        _Color ("Cor", Color) = (0, 0, 0, 1)
        _Progress ("Progress", Range(0, 1)) = 0
        _Scale ("Densidade da Retícula", Float) = 30
        _Sweep ("Varredura Diagonal", Range(0, 1)) = 0.3
        _Invert ("Inverter (revelar)", Range(0, 1)) = 0
    }
    SubShader
    {
        Tags { "Queue"="Transparent" "RenderType"="Transparent" "IgnoreProjector"="True" }
        Blend SrcAlpha OneMinusSrcAlpha
        ZWrite Off
        Cull Off
        ZTest Always

        Pass
        {
            CGPROGRAM
            #pragma vertex vert
            #pragma fragment frag
            #include "UnityCG.cginc"

            fixed4 _Color;
            float _Progress;
            float _Scale;
            float _Sweep;
            float _Invert;

            struct appdata
            {
                float4 vertex : POSITION;
                float2 uv : TEXCOORD0;
            };

            struct v2f
            {
                float4 pos : SV_POSITION;
                float2 uv : TEXCOORD0;
            };

            v2f vert (appdata v)
            {
                v2f o;
                o.pos = UnityObjectToClipPos(v.vertex);
                o.uv = v.uv;
                return o;
            }

            fixed4 frag (v2f i) : SV_Target
            {
                // corrige o aspecto para as bolinhas ficarem redondas
                float2 aspect = float2(_ScreenParams.x / _ScreenParams.y, 1);
                float2 grid = i.uv * aspect * _Scale;

                // distância até o centro da célula da retícula
                float dist = length(frac(grid) - 0.5);

                // varredura diagonal: cantos começam antes, criando a "onda"
                float sweep = (i.uv.x + i.uv.y) * 0.5;
                float progress = saturate(_Progress * (1 + _Sweep) - sweep * _Sweep);

                // 0.7071 = meia diagonal da célula: bolinha cobre a célula inteira
                float radius = progress * 0.7071 * 1.05;

                // smoothstep dá anti-aliasing na borda das bolinhas
                float alpha = smoothstep(dist, dist + 0.03, radius);
                alpha = lerp(alpha, 1 - alpha, _Invert); 

                return fixed4(_Color.rgb, alpha * _Color.a);
            }
            ENDCG
        }
    }
}
