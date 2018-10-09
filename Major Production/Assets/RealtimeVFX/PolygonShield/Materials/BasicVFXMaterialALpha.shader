// Shader created with Shader Forge v1.38 
// Shader Forge (c) Freya Holmer - http://www.acegikmo.com/shaderforge/
// Note: Manually altering this data may prevent you from opening it in Shader Forge
/*SF_DATA;ver:1.38;sub:START;pass:START;ps:flbk:,iptp:0,cusa:False,bamd:0,cgin:,lico:1,lgpr:1,limd:0,spmd:1,trmd:0,grmd:0,uamb:True,mssp:True,bkdf:False,hqlp:False,rprd:False,enco:False,rmgx:True,imps:True,rpth:0,vtps:0,hqsc:True,nrmq:1,nrsp:0,vomd:0,spxs:False,tesm:0,olmd:1,culm:0,bsrc:3,bdst:7,dpts:2,wrdp:False,dith:0,atcv:False,rfrpo:True,rfrpn:Refraction,coma:15,ufog:False,aust:True,igpj:True,qofs:0,qpre:3,rntp:2,fgom:False,fgoc:False,fgod:False,fgor:False,fgmd:0,fgcr:0.5,fgcg:0.5,fgcb:0.5,fgca:1,fgde:0.01,fgrn:0,fgrf:300,stcl:False,atwp:False,stva:128,stmr:255,stmw:255,stcp:6,stps:0,stfa:0,stfz:0,ofsf:0,ofsu:0,f2p0:False,fnsp:False,fnfb:False,fsmp:False;n:type:ShaderForge.SFN_Final,id:3138,x:33681,y:32681,varname:node_3138,prsc:2|emission-9208-OUT,alpha-9796-OUT,refract-3707-OUT;n:type:ShaderForge.SFN_Tex2d,id:7247,x:32090,y:32624,ptovrint:False,ptlb:Shape,ptin:_Shape,varname:node_7247,prsc:2,glob:False,taghide:False,taghdr:False,tagprd:False,tagnsco:False,tagnrm:False,tex:cf582946fccf1074d84f2fc5a11d0ffa,ntxv:0,isnm:False;n:type:ShaderForge.SFN_Tex2d,id:6374,x:32090,y:32825,ptovrint:False,ptlb:Alpha,ptin:_Alpha,varname:node_6374,prsc:2,glob:False,taghide:False,taghdr:False,tagprd:False,tagnsco:False,tagnrm:False,tex:cf582946fccf1074d84f2fc5a11d0ffa,ntxv:0,isnm:False;n:type:ShaderForge.SFN_VertexColor,id:6593,x:32272,y:32450,varname:node_6593,prsc:2;n:type:ShaderForge.SFN_Multiply,id:9342,x:32650,y:32777,varname:node_9342,prsc:2|A-6593-A,B-6374-RGB;n:type:ShaderForge.SFN_Fresnel,id:4783,x:32075,y:33262,varname:node_4783,prsc:2|EXP-1014-OUT;n:type:ShaderForge.SFN_Add,id:5463,x:32888,y:32585,varname:node_5463,prsc:2|A-4783-OUT,B-6593-RGB;n:type:ShaderForge.SFN_Slider,id:1014,x:31714,y:33286,ptovrint:False,ptlb:FresnelPosition,ptin:_FresnelPosition,varname:node_1014,prsc:2,glob:False,taghide:False,taghdr:False,tagprd:False,tagnsco:False,tagnrm:False,min:0,cur:2,max:5;n:type:ShaderForge.SFN_SwitchProperty,id:9796,x:32856,y:32925,ptovrint:False,ptlb:UseFresnelAsOpacity?,ptin:_UseFresnelAsOpacity,varname:node_9796,prsc:2,glob:False,taghide:False,taghdr:False,tagprd:False,tagnsco:False,tagnrm:False,on:False|A-9342-OUT,B-7481-OUT;n:type:ShaderForge.SFN_Multiply,id:7481,x:32628,y:33120,varname:node_7481,prsc:2|A-6593-A,B-4783-OUT;n:type:ShaderForge.SFN_Multiply,id:8870,x:32834,y:33441,varname:node_8870,prsc:2|A-6418-OUT,B-413-OUT;n:type:ShaderForge.SFN_Vector1,id:413,x:32311,y:33558,varname:node_413,prsc:2,v1:2;n:type:ShaderForge.SFN_SwitchProperty,id:3707,x:33426,y:33244,ptovrint:False,ptlb:UseRefraction?,ptin:_UseRefraction,varname:node_3707,prsc:2,glob:False,taghide:False,taghdr:False,tagprd:False,tagnsco:False,tagnrm:False,on:False|A-7940-OUT,B-6879-OUT;n:type:ShaderForge.SFN_OneMinus,id:8849,x:32434,y:33284,varname:node_8849,prsc:2|IN-4783-OUT;n:type:ShaderForge.SFN_Power,id:6418,x:32597,y:33308,varname:node_6418,prsc:2|VAL-8849-OUT,EXP-999-OUT;n:type:ShaderForge.SFN_Vector1,id:999,x:32370,y:33426,varname:node_999,prsc:2,v1:10;n:type:ShaderForge.SFN_Lerp,id:5944,x:33002,y:33332,varname:node_5944,prsc:2|A-6067-OUT,B-6171-OUT,T-8870-OUT;n:type:ShaderForge.SFN_Vector1,id:6067,x:32736,y:33279,varname:node_6067,prsc:2,v1:1;n:type:ShaderForge.SFN_Vector1,id:6171,x:32736,y:33342,varname:node_6171,prsc:2,v1:0;n:type:ShaderForge.SFN_Append,id:6879,x:33114,y:33451,varname:node_6879,prsc:2|A-8870-OUT,B-5944-OUT;n:type:ShaderForge.SFN_Multiply,id:9208,x:33404,y:32541,varname:node_9208,prsc:2|A-7926-OUT,B-5463-OUT;n:type:ShaderForge.SFN_Slider,id:7926,x:32995,y:32457,ptovrint:False,ptlb:EmissionBoost,ptin:_EmissionBoost,varname:node_7926,prsc:2,glob:False,taghide:False,taghdr:False,tagprd:False,tagnsco:False,tagnrm:False,min:0,cur:1,max:20;n:type:ShaderForge.SFN_Slider,id:6977,x:32588,y:33808,ptovrint:False,ptlb:RefractionTester,ptin:_RefractionTester,varname:node_6977,prsc:2,glob:False,taghide:False,taghdr:False,tagprd:False,tagnsco:False,tagnrm:False,min:0,cur:0.07114717,max:1;n:type:ShaderForge.SFN_Tex2d,id:1532,x:32613,y:33625,ptovrint:False,ptlb:node_1532,ptin:_node_1532,varname:node_1532,prsc:2,glob:False,taghide:False,taghdr:False,tagprd:False,tagnsco:False,tagnrm:False,tex:a06364b2b83f68e4b8a5abc216ba6d5d,ntxv:0,isnm:False|UVIN-7298-UVOUT;n:type:ShaderForge.SFN_Multiply,id:5496,x:33137,y:33758,varname:node_5496,prsc:2|A-6977-OUT,B-8860-OUT;n:type:ShaderForge.SFN_ComponentMask,id:4525,x:32806,y:33606,varname:node_4525,prsc:2,cc1:0,cc2:1,cc3:-1,cc4:-1|IN-1532-RGB;n:type:ShaderForge.SFN_Vector1,id:8860,x:32729,y:33888,varname:node_8860,prsc:2,v1:0.2;n:type:ShaderForge.SFN_Multiply,id:5794,x:33315,y:33674,varname:node_5794,prsc:2|A-4525-OUT,B-5496-OUT;n:type:ShaderForge.SFN_Multiply,id:7940,x:33525,y:33541,varname:node_7940,prsc:2|A-5794-OUT,B-5944-OUT;n:type:ShaderForge.SFN_Panner,id:7298,x:32400,y:33684,varname:node_7298,prsc:2,spu:1,spv:1|UVIN-6388-UVOUT;n:type:ShaderForge.SFN_TexCoord,id:6388,x:32239,y:33694,varname:node_6388,prsc:2,uv:0,uaff:False;proporder:7247-6374-1014-9796-3707-7926-6977-1532;pass:END;sub:END;*/

