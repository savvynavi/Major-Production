// Shader created with Shader Forge v1.38 
// Shader Forge (c) Freya Holmer - http://www.acegikmo.com/shaderforge/
// Note: Manually altering this data may prevent you from opening it in Shader Forge
/*SF_DATA;ver:1.38;sub:START;pass:START;ps:flbk:,iptp:0,cusa:False,bamd:0,cgin:,lico:1,lgpr:1,limd:0,spmd:1,trmd:0,grmd:0,uamb:True,mssp:True,bkdf:False,hqlp:False,rprd:False,enco:False,rmgx:True,imps:True,rpth:0,vtps:0,hqsc:True,nrmq:1,nrsp:0,vomd:0,spxs:False,tesm:0,olmd:1,culm:2,bsrc:0,bdst:1,dpts:2,wrdp:True,dith:0,atcv:False,rfrpo:True,rfrpn:Refraction,coma:15,ufog:False,aust:True,igpj:False,qofs:0,qpre:2,rntp:3,fgom:False,fgoc:False,fgod:False,fgor:False,fgmd:0,fgcr:0.5,fgcg:0.5,fgcb:0.5,fgca:1,fgde:0.01,fgrn:0,fgrf:300,stcl:False,atwp:False,stva:128,stmr:255,stmw:255,stcp:6,stps:0,stfa:0,stfz:0,ofsf:0,ofsu:0,f2p0:False,fnsp:False,fnfb:False,fsmp:False;n:type:ShaderForge.SFN_Final,id:3138,x:33345,y:32516,varname:node_3138,prsc:2|emission-587-OUT,clip-3524-OUT,voffset-2588-OUT,tess-3699-OUT;n:type:ShaderForge.SFN_Color,id:7241,x:32125,y:31866,ptovrint:False,ptlb:Start Colour,ptin:_StartColour,varname:node_7241,prsc:2,glob:False,taghide:False,taghdr:False,tagprd:False,tagnsco:False,tagnrm:False,c1:1,c2:0.9845842,c3:0.8602941,c4:1;n:type:ShaderForge.SFN_TexCoord,id:9036,x:30643,y:33142,varname:node_9036,prsc:2,uv:0,uaff:True;n:type:ShaderForge.SFN_Tex2d,id:4649,x:31296,y:32853,ptovrint:False,ptlb:Deformation Noise,ptin:_DeformationNoise,varname:node_4649,prsc:2,glob:False,taghide:False,taghdr:False,tagprd:False,tagnsco:False,tagnrm:False,ntxv:0,isnm:False|UVIN-5731-UVOUT;n:type:ShaderForge.SFN_NormalVector,id:523,x:31587,y:33354,prsc:2,pt:False;n:type:ShaderForge.SFN_Multiply,id:3268,x:31990,y:33333,varname:node_3268,prsc:2|A-8275-OUT,B-523-OUT;n:type:ShaderForge.SFN_Slider,id:3699,x:32306,y:33750,ptovrint:False,ptlb:TessellationValue,ptin:_TessellationValue,varname:node_3699,prsc:2,glob:False,taghide:False,taghdr:False,tagprd:False,tagnsco:False,tagnrm:False,min:0,cur:2,max:10;n:type:ShaderForge.SFN_Panner,id:5731,x:30953,y:32859,varname:node_5731,prsc:2,spu:0.05,spv:0.05|UVIN-9036-UVOUT;n:type:ShaderForge.SFN_Tex2d,id:2897,x:31451,y:32339,ptovrint:False,ptlb:Alpha Clip Noise,ptin:_AlphaClipNoise,varname:node_2897,prsc:2,glob:False,taghide:False,taghdr:False,tagprd:False,tagnsco:False,tagnrm:False,ntxv:0,isnm:False|UVIN-5731-UVOUT;n:type:ShaderForge.SFN_OneMinus,id:6823,x:31423,y:33591,varname:node_6823,prsc:2|IN-9036-W;n:type:ShaderForge.SFN_Lerp,id:2588,x:32328,y:33480,varname:node_2588,prsc:2|A-3268-OUT,B-6103-OUT,T-9367-OUT;n:type:ShaderForge.SFN_Vector1,id:6103,x:32057,y:33494,varname:node_6103,prsc:2,v1:-1;n:type:ShaderForge.SFN_Multiply,id:8275,x:31612,y:33131,varname:node_8275,prsc:2|A-8852-OUT,B-4649-R;n:type:ShaderForge.SFN_Slider,id:8852,x:30840,y:32685,ptovrint:False,ptlb:Deformation Multiplier,ptin:_DeformationMultiplier,varname:node_8852,prsc:2,glob:False,taghide:False,taghdr:False,tagprd:False,tagnsco:False,tagnrm:False,min:1,cur:1,max:20;n:type:ShaderForge.SFN_RemapRange,id:7798,x:31700,y:33648,varname:node_7798,prsc:2,frmn:0,frmx:1,tomn:-2,tomx:2|IN-6823-OUT;n:type:ShaderForge.SFN_Clamp01,id:9367,x:31935,y:33648,varname:node_9367,prsc:2|IN-7798-OUT;n:type:ShaderForge.SFN_Vector1,id:4258,x:31782,y:32291,varname:node_4258,prsc:2,v1:0.5;n:type:ShaderForge.SFN_Multiply,id:7637,x:31782,y:32108,varname:node_7637,prsc:2|A-4649-R,B-5491-OUT;n:type:ShaderForge.SFN_Vector1,id:5491,x:31510,y:32136,varname:node_5491,prsc:2,v1:2;n:type:ShaderForge.SFN_Clamp01,id:4156,x:32170,y:32076,varname:node_4156,prsc:2|IN-7637-OUT;n:type:ShaderForge.SFN_Subtract,id:3365,x:32015,y:32257,varname:node_3365,prsc:2|A-7637-OUT,B-4258-OUT;n:type:ShaderForge.SFN_Clamp01,id:1335,x:32210,y:32235,varname:node_1335,prsc:2|IN-3365-OUT;n:type:ShaderForge.SFN_Color,id:579,x:32108,y:31702,ptovrint:False,ptlb:Middle Colour,ptin:_MiddleColour,varname:node_579,prsc:2,glob:False,taghide:False,taghdr:False,tagprd:False,tagnsco:False,tagnrm:False,c1:1,c2:0.2689655,c3:0,c4:1;n:type:ShaderForge.SFN_Color,id:6163,x:31782,y:31850,ptovrint:False,ptlb:End Colour,ptin:_EndColour,varname:node_6163,prsc:2,glob:False,taghide:False,taghdr:False,tagprd:False,tagnsco:False,tagnrm:False,c1:0.1470588,c2:0.0724481,c3:0.0724481,c4:1;n:type:ShaderForge.SFN_Lerp,id:284,x:32381,y:31820,varname:node_284,prsc:2|A-7241-RGB,B-579-RGB,T-4156-OUT;n:type:ShaderForge.SFN_Lerp,id:2054,x:32557,y:31947,varname:node_2054,prsc:2|A-284-OUT,B-6163-RGB,T-1335-OUT;n:type:ShaderForge.SFN_Multiply,id:587,x:32926,y:32273,varname:node_587,prsc:2|A-2054-OUT,B-7003-OUT;n:type:ShaderForge.SFN_Slider,id:7003,x:32177,y:32428,ptovrint:False,ptlb:Emission Multiiplier,ptin:_EmissionMultiiplier,varname:node_7003,prsc:2,glob:False,taghide:False,taghdr:False,tagprd:False,tagnsco:False,tagnrm:False,min:1,cur:1,max:10;n:type:ShaderForge.SFN_Multiply,id:8982,x:32246,y:32892,varname:node_8982,prsc:2|A-6823-OUT,B-7669-OUT;n:type:ShaderForge.SFN_Multiply,id:7669,x:31888,y:32444,varname:node_7669,prsc:2|A-2897-R,B-5491-OUT;n:type:ShaderForge.SFN_Lerp,id:3524,x:32582,y:32969,varname:node_3524,prsc:2|A-8982-OUT,B-136-OUT,T-6823-OUT;n:type:ShaderForge.SFN_Vector1,id:136,x:32382,y:32987,varname:node_136,prsc:2,v1:1;n:type:ShaderForge.SFN_Lerp,id:1146,x:33142,y:32459,varname:node_1146,prsc:2|A-587-OUT,B-7241-RGB,T-9367-OUT;proporder:4649-8852-3699-2897-7241-579-6163-7003;pass:END;sub:END;*/

