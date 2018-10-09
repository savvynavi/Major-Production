// Shader created with Shader Forge v1.38 
// Shader Forge (c) Freya Holmer - http://www.acegikmo.com/shaderforge/
// Note: Manually altering this data may prevent you from opening it in Shader Forge
/*SF_DATA;ver:1.38;sub:START;pass:START;ps:flbk:,iptp:0,cusa:False,bamd:0,cgin:,lico:1,lgpr:1,limd:0,spmd:1,trmd:0,grmd:0,uamb:False,mssp:True,bkdf:False,hqlp:False,rprd:False,enco:False,rmgx:True,imps:True,rpth:0,vtps:0,hqsc:True,nrmq:1,nrsp:0,vomd:0,spxs:False,tesm:0,olmd:1,culm:0,bsrc:3,bdst:7,dpts:2,wrdp:False,dith:0,atcv:False,rfrpo:True,rfrpn:Refraction,coma:15,ufog:False,aust:True,igpj:True,qofs:0,qpre:3,rntp:2,fgom:False,fgoc:False,fgod:False,fgor:False,fgmd:0,fgcr:0.5,fgcg:0.5,fgcb:0.5,fgca:1,fgde:0.01,fgrn:0,fgrf:300,stcl:False,atwp:False,stva:128,stmr:255,stmw:255,stcp:6,stps:0,stfa:0,stfz:0,ofsf:0,ofsu:0,f2p0:False,fnsp:False,fnfb:False,fsmp:False;n:type:ShaderForge.SFN_Final,id:3138,x:33573,y:32764,varname:node_3138,prsc:2|normal-3867-RGB,emission-5102-OUT,alpha-5074-OUT,clip-4072-OUT;n:type:ShaderForge.SFN_Tex2d,id:3290,x:30849,y:32225,ptovrint:False,ptlb:AlphaMap,ptin:_AlphaMap,varname:node_964,prsc:2,glob:False,taghide:False,taghdr:False,tagprd:False,tagnsco:False,tagnrm:False,tex:a06364b2b83f68e4b8a5abc216ba6d5d,ntxv:0,isnm:False|UVIN-4333-UVOUT;n:type:ShaderForge.SFN_TexCoord,id:1890,x:30085,y:32683,varname:node_1890,prsc:2,uv:0,uaff:True;n:type:ShaderForge.SFN_VertexColor,id:400,x:31620,y:32324,varname:node_400,prsc:2;n:type:ShaderForge.SFN_Multiply,id:2034,x:32218,y:32228,varname:node_2034,prsc:2|A-2028-OUT,B-400-RGB;n:type:ShaderForge.SFN_Panner,id:4333,x:30545,y:32153,varname:node_4333,prsc:2,spu:0.1,spv:0.1|UVIN-8107-OUT;n:type:ShaderForge.SFN_Add,id:4072,x:31459,y:32547,varname:node_4072,prsc:2|A-743-OUT,B-3798-OUT;n:type:ShaderForge.SFN_Multiply,id:8107,x:30379,y:32410,varname:node_8107,prsc:2|A-2446-OUT,B-1890-UVOUT;n:type:ShaderForge.SFN_Slider,id:2446,x:29951,y:32376,ptovrint:False,ptlb:CloudClipSize,ptin:_CloudClipSize,varname:node_5976,prsc:2,glob:False,taghide:False,taghdr:False,tagprd:False,tagnsco:False,tagnrm:False,min:0.1,cur:1,max:3;n:type:ShaderForge.SFN_Power,id:6179,x:32665,y:31734,varname:node_6179,prsc:2;n:type:ShaderForge.SFN_Lerp,id:5102,x:32943,y:32434,varname:node_5102,prsc:2|A-9069-OUT,B-2034-OUT,T-7238-OUT;n:type:ShaderForge.SFN_Slider,id:4244,x:32256,y:32946,ptovrint:False,ptlb:EmissionStrength,ptin:_EmissionStrength,varname:node_4244,prsc:2,glob:False,taghide:False,taghdr:False,tagprd:False,tagnsco:False,tagnrm:False,min:0,cur:0,max:20;n:type:ShaderForge.SFN_Add,id:3798,x:31089,y:32242,varname:node_3798,prsc:2|A-3290-R,B-3290-B;n:type:ShaderForge.SFN_OneMinus,id:2312,x:30458,y:32922,varname:node_2312,prsc:2|IN-1890-Z;n:type:ShaderForge.SFN_RemapRange,id:743,x:30970,y:32586,varname:node_743,prsc:2,frmn:0,frmx:1,tomn:-1,tomx:1|IN-2312-OUT;n:type:ShaderForge.SFN_Tex2d,id:3867,x:30864,y:32014,ptovrint:False,ptlb:NormalMap,ptin:_NormalMap,varname:node_3867,prsc:2,glob:False,taghide:False,taghdr:False,tagprd:False,tagnsco:False,tagnrm:False,tex:008275121ac26434689f1a6fc0781d16,ntxv:3,isnm:True|UVIN-4333-UVOUT;n:type:ShaderForge.SFN_Multiply,id:9069,x:32656,y:32516,varname:node_9069,prsc:2|A-400-RGB,B-4244-OUT;n:type:ShaderForge.SFN_Vector1,id:2028,x:31957,y:32139,varname:node_2028,prsc:2,v1:0.2;n:type:ShaderForge.SFN_Negate,id:367,x:31930,y:31985,varname:node_367,prsc:2|IN-1667-OUT;n:type:ShaderForge.SFN_NormalVector,id:1667,x:31144,y:31705,prsc:2,pt:False;n:type:ShaderForge.SFN_Transform,id:7070,x:32151,y:31981,varname:node_7070,prsc:2,tffrom:1,tfto:0|IN-367-OUT;n:type:ShaderForge.SFN_ComponentMask,id:7238,x:32383,y:31888,varname:node_7238,prsc:2,cc1:1,cc2:-1,cc3:-1,cc4:-1|IN-7070-XYZ;n:type:ShaderForge.SFN_SwitchProperty,id:5074,x:32783,y:33131,ptovrint:False,ptlb:UseParticleAlphaAsOpacity?,ptin:_UseParticleAlphaAsOpacity,varname:node_5074,prsc:2,glob:False,taghide:False,taghdr:False,tagprd:False,tagnsco:False,tagnrm:False,on:False|A-5438-OUT,B-400-A;n:type:ShaderForge.SFN_Vector1,id:5438,x:32510,y:33352,varname:node_5438,prsc:2,v1:1;proporder:3290-2446-4244-3867-5074;pass:END;sub:END;*/

