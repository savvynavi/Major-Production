// Shader created with Shader Forge v1.38 
// Shader Forge (c) Freya Holmer - http://www.acegikmo.com/shaderforge/
// Note: Manually altering this data may prevent you from opening it in Shader Forge
/*SF_DATA;ver:1.38;sub:START;pass:START;ps:flbk:,iptp:0,cusa:False,bamd:0,cgin:,lico:1,lgpr:1,limd:0,spmd:1,trmd:0,grmd:0,uamb:True,mssp:True,bkdf:False,hqlp:False,rprd:False,enco:False,rmgx:True,imps:True,rpth:0,vtps:0,hqsc:True,nrmq:1,nrsp:0,vomd:0,spxs:False,tesm:0,olmd:1,culm:2,bsrc:3,bdst:7,dpts:2,wrdp:False,dith:0,atcv:False,rfrpo:True,rfrpn:Refraction,coma:15,ufog:False,aust:True,igpj:True,qofs:0,qpre:3,rntp:2,fgom:False,fgoc:False,fgod:False,fgor:False,fgmd:0,fgcr:0.5,fgcg:0.5,fgcb:0.5,fgca:1,fgde:0.01,fgrn:0,fgrf:300,stcl:False,atwp:False,stva:128,stmr:255,stmw:255,stcp:6,stps:0,stfa:0,stfz:0,ofsf:0,ofsu:0,f2p0:False,fnsp:False,fnfb:False,fsmp:False;n:type:ShaderForge.SFN_Final,id:3138,x:33526,y:32603,varname:node_3138,prsc:2|emission-2821-OUT,alpha-956-OUT,voffset-1496-OUT;n:type:ShaderForge.SFN_VertexColor,id:8675,x:32115,y:32831,varname:node_8675,prsc:2;n:type:ShaderForge.SFN_TexCoord,id:7147,x:31822,y:32312,varname:node_7147,prsc:2,uv:2,uaff:False;n:type:ShaderForge.SFN_Tex2d,id:3897,x:32217,y:32532,ptovrint:False,ptlb:node_3897,ptin:_node_3897,varname:node_3897,prsc:2,glob:False,taghide:False,taghdr:False,tagprd:False,tagnsco:False,tagnrm:False,tex:4a8ccc660c311734a86a8c20d926dfc4,ntxv:0,isnm:False|UVIN-7147-UVOUT;n:type:ShaderForge.SFN_Add,id:4392,x:32748,y:32469,varname:node_4392,prsc:2|A-128-OUT,B-8675-RGB;n:type:ShaderForge.SFN_Vector1,id:6527,x:32018,y:33107,varname:node_6527,prsc:2,v1:0.2;n:type:ShaderForge.SFN_Multiply,id:8063,x:33024,y:32490,varname:node_8063,prsc:2|A-493-OUT,B-4392-OUT;n:type:ShaderForge.SFN_Vector1,id:493,x:32791,y:32358,varname:node_493,prsc:2,v1:2;n:type:ShaderForge.SFN_FaceSign,id:8765,x:32782,y:33036,varname:node_8765,prsc:2,fstp:0;n:type:ShaderForge.SFN_Lerp,id:2821,x:33263,y:32666,varname:node_2821,prsc:2|A-8063-OUT,B-6748-OUT,T-442-OUT;n:type:ShaderForge.SFN_OneMinus,id:442,x:33090,y:32944,varname:node_442,prsc:2|IN-8765-VFACE;n:type:ShaderForge.SFN_NormalVector,id:4497,x:31583,y:33340,prsc:2,pt:False;n:type:ShaderForge.SFN_Multiply,id:9944,x:31995,y:33407,varname:node_9944,prsc:2|A-4497-OUT,B-431-OUT;n:type:ShaderForge.SFN_Vector1,id:5611,x:31539,y:33623,varname:node_5611,prsc:2,v1:-0.5;n:type:ShaderForge.SFN_Multiply,id:6748,x:32782,y:32703,varname:node_6748,prsc:2|A-8675-RGB,B-3441-OUT;n:type:ShaderForge.SFN_Time,id:6252,x:31503,y:32160,varname:node_6252,prsc:2;n:type:ShaderForge.SFN_Sin,id:5090,x:32113,y:32225,varname:node_5090,prsc:2|IN-5568-OUT;n:type:ShaderForge.SFN_Multiply,id:128,x:32454,y:32258,varname:node_128,prsc:2|A-142-OUT,B-3897-RGB;n:type:ShaderForge.SFN_RemapRange,id:142,x:32281,y:32084,varname:node_142,prsc:2,frmn:0,frmx:1,tomn:0.65,tomx:1|IN-5090-OUT;n:type:ShaderForge.SFN_Multiply,id:1581,x:32334,y:33337,varname:node_1581,prsc:2|A-8675-A,B-9944-OUT,C-820-OUT;n:type:ShaderForge.SFN_Multiply,id:956,x:32733,y:32875,varname:node_956,prsc:2|A-8675-A,B-6527-OUT;n:type:ShaderForge.SFN_Vector1,id:3441,x:32476,y:32803,varname:node_3441,prsc:2,v1:1.1;n:type:ShaderForge.SFN_Multiply,id:5568,x:31884,y:32138,varname:node_5568,prsc:2|A-8338-OUT,B-6252-TTR;n:type:ShaderForge.SFN_Vector1,id:8338,x:31653,y:32096,varname:node_8338,prsc:2,v1:3;n:type:ShaderForge.SFN_Multiply,id:820,x:31847,y:32747,varname:node_820,prsc:2|A-3178-OUT,B-277-OUT;n:type:ShaderForge.SFN_TexCoord,id:6509,x:31233,y:32810,varname:node_6509,prsc:2,uv:1,uaff:False;n:type:ShaderForge.SFN_RemapRange,id:3178,x:31430,y:32568,varname:node_3178,prsc:2,frmn:0,frmx:1,tomn:0.95,tomx:1|IN-5090-OUT;n:type:ShaderForge.SFN_Rotator,id:8737,x:31467,y:32881,varname:node_8737,prsc:2|UVIN-6509-UVOUT,SPD-1108-OUT;n:type:ShaderForge.SFN_ComponentMask,id:277,x:31624,y:32881,varname:node_277,prsc:2,cc1:1,cc2:-1,cc3:-1,cc4:-1|IN-8737-UVOUT;n:type:ShaderForge.SFN_Lerp,id:1496,x:32929,y:33346,varname:node_1496,prsc:2|A-9259-OUT,B-1581-OUT,T-8675-A;n:type:ShaderForge.SFN_Vector1,id:9259,x:32828,y:33612,varname:node_9259,prsc:2,v1:0;n:type:ShaderForge.SFN_Lerp,id:431,x:31810,y:33582,varname:node_431,prsc:2|A-5611-OUT,B-5705-OUT,T-8675-A;n:type:ShaderForge.SFN_Vector1,id:5705,x:31552,y:33709,varname:node_5705,prsc:2,v1:0.55;n:type:ShaderForge.SFN_Sin,id:1108,x:31082,y:32957,varname:node_1108,prsc:2|IN-5304-OUT;n:type:ShaderForge.SFN_Multiply,id:5304,x:30826,y:32797,varname:node_5304,prsc:2|A-6252-TSL,B-9193-OUT,C-8675-A;n:type:ShaderForge.SFN_Slider,id:9193,x:30481,y:32949,ptovrint:False,ptlb:SpeedOfHexFlutter,ptin:_SpeedOfHexFlutter,varname:node_9193,prsc:2,glob:False,taghide:False,taghdr:False,tagprd:False,tagnsco:False,tagnrm:False,min:0,cur:0.1,max:1;proporder:3897-9193;pass:END;sub:END;*/

