// Shader created with Shader Forge v1.38 
// Shader Forge (c) Freya Holmer - http://www.acegikmo.com/shaderforge/
// Note: Manually altering this data may prevent you from opening it in Shader Forge
/*SF_DATA;ver:1.38;sub:START;pass:START;ps:flbk:,iptp:0,cusa:False,bamd:0,cgin:,lico:1,lgpr:1,limd:0,spmd:1,trmd:0,grmd:0,uamb:True,mssp:True,bkdf:False,hqlp:False,rprd:False,enco:False,rmgx:True,imps:True,rpth:0,vtps:0,hqsc:True,nrmq:1,nrsp:0,vomd:0,spxs:False,tesm:0,olmd:1,culm:0,bsrc:0,bdst:0,dpts:2,wrdp:False,dith:0,atcv:False,rfrpo:True,rfrpn:Refraction,coma:15,ufog:True,aust:True,igpj:True,qofs:0,qpre:3,rntp:2,fgom:False,fgoc:True,fgod:False,fgor:False,fgmd:0,fgcr:0.5,fgcg:0.5,fgcb:0.5,fgca:1,fgde:0.01,fgrn:0,fgrf:300,stcl:False,atwp:False,stva:128,stmr:255,stmw:255,stcp:6,stps:0,stfa:0,stfz:0,ofsf:0,ofsu:0,f2p0:False,fnsp:True,fnfb:True,fsmp:False;n:type:ShaderForge.SFN_Final,id:4795,x:32852,y:32701,varname:node_4795,prsc:2|emission-2393-OUT;n:type:ShaderForge.SFN_Tex2d,id:6074,x:32235,y:32600,varname:_MainTex,prsc:2,ntxv:0,isnm:False|UVIN-8584-UVOUT,TEX-9589-TEX;n:type:ShaderForge.SFN_Multiply,id:2393,x:32570,y:32775,varname:node_2393,prsc:2|A-6074-RGB,B-2053-RGB,C-797-RGB,D-9921-OUT;n:type:ShaderForge.SFN_VertexColor,id:2053,x:32235,y:32772,varname:node_2053,prsc:2;n:type:ShaderForge.SFN_Color,id:797,x:32235,y:32930,ptovrint:True,ptlb:Color,ptin:_TintColor,varname:_TintColor,prsc:2,glob:False,taghide:False,taghdr:False,tagprd:False,tagnsco:False,tagnrm:False,c1:0.5,c2:0.5,c3:0.5,c4:1;n:type:ShaderForge.SFN_UVTile,id:8584,x:31992,y:32670,varname:node_8584,prsc:2|UVIN-9027-UVOUT,WDT-6788-OUT,HGT-6849-OUT,TILE-3899-OUT;n:type:ShaderForge.SFN_TexCoord,id:9027,x:31466,y:32553,varname:node_9027,prsc:2,uv:0,uaff:False;n:type:ShaderForge.SFN_Slider,id:2037,x:31124,y:32711,ptovrint:False,ptlb:FlipbookWidth,ptin:_FlipbookWidth,varname:node_2037,prsc:2,glob:False,taghide:False,taghdr:False,tagprd:False,tagnsco:False,tagnrm:False,min:1,cur:4,max:10;n:type:ShaderForge.SFN_Round,id:6788,x:31581,y:32725,varname:node_6788,prsc:2|IN-2037-OUT;n:type:ShaderForge.SFN_Slider,id:7491,x:31097,y:32829,ptovrint:False,ptlb:FlipBookHeight,ptin:_FlipBookHeight,varname:node_7491,prsc:2,glob:False,taghide:False,taghdr:False,tagprd:False,tagnsco:False,tagnrm:False,min:1,cur:2,max:10;n:type:ShaderForge.SFN_Round,id:6849,x:31580,y:33018,varname:node_6849,prsc:2|IN-7491-OUT;n:type:ShaderForge.SFN_Vector1,id:1040,x:31655,y:33195,varname:node_1040,prsc:2,v1:2;n:type:ShaderForge.SFN_Time,id:9173,x:31081,y:33210,varname:node_9173,prsc:2;n:type:ShaderForge.SFN_Sin,id:1767,x:31504,y:33312,varname:node_1767,prsc:2|IN-5482-OUT;n:type:ShaderForge.SFN_RemapRange,id:8226,x:31756,y:33286,varname:node_8226,prsc:2,frmn:0,frmx:1,tomn:0,tomx:7|IN-1767-OUT;n:type:ShaderForge.SFN_Tex2dAsset,id:9589,x:31992,y:32836,ptovrint:False,ptlb:Flipbook,ptin:_Flipbook,varname:node_9589,glob:False,taghide:False,taghdr:False,tagprd:False,tagnsco:False,tagnrm:False,ntxv:0,isnm:False;n:type:ShaderForge.SFN_Vector1,id:2547,x:31739,y:33131,varname:node_2547,prsc:2,v1:3;n:type:ShaderForge.SFN_Round,id:3899,x:31912,y:33379,varname:node_3899,prsc:2|IN-8226-OUT;n:type:ShaderForge.SFN_Multiply,id:5482,x:31361,y:33362,varname:node_5482,prsc:2|A-9173-TTR,B-3570-OUT;n:type:ShaderForge.SFN_Slider,id:3570,x:30947,y:33438,ptovrint:False,ptlb:AnimationSpeed,ptin:_AnimationSpeed,varname:node_3570,prsc:2,glob:False,taghide:False,taghdr:False,tagprd:False,tagnsco:False,tagnrm:False,min:1,cur:1,max:5;n:type:ShaderForge.SFN_Slider,id:9921,x:32262,y:33253,ptovrint:False,ptlb:EmissionBoost,ptin:_EmissionBoost,varname:node_9921,prsc:2,glob:False,taghide:False,taghdr:False,tagprd:False,tagnsco:False,tagnrm:False,min:1,cur:1,max:20;proporder:797-2037-7491-9589-3570-9921;pass:END;sub:END;*/

