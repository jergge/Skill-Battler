Shader "Custom/waterNoise"
{
    Properties
    {
        //_MainTex ("Texture", 2D) = "white" {}
        SimplexScale ("SimplexScale", float) = 0
        SimplexSampleSpeed ("SimplexSampleSpeed", float) = 0
        ringStrength ("RingStrength", float) = 1 
        ringShift ("RingShift", float) = .5
        [MaterialToggle] useAlpha("useAlpha", float) = 0
    }
    SubShader
    {
        Tags {"Queue" = "Transparent" "RenderType"="Transparent" }
        LOD 200

        CGPROGRAM 
        // Physically based Standard lighting model, and enable shadows on all light types
        #pragma surface surf Standard alpha fullforwardshadows

        #include "3DSimplex.cginc"  
        // Use shader model 3.0 target, to get nicer looking lighting
        #pragma target 3.0

        struct Input
        {
            float2 uv_MainTex;
            float3 worldPos;
            float3 worldNormal;
        };

            sampler2D testTexture;
            float testScale;
            float SimplexScale;
            float SimplexSampleSpeed;
            float ringStrength;
            float ringShift;
            float useAlpha;

        // Add instancing support for this shader. You need to check 'Enable Instancing' on materials that use the shader.
        // See https://docs.unity3d.com/Manual/GPUInstancing.html for more information about instancing.
        // #pragma instancing_options assumeuniformscaling
        UNITY_INSTANCING_BUFFER_START(Props)
            // put more per-instance properties here
        UNITY_INSTANCING_BUFFER_END(Props)

        void surf (Input i, inout SurfaceOutputStandard o)
        {

            float scale = SimplexScale;
            float speed = SimplexSampleSpeed;

            //float pinchIn = (i.uv.x / 2 + 1) * i.uv.y * pinchInScale;

            float sampleX = (i.worldPos.x  ) / (scale * 1);
            float sampleZ = (i.worldPos.z  ) / (scale * 1);

            float sampleY = _Time.x * speed -i.worldPos.y*(1/scale)*4;

            float3 noiseInput = float3(sampleX, sampleY, sampleZ);

            float sampleNoise = snoise(noiseInput);


            float normalisedSampleNoise = (sampleNoise + 1 )/ 2;

            float squishedNoise = exp(  -(normalisedSampleNoise - ringShift)*(normalisedSampleNoise - ringShift) * ringStrength  );

            float r = squishedNoise;
            float g = squishedNoise;
            float b = 1;
            
            fixed3 output = fixed3(r, g, b);

            fixed alpha = 1;
            if (useAlpha == 1)
            {
                alpha = squishedNoise;
            }
            

            o.Albedo = output;
            o.Alpha = alpha;

        }
        ENDCG
    }
    FallBack "Diffuse"
}