Shader "Shader Forge/PolygonShield" {
    Properties {
        _node_3897 ("node_3897", 2D) = "white" {}
        _SpeedOfHexFlutter ("SpeedOfHexFlutter", Range(0, 1)) = 0.1
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
            Cull Off
            ZWrite Off
            
            CGPROGRAM
            #pragma vertex vert
            #pragma fragment frag
            #define UNITY_PASS_FORWARDBASE
            #include "UnityCG.cginc"
            #pragma multi_compile_fwdbase
            #pragma only_renderers d3d9 d3d11 glcore gles 
            #pragma target 3.0
            uniform sampler2D _node_3897; uniform float4 _node_3897_ST;
            uniform float _SpeedOfHexFlutter;
            struct VertexInput {
                float4 vertex : POSITION;
                float3 normal : NORMAL;
                float2 texcoord1 : TEXCOORD1;
                float2 texcoord2 : TEXCOORD2;
                float4 vertexColor : COLOR;
            };
            struct VertexOutput {
                float4 pos : SV_POSITION;
                float2 uv1 : TEXCOORD0;
                float2 uv2 : TEXCOORD1;
                float4 posWorld : TEXCOORD2;
                float3 normalDir : TEXCOORD3;
                float4 vertexColor : COLOR;
            };
            VertexOutput vert (VertexInput v) {
                VertexOutput o = (VertexOutput)0;
                o.uv1 = v.texcoord1;
                o.uv2 = v.texcoord2;
                o.vertexColor = v.vertexColor;
                o.normalDir = UnityObjectToWorldNormal(v.normal);
                float node_9259 = 0.0;
                float4 node_6252 = _Time;
                float node_5090 = sin((3.0*node_6252.a));
                float4 node_324 = _Time;
                float node_8737_ang = node_324.g;
                float node_8737_spd = sin((node_6252.r*_SpeedOfHexFlutter*o.vertexColor.a));
                float node_8737_cos = cos(node_8737_spd*node_8737_ang);
                float node_8737_sin = sin(node_8737_spd*node_8737_ang);
                float2 node_8737_piv = float2(0.5,0.5);
                float2 node_8737 = (mul(o.uv1-node_8737_piv,float2x2( node_8737_cos, -node_8737_sin, node_8737_sin, node_8737_cos))+node_8737_piv);
                v.vertex.xyz += lerp(float3(node_9259,node_9259,node_9259),(o.vertexColor.a*(v.normal*lerp((-0.5),0.55,o.vertexColor.a))*((node_5090*0.05000001+0.95)*node_8737.g)),o.vertexColor.a);
                o.posWorld = mul(unity_ObjectToWorld, v.vertex);
                o.pos = UnityObjectToClipPos( v.vertex );
                return o;
            }
            float4 frag(VertexOutput i, float facing : VFACE) : COLOR {
                float isFrontFace = ( facing >= 0 ? 1 : 0 );
                float faceSign = ( facing >= 0 ? 1 : -1 );
                i.normalDir = normalize(i.normalDir);
                i.normalDir *= faceSign;
                float3 viewDirection = normalize(_WorldSpaceCameraPos.xyz - i.posWorld.xyz);
                float3 normalDirection = i.normalDir;
////// Lighting:
////// Emissive:
                float4 node_6252 = _Time;
                float node_5090 = sin((3.0*node_6252.a));
                float4 _node_3897_var = tex2D(_node_3897,TRANSFORM_TEX(i.uv2, _node_3897));
                float3 emissive = lerp((2.0*(((node_5090*0.35+0.65)*_node_3897_var.rgb)+i.vertexColor.rgb)),(i.vertexColor.rgb*1.1),(1.0 - isFrontFace));
                float3 finalColor = emissive;
                return fixed4(finalColor,(i.vertexColor.a*0.2));
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
            #pragma vertex vert
            #pragma fragment frag
            #define UNITY_PASS_SHADOWCASTER
            #include "UnityCG.cginc"
            #include "Lighting.cginc"
            #pragma fragmentoption ARB_precision_hint_fastest
            #pragma multi_compile_shadowcaster
            #pragma only_renderers d3d9 d3d11 glcore gles 
            #pragma target 3.0
            uniform float _SpeedOfHexFlutter;
            struct VertexInput {
                float4 vertex : POSITION;
                float3 normal : NORMAL;
                float2 texcoord1 : TEXCOORD1;
                float4 vertexColor : COLOR;
            };
            struct VertexOutput {
                V2F_SHADOW_CASTER;
                float2 uv1 : TEXCOORD1;
                float4 posWorld : TEXCOORD2;
                float3 normalDir : TEXCOORD3;
                float4 vertexColor : COLOR;
            };
            VertexOutput vert (VertexInput v) {
                VertexOutput o = (VertexOutput)0;
                o.uv1 = v.texcoord1;
                o.vertexColor = v.vertexColor;
                o.normalDir = UnityObjectToWorldNormal(v.normal);
                float node_9259 = 0.0;
                float4 node_6252 = _Time;
                float node_5090 = sin((3.0*node_6252.a));
                float4 node_2368 = _Time;
                float node_8737_ang = node_2368.g;
                float node_8737_spd = sin((node_6252.r*_SpeedOfHexFlutter*o.vertexColor.a));
                float node_8737_cos = cos(node_8737_spd*node_8737_ang);
                float node_8737_sin = sin(node_8737_spd*node_8737_ang);
                float2 node_8737_piv = float2(0.5,0.5);
                float2 node_8737 = (mul(o.uv1-node_8737_piv,float2x2( node_8737_cos, -node_8737_sin, node_8737_sin, node_8737_cos))+node_8737_piv);
                v.vertex.xyz += lerp(float3(node_9259,node_9259,node_9259),(o.vertexColor.a*(v.normal*lerp((-0.5),0.55,o.vertexColor.a))*((node_5090*0.05000001+0.95)*node_8737.g)),o.vertexColor.a);
                o.posWorld = mul(unity_ObjectToWorld, v.vertex);
                o.pos = UnityObjectToClipPos( v.vertex );
                TRANSFER_SHADOW_CASTER(o)
                return o;
            }
            float4 frag(VertexOutput i, float facing : VFACE) : COLOR {
                float isFrontFace = ( facing >= 0 ? 1 : 0 );
                float faceSign = ( facing >= 0 ? 1 : -1 );
                i.normalDir = normalize(i.normalDir);
                i.normalDir *= faceSign;
                float3 viewDirection = normalize(_WorldSpaceCameraPos.xyz - i.posWorld.xyz);
                float3 normalDirection = i.normalDir;
                SHADOW_CASTER_FRAGMENT(i)
            }
            ENDCG
        }
    }
    FallBack "Diffuse"
    CustomEditor "ShaderForgeMaterialInspector"
}
