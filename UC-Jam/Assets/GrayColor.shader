Shader "Custom/GrayColor"
{
    Properties{
        _Color ("Main Color", Color) = (1,1,1,1)  // We use _Color.r to control the transition
        _MainTex ("Texture", 2D) = "white" {}
    }

    Subshader {
        Tags { 
            "Queue" = "Overlay" 
            "RenderType" = "Opaque" 
        }

        Pass {
            ZWrite Off
            Blend SrcAlpha OneMinusSrcAlpha
            Cull Off
            Lighting Off

            CGPROGRAM
            #pragma vertex vert
            #pragma fragment frag

            struct v2f {
                float4 position : SV_POSITION;
                float2 uv_mainTex : TEXCOORD;
                float4 color : COLOR;
            };

            uniform float4 _MainTex_ST;
            uniform sampler2D _MainTex;
            uniform fixed4 _Color;

            v2f vert (float4 position : POSITION, float2 uv : TEXCOORD0, float4 color: COLOR) {
                v2f o;
                o.position = UnityObjectToClipPos(position);
                o.uv_mainTex = uv * _MainTex_ST.xy + _MainTex_ST.zw;
                o.color = color;
                return o;
            }

            fixed4 frag(v2f input) : COLOR {
                fixed4 texColor = tex2D(_MainTex, input.uv_mainTex); 

                // Convert the texture color to grayscale (using standard luminosity method)
                fixed3 grayscale = dot(texColor.rgb, fixed3(0.299, 0.587, 0.114)); 

                // Interpolate between grayscale and the original color based on _Color.r (from gray to original)
                fixed4 finalColor;
                finalColor.rgb = lerp(grayscale, texColor.rgb, _Color.r); // _Color.r is the key to transition
                finalColor.a = texColor.a;

                return finalColor;
            }

            ENDCG
        }
    }

    Fallback "Diffuse"
}