Shader "Shader Forge/VertexOffsetNoise" {
    Properties {
        _DeformationNoise ("Deformation Noise", 2D) = "white" {}
        _DeformationMultiplier ("Deformation Multiplier", Range(1, 20)) = 1
        _TessellationValue ("TessellationValue", Range(0, 10)) = 2
        _AlphaClipNoise ("Alpha Clip Noise", 2D) = "white" {}
        _StartColour ("Start Colour", Color) = (1,0.9845842,0.8602941,1)
        _MiddleColour ("Middle Colour", Color) = (1,0.2689655,0,1)
        _EndColour ("End Colour", Color) = (0.1470588,0.0724481,0.0724481,1)
        _EmissionMultiiplier ("Emission Multiiplier", Range(1, 10)) = 1
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
            Cull Off
            
            
            CGPROGRAM
            #pragma hull hull
            #pragma domain domain
            #pragma vertex tessvert
            #pragma fragment frag
            #define UNITY_PASS_FORWARDBASE
            #include "UnityCG.cginc"
            #include "Tessellation.cginc"
            #pragma multi_compile_fwdbase_fullshadows
            #pragma only_renderers d3d9 d3d11 glcore gles 
            #pragma target 5.0
            uniform float4 _StartColour;
            uniform sampler2D _DeformationNoise; uniform float4 _DeformationNoise_ST;
            uniform float _TessellationValue;
            uniform sampler2D _AlphaClipNoise; uniform float4 _AlphaClipNoise_ST;
            uniform float _DeformationMultiplier;
            uniform float4 _MiddleColour;
            uniform float4 _EndColour;
            uniform float _EmissionMultiiplier;
            struct VertexInput {
                float4 vertex : POSITION;
                float3 normal : NORMAL;
                float4 tangent : TANGENT;
                float4 texcoord0 : TEXCOORD0;
            };
            struct VertexOutput {
                float4 pos : SV_POSITION;
                float4 uv0 : TEXCOORD0;
                float4 posWorld : TEXCOORD1;
                float3 normalDir : TEXCOORD2;
            };
            VertexOutput vert (VertexInput v) {
                VertexOutput o = (VertexOutput)0;
                o.uv0 = v.texcoord0;
                o.normalDir = UnityObjectToWorldNormal(v.normal);
                float4 node_6631 = _Time;
                float2 node_5731 = (o.uv0+node_6631.g*float2(0.05,0.05));
                float4 _DeformationNoise_var = tex2Dlod(_DeformationNoise,float4(TRANSFORM_TEX(node_5731, _DeformationNoise),0.0,0));
                float node_6103 = (-1.0);
                float node_6823 = (1.0 - o.uv0.a);
                float node_9367 = saturate((node_6823*4.0+-2.0));
                v.vertex.xyz += lerp(((_DeformationMultiplier*_DeformationNoise_var.r)*v.normal),float3(node_6103,node_6103,node_6103),node_9367);
                o.posWorld = mul(unity_ObjectToWorld, v.vertex);
                o.pos = UnityObjectToClipPos( v.vertex );
                return o;
            }
            #ifdef UNITY_CAN_COMPILE_TESSELLATION
                struct TessVertex {
                    float4 vertex : INTERNALTESSPOS;
                    float3 normal : NORMAL;
                    float4 tangent : TANGENT;
                    float4 texcoord0 : TEXCOORD0;
                };
                struct OutputPatchConstant {
                    float edge[3]         : SV_TessFactor;
                    float inside          : SV_InsideTessFactor;
                    float3 vTangent[4]    : TANGENT;
                    float4 vUV[4]         : TEXCOORD;
                    float3 vTanUCorner[4] : TANUCORNER;
                    float3 vTanVCorner[4] : TANVCORNER;
                    float4 vCWts          : TANWEIGHTS;
                };
                TessVertex tessvert (VertexInput v) {
                    TessVertex o;
                    o.vertex = v.vertex;
                    o.normal = v.normal;
                    o.tangent = v.tangent;
                    o.texcoord0 = v.texcoord0;
                    return o;
                }
                float Tessellation(TessVertex v){
                    return _TessellationValue;
                }
                float4 Tessellation(TessVertex v, TessVertex v1, TessVertex v2){
                    float tv = Tessellation(v);
                    float tv1 = Tessellation(v1);
                    float tv2 = Tessellation(v2);
                    return float4( tv1+tv2, tv2+tv, tv+tv1, tv+tv1+tv2 ) / float4(2,2,2,3);
                }
                OutputPatchConstant hullconst (InputPatch<TessVertex,3> v) {
                    OutputPatchConstant o = (OutputPatchConstant)0;
                    float4 ts = Tessellation( v[0], v[1], v[2] );
                    o.edge[0] = ts.x;
                    o.edge[1] = ts.y;
                    o.edge[2] = ts.z;
                    o.inside = ts.w;
                    return o;
                }
                [domain("tri")]
                [partitioning("fractional_odd")]
                [outputtopology("triangle_cw")]
                [patchconstantfunc("hullconst")]
                [outputcontrolpoints(3)]
                TessVertex hull (InputPatch<TessVertex,3> v, uint id : SV_OutputControlPointID) {
                    return v[id];
                }
                [domain("tri")]
                VertexOutput domain (OutputPatchConstant tessFactors, const OutputPatch<TessVertex,3> vi, float3 bary : SV_DomainLocation) {
                    VertexInput v = (VertexInput)0;
                    v.vertex = vi[0].vertex*bary.x + vi[1].vertex*bary.y + vi[2].vertex*bary.z;
                    v.normal = vi[0].normal*bary.x + vi[1].normal*bary.y + vi[2].normal*bary.z;
                    v.tangent = vi[0].tangent*bary.x + vi[1].tangent*bary.y + vi[2].tangent*bary.z;
                    v.texcoord0 = vi[0].texcoord0*bary.x + vi[1].texcoord0*bary.y + vi[2].texcoord0*bary.z;
                    VertexOutput o = vert(v);
                    return o;
                }
            #endif
            float4 frag(VertexOutput i, float facing : VFACE) : COLOR {
                float isFrontFace = ( facing >= 0 ? 1 : 0 );
                float faceSign = ( facing >= 0 ? 1 : -1 );
                i.normalDir = normalize(i.normalDir);
                i.normalDir *= faceSign;
                float3 viewDirection = normalize(_WorldSpaceCameraPos.xyz - i.posWorld.xyz);
                float3 normalDirection = i.normalDir;
                float node_6823 = (1.0 - i.uv0.a);
                float4 node_6631 = _Time;
                float2 node_5731 = (i.uv0+node_6631.g*float2(0.05,0.05));
                float4 _AlphaClipNoise_var = tex2D(_AlphaClipNoise,TRANSFORM_TEX(node_5731, _AlphaClipNoise));
                float node_5491 = 2.0;
                clip(lerp((node_6823*(_AlphaClipNoise_var.r*node_5491)),1.0,node_6823) - 0.5);
////// Lighting:
////// Emissive:
                float4 _DeformationNoise_var = tex2D(_DeformationNoise,TRANSFORM_TEX(node_5731, _DeformationNoise));
                float node_7637 = (_DeformationNoise_var.r*node_5491);
                float3 node_587 = (lerp(lerp(_StartColour.rgb,_MiddleColour.rgb,saturate(node_7637)),_EndColour.rgb,saturate((node_7637-0.5)))*_EmissionMultiiplier);
                float3 emissive = node_587;
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
            Cull Off
            
            CGPROGRAM
            #pragma hull hull
            #pragma domain domain
            #pragma vertex tessvert
            #pragma fragment frag
            #define UNITY_PASS_SHADOWCASTER
            #include "UnityCG.cginc"
            #include "Lighting.cginc"
            #include "Tessellation.cginc"
            #pragma fragmentoption ARB_precision_hint_fastest
            #pragma multi_compile_shadowcaster
            #pragma only_renderers d3d9 d3d11 glcore gles 
            #pragma target 5.0
            uniform sampler2D _DeformationNoise; uniform float4 _DeformationNoise_ST;
            uniform float _TessellationValue;
            uniform sampler2D _AlphaClipNoise; uniform float4 _AlphaClipNoise_ST;
            uniform float _DeformationMultiplier;
            struct VertexInput {
                float4 vertex : POSITION;
                float3 normal : NORMAL;
                float4 tangent : TANGENT;
                float4 texcoord0 : TEXCOORD0;
            };
            struct VertexOutput {
                V2F_SHADOW_CASTER;
                float4 uv0 : TEXCOORD1;
                float4 posWorld : TEXCOORD2;
                float3 normalDir : TEXCOORD3;
            };
            VertexOutput vert (VertexInput v) {
                VertexOutput o = (VertexOutput)0;
                o.uv0 = v.texcoord0;
                o.normalDir = UnityObjectToWorldNormal(v.normal);
                float4 node_6687 = _Time;
                float2 node_5731 = (o.uv0+node_6687.g*float2(0.05,0.05));
                float4 _DeformationNoise_var = tex2Dlod(_DeformationNoise,float4(TRANSFORM_TEX(node_5731, _DeformationNoise),0.0,0));
                float node_6103 = (-1.0);
                float node_6823 = (1.0 - o.uv0.a);
                float node_9367 = saturate((node_6823*4.0+-2.0));
                v.vertex.xyz += lerp(((_DeformationMultiplier*_DeformationNoise_var.r)*v.normal),float3(node_6103,node_6103,node_6103),node_9367);
                o.posWorld = mul(unity_ObjectToWorld, v.vertex);
                o.pos = UnityObjectToClipPos( v.vertex );
                TRANSFER_SHADOW_CASTER(o)
                return o;
            }
            #ifdef UNITY_CAN_COMPILE_TESSELLATION
                struct TessVertex {
                    float4 vertex : INTERNALTESSPOS;
                    float3 normal : NORMAL;
                    float4 tangent : TANGENT;
                    float4 texcoord0 : TEXCOORD0;
                };
                struct OutputPatchConstant {
                    float edge[3]         : SV_TessFactor;
                    float inside          : SV_InsideTessFactor;
                    float3 vTangent[4]    : TANGENT;
                    float4 vUV[4]         : TEXCOORD;
                    float3 vTanUCorner[4] : TANUCORNER;
                    float3 vTanVCorner[4] : TANVCORNER;
                    float4 vCWts          : TANWEIGHTS;
                };
                TessVertex tessvert (VertexInput v) {
                    TessVertex o;
                    o.vertex = v.vertex;
                    o.normal = v.normal;
                    o.tangent = v.tangent;
                    o.texcoord0 = v.texcoord0;
                    return o;
                }
                float Tessellation(TessVertex v){
                    return _TessellationValue;
                }
                float4 Tessellation(TessVertex v, TessVertex v1, TessVertex v2){
                    float tv = Tessellation(v);
                    float tv1 = Tessellation(v1);
                    float tv2 = Tessellation(v2);
                    return float4( tv1+tv2, tv2+tv, tv+tv1, tv+tv1+tv2 ) / float4(2,2,2,3);
                }
                OutputPatchConstant hullconst (InputPatch<TessVertex,3> v) {
                    OutputPatchConstant o = (OutputPatchConstant)0;
                    float4 ts = Tessellation( v[0], v[1], v[2] );
                    o.edge[0] = ts.x;
                    o.edge[1] = ts.y;
                    o.edge[2] = ts.z;
                    o.inside = ts.w;
                    return o;
                }
                [domain("tri")]
                [partitioning("fractional_odd")]
                [outputtopology("triangle_cw")]
                [patchconstantfunc("hullconst")]
                [outputcontrolpoints(3)]
                TessVertex hull (InputPatch<TessVertex,3> v, uint id : SV_OutputControlPointID) {
                    return v[id];
                }
                [domain("tri")]
                VertexOutput domain (OutputPatchConstant tessFactors, const OutputPatch<TessVertex,3> vi, float3 bary : SV_DomainLocation) {
                    VertexInput v = (VertexInput)0;
                    v.vertex = vi[0].vertex*bary.x + vi[1].vertex*bary.y + vi[2].vertex*bary.z;
                    v.normal = vi[0].normal*bary.x + vi[1].normal*bary.y + vi[2].normal*bary.z;
                    v.tangent = vi[0].tangent*bary.x + vi[1].tangent*bary.y + vi[2].tangent*bary.z;
                    v.texcoord0 = vi[0].texcoord0*bary.x + vi[1].texcoord0*bary.y + vi[2].texcoord0*bary.z;
                    VertexOutput o = vert(v);
                    return o;
                }
            #endif
            float4 frag(VertexOutput i, float facing : VFACE) : COLOR {
                float isFrontFace = ( facing >= 0 ? 1 : 0 );
                float faceSign = ( facing >= 0 ? 1 : -1 );
                i.normalDir = normalize(i.normalDir);
                i.normalDir *= faceSign;
                float3 viewDirection = normalize(_WorldSpaceCameraPos.xyz - i.posWorld.xyz);
                float3 normalDirection = i.normalDir;
                float node_6823 = (1.0 - i.uv0.a);
                float4 node_6687 = _Time;
                float2 node_5731 = (i.uv0+node_6687.g*float2(0.05,0.05));
                float4 _AlphaClipNoise_var = tex2D(_AlphaClipNoise,TRANSFORM_TEX(node_5731, _AlphaClipNoise));
                float node_5491 = 2.0;
                clip(lerp((node_6823*(_AlphaClipNoise_var.r*node_5491)),1.0,node_6823) - 0.5);
                SHADOW_CASTER_FRAGMENT(i)
            }
            ENDCG
        }
    }
    FallBack "Diffuse"
    CustomEditor "ShaderForgeMaterialInspector"
}
