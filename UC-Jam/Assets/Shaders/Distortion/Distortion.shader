Shader "Custom/2D_Distortion"
{
    Properties
    {
        _MainTex ("Sprite Texture", 2D) = "white" {} // Fire Sprite Texture
        _DispTex ("Distortion Noise", 2D) = "white" {} // Noise Texture for Distortion
        _Speed ("Distortion Speed", Float) = 0.5 // Speed of Distortion
        _Intensity ("Distortion Strength", Float) = 0.025 // Strength of Distortion Effect
    }

    SubShader
    {
        Tags
        {
            "RenderType" = "Transparent"
            "Queue" = "Overlay" // Ensure it renders on top of everything
        }

        Pass
        {
            ZWrite Off
            Blend SrcAlpha OneMinusSrcAlpha

            CGPROGRAM

            #pragma vertex vert
            #pragma fragment frag
            #include "UnityCG.cginc"

            struct appdata_t
            {
                float4 vertex : POSITION;
                float2 uv : TEXCOORD0;
            };

            struct v2f
            {
                float2 uv1 : TEXCOORD0; // UV for Distortion
                float2 uv2 : TEXCOORD1; // UV for Main Sprite
                float4 vertex : SV_POSITION;
            };

            sampler2D _MainTex;
            sampler2D _DispTex;
            float4 _MainTex_ST;
            float4 _DispTex_ST;
            float _Speed;
            float _Intensity;
            
            v2f vert (appdata_t v)
            {
                v2f o;
                o.vertex = UnityObjectToClipPos(v.vertex);
                o.uv1 = TRANSFORM_TEX(v.uv, _DispTex); // Noise Texture UV
                o.uv2 = TRANSFORM_TEX(v.uv, _MainTex); // Main Sprite Texture UV
                return o;
            }
            
            fixed4 frag (v2f i) : SV_Target
            {
                // Use time and sin/cos to create a repeating distortion pattern
                float t = _Time.y * _Speed;
                float2 dispUV = i.uv1 + float2(sin(t), cos(t)); // Oscillate in X and Y directions

                // Sample distortion texture to get displacement value
                fixed disp = tex2D(_DispTex, frac(dispUV)).r; // frac() ensures UVs wrap around correctly
                
                // Apply the distortion to the sprite texture's UVs
                fixed4 col = tex2D(_MainTex, i.uv2 + disp * _Intensity); // Offset sprite UVs based on noise
                return col;
            }

            ENDCG
        }
    }
}
