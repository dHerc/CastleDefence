Shader "Custom/DamageShader"
{
    Properties
    {
        [NoScaleOffset] _MainTex("Texture", 2D) = "white" {}
        _DamageTex("Damage Texture", 2D) = "white" {}
        _Color("Color", Color) = (1,1,1,1)
        _PermamentDamageColor("Permament Damage Color", Color) = (1,1,1,1)
        _PermamentDamageScale("Permament Damage Scale", Range(0,1)) = 0
        _TemporaryDamageColor("Temporary Damage Color", Color) = (1,1,1,1)
        _TemporaryDamageScale("Temporary Damage Scale", Range(0,1)) = 0
    }
        SubShader
    {
        Pass
        {
            // indicate that our pass is the "base" pass in forward
            // rendering pipeline. It gets ambient and main directional
            // light data set up; light direction in _WorldSpaceLightPos0
            // and color in _LightColor0
            Tags {"LightMode" = "ForwardBase"}

            CGPROGRAM
            #pragma vertex vert
            #pragma fragment frag
            #include "UnityCG.cginc" // for UnityObjectToWorldNormal
            #include "UnityLightingCommon.cginc" // for _LightColor0

            struct v2f
            {
                float2 uv : TEXCOORD0;
                fixed4 diff : COLOR0; // diffuse lighting color
                float4 vertex : SV_POSITION;
            };

            v2f vert(appdata_base v)
            {
                v2f o;
                o.vertex = UnityObjectToClipPos(v.vertex);
                o.uv = v.texcoord;
                // get vertex normal in world space
                half3 worldNormal = UnityObjectToWorldNormal(v.normal);
                // dot product between normal and light direction for
                // standard diffuse (Lambert) lighting
                half nl = max(0, dot(worldNormal, _WorldSpaceLightPos0.xyz));
                // factor in the light color
                o.diff = nl * _LightColor0;
                o.diff.rgb += ShadeSH9(half4(worldNormal, 1));
                UNITY_TRANSFER_FOG(o, o.position);
                return o;
            }

            sampler2D _MainTex;
            sampler2D _DamageTex;
            float4 _Color;
            float4 _PermamentDamageColor;
            float _PermamentDamageScale;
            float4 _TemporaryDamageColor;
            float _TemporaryDamageScale;

            fixed4 frag(v2f i) : SV_Target
            {
                // sample texture
            fixed4 col = tex2D(_MainTex, i.uv) * _Color - floor((tex2D(_DamageTex, i.uv) * ((1,1,1,1) - _PermamentDamageColor) * _PermamentDamageScale * 5))/8;
            col -= (_Color - _TemporaryDamageColor) * _TemporaryDamageScale;
            // multiply by lighting
            col *= i.diff;
            return col;
        }
        ENDCG
    }
    UsePass "Legacy Shaders/VertexLit/SHADOWCASTER"
    }
}