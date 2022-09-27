Shader "Unlit/2DFire"
{
    Properties
    {
        _MainTex ("Texture", 2D) = "white" {}
        SimplexScale ("SimplexScale", float) = 0
        SimplexSampleSpeed ("SimplexSampleSpeed", float) = 0
        PinchInScale("Pinch In Scale", float) = 0
        BaseHeight("Base Height", float) = 0
    }
    SubShader
    {
        Tags { "Queue"="Transparent" "RenderType"="Transparent" }
        Cull Off
        LOD 100

        
 
        Pass
        {
            CGPROGRAM


            #pragma vertex vert
            #pragma fragment frag
            // make fog work
            #pragma multi_compile_fog

            #include "UnityCG.cginc"
            #include "3DSimplex.cginc"  
            
            struct appdata
            {
                float4 vertex : POSITION;
                float2 uv : TEXCOORD0;
                //float SimplexScale : TEXCOORD1;
            };

            struct v2f
            {
                float2 uv : TEXCOORD0;
                //UNITY_FOG_COORDS(1)
                float4 vertex : SV_POSITION;
                float3 worldPos : TEXCOORD1;
            };

            sampler2D _MainTex;
            float4 _MainTex_ST;
            float SimplexScale;
            float SimplexSampleSpeed;
            float PinchInScale;
            float BaseHeight;

            v2f vert (appdata v)
            {
                v2f o;
                o.vertex = UnityObjectToClipPos(v.vertex);
                o.worldPos = mul (unity_ObjectToWorld, v.vertex);
                o.uv = TRANSFORM_TEX(v.uv, _MainTex);
                //UNITY_TRANSFER_FOG(o,o.vertex);
                return o;
            }

            fixed4 frag (v2f i) : SV_Target
            {
                float scale = SimplexScale;
                float speed = SimplexSampleSpeed;
                float pinchInScale = PinchInScale;
                float baseHeight = BaseHeight;

                float pinchIn = (i.uv.x / 2 + 1) * i.uv.y * pinchInScale;

                float sampleX = (i.worldPos.x + pinchIn ) / (scale * 1);
                float sampleY = (i.worldPos.z + pinchIn ) / (scale * 1);
                float sampleZ = _Time.x * speed -i.uv.y*(1/scale)*4;
                //float sampleZ = (i.worldPos.y + pinchIn ) / (scale * 1);

                float3 samplePoints = float3 (sampleX, sampleY, sampleZ); 
                
                float noiseOut = (snoise(samplePoints)+1)/2;

                float heightMask = 1-i.uv.y * 1-i.uv.y + 1;
                float otherMask =  1-i.uv.y/2-baseHeight;

                float output = heightMask * (noiseOut + otherMask);

                if (output < .5)
                {
                    //discard;
                }
                //return float4( output, output, output, .4);
                float4 testMask = float4(0,0,0,0);



                testMask = float4(noiseOut, noiseOut, noiseOut, 1);

                float testRed =     smoothstep(.3*(i.uv.y-baseHeight), .9, noiseOut);
                float testGreen =   smoothstep(.3*(i.uv.y-baseHeight), .9, noiseOut);
                float testBlue =    smoothstep(.3*(i.uv.y-baseHeight), .9, noiseOut);

                testMask = float4 (testRed, testGreen, testBlue, 1);

                return testMask;
            }


            ENDCG
        }
    }
}
