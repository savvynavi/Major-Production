// Upgrade NOTE: replaced 'mul(UNITY_MATRIX_MVP,*)' with 'UnityObjectToClipPos(*)'

// Shader created with Shader Forge v1.36 
// Shader Forge (c) Neat Corporation / Joachim Holmer - http://www.acegikmo.com/shaderforge/
// Note: Manually altering this data may prevent you from opening it in Shader Forge
/*SF_DATA;ver:1.36;sub:START;pass:START;ps:flbk:,iptp:0,cusa:False,bamd:0,cgin:,lico:1,lgpr:1,limd:0,spmd:1,trmd:0,grmd:0,uamb:True,mssp:True,bkdf:False,hqlp:False,rprd:False,enco:False,rmgx:True,imps:True,rpth:0,vtps:0,hqsc:True,nrmq:1,nrsp:0,vomd:0,spxs:False,tesm:0,olmd:1,culm:0,bsrc:0,bdst:1,dpts:2,wrdp:True,dith:0,atcv:False,rfrpo:True,rfrpn:Refraction,coma:15,ufog:False,aust:True,igpj:False,qofs:0,qpre:2,rntp:3,fgom:False,fgoc:False,fgod:False,fgor:False,fgmd:0,fgcr:0.5,fgcg:0.5,fgcb:0.5,fgca:1,fgde:0.01,fgrn:0,fgrf:300,stcl:False,stva:128,stmr:255,stmw:255,stcp:6,stps:0,stfa:0,stfz:0,ofsf:0,ofsu:0,f2p0:False,fnsp:False,fnfb:False,fsmp:False;n:type:ShaderForge.SFN_Final,id:3138,x:34896,y:32376,varname:node_3138,prsc:2|emission-5720-OUT,clip-4323-OUT;n:type:ShaderForge.SFN_Tex2d,id:5879,x:32510,y:32598,ptovrint:False,ptlb:FireMasks,ptin:_FireMasks,varname:node_5879,prsc:2,glob:False,taghide:False,taghdr:False,tagprd:False,tagnsco:False,tagnrm:False,tex:a06364b2b83f68e4b8a5abc216ba6d5d,ntxv:0,isnm:False|UVIN-4619-UVOUT;n:type:ShaderForge.SFN_Tex2d,id:6173,x:32510,y:32804,ptovrint:False,ptlb:FireMasks2,ptin:_FireMasks2,varname:_FireMasks_copy,prsc:2,glob:False,taghide:False,taghdr:False,tagprd:False,tagnsco:False,tagnrm:False,tex:a06364b2b83f68e4b8a5abc216ba6d5d,ntxv:0,isnm:False|UVIN-1614-UVOUT;n:type:ShaderForge.SFN_Panner,id:4619,x:32231,y:32596,varname:node_4619,prsc:2,spu:0.2,spv:-2.5|UVIN-5786-OUT,DIST-394-OUT;n:type:ShaderForge.SFN_TexCoord,id:5529,x:31320,y:32516,varname:node_5529,prsc:2,uv:0,uaff:False;n:type:ShaderForge.SFN_Time,id:7558,x:31382,y:32934,varname:node_7558,prsc:2;n:type:ShaderForge.SFN_Multiply,id:394,x:31830,y:32817,varname:node_394,prsc:2|A-7558-T,B-4372-OUT;n:type:ShaderForge.SFN_Slider,id:4372,x:31343,y:32817,ptovrint:False,ptlb:DistortionSpeed,ptin:_DistortionSpeed,varname:node_4372,prsc:2,glob:False,taghide:False,taghdr:False,tagprd:False,tagnsco:False,tagnrm:False,min:0,cur:0.15,max:1;n:type:ShaderForge.SFN_Panner,id:1614,x:32195,y:32807,varname:node_1614,prsc:2,spu:0.1,spv:-5|UVIN-5786-OUT,DIST-1557-OUT;n:type:ShaderForge.SFN_Multiply,id:1557,x:32032,y:32946,varname:node_1557,prsc:2|A-394-OUT,B-6064-OUT;n:type:ShaderForge.SFN_Vector1,id:6753,x:31412,y:33132,varname:node_6753,prsc:2,v1:3;n:type:ShaderForge.SFN_Add,id:2292,x:32710,y:32641,varname:node_2292,prsc:2|A-5879-R,B-6173-G;n:type:ShaderForge.SFN_Clamp01,id:5536,x:33102,y:32719,varname:node_5536,prsc:2|IN-4866-OUT;n:type:ShaderForge.SFN_Multiply,id:4498,x:32762,y:32341,varname:node_4498,prsc:2|A-4606-OUT,B-4653-OUT;n:type:ShaderForge.SFN_Vector1,id:4653,x:32532,y:32429,varname:node_4653,prsc:2,v1:1;n:type:ShaderForge.SFN_Multiply,id:4606,x:32512,y:32185,varname:node_4606,prsc:2|A-5529-V,B-6458-A;n:type:ShaderForge.SFN_Multiply,id:2234,x:33206,y:32418,varname:node_2234,prsc:2|A-5227-OUT,B-4498-OUT;n:type:ShaderForge.SFN_Add,id:7930,x:33397,y:32110,varname:node_7930,prsc:2|A-6404-UVOUT,B-2234-OUT;n:type:ShaderForge.SFN_TexCoord,id:6404,x:32848,y:32013,varname:node_6404,prsc:2,uv:0,uaff:False;n:type:ShaderForge.SFN_Tex2d,id:6627,x:33757,y:32027,ptovrint:False,ptlb:FireShape2,ptin:_FireShape2,varname:node_6627,prsc:2,glob:False,taghide:False,taghdr:False,tagprd:False,tagnsco:False,tagnrm:False,tex:59b6c19edd690124eb5f9122ef921b12,ntxv:0,isnm:False|UVIN-7930-OUT;n:type:ShaderForge.SFN_Slider,id:8058,x:32877,y:33013,ptovrint:False,ptlb:DistortionFactor,ptin:_DistortionFactor,varname:node_8058,prsc:2,glob:False,taghide:False,taghdr:False,tagprd:False,tagnsco:False,tagnrm:False,min:0,cur:0.8897883,max:5;n:type:ShaderForge.SFN_Multiply,id:5208,x:34148,y:32576,varname:node_5208,prsc:2|A-9784-RGB,B-6627-R;n:type:ShaderForge.SFN_Color,id:8858,x:33755,y:32706,ptovrint:False,ptlb:InnerFireColour,ptin:_InnerFireColour,varname:node_8858,prsc:2,glob:False,taghide:False,taghdr:False,tagprd:False,tagnsco:False,tagnrm:False,c1:0.9191176,c2:0.8937626,c3:0,c4:1;n:type:ShaderForge.SFN_Color,id:9784,x:33755,y:32505,ptovrint:False,ptlb:OuterFireColour,ptin:_OuterFireColour,varname:node_9784,prsc:2,glob:False,taghide:False,taghdr:False,tagprd:False,tagnsco:False,tagnrm:False,c1:0.8970588,c2:0.5962803,c3:0.1451124,c4:1;n:type:ShaderForge.SFN_Multiply,id:5191,x:34148,y:32712,varname:node_5191,prsc:2|A-8858-RGB,B-6627-G;n:type:ShaderForge.SFN_Add,id:936,x:34444,y:32495,varname:node_936,prsc:2|A-5208-OUT,B-5191-OUT;n:type:ShaderForge.SFN_Multiply,id:5720,x:34610,y:32299,varname:node_5720,prsc:2|A-936-OUT,B-8518-OUT;n:type:ShaderForge.SFN_Slider,id:8518,x:34231,y:32255,ptovrint:False,ptlb:GlowFactor,ptin:_GlowFactor,varname:node_8518,prsc:2,glob:False,taghide:False,taghdr:False,tagprd:False,tagnsco:False,tagnrm:False,min:1,cur:2.689796,max:10;n:type:ShaderForge.SFN_OneMinus,id:4323,x:34193,y:32370,varname:node_4323,prsc:2|IN-6627-B;n:type:ShaderForge.SFN_Tex2d,id:6458,x:32175,y:31956,ptovrint:False,ptlb:FireShape,ptin:_FireShape,varname:node_6458,prsc:2,glob:False,taghide:False,taghdr:False,tagprd:False,tagnsco:False,tagnrm:False,tex:59b6c19edd690124eb5f9122ef921b12,ntxv:0,isnm:False;n:type:ShaderForge.SFN_Add,id:4866,x:32902,y:32694,varname:node_4866,prsc:2|A-6173-B,B-2292-OUT;n:type:ShaderForge.SFN_Multiply,id:5227,x:33348,y:32735,varname:node_5227,prsc:2|A-8058-OUT,B-5536-OUT;n:type:ShaderForge.SFN_Multiply,id:6064,x:31746,y:33027,varname:node_6064,prsc:2|A-4372-OUT,B-6753-OUT;n:type:ShaderForge.SFN_Multiply,id:5786,x:31950,y:32400,varname:node_5786,prsc:2|A-5529-UVOUT,B-8642-OUT;n:type:ShaderForge.SFN_ValueProperty,id:8642,x:31320,y:32366,ptovrint:False,ptlb:DistortionTile,ptin:_DistortionTile,varname:node_8642,prsc:2,glob:False,taghide:False,taghdr:False,tagprd:False,tagnsco:False,tagnrm:False,v1:1;proporder:8858-9784-8058-4372-8518-5879-6173-6627-6458-8642;pass:END;sub:END;*/

