// Upgrade NOTE: replaced 'mul(UNITY_MATRIX_MVP,*)' with 'UnityObjectToClipPos(*)'

Shader "Custom/My First Lighting Shader"
{
    Properties
    {
        _Tint("Tint", Color) = (1,1,1,1)
        _MainTex ("Albedo", 2D) = "white" {}
        //        _SpecularTint("Specular", Color) = (0.5,0.5,0.5)
        [Gamma]_Metallic("Metallic", Range(0,1)) = 0
        _Smoothness("Smoothness", Range(0,1)) = 0.5
    }

    SubShader
    {
        Pass
        {
            Tags
            {
                "LightMode" = "ForwardBase"
            }


            CGPROGRAM
            #pragma target 3.0
            #pragma vertex MyVertexProgram
            #pragma fragment MyFragmentProgram

            // #include "UnityCG.cginc"
            // #include "UnityStandardBRDF.cginc"
            // #include "UnityStandardUtils.cginc"
            #include "UnityPBSLighting.cginc"

            float4 _Tint;
            sampler2D _MainTex;
            float4 _MainTex_ST; // _ST后缀代表“缩放”和“平移”或类似名称
            // float4 _SpecularTint;
            float _Metallic;
            float _Smoothness;


            struct VertexData
            {
                float4 position:POSITION;
                float3 normal:NORMAL;
                float2 uv:TEXCOORD0;
            };


            struct Interpolators
            {
                float4 position:SV_POSITION; // SV代表系统值，POSITION代表最终顶点位置
                float2 uv:TEXCOORD0;
                float3 normal:TEXCOORD1;
                float worldPos:TEXCOORD2;
            };


            // 定义一个顶点着色器程序，负责将顶点数据转换为片元着色器所需的插值数据
            Interpolators MyVertexProgram(VertexData v)
            {
                // 初始化插值数据结构体，用于存储从顶点着色器传递到片元着色器的数据
                Interpolators i;

                // 将纹理坐标转换到正确的空间，并赋值给插值器
                i.uv = TRANSFORM_TEX(v.uv, _MainTex);

                // 将顶点位置从模型空间转换到裁剪空间，以便进行透视投影和裁剪
                i.position = UnityObjectToClipPos(v.position); // mul(UNITY_MATRIX_MVP,position)

                // 将顶点位置从模型空间转换到世界空间，以便在世界坐标系中进行光照计算等操作
                i.worldPos = mul(unity_ObjectToWorld, v.position);

                // 将法线方向从模型空间转换到世界空间，使用Unity提供的函数以确保数值稳定性和性能
                // 注释掉的代码展示了法线变换的多种不推荐或不正确的方式
                // i.normal = mul(unity_ObjectToWorld, float4(v.normal, 0));
                // i.normal = mul((float3x3)unity_ObjectToWorld, v.normal);
                // i.normal = mul(transpose((float3x3)unity_ObjectToWorld), v.normal);
                // i.normal = normalize(i.normal);
                i.normal = UnityObjectToWorldNormal(v.normal);

                // 返回填充了计算结果的插值数据结构体
                return i;
            }


            float4 MyFragmentProgram(Interpolators i):SV_TARGET
            {
                i.normal = normalize(i.normal);

                float3 lightDir = _WorldSpaceLightPos0.xyz; // 当前灯光
                float3 viewDir = normalize(_WorldSpaceCameraPos - i.worldPos);
                float3 lightColor = _LightColor0.rgb;

                float3 albedo = tex2D(_MainTex, i.uv).rgb * _Tint.rgb; // 计算漫反射颜色，结合纹理贴图和颜色tint
                // albedo*= 1-_SpecularTint.rgb;
                // albedo*= 1-max(_SpecularTint.r, max(_SpecularTint.g, _SpecularTint.b));
                float3 spcularTint = albedo * _Metallic;
                float oneMinusReflectivity = 1 - spcularTint;
                // albedo = EnergyConservationBetweenDiffuseAndSpecular(albedo, _SpecularTint.rgb, oneMinusReflectivity);
                // albedo *= oneMinusReflectivity;
                albedo = DiffuseAndSpecularFromMetallic(albedo, _Metallic, spcularTint, oneMinusReflectivity);

                float3 reflectionDir = reflect(-lightDir, i.normal);
                float3 halfVector = normalize(lightDir + viewDir);
                // float3 specular = _SpecularTint.rgb * lightColor * pow(DotClamped(halfVector, i.normal),
                //     _Smoothness * 100);
                float3 specular = spcularTint * lightColor * pow(DotClamped(halfVector, i.normal),
                                                                 _Smoothness * 100);
                float3 diffuse = albedo * lightColor * DotClamped(lightDir, i.normal); // 计算漫反射光照，结合光线颜色和法线与光线方向的点积

                UnityLight light;
                light.color = lightColor;
                light.dir = lightDir;
                light.ndotl = DotClamped(i.normal, lightDir);

                UnityIndirect indirectLight;
				indirectLight.diffuse = 0;
				indirectLight.specular = 0;

                // return float4(i.normal * 0.5 + 0.5, 1);
                // return dot(float3(0,1,0), i.normal);
                // return max(0, dot(float3(0,1,0), i.normal)) ;
                // return saturate(dot(float3(0, 1, 0), i.normal));
                // return DotClamped(lightDir, i.normal);
                // return float4(reflectionDir*0.5+0.5, 1);
                // return DotClamped(viewDir, reflectionDir);
                // return pow( DotClamped(viewDir, reflectionDir), _Smoothness*100);
                // return pow(DotClamped(halfVector, i.normal), _Smoothness * 100);
                // return float4(diffuse, 1);
                // return float4(specular, 1);
                // return float4(diffuse + specular, 1);
                return BRDF1_Unity_PBS(
                    albedo, spcularTint,
                    oneMinusReflectivity, _Smoothness,
                    i.normal, viewDir,
                    light,indirectLight);
            }
            ENDCG
        }
    }
}