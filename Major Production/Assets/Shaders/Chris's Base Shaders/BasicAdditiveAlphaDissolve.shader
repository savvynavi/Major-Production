// Shader created with Shader Forge v1.38 
// Shader Forge (c) Freya Holmer - http://www.acegikmo.com/shaderforge/
// Note: Manually altering this data may prevent you from opening it in Shader Forge
/*SF_DATA;ver:1.38;sub:START;pass:START;ps:flbk:,iptp:0,cusa:False,bamd:0,cgin:,lico:1,lgpr:1,limd:0,spmd:1,trmd:0,grmd:0,uamb:True,mssp:True,bkdf:False,hqlp:False,rprd:False,enco:False,rmgx:True,imps:True,rpth:0,vtps:0,hqsc:True,nrmq:1,nrsp:0,vomd:0,spxs:False,tesm:0,olmd:1,culm:0,bsrc:0,bdst:0,dpts:2,wrdp:False,dith:0,atcv:False,rfrpo:True,rfrpn:Refraction,coma:15,ufog:True,aust:True,igpj:True,qofs:0,qpre:3,rntp:2,fgom:False,fgoc:True,fgod:False,fgor:False,fgmd:0,fgcr:0.5,fgcg:0.5,fgcb:0.5,fgca:1,fgde:0.01,fgrn:0,fgrf:300,stcl:False,atwp:False,stva:128,stmr:255,stmw:255,stcp:6,stps:0,stfa:0,stfz:0,ofsf:0,ofsu:0,f2p0:False,fnsp:True,fnfb:True,fsmp:False;n:type:ShaderForge.SFN_Final,id:4795,x:33462,y:32730,varname:node_4795,prsc:2|emission-351-OUT;n:type:ShaderForge.SFN_Tex2d,id:6074,x:32235,y:32601,ptovrint:False,ptlb:MainTex,ptin:_MainTex,varname:_MainTex,prsc:2,glob:False,taghide:False,taghdr:False,tagprd:False,tagnsco:False,tagnrm:False,tex:250dc532ec5737240b3409958bc34102,ntxv:3,isnm:False;n:type:ShaderForge.SFN_Multiply,id:2393,x:32670,y:32618,varname:node_2393,prsc:2|A-6074-R,B-2053-RGB;n:type:ShaderForge.SFN_VertexColor,id:2053,x:32235,y:32772,varname:node_2053,prsc:2;n:type:ShaderForge.SFN_Tex2d,id:640,x:32251,y:32928,ptovrint:False,ptlb:Dissolve Texture,ptin:_DissolveTexture,varname:node_640,prsc:2,glob:False,taghide:False,taghdr:False,tagprd:False,tagnsco:False,tagnrm:False,tex:729ed3f96a11bb24bba9e00ded5d28fc,ntxv:0,isnm:False|UVIN-4964-UVOUT;n:type:ShaderForge.SFN_Panner,id:4964,x:31897,y:32392,varname:node_4964,prsc:2,spu:1,spv:1|UVIN-1435-UVOUT;n:type:ShaderForge.SFN_TexCoord,id:1435,x:31687,y:32392,varname:node_1435,prsc:2,uv:0,uaff:False;n:type:ShaderForge.SFN_Multiply,id:4290,x:32901,y:32678,varname:node_4290,prsc:2|A-2053-A,B-2393-OUT;n:type:ShaderForge.SFN_Add,id:1959,x:32794,y:33033,varname:node_1959,prsc:2|A-1512-OUT,B-640-R;n:type:ShaderForge.SFN_RemapRange,id:1512,x:32586,y:32897,varname:node_1512,prsc:2,frmn:0,frmx:1,tomn:-2,tomx:1|IN-2053-A;n:type:ShaderForge.SFN_Multiply,id:351,x:33103,y:32919,varname:node_351,prsc:2|A-1959-OUT,B-4290-OUT;n:type:ShaderForge.SFN_Lerp,id:8271,x:33275,y:32677,varname:node_8271,prsc:2|A-351-OUT,B-4290-OUT,T-2053-A;proporder:6074-640;pass:END;sub:END;*/

Shader "Shader Forge/BasicAdditiveAlphaDissolve" {
    Properties {
        _MainTex ("MainTex", 2D) = "bump" {}
        _DissolveTexture ("Dissolve Texture", 2D) = "white" {}
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
            uniform sampler2D _MainTex; uniform float4 _MainTex_ST;
            uniform sampler2D _DissolveTexture; uniform float4 _DissolveTexture_ST;
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
                float4 node_3194 = _Time;
                float2 node_4964 = (i.uv0+node_3194.g*float2(1,1));
                float4 _DissolveTexture_var = tex2D(_DissolveTexture,TRANSFORM_TEX(node_4964, _DissolveTexture));
                float4 _MainTex_var = tex2D(_MainTex,TRANSFORM_TEX(i.uv0, _MainTex));
                float3 node_4290 = (i.vertexColor.a*(_MainTex_var.r*i.vertexColor.rgb));
                float3 node_351 = (((i.vertexColor.a*3.0+-2.0)+_DissolveTexture_var.r)*node_4290);
                float3 emissive = node_351;
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
