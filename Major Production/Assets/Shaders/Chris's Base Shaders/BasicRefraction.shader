// Shader created with Shader Forge v1.38 
// Shader Forge (c) Freya Holmer - http://www.acegikmo.com/shaderforge/
// Note: Manually altering this data may prevent you from opening it in Shader Forge
/*SF_DATA;ver:1.38;sub:START;pass:START;ps:flbk:,iptp:0,cusa:False,bamd:0,cgin:,lico:1,lgpr:1,limd:0,spmd:1,trmd:0,grmd:0,uamb:True,mssp:True,bkdf:True,hqlp:False,rprd:True,enco:False,rmgx:True,imps:True,rpth:0,vtps:0,hqsc:True,nrmq:1,nrsp:0,vomd:0,spxs:False,tesm:0,olmd:1,culm:0,bsrc:3,bdst:7,dpts:2,wrdp:False,dith:0,atcv:False,rfrpo:True,rfrpn:Refraction,coma:15,ufog:True,aust:True,igpj:True,qofs:0,qpre:3,rntp:2,fgom:False,fgoc:False,fgod:False,fgor:False,fgmd:0,fgcr:0.5,fgcg:0.5,fgcb:0.5,fgca:1,fgde:0.01,fgrn:0,fgrf:300,stcl:False,atwp:False,stva:128,stmr:255,stmw:255,stcp:6,stps:0,stfa:0,stfz:0,ofsf:0,ofsu:0,f2p0:False,fnsp:False,fnfb:False,fsmp:False;n:type:ShaderForge.SFN_Final,id:2865,x:32719,y:32712,varname:node_2865,prsc:2|alpha-1912-OUT,refract-3456-OUT;n:type:ShaderForge.SFN_Fresnel,id:7835,x:32328,y:33570,varname:node_7835,prsc:2|EXP-6283-OUT;n:type:ShaderForge.SFN_Slider,id:968,x:31574,y:33385,ptovrint:False,ptlb:RefractionIntensity,ptin:_RefractionIntensity,varname:_Exponent,prsc:2,glob:False,taghide:False,taghdr:False,tagprd:False,tagnsco:False,tagnrm:False,min:0,cur:0.1,max:5;n:type:ShaderForge.SFN_Slider,id:1912,x:32029,y:32754,ptovrint:False,ptlb:Opacity,ptin:_Opacity,varname:node_1912,prsc:2,glob:False,taghide:False,taghdr:False,tagprd:False,tagnsco:False,tagnrm:False,min:0,cur:0,max:1;n:type:ShaderForge.SFN_TexCoord,id:4670,x:31338,y:32907,varname:node_4670,prsc:2,uv:0,uaff:False;n:type:ShaderForge.SFN_Tex2d,id:3679,x:31605,y:32909,ptovrint:False,ptlb:RefractionNormalMap,ptin:_RefractionNormalMap,varname:node_3679,prsc:2,glob:False,taghide:False,taghdr:False,tagprd:False,tagnsco:False,tagnrm:False,tex:008275121ac26434689f1a6fc0781d16,ntxv:3,isnm:True|UVIN-4670-UVOUT;n:type:ShaderForge.SFN_ComponentMask,id:3990,x:31884,y:32929,varname:node_3990,prsc:2,cc1:0,cc2:1,cc3:-1,cc4:-1|IN-3679-RGB;n:type:ShaderForge.SFN_Vector1,id:4619,x:31718,y:33495,varname:node_4619,prsc:2,v1:0.2;n:type:ShaderForge.SFN_Multiply,id:8247,x:31941,y:33432,varname:node_8247,prsc:2|A-968-OUT,B-4619-OUT;n:type:ShaderForge.SFN_Slider,id:6283,x:31873,y:33644,ptovrint:False,ptlb:FresnelPosition,ptin:_FresnelPosition,varname:node_6283,prsc:2,glob:False,taghide:False,taghdr:False,tagprd:False,tagnsco:False,tagnrm:False,min:0.1,cur:1.188804,max:5;n:type:ShaderForge.SFN_Multiply,id:9532,x:32141,y:33356,varname:node_9532,prsc:2|A-3990-OUT,B-8247-OUT;n:type:ShaderForge.SFN_Lerp,id:3456,x:32468,y:33231,varname:node_3456,prsc:2|A-5521-OUT,B-9532-OUT,T-7835-OUT;n:type:ShaderForge.SFN_Vector1,id:5521,x:32141,y:33194,varname:node_5521,prsc:2,v1:0;proporder:968-1912-3679-6283;pass:END;sub:END;*/