Shader "Shader Forge/BasicVFXMaterialALpha" {
    Properties {
        _Shape ("Shape", 2D) = "white" {}
        _Alpha ("Alpha", 2D) = "white" {}
        _FresnelPosition ("FresnelPosition", Range(0, 5)) = 2
        [MaterialToggle] _UseFresnelAsOpacity ("UseFresnelAsOpacity?", Float ) = 0
        [MaterialToggle] _UseRefraction ("UseRefraction?", Float ) = 0
        _EmissionBoost ("EmissionBoost", Range(0, 20)) = 1
        _RefractionTester ("RefractionTester", Range(0, 1)) = 0.07114717
        _node_1532 ("node_1532", 2D) = "white" {}
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
            #include "UnityCG.cginc"
            #pragma multi_compile_fwdbase
            #pragma only_renderers d3d9 d3d11 glcore gles 
            #pragma target 3.0
            uniform sampler2D _GrabTexture;
            uniform sampler2D _Alpha; uniform float4 _Alpha_ST;
            uniform float _FresnelPosition;
            uniform fixed _UseFresnelAsOpacity;
            uniform fixed _UseRefraction;
            uniform float _EmissionBoost;
            uniform float _RefractionTester;
            uniform sampler2D _node_1532; uniform float4 _node_1532_ST;
            struct VertexInput {
                float4 vertex : POSITION;
                float3 normal : NORMAL;
                float2 texcoord0 : TEXCOORD0;
                float4 vertexColor : COLOR;
            };
            struct VertexOutput {
                float4 pos : SV_POSITION;
                float2 uv0 : TEXCOORD0;
                float4 posWorld : TEXCOORD1;
                float3 normalDir : TEXCOORD2;
                float4 vertexColor : COLOR;
                float4 projPos : TEXCOORD3;
            };
            VertexOutput vert (VertexInput v) {
                VertexOutput o = (VertexOutput)0;
                o.uv0 = v.texcoord0;
                o.vertexColor = v.vertexColor;
                o.normalDir = UnityObjectToWorldNormal(v.normal);
                o.posWorld = mul(unity_ObjectToWorld, v.vertex);
                o.pos = UnityObjectToClipPos( v.vertex );
                o.projPos = ComputeScreenPos (o.pos);
                COMPUTE_EYEDEPTH(o.projPos.z);
                return o;
            }
            float4 frag(VertexOutput i) : COLOR {
                i.normalDir = normalize(i.normalDir);
                float3 viewDirection = normalize(_WorldSpaceCameraPos.xyz - i.posWorld.xyz);
                float3 normalDirection = i.normalDir;
                float4 node_4360 = _Time;
                float2 node_7298 = (i.uv0+node_4360.g*float2(1,1));
                float4 _node_1532_var = tex2D(_node_1532,TRANSFORM_TEX(node_7298, _node_1532));
                float node_4783 = pow(1.0-max(0,dot(normalDirection, viewDirection)),_FresnelPosition);
                float node_8870 = (pow((1.0 - node_4783),10.0)*2.0);
                float node_5944 = lerp(1.0,0.0,node_8870);
                float2 sceneUVs = (i.projPos.xy / i.projPos.w) + lerp( ((_node_1532_var.rgb.rg*(_RefractionTester*0.2))*node_5944), float2(node_8870,node_5944), _UseRefraction );
                float4 sceneColor = tex2D(_GrabTexture, sceneUVs);
////// Lighting:
////// Emissive:
                float3 emissive = (_EmissionBoost*(node_4783+i.vertexColor.rgb));
                float3 finalColor = emissive;
                float4 _Alpha_var = tex2D(_Alpha,TRANSFORM_TEX(i.uv0, _Alpha));
                return fixed4(lerp(sceneColor.rgb, finalColor,lerp( (i.vertexColor.a*_Alpha_var.rgb), (i.vertexColor.a*node_4783), _UseFresnelAsOpacity )),1);
            }
            ENDCG
        }
    }
    FallBack "Diffuse"
    CustomEditor "ShaderForgeMaterialInspector"
}
