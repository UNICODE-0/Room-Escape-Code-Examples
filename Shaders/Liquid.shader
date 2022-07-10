Shader "Unlit/Liquid"
{
    Properties
    {
        _MainTex ("Texture", 2D) = "white" {}
        _Color ("Color", Color) = (1,1,1,1)
        _WaveSpeed ("Wave Speed", Range(0.01,0.7)) = 0.1
        _WaveFrequency ("Wave Frequency", Range(1,100)) = 5
        _TransparencyLevelOfWave ("Transparency Level Of Wave", Range(0,1)) = 0.5
        _TransparencyLevelOfBack ("Transparency Level Of Back", Range(0,1)) = 0.5
        _WaterDisplacment ("Water Displacment", Range(0,0.1)) = 0.01
    }
    SubShader
    {
        Tags 
        { 
            "RenderType" = "Transparent"
            "Queue" = "Transparent"
        }

        Pass
        {
            Cull off
            Zwrite off
            Blend One One



            CGPROGRAM
            #pragma vertex vert
            #pragma fragment frag
            // make fog work
            #pragma multi_compile_fog

            #include "UnityCG.cginc"

            #define TAU 6.28318530718

            float4 _Color;
            float _WaveSpeed;
            float _WaveFrequency;
            float _TransparencyLevelOfWave;
            float _TransparencyLevelOfBack;
            float _WaterDisplacment;

            
            sampler2D _MainTex;
            float4 _MainTex_ST;

            struct appdata
            {
                float4 vertex : POSITION;
                float2 uv0 : TEXCOORD0;
                float3 normals : NORMAL;
            };

            struct v2f
            {
                float2 uv : TEXCOORD1;
                float4 vertex : SV_POSITION;
                float3 normal : TEXCOORD0;
            };

            float InverseLerp(float a, float b, float v)
            {
                return (v-a)/(b-a);
            }

            v2f vert (appdata v)
            {
                v2f o;
                o.vertex = UnityObjectToClipPos(v.vertex);
                o.normal = UnityObjectToWorldNormal(v.normals);
                o.uv = v.uv0;
                o.uv = TRANSFORM_TEX(v.uv0, _MainTex);
                float Waves = cos( (v.uv0.y - _Time.z * -_WaterDisplacment) * TAU * _WaveFrequency ) * _TransparencyLevelOfWave + _TransparencyLevelOfBack;
                o.vertex.y -= Waves * _Color * 30;
                return o;
            }

            fixed4 frag (v2f i) : SV_Target
            {
                fixed4 tex = tex2D(_MainTex, i.uv);
                float Waves = cos( (i.uv.y + tex - _Time.z * -_WaveSpeed) * TAU * _WaveFrequency ) * _TransparencyLevelOfWave + _TransparencyLevelOfBack;
                return Waves  * tex * _Color;
            }
            ENDCG
        }
    }
}