Shader "Shader Forge/Glass" {
    Properties {
        _RefractionIntensity ("RefractionIntensity", Range(0, 5)) = 0.1
        _Opacity ("Opacity", Range(0, 1)) = 0
        _RefractionNormalMap ("RefractionNormalMap", 2D) = "bump" {}
        _FresnelPosition ("FresnelPosition", Range(0.1, 5)) = 1.188804
        [HideInInspector]_Cutoff ("Alpha cutoff", Range(0,1)) = 0.5
    }
    SubShader {
        Tags {
            "IgnoreProjector"="True"
            "Queue"="Transparent"
            "RenderType"="Transparent"
        }
        GrabPass{ }
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
            #define _GLOSSYENV 1
            #include "UnityCG.cginc"
            #include "UnityPBSLighting.cginc"
            #include "UnityStandardBRDF.cginc"
            #pragma multi_compile_fwdbase
            #pragma multi_compile_fog
            #pragma only_renderers d3d9 d3d11 glcore gles 
            #pragma target 3.0
            uniform sampler2D _GrabTexture;
            uniform float _RefractionIntensity;
            uniform float _Opacity;
            uniform sampler2D _RefractionNormalMap; uniform float4 _RefractionNormalMap_ST;
            uniform float _FresnelPosition;
            struct VertexInput {
                float4 vertex : POSITION;
                float3 normal : NORMAL;
                float2 texcoord0 : TEXCOORD0;
            };
            struct VertexOutput {
                float4 pos : SV_POSITION;
                float2 uv0 : TEXCOORD0;
                float4 posWorld : TEXCOORD1;
                float3 normalDir : TEXCOORD2;
                float4 projPos : TEXCOORD3;
                UNITY_FOG_COORDS(4)
            };
            VertexOutput vert (VertexInput v) {
                VertexOutput o = (VertexOutput)0;
                o.uv0 = v.texcoord0;
                o.normalDir = UnityObjectToWorldNormal(v.normal);
                o.posWorld = mul(unity_ObjectToWorld, v.vertex);
                o.pos = UnityObjectToClipPos( v.vertex );
                UNITY_TRANSFER_FOG(o,o.pos);
                o.projPos = ComputeScreenPos (o.pos);
                COMPUTE_EYEDEPTH(o.projPos.z);
                return o;
            }
            float4 frag(VertexOutput i) : COLOR {
                i.normalDir = normalize(i.normalDir);
                float3 viewDirection = normalize(_WorldSpaceCameraPos.xyz - i.posWorld.xyz);
                float3 normalDirection = i.normalDir;
                float node_5521 = 0.0;
                float3 _RefractionNormalMap_var = UnpackNormal(tex2D(_RefractionNormalMap,TRANSFORM_TEX(i.uv0, _RefractionNormalMap)));
                float node_7835 = pow(1.0-max(0,dot(normalDirection, viewDirection)),_FresnelPosition);
                float2 sceneUVs = (i.projPos.xy / i.projPos.w) + lerp(float2(node_5521,node_5521),(_RefractionNormalMap_var.rgb.rg*(_RefractionIntensity*0.2)),node_7835);
                float4 sceneColor = tex2D(_GrabTexture, sceneUVs);
////// Lighting:
                float3 finalColor = 0;
                fixed4 finalRGBA = fixed4(lerp(sceneColor.rgb, finalColor,_Opacity),1);
                UNITY_APPLY_FOG(i.fogCoord, finalRGBA);
                return finalRGBA;
            }
            ENDCG
        }
    }
    FallBack "Diffuse"
    CustomEditor "ShaderForgeMaterialInspector"
}
