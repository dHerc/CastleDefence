﻿Shader "Custom/Post Outline"
{
    Properties
    {
        _MainTex("Main Texture",2D)="black"{}
        _SceneTex("Scene Texture",2D)="black"{}
        _Color("Color",Color)=(0.5,0,0,1)
    }
    SubShader 
    {
        Pass 
        {
            CGPROGRAM

            sampler2D _MainTex;    
            sampler2D _SceneTex;
            float2 _MainTex_TexelSize;
            half4 _Color;

            #pragma vertex vert
            #pragma fragment frag

            #include "UnityCG.cginc"
             
            struct v2f 
            {
                float4 pos : SV_POSITION;
                float2 uvs : TEXCOORD0;
            };
             
            v2f vert (appdata_base v) 
            {
                v2f o;
				o.pos = UnityObjectToClipPos(v.vertex);
                o.uvs = o.pos.xy / 2 + 0.5;
                return o;
            }
                          
            half4 frag(v2f i) : COLOR 
            {                
                if(tex2D(_MainTex,i.uvs.xy).r>0)
                {
                    return tex2D(_SceneTex,i.uvs.xy);
                }
                int NumberOfIterations=19;
 
                float TX_x = _MainTex_TexelSize.x;
                float TX_y = _MainTex_TexelSize.y;
 
                float ColorIntensityInRadius=0;
 
                for(int k=0; k < NumberOfIterations; k++)
                {
                    for(int j=0; j < NumberOfIterations; j++)
                    {
                        ColorIntensityInRadius += tex2D(_MainTex,i.uvs.xy + float2 ((k-NumberOfIterations/2)*TX_x, (j-NumberOfIterations/2)*TX_y)).r;
                    }
                }
                ColorIntensityInRadius *= 0.01;
 
                half4 color = tex2D(_SceneTex,i.uvs.xy) + ColorIntensityInRadius * _Color;

                return color;
            }
             
            ENDCG
 
        }      
    }
}