Shader "Shader Forge/StylisedFire" {
    Properties {
        _InnerFireColour ("InnerFireColour", Color) = (0.9191176,0.8937626,0,1)
        _OuterFireColour ("OuterFireColour", Color) = (0.8970588,0.5962803,0.1451124,1)
        _DistortionFactor ("DistortionFactor", Range(0, 5)) = 0.8897883
        _DistortionSpeed ("DistortionSpeed", Range(0, 1)) = 0.15
        _GlowFactor ("GlowFactor", Range(1, 10)) = 2.689796
        _FireMasks ("FireMasks", 2D) = "white" {}
        _FireMasks2 ("FireMasks2", 2D) = "white" {}
        _FireShape2 ("FireShape2", 2D) = "white" {}
        _FireShape ("FireShape", 2D) = "white" {}
        _DistortionTile ("DistortionTile", Float ) = 1
        [HideInInspector]_Cutoff ("Alpha cutoff", Range(0,1)) = 0.5
    }
    SubShader {
        Tags {
            "Queue"="AlphaTest"
            "RenderType"="TransparentCutout"
        }
        Pass {
            Name "FORWARD"
            Tags {
                "LightMode"="ForwardBase"
            }
            
            
            CGPROGRAM
            #pragma vertex vert
            #pragma fragment frag
            #define UNITY_PASS_FORWARDBASE
            #include "UnityCG.cginc"
            #pragma multi_compile_fwdbase_fullshadows
            #pragma only_renderers d3d9 d3d11 glcore gles 
            #pragma target 3.0
            uniform float4 _TimeEditor;
            uniform sampler2D _FireMasks; uniform float4 _FireMasks_ST;
            uniform sampler2D _FireMasks2; uniform float4 _FireMasks2_ST;
            uniform float _DistortionSpeed;
            uniform sampler2D _FireShape2; uniform float4 _FireShape2_ST;
            uniform float _DistortionFactor;
            uniform float4 _InnerFireColour;
            uniform float4 _OuterFireColour;
            uniform float _GlowFactor;
            uniform sampler2D _FireShape; uniform float4 _FireShape_ST;
            uniform float _DistortionTile;
            struct VertexInput {
                float4 vertex : POSITION;
                float2 texcoord0 : TEXCOORD0;
            };
            struct VertexOutput {
                float4 pos : SV_POSITION;
                float2 uv0 : TEXCOORD0;
            };
            VertexOutput vert (VertexInput v) {
                VertexOutput o = (VertexOutput)0;
                o.uv0 = v.texcoord0;
                o.pos = UnityObjectToClipPos(v.vertex );
                return o;
            }
            float4 frag(VertexOutput i) : COLOR {
                float4 node_7558 = _Time + _TimeEditor;
                float node_394 = (node_7558.g*_DistortionSpeed);
                float2 node_5786 = (i.uv0*_DistortionTile);
                float2 node_1614 = (node_5786+(node_394*(_DistortionSpeed*3.0))*float2(0.1,-5));
                float4 _FireMasks2_var = tex2D(_FireMasks2,TRANSFORM_TEX(node_1614, _FireMasks2));
                float2 node_4619 = (node_5786+node_394*float2(0.2,-2.5));
                float4 _FireMasks_var = tex2D(_FireMasks,TRANSFORM_TEX(node_4619, _FireMasks));
                float4 _FireShape_var = tex2D(_FireShape,TRANSFORM_TEX(i.uv0, _FireShape));
                float2 node_7930 = (i.uv0+((_DistortionFactor*saturate((_FireMasks2_var.b+(_FireMasks_var.r+_FireMasks2_var.g))))*((i.uv0.g*_FireShape_var.a)*1.0)));
                float4 _FireShape2_var = tex2D(_FireShape2,TRANSFORM_TEX(node_7930, _FireShape2));
                clip((1.0 - _FireShape2_var.b) - 0.5);
////// Lighting:
////// Emissive:
                float3 emissive = (((_OuterFireColour.rgb*_FireShape2_var.r)+(_InnerFireColour.rgb*_FireShape2_var.g))*_GlowFactor);
                float3 finalColor = emissive;
                return fixed4(finalColor,1);
            }
            ENDCG
        }
        Pass {
            Name "ShadowCaster"
            Tags {
                "LightMode"="ShadowCaster"
            }
            Offset 1, 1
            Cull Back
            
            CGPROGRAM
            #pragma vertex vert
            #pragma fragment frag
            #define UNITY_PASS_SHADOWCASTER
            #include "UnityCG.cginc"
            #include "Lighting.cginc"
            #pragma fragmentoption ARB_precision_hint_fastest
            #pragma multi_compile_shadowcaster
            #pragma only_renderers d3d9 d3d11 glcore gles 
            #pragma target 3.0
            uniform float4 _TimeEditor;
            uniform sampler2D _FireMasks; uniform float4 _FireMasks_ST;
            uniform sampler2D _FireMasks2; uniform float4 _FireMasks2_ST;
            uniform float _DistortionSpeed;
            uniform sampler2D _FireShape2; uniform float4 _FireShape2_ST;
            uniform float _DistortionFactor;
            uniform sampler2D _FireShape; uniform float4 _FireShape_ST;
            uniform float _DistortionTile;
            struct VertexInput {
                float4 vertex : POSITION;
                float2 texcoord0 : TEXCOORD0;
            };
            struct VertexOutput {
                V2F_SHADOW_CASTER;
                float2 uv0 : TEXCOORD1;
            };
            VertexOutput vert (VertexInput v) {
                VertexOutput o = (VertexOutput)0;
                o.uv0 = v.texcoord0;
                o.pos = UnityObjectToClipPos(v.vertex );
                TRANSFER_SHADOW_CASTER(o)
                return o;
            }
            float4 frag(VertexOutput i) : COLOR {
                float4 node_7558 = _Time + _TimeEditor;
                float node_394 = (node_7558.g*_DistortionSpeed);
                float2 node_5786 = (i.uv0*_DistortionTile);
                float2 node_1614 = (node_5786+(node_394*(_DistortionSpeed*3.0))*float2(0.1,-5));
                float4 _FireMasks2_var = tex2D(_FireMasks2,TRANSFORM_TEX(node_1614, _FireMasks2));
                float2 node_4619 = (node_5786+node_394*float2(0.2,-2.5));
                float4 _FireMasks_var = tex2D(_FireMasks,TRANSFORM_TEX(node_4619, _FireMasks));
                float4 _FireShape_var = tex2D(_FireShape,TRANSFORM_TEX(i.uv0, _FireShape));
                float2 node_7930 = (i.uv0+((_DistortionFactor*saturate((_FireMasks2_var.b+(_FireMasks_var.r+_FireMasks2_var.g))))*((i.uv0.g*_FireShape_var.a)*1.0)));
                float4 _FireShape2_var = tex2D(_FireShape2,TRANSFORM_TEX(node_7930, _FireShape2));
                clip((1.0 - _FireShape2_var.b) - 0.5);
                SHADOW_CASTER_FRAGMENT(i)
            }
            ENDCG
        }
    }
    FallBack "Diffuse"
    CustomEditor "ShaderForgeMaterialInspector"
}