Shader "Shader Forge/StylisedSmokeAlphaOverLife" {
    Properties {
        _AlphaMap ("AlphaMap", 2D) = "white" {}
        _CloudClipSize ("CloudClipSize", Range(0.1, 3)) = 1
        _EmissionStrength ("EmissionStrength", Range(0, 20)) = 0
        _NormalMap ("NormalMap", 2D) = "bump" {}
        [MaterialToggle] _UseParticleAlphaAsOpacity ("UseParticleAlphaAsOpacity?", Float ) = 1
        [HideInInspector]_Cutoff ("Alpha cutoff", Range(0,1)) = 0.5
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
            Blend SrcAlpha OneMinusSrcAlpha
            ZWrite Off
            
            CGPROGRAM
            #pragma vertex vert
            #pragma fragment frag
            #define UNITY_PASS_FORWARDBASE
            #include "UnityCG.cginc"
            #pragma multi_compile_fwdbase
            #pragma only_renderers d3d9 d3d11 glcore gles 
            #pragma target 3.0
            uniform sampler2D _AlphaMap; uniform float4 _AlphaMap_ST;
            uniform float _CloudClipSize;
            uniform float _EmissionStrength;
            uniform sampler2D _NormalMap; uniform float4 _NormalMap_ST;
            uniform fixed _UseParticleAlphaAsOpacity;
            struct VertexInput {
                float4 vertex : POSITION;
                float3 normal : NORMAL;
                float4 tangent : TANGENT;
                float4 texcoord0 : TEXCOORD0;
                float4 vertexColor : COLOR;
            };
            struct VertexOutput {
                float4 pos : SV_POSITION;
                float4 uv0 : TEXCOORD0;
                float3 normalDir : TEXCOORD1;
                float3 tangentDir : TEXCOORD2;
                float3 bitangentDir : TEXCOORD3;
                float4 vertexColor : COLOR;
            };
            VertexOutput vert (VertexInput v) {
                VertexOutput o = (VertexOutput)0;
                o.uv0 = v.texcoord0;
                o.vertexColor = v.vertexColor;
                o.normalDir = UnityObjectToWorldNormal(v.normal);
                o.tangentDir = normalize( mul( unity_ObjectToWorld, float4( v.tangent.xyz, 0.0 ) ).xyz );
                o.bitangentDir = normalize(cross(o.normalDir, o.tangentDir) * v.tangent.w);
                o.pos = UnityObjectToClipPos( v.vertex );
                return o;
            }
            float4 frag(VertexOutput i) : COLOR {
                i.normalDir = normalize(i.normalDir);
                float3x3 tangentTransform = float3x3( i.tangentDir, i.bitangentDir, i.normalDir);
                float4 node_647 = _Time;
                float2 node_4333 = ((_CloudClipSize*i.uv0)+node_647.g*float2(0.1,0.1));
                float3 _NormalMap_var = UnpackNormal(tex2D(_NormalMap,TRANSFORM_TEX(node_4333, _NormalMap)));
                float3 normalLocal = _NormalMap_var.rgb;
                float3 normalDirection = normalize(mul( normalLocal, tangentTransform )); // Perturbed normals
                float4 _AlphaMap_var = tex2D(_AlphaMap,TRANSFORM_TEX(node_4333, _AlphaMap));
                clip((((1.0 - i.uv0.b)*2.0+-1.0)+(_AlphaMap_var.r+_AlphaMap_var.b)) - 0.5);
////// Lighting:
////// Emissive:
                float3 emissive = lerp((i.vertexColor.rgb*_EmissionStrength),(0.2*i.vertexColor.rgb),mul( unity_ObjectToWorld, float4((-1*i.normalDir),0) ).xyz.rgb.g);
                float3 finalColor = emissive;
                return fixed4(finalColor,lerp( 1.0, i.vertexColor.a, _UseParticleAlphaAsOpacity ));
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
            uniform sampler2D _AlphaMap; uniform float4 _AlphaMap_ST;
            uniform float _CloudClipSize;
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
                float4 node_5481 = _Time;
                float2 node_4333 = ((_CloudClipSize*i.uv0)+node_5481.g*float2(0.1,0.1));
                float4 _AlphaMap_var = tex2D(_AlphaMap,TRANSFORM_TEX(node_4333, _AlphaMap));
                clip((((1.0 - i.uv0.b)*2.0+-1.0)+(_AlphaMap_var.r+_AlphaMap_var.b)) - 0.5);
                SHADOW_CASTER_FRAGMENT(i)
            }
            ENDCG
        }
    }
    FallBack "Diffuse"
    CustomEditor "ShaderForgeMaterialInspector"
}
