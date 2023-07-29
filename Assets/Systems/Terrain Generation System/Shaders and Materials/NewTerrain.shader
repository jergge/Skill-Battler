Shader "Custom/NewTerrain"
{
    Properties
    {
        _MainTex("Texture", 2D) = "white"{}
        _TotalScale("Total Scale", float) = 1
        _UseTest("Use Test", float) = 0
    }
    SubShader
    {
        Tags { "RenderType"="Opaque" }
        LOD 200

        CGPROGRAM
        // Physically based Standard lighting model, and enable shadows on all light types
        #pragma surface surf Standard

        // Use shader model 3.0 target, to get nicer looking lighting
        #pragma target 3.0

        const static int maxLayerCount = 8;
        const static float epsilon = 1E-4;

        int layerCount;
        UNITY_DECLARE_TEX2DARRAY(baseAlbedos);
        UNITY_DECLARE_TEX2DARRAY(baseNormals);
        UNITY_DECLARE_TEX2DARRAY(baseDisplacements);
        float3 baseColours[maxLayerCount];
        float baseStartingHeights[maxLayerCount];
        float baseBlends[maxLayerCount];
        float baseColourStrength[maxLayerCount];
        float baseTextureScales[maxLayerCount];

        float minHeight;
        float maxHeight;


        sampler2D _MainTex;
        float _TotalScale;
        float _UseTest;

        struct Input
        {
            float2 uv_MainTex;
            float3 worldPos;
            float3 worldNormal;
        };


        // // Add instancing support for this shader. You need to check 'Enable Instancing' on materials that use the shader.
        // // See https://docs.unity3d.com/Manual/GPUInstancing.html for more information about instancing.
        // // #pragma instancing_options assumeuniformscaling
        UNITY_INSTANCING_BUFFER_START(Props)
        //     // put more per-instance properties here
        UNITY_INSTANCING_BUFFER_END(Props)

        float inverseLerp(float a, float b, float t)
        {
            return saturate((t-a)/(b-a));
        }

        float3 triplanar(float3 worldPos, float scale, float3 blendAxes, int textureIndex)
        // float3 triplanar(float3 worldPos, float scale, float3 blendAxes)
        {
            float3 scaledWorldPosition = worldPos / (scale * _TotalScale);
            float3 xProjection = UNITY_SAMPLE_TEX2DARRAY(baseAlbedos, float3(scaledWorldPosition.x, scaledWorldPosition.z, textureIndex)) * blendAxes.y;
            float3 yProjection = UNITY_SAMPLE_TEX2DARRAY(baseAlbedos, float3(scaledWorldPosition.x, scaledWorldPosition.y, textureIndex)) * blendAxes.z;
            float3 zProjection = UNITY_SAMPLE_TEX2DARRAY(baseAlbedos, float3(scaledWorldPosition.y, scaledWorldPosition.z, textureIndex)) * blendAxes.x;
            // return tex2D(_MainTex, scaledWorldPosition.xz);
            //return UNITY_SAMPLE_TEX2DARRAY(baseAlbedos, float3(scaledWorldPosition.xz, textureIndex));
            // // return tex2D(testTexture, float2(worldPos.xz));
            return xProjection + yProjection + zProjection; 
        }

        void surf (Input IN, inout SurfaceOutputStandard o)
        {
            float3 scaledWorldPosition = IN.worldPos / (1 * _TotalScale);

            float heightPercent = inverseLerp(minHeight, maxHeight, IN.worldPos.y);
            if (_UseTest == 1)
            {
                // o.Albedo = tex2D(_MainTex, scaledWorldPosition.xz);
                // o.Albedo = float3(heightPercent, heightPercent, heightPercent);
                o.Albedo = UNITY_SAMPLE_TEX2DARRAY(baseAlbedos, float3(scaledWorldPosition.xz, 1));
                return;
            }

            float3 blendAxes = abs(IN.worldNormal);
            blendAxes /= blendAxes.x + blendAxes.y + blendAxes.z;

            for(int i = 0; i < layerCount; i++)
            {
               float drawStrength = inverseLerp(-baseBlends[i]/2 - epsilon, baseBlends[i]/2, heightPercent - baseStartingHeights[i]);
               float3 baseColour = baseColours[i] * baseColourStrength[i];
               float3 textureColour = triplanar(IN.worldPos, baseTextureScales[i], blendAxes, i) * (1- baseColourStrength[i]);
               o.Albedo = o.Albedo * (1-drawStrength) + (baseColour+textureColour) * drawStrength;
            }

           // o.Normal = IN.worldNormal;
            //float3 tangent = UnityObjectToWorldDir(v.tangent.xyz);
            //float3 bitangent = cross( o.worldNormal, o.tangent) * ( v.tangent.w * unity_WorldTransformParams.w);


        }
        ENDCG
    }
    FallBack "Diffuse"
}