Shader "Shader Forge/BasicAdditiveFlipbook" {
    Properties {
        _TintColor ("Color", Color) = (0.5,0.5,0.5,1)
        _FlipbookWidth ("FlipbookWidth", Range(1, 10)) = 4
        _FlipBookHeight ("FlipBookHeight", Range(1, 10)) = 2
        _Flipbook ("Flipbook", 2D) = "white" {}
        _AnimationSpeed ("AnimationSpeed", Range(1, 5)) = 1
        _EmissionBoost ("EmissionBoost", Range(1, 20)) = 1
    }
    SubShader {
        Tags {
            "IgnoreProjector"="True"
            "Queue"="Transparent"
            "RenderType"="Transparent"
        }
        Pass {
            Name "FORWARD"
            Tags {
                "LightMode"="ForwardBase"
            }
            Blend One One
            ZWrite Off
            
            CGPROGRAM
            #pragma vertex vert
            #pragma fragment frag
            #define UNITY_PASS_FORWARDBASE
            #include "UnityCG.cginc"
            #pragma multi_compile_fwdbase
            #pragma multi_compile_fog
            #pragma only_renderers d3d9 d3d11 glcore gles 
            #pragma target 3.0
            uniform float4 _TintColor;
            uniform float _FlipbookWidth;
            uniform float _FlipBookHeight;
            uniform sampler2D _Flipbook; uniform float4 _Flipbook_ST;
            uniform float _AnimationSpeed;
            uniform float _EmissionBoost;
            struct VertexInput {
                float4 vertex : POSITION;
                float2 texcoord0 : TEXCOORD0;
                float4 vertexColor : COLOR;
            };
            struct VertexOutput {
                float4 pos : SV_POSITION;
                float2 uv0 : TEXCOORD0;
                float4 vertexColor : COLOR;
                UNITY_FOG_COORDS(1)
            };
            VertexOutput vert (VertexInput v) {
                VertexOutput o = (VertexOutput)0;
                o.uv0 = v.texcoord0;
                o.vertexColor = v.vertexColor;
                o.pos = UnityObjectToClipPos( v.vertex );
                UNITY_TRANSFER_FOG(o,o.pos);
                return o;
            }
            float4 frag(VertexOutput i) : COLOR {
////// Lighting:
////// Emissive:
                float node_6788 = round(_FlipbookWidth);
                float4 node_9173 = _Time;
                float node_3899 = round((sin((node_9173.a*_AnimationSpeed))*7.0+0.0));
                float2 node_8584_tc_rcp = float2(1.0,1.0)/float2( node_6788, round(_FlipBookHeight) );
                float node_8584_ty = floor(node_3899 * node_8584_tc_rcp.x);
                float node_8584_tx = node_3899 - node_6788 * node_8584_ty;
                float2 node_8584 = (i.uv0 + float2(node_8584_tx, node_8584_ty)) * node_8584_tc_rcp;
                float4 _MainTex = tex2D(_Flipbook,TRANSFORM_TEX(node_8584, _Flipbook));
                float3 emissive = (_MainTex.rgb*i.vertexColor.rgb*_TintColor.rgb*_EmissionBoost);
                float3 finalColor = emissive;
                fixed4 finalRGBA = fixed4(finalColor,1);
                UNITY_APPLY_FOG_COLOR(i.fogCoord, finalRGBA, fixed4(0.5,0.5,0.5,1));
                return finalRGBA;
            }
            ENDCG
        }
    }
    CustomEditor "ShaderForgeMaterialInspector"
}
