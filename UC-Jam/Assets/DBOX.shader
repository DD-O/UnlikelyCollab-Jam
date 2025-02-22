Shader "Custom/ClipBoxPortal" {
    Properties {
        _CreepyTexture ("Creepy Dimension View", 2D) = "white" {}
        _BoxSize ("Box Size", Vector) = (1, 1, 1, 0)
    }

    SubShader {
        Tags { "RenderType"="Opaque" }
        LOD 200

        CGPROGRAM
        #pragma surface surf Standard fullforwardshadows addshadow
        #pragma target 3.0

        sampler2D _CreepyTexture;
        float4 _BoxSize;

        struct Input {
            float2 uv_MainTex;
            float3 worldPos;
        };

        void surf(Input IN, inout SurfaceOutputStandard o) {
            // Normalize the world position relative to the box size
            float2 uvBox = (IN.worldPos.xz - _BoxSize.xy) / _BoxSize.zw;

            // Ensure the UVs are within the bounds of the box
            uvBox = saturate(uvBox);

            // Sample the creepy dimension texture using the normalized UVs
            fixed4 c = tex2D(_CreepyTexture, uvBox);

            // Set the final surface properties
            o.Albedo = c.rgb;
            o.Alpha = c.a;
        }
        ENDCG
    }

    FallBack "Diffuse"
}
