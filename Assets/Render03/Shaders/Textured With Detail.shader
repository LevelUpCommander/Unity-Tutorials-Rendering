// Upgrade NOTE: replaced 'mul(UNITY_MATRIX_MVP,*)' with 'UnityObjectToClipPos(*)'

Shader "Custom/Textured With Detail"
{
    Properties
    {
        _Tint("Tint", Color) = (1,1,1,1)
        _MainTex("Texture", 2D) = "white"{}
        _DetailTex("Detail Texture", 2D) = "gray"{}
    }

    SubShader
    {
        Pass
        {
            CGPROGRAM
            #pragma vertex MyVertexProgram
            #pragma fragment MyFragmentProgram

            #include "UnityCG.cginc"

            float4 _Tint;
            sampler2D _MainTex, _DetailTex;
            float4 _MainTex_ST, _DetailTex_ST; // _ST后缀代表“缩放”和“平移”或类似名称

            struct Interpolators
            {
                float4 position:SV_POSITION; // SV代表系统值，POSITION代表最终顶点位置
                //                float3 localPosition:TEXCOORD0; // 纹理坐标语义
                float2 uv:TEXCOORD0;
                float2 uvDetail:TEXCOORD1;
            };

            struct VertexData
            {
                float4 position:POSITION;
                float2 uv:TEXCOORD0;
            };

            Interpolators MyVertexProgram(VertexData v)
            {
                Interpolators i;
                //                i.localPosition = v.position.xyz;
                i.position = UnityObjectToClipPos(v.position); // mul(UNITY_MATRIX_MVP,position)
                // i.uv = v.uv * _MainTex_ST.xy + _MainTex_ST.zw;
                i.uv = TRANSFORM_TEX(v.uv, _MainTex); // (tex.xy * name##_ST.xy + name##_ST.zw)
                i.uvDetail = TRANSFORM_TEX(v.uv, _DetailTex);
                return i;
            }

            float4 MyFragmentProgram(Interpolators i):SV_TARGET
            {
                float4 color = tex2D(_MainTex, i.uv) * _Tint;
                // color *= tex2D(_DetailTex, i.uvDetail) * 2;
                 color *= tex2D(_DetailTex, i.uvDetail) * unity_ColorSpaceDouble;
                return color;
            }
            ENDCG
        }
    }
}