// Shader created with Shader Forge v1.38 
// Shader Forge (c) Freya Holmer - http://www.acegikmo.com/shaderforge/
// Note: Manually altering this data may prevent you from opening it in Shader Forge
/*SF_DATA;ver:1.38;sub:START;pass:START;ps:flbk:,iptp:0,cusa:False,bamd:0,cgin:,lico:1,lgpr:1,limd:0,spmd:1,trmd:0,grmd:0,uamb:True,mssp:True,bkdf:False,hqlp:False,rprd:False,enco:False,rmgx:True,imps:True,rpth:0,vtps:0,hqsc:True,nrmq:1,nrsp:0,vomd:0,spxs:False,tesm:0,olmd:1,culm:0,bsrc:0,bdst:1,dpts:2,wrdp:True,dith:0,atcv:False,rfrpo:True,rfrpn:Refraction,coma:15,ufog:False,aust:True,igpj:False,qofs:0,qpre:2,rntp:3,fgom:False,fgoc:False,fgod:False,fgor:False,fgmd:0,fgcr:0.5,fgcg:0.5,fgcb:0.5,fgca:1,fgde:0.01,fgrn:0,fgrf:300,stcl:False,atwp:False,stva:128,stmr:255,stmw:255,stcp:6,stps:0,stfa:0,stfz:0,ofsf:0,ofsu:0,f2p0:False,fnsp:False,fnfb:False,fsmp:False;n:type:ShaderForge.SFN_Final,id:3138,x:32719,y:32712,varname:node_3138,prsc:2|emission-4418-OUT,clip-7242-OUT;n:type:ShaderForge.SFN_TexCoord,id:4736,x:30984,y:32879,varname:node_4736,prsc:2,uv:0,uaff:True;n:type:ShaderForge.SFN_OneMinus,id:9498,x:31362,y:33038,varname:node_9498,prsc:2|IN-4736-W;n:type:ShaderForge.SFN_RemapRange,id:3218,x:31933,y:32949,varname:node_3218,prsc:2,frmn:0,frmx:1,tomn:-1,tomx:1|IN-9498-OUT;n:type:ShaderForge.SFN_Tex2d,id:4978,x:31619,y:32579,ptovrint:False,ptlb:ErosionMap,ptin:_ErosionMap,varname:node_4978,prsc:2,glob:False,taghide:False,taghdr:False,tagprd:False,tagnsco:False,tagnrm:False,tex:75cdef7015ab77e4984c2267bb74c504,ntxv:0,isnm:False|UVIN-1937-UVOUT;n:type:ShaderForge.SFN_Add,id:7242,x:32220,y:32921,varname:node_7242,prsc:2|A-3218-OUT,B-4978-R;n:type:ShaderForge.SFN_Panner,id:1937,x:31293,y:32568,varname:node_1937,prsc:2,spu:0.5,spv:0.5|UVIN-4736-UVOUT;n:type:ShaderForge.SFN_VertexColor,id:684,x:32152,y:32504,varname:node_684,prsc:2;n:type:ShaderForge.SFN_Multiply,id:4418,x:32411,y:32415,varname:node_4418,prsc:2|A-4129-OUT,B-684-RGB;n:type:ShaderForge.SFN_Vector1,id:4129,x:32167,y:32391,varname:node_4129,prsc:2,v1:2;proporder:4978;pass:END;sub:END;*/

Shader "Shader Forge/OpaqueLifeErosion" {
    Properties {
        _ErosionMap ("ErosionMap", 2D) = "white" {}
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
            uniform sampler2D _ErosionMap; uniform float4 _ErosionMap_ST;
            struct VertexInput {
                float4 vertex : POSITION;
                float4 texcoord0 : TEXCOORD0;
                float4 vertexColor : COLOR;
            };
            struct VertexOutput {
                float4 pos : SV_POSITION;
                float4 uv0 : TEXCOORD0;
                float4 vertexColor : COLOR;
            };
            VertexOutput vert (VertexInput v) {
                VertexOutput o = (VertexOutput)0;
                o.uv0 = v.texcoord0;
                o.vertexColor = v.vertexColor;
                o.pos = UnityObjectToClipPos( v.vertex );
                return o;
            }
            float4 frag(VertexOutput i) : COLOR {
                float4 node_939 = _Time;
                float2 node_1937 = (i.uv0+node_939.g*float2(0.5,0.5));
                float4 _ErosionMap_var = tex2D(_ErosionMap,TRANSFORM_TEX(node_1937, _ErosionMap));
                clip((((1.0 - i.uv0.a)*2.0+-1.0)+_ErosionMap_var.r) - 0.5);
////// Lighting:
////// Emissive:
                float3 emissive = (2.0*i.vertexColor.rgb);
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
            uniform sampler2D _ErosionMap; uniform float4 _ErosionMap_ST;
            struct VertexInput {
                float4 vertex : POSITION;
                float4 texcoord0 : TEXCOORD0;
            };
            struct VertexOutput {
                V2F_SHADOW_CASTER;
                float4 uv0 : TEXCOORD1;
            };
            VertexOutput vert (VertexInput v) {
                VertexOutput o = (VertexOutput)0;
                o.uv0 = v.texcoord0;
                o.pos = UnityObjectToClipPos( v.vertex );
                TRANSFER_SHADOW_CASTER(o)
                return o;
            }
            float4 frag(VertexOutput i) : COLOR {
                float4 node_1570 = _Time;
                float2 node_1937 = (i.uv0+node_1570.g*float2(0.5,0.5));
                float4 _ErosionMap_var = tex2D(_ErosionMap,TRANSFORM_TEX(node_1937, _ErosionMap));
                clip((((1.0 - i.uv0.a)*2.0+-1.0)+_ErosionMap_var.r) - 0.5);
                SHADOW_CASTER_FRAGMENT(i)
            }
            ENDCG
        }
    }
    FallBack "Diffuse"
    CustomEditor "ShaderForgeMaterialInspector"
}
