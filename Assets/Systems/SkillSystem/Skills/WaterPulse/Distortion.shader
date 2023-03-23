Shader "Custom/Distort"
{
    SubShader
    {
        Tags  { "Queue" = "Transparent" }

        // Grab the screen behind the object into _BackgroundTexture
        GrabPass
        {
            "_BackgroundTexture"
        }

        // Render the object with the texture generated above, and invert the colors
        Pass
        {
            CGPROGRAM
            #pragma vertex vert
            #pragma fragment frag
            #include "UnityCG.cginc"

            struct v2f
            {
                float4 grabPos : TEXCOORD0;
                float4 pos : SV_POSITION;
            };

            v2f vert(appdata_base i){
                v2f o;
                o.pos = UnityObjectToClipPos(i.vertex);               
                o.grabPos = ComputeGrabScreenPos(o.pos);
                return o;
            }
            
            sampler2D _BackgroundTexture;

            half4 frag(v2f i) : SV_Target
            {
                half4 bg = tex2Dproj(_BackgroundTexture, i.grabPos);
                return 1-bg;
            }
            ENDCG
        }

    }
}