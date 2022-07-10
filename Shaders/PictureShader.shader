Shader "Custom/PictureShader"
{
    Properties
    {
        _Color ("Color", Color) = (1,1,1,1)
        _BackPicture ("Back Picture", 2D) = "white" {}
        _BackPictureVisibility ("Back Picture Visibility", Range(0.0,1.0)) = 1
        _FrontPicture ("Front Picture", 2D) = "white" {}
        _FrontPictureVisibility ("Front Picture Visibility", Range(0.0,1.0)) = 1
        _Glossiness ("Smoothness", Range(0,1)) = 0.5
        _Metallic ("Metallic", Range(0,1)) = 0.0
    }
    SubShader
    {
        Tags { "RenderType"="Opaque" }
        LOD 200

        CGPROGRAM
        // Physically based Standard lighting model, and enable shadows on all light types
        #pragma surface surf Standard fullforwardshadows

        // Use shader model 3.0 target, to get nicer looking lighting
        #pragma target 3.0

        sampler2D _BackPicture;
        sampler2D _FrontPicture;

        struct Input
        {
            float2 uv_BackPicture;
            float2 uv_FrontPicture;
        };

        half _Glossiness;
        half _Metallic;
        fixed4 _Color;
        float _BackPictureVisibility;
        float _FrontPictureVisibility;

        // Add instancing support for this shader. You need to check 'Enable Instancing' on materials that use the shader.
        // See https://docs.unity3d.com/Manual/GPUInstancing.html for more information about instancing.
        // #pragma instancing_options assumeuniformscaling
        // UNITY_INSTANCING_BUFFER_START(Props)
            // put more per-instance properties here
        // UNITY_INSTANCING_BUFFER_END(Props)

        void surf (Input IN, inout SurfaceOutputStandard o)
        {
            fixed4 BackPicture = tex2D (_BackPicture, IN.uv_BackPicture) * _Color;
            fixed4 FrontPicture = tex2D (_FrontPicture, IN.uv_FrontPicture) * _Color;
            o.Albedo = BackPicture.rgb * _BackPictureVisibility + FrontPicture.rgb * _FrontPictureVisibility;
            o.Metallic = _Metallic;
            o.Smoothness = _Glossiness;
            //o.Alpha = BackPicture.a;
        }
        ENDCG
    }
    FallBack "Subtrack"
}
