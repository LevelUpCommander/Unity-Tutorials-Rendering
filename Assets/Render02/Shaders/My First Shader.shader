// Upgrade NOTE: replaced 'mul(UNITY_MATRIX_MVP,*)' with 'UnityObjectToClipPos(*)'

Shader "Custom/My First Shader"
{
    Properties
    {
        _Tint("Tint", Color) = (1,1,1,1)
        _MainTex("Texture", 2D) = "white"{}
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
            sampler2D _MainTex;
            float4 _MainTex_ST; // _ST后缀代表“缩放”和“平移”或类似名称

            struct Interpolators
            {
                float4 position:SV_POSITION; // SV代表系统值，POSITION代表最终顶点位置
//                float3 localPosition:TEXCOORD0; // 纹理坐标语义
                float2 uv:TEXCOORD0;
            };
            
            struct VertexData{
                float4 position:POSITION;
                float2 uv:TEXCOORD0;
            };

            Interpolators MyVertexProgram(VertexData v)
            {
                Interpolators i;
//                i.localPosition = v.position.xyz;
                i.position =  UnityObjectToClipPos(v.position); // mul(UNITY_MATRIX_MVP,position)
                i.uv = v.uv*_MainTex_ST.xy+_MainTex_ST.zw;
                return i;
            }
            
            float4 MyFragmentProgram(Interpolators i):SV_TARGET
            {
//                return float4(i.uv, 1, 1)*_Tint;
                return tex2D(_MainTex, i.uv)*_Tint;
            }
            ENDCG
        }
    }
}