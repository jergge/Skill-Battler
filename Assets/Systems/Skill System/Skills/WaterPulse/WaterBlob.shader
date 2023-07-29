Shader "Unlit/WaterBlob"
{
    Properties
    {
        _MainTex ("Texture", 2D) = "white" {}
        [NoScaleOffset]_MainNorm ("Main Normal Map", 2D) = "white" {}
        [NoScaleOffset]_SecondNorm ("Second Normal Map", 2D) = "white" {}
        _RippleStrength ("Ripple Strength", Range(1,2)) = 1
        _TimeSpeed ("Time Speed", Range(0,.5)) = .3
        _NormalTest ("Normal Test", Range(0,1)) = 1
        _Gloss ("Gloss", Range(0,1)) = 1
        [MaterialToggle] _VertexEdit("Vertex Edit", Float) = 0
        _Color ("Colour", Color) = (1,1,1,1)
        _NormalIntensity("Normal Intensity", Range(0,1)) = 1
        _Transpareny("Transparency", Range(0,1)) = 1
        [NoScaleOffset]_StrengthFilter("Strength Filter", 2D) = "white" {}
        _Strength("Distort Strength", Range(0,5)) = 1.0
       [NoScaleOffset]_Noise("Noise Texture", 2D) = "white" {}
    }
    SubShader
    {
        // Tags { "RenderType"="Opaque" }
        // zWrite on

        Tags { 
            "Queue" = "Transparent" 
            //"IgnoreProjector" = "True" 
            //"RenderType" = "Transarent"
        }

        ZWrite off
        //Blend SrcAlpha OneMinusSrcAlpha
        //Cull front
        //LOD 100
        // ZTest Always

        GrabPass {
            "_BackgroundTexture"
        }

        //First pass - directional light only
        Pass
        {
            CGPROGRAM
            // Use shader model 3.0 target, to get nicer looking lighting
            #pragma target 3.0
            #pragma vertex vert
            #pragma fragment frag

            #include "UnityCG.cginc"
            #include "SimplexNoise.cginc"
            #include "Lighting.cginc"

            struct meshData
            {
                float2 uv : TEXCOORD0;
                float4 vertex : POSITION;
                float3 normal : NORMAL;
                float4 tangent: TANGENT;
            };

            struct Interps
            {
                float2 uv : TEXCOORD0;
                float2 uv2 : TEXCOORD7;
                float4 vertex : SV_POSITION;
                float3 worldNormal : TEXCOORD1;
                float3 localNormal : TEXCOORD2;
                float3 worldPosition : TEXCOORD3; 
                float3 tangent : TEXCOORD4;
                float3 bitangent : TEXCOORD5;
                float4 grabPos: TEXCOORD6;
                float4 grabPosDistorted: TEXCOORD8;
            };

            sampler2D _MainTex;
            sampler2D _MainNorm;
            sampler2D _SecondNorm;
            sampler2D _BackgroundTexture;
            sampler2D _StrengthFilter;
            sampler2D _Noise;
            float _Strength;
            float4 _MainTex_ST;
            float4 _StrengthFilter_ST;
            float _RippleStrength;
            float _TimeSpeed;
            float _NormalTest;
            float _Gloss;
            float _VertexEdit;
            float4 _Color;
            float _NormalIntensity;
            float _Transpareny;

            Interps vert (meshData v)
            {
                Interps o;
                //float heightChange = lerp(.9, 1.1, cos(_Time.y*_RippleStrength)*.5 + .5);
                //v.vertex *= heightChange;

                if (_VertexEdit == 1)
                {
                    float timeLerp = (cos(_Time.y*_TimeSpeed)+1)/2;
                    float noise = (snoise( (v.vertex + _Time.y*_TimeSpeed)*_RippleStrength ));
                    v.vertex *= lerp(1, 1.3, noise);
                }
                o.vertex = UnityObjectToClipPos(v.vertex);
                o.grabPos = ComputeGrabScreenPos(o.vertex); 
                
                o.grabPosDistorted = o.grabPos;
                // float noise = tex2D(_Noise, v.uv).x  ;
                // float3 filt = tex2D(_StrengthFilter,v.uv).rgb;
                // o.grabPosDistorted.x += cos(_Time.x) ;
                // o.grabPosDistorted.y += sin(_Time.x) ;

                float noise = tex2Dlod(_Noise, float4(v.uv, 0, 0)).rgb;
                // float filt = tex2Dlod(_StrengthFilter, float4(v.uv, 0, 0)).rgb;
                // float filt = tex2D(_StrengthFilter, v.uv).rgb;
                float filt = .6;
                // o.grabPosDistorted.x += cos(noise * _Time.x * _TimeSpeed) * filt.x * _Strength;
                // o.grabPosDistorted.y += sin(noise * _Time.x * _TimeSpeed) * filt.x * _Strength;
                o.grabPosDistorted.x += (10 *_Strength) * filt.x;
                //o.grabPosDistorted.y += (10 * _Strength);

                //o.uv = TRANSFORM_TEX(v.uv, _MainTex);
                o.uv = TRANSFORM_TEX(float2(v.uv.x + _Time.y*_TimeSpeed, v.uv.y + _Time.y*_TimeSpeed), _MainTex);

                //o.uv2 = TRANSFORM_TEX(v.uv, _MainTex);
                o.uv2 = TRANSFORM_TEX(float2(v.uv.x - _Time.y*_TimeSpeed/2, v.uv.y - _Time.y*_TimeSpeed/2), _MainTex);

                o.worldNormal = UnityObjectToWorldNormal(v.normal);
                o.localNormal = v.normal;
                o.tangent = UnityObjectToWorldDir(v.tangent.xyz);
                o.bitangent = cross( o.worldNormal, o.tangent) * ( v.tangent.w * unity_WorldTransformParams.w);

                o.worldPosition =  mul( unity_ObjectToWorld, v.vertex);

                return o;
            }

            float4 frag (Interps i) : SV_Target
            {

                // float4 bg = tex2Dproj(_BackgroundTexture, i.grabPosDistorted);
                float4 bg = tex2Dproj(_BackgroundTexture, i.grabPosDistorted);
                //bg = tex2Dproj(_BackgroundTexture, i.grabPos);
                //return 1-bg;


                //
                // Normal Mapping
                //
                fixed4 col = tex2D(_MainTex, i.uv);
                //fixed4 normalMap = tex2D(_MainNorm, i.uv);
                //float3 extractedNormalsFromMap = normalize(float3(normalMap.w, normalMap.y, 1));

                float4 combinedNormals = normalize(tex2D(_MainNorm, i.uv) + tex2D(_SecondNorm, i.uv2));

                // float3 tangetSpaceNormal = UnpackNormal( tex2D(_MainNorm, i.uv) );
                float3 tangetSpaceNormal = UnpackNormal( combinedNormals );
                tangetSpaceNormal = normalize(lerp( float3(0,0,1), tangetSpaceNormal, _NormalIntensity));

                float3x3 mtxTangetToWorld = {
                    i.tangent.x, i.bitangent.x, i.worldNormal.x,
                    i.tangent.y, i.bitangent.y, i.worldNormal.y,
                    i.tangent.z, i.bitangent.z, i.worldNormal.z
                };
                
                float3 N = mul( mtxTangetToWorld, tangetSpaceNormal);

                
                //
                //Diffuse Lighting
                //
                //float3 N = normalize(i.worldNormal);
                //return float4(N, 1);
                float3 L = _WorldSpaceLightPos0.xyz;
                // Lambertion Shading (mask)
                float3 diffuseLight = saturate ( dot (N, L) );
                float3 coloredDiffuseLight = diffuseLight * _LightColor0;
                 //coloredDiffuseLight = float3(1,1,1);

                //return float4(coloredDiffuseLight, 1);

                float specualrExponent = exp2( _Gloss * 11) + 2 ;


                //
                //Phong specualr lighting
                //
                //Direction from surface to the camera
                float3 viewVector = normalize(_WorldSpaceCameraPos - i.worldPosition);
                float3 reflectedLightDirection = reflect( -L, N );
                float3 specualrLight = saturate(dot(viewVector, reflectedLightDirection));
                specualrLight = pow(specualrLight, specualrExponent);
                specualrLight *= _LightColor0.xyz;
                //return float4(specualrLight.xxx, 1);

                //Blinn-Phonng

                //average of light vector and view vector
                float3 halfVector = normalize (L + viewVector);
                float3 blinnPhong = saturate(dot(halfVector, N));
                //cull backface highlights
                //blinnPhong = blinnPhong * (diffuseLight > 0);
                blinnPhong = pow(blinnPhong, specualrExponent);
                blinnPhong *= _LightColor0.xyz;

                
                // return float4(blinnPhong + coloredDiffuseLight * col * _Color, _Transpareny);
                return float4(blinnPhong  + bg * _Color, 1);
            }
            ENDCG
        }
    }
}
