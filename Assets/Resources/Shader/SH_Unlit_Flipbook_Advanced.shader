// Made with Amplify Shader Editor
// Available at the Unity Asset Store - http://u3d.as/y3X 
Shader "Vefects/SH_Unlit_Flipbook_Advanced"
{
	Properties
	{
		[Space(13)][Header(Main Texture)][Space(13)]_MainTexture("Main Texture", 2D) = "white" {}
		_UVS("UV S", Vector) = (1,1,0,0)
		_UVP("UV P", Vector) = (0,0,0,0)
		[HDR]_R("R", Color) = (1,0.9719134,0.5896226,0)
		[HDR]_G("G", Color) = (1,0.7230805,0.25,0)
		[HDR]_B("B", Color) = (0.5943396,0.259371,0.09812209,0)
		[HDR]_Outline("Outline", Color) = (0.2169811,0.03320287,0.02354041,0)
		[Space(13)][Header(DisolveMapping)][Space(13)]_disolveMap("disolveMap", 2D) = "white" {}
		[Header(TextureProps)][Space(13)]_Intensity("Intensity", Range( 0 , 5)) = 1
		_ErosionSmoothness("Erosion Smoothness", Range( 0.1 , 15)) = 0.1
		_FlatColor("Flat Color", Range( 0 , 1)) = 0
		[Space(13)][Header(Distortion)][Space(13)]_DistortionTexture("Distortion Texture", 2D) = "white" {}
		_DistortionLerp("Distortion Lerp", Range( 0 , 0.1)) = 0
		[Header(SecondDistortion)][Space(13)]_DistortionSecond("DistortionSecond", 2D) = "white" {}
		_SecondDistortionLerp("SecondDistortionLerp", Range( 0.5 , 1)) = 0.5
		_UVDS("UV D S", Vector) = (1,1,0,0)
		_UVDP("UV D P", Vector) = (0.1,-0.2,0,0)
		[Space(13)][Header(AR)][Space(13)]_Cull("Cull", Float) = 2
		_ZWrite("ZWrite", Float) = 0
		_ZTest("ZTest", Float) = 2
		_Src("Src", Float) = 5
		_Dst("Dst", Float) = 10
		[HideInInspector] _texcoord( "", 2D ) = "white" {}
		[HideInInspector] __dirty( "", Int ) = 1
	}

	SubShader
	{
		Tags{ "RenderType" = "Transparent"  "Queue" = "Transparent+0" "IgnoreProjector" = "True" "IsEmissive" = "true"  }
		Cull [_Cull]
		ZWrite [_ZWrite]
		ZTest [_ZTest]
		Blend [_Src] [_Dst] , OneMinusDstColor One
		
		CGPROGRAM
		#include "UnityShaderVariables.cginc"
		#pragma target 3.0
		#pragma surface surf Unlit keepalpha noshadow 
		#undef TRANSFORM_TEX
		#define TRANSFORM_TEX(tex,name) float4(tex.xy * name##_ST.xy + name##_ST.zw, tex.z, tex.w)
		struct Input
		{
			float4 vertexColor : COLOR;
			float4 uv_texcoord;
		};

		uniform float _Dst;
		uniform float _ZTest;
		uniform float _Cull;
		uniform float _Src;
		uniform float _ZWrite;
		uniform float4 _Outline;
		uniform float4 _B;
		uniform sampler2D _MainTexture;
		uniform float2 _UVP;
		uniform float2 _UVS;
		uniform sampler2D _DistortionTexture;
		uniform float2 _UVDP;
		uniform float2 _UVDS;
		uniform float _DistortionLerp;
		uniform float4 _G;
		uniform float4 _R;
		uniform float _FlatColor;
		uniform float _Intensity;
		uniform sampler2D _DistortionSecond;
		uniform float _SecondDistortionLerp;
		uniform float _ErosionSmoothness;
		uniform sampler2D _disolveMap;
		uniform float4 _disolveMap_ST;

		inline half4 LightingUnlit( SurfaceOutput s, half3 lightDir, half atten )
		{
			return half4 ( 0, 0, 0, s.Alpha );
		}

		void surf( Input i , inout SurfaceOutput o )
		{
			float2 panner6 = ( 1.0 * _Time.y * _UVP + ( i.uv_texcoord.xy * _UVS ));
			float2 panner27 = ( 1.0 * _Time.y * _UVDP + ( i.uv_texcoord.xy * _UVDS ));
			float2 lerpResult22 = lerp( float2( 0,0 ) , ( ( (tex2D( _DistortionTexture, panner27 )).rg + -0.5 ) * 2.0 ) , _DistortionLerp);
			float2 DistortionRegister116 = ( panner6 + lerpResult22 );
			float4 tex2DNode1 = tex2D( _MainTexture, DistortionRegister116 );
			float4 lerpResult53 = lerp( _Outline , _B , tex2DNode1.b);
			float4 lerpResult51 = lerp( lerpResult53 , _G , tex2DNode1.g);
			float4 lerpResult50 = lerp( lerpResult51 , _R , tex2DNode1.r);
			float4 lerpResult14 = lerp( ( i.vertexColor * lerpResult50 ) , i.vertexColor , _FlatColor);
			float2 panner82 = ( 1.0 * _Time.y * _UVDP + ( i.uv_texcoord.xy * _UVDS ));
			float4 SecondDistortion103 = ( tex2D( _DistortionSecond, panner82 ) + _SecondDistortionLerp );
			o.Emission = ( ( lerpResult14 * _Intensity ) * SecondDistortion103 ).rgb;
			float mainTex_alpha144 = tex2DNode1.a;
			float smoothstepResult8 = smoothstep( i.uv_texcoord.z , ( i.uv_texcoord.z + _ErosionSmoothness ) , mainTex_alpha144);
			float mainTex_VC_alha112 = i.vertexColor.a;
			float Opacity_VTC_W107 = i.uv_texcoord.z;
			float Opacity_VTC_T106 = i.uv_texcoord.w;
			float temp_output_62_0 = (( Opacity_VTC_T106 - 1.0 ) + (Opacity_VTC_W107 - 0.0) * (1.0 - ( Opacity_VTC_T106 - 1.0 )) / (1.0 - 0.0));
			float2 uv_disolveMap = i.uv_texcoord * _disolveMap_ST.xy + _disolveMap_ST.zw;
			float smoothstepResult72 = smoothstep( temp_output_62_0 , ( temp_output_62_0 + Opacity_VTC_T106 ) , tex2D( _disolveMap, uv_disolveMap ).r);
			float disolveMapping105 = smoothstepResult72;
			float OpacityRegister114 = ( ( smoothstepResult8 * mainTex_VC_alha112 ) * disolveMapping105 );
			o.Alpha = OpacityRegister114;
		}

		ENDCG
	}
	CustomEditor "ASEMaterialInspector"
}
/*ASEBEGIN
Version=18935
7;7;2552;1367;5586.593;335.9136;1.888826;True;False
Node;AmplifyShaderEditor.CommentaryNode;54;-4343.227,-1594.445;Inherit;False;1992;995;Distortion;18;20;6;22;31;21;4;5;23;3;30;2;29;27;28;25;24;26;116;;0,0,0,1;0;0
Node;AmplifyShaderEditor.TextureCoordinatesNode;24;-4293.227,-904.4455;Inherit;False;0;-1;2;3;2;SAMPLER2D;;False;0;FLOAT2;1,1;False;1;FLOAT2;0,0;False;5;FLOAT2;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.Vector2Node;26;-4037.227,-776.4455;Inherit;False;Property;_UVDS;UV D S;15;0;Create;True;0;0;0;False;0;False;1,1;1,1;0;3;FLOAT2;0;FLOAT;1;FLOAT;2
Node;AmplifyShaderEditor.SimpleMultiplyOpNode;25;-4037.227,-904.4455;Inherit;False;2;2;0;FLOAT2;0,0;False;1;FLOAT2;0,0;False;1;FLOAT2;0
Node;AmplifyShaderEditor.Vector2Node;28;-3781.228,-776.4455;Inherit;False;Property;_UVDP;UV D P;16;0;Create;True;0;0;0;False;0;False;0.1,-0.2;0.1,-0.2;0;3;FLOAT2;0;FLOAT;1;FLOAT;2
Node;AmplifyShaderEditor.PannerNode;27;-3781.228,-904.4455;Inherit;False;3;0;FLOAT2;0,0;False;2;FLOAT2;0,0;False;1;FLOAT;1;False;1;FLOAT2;0
Node;AmplifyShaderEditor.SamplerNode;29;-3525.229,-904.4455;Inherit;True;Property;_DistortionTexture;Distortion Texture;11;0;Create;True;0;0;0;False;3;Space(13);Header(Distortion);Space(13);False;-1;98c3d568d9032a34eb5b038e20fea05d;98c3d568d9032a34eb5b038e20fea05d;True;0;False;white;Auto;False;Object;-1;Auto;Texture2D;8;0;SAMPLER2D;;False;1;FLOAT2;0,0;False;2;FLOAT;0;False;3;FLOAT2;0,0;False;4;FLOAT2;0,0;False;5;FLOAT;1;False;6;FLOAT;0;False;7;SAMPLERSTATE;;False;5;COLOR;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.Vector2Node;3;-3397.228,-1416.445;Inherit;False;Property;_UVS;UV S;1;0;Create;True;0;0;0;False;0;False;1,1;1,1;0;3;FLOAT2;0;FLOAT;1;FLOAT;2
Node;AmplifyShaderEditor.TextureCoordinatesNode;2;-3653.228,-1544.445;Inherit;False;0;-1;2;3;2;SAMPLER2D;;False;0;FLOAT2;1,1;False;1;FLOAT2;0,0;False;5;FLOAT2;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.ComponentMaskNode;30;-3141.228,-904.4455;Inherit;False;True;True;False;False;1;0;COLOR;0,0,0,0;False;1;FLOAT2;0
Node;AmplifyShaderEditor.RangedFloatNode;21;-3141.228,-1032.445;Inherit;False;Property;_DistortionLerp;Distortion Lerp;12;0;Create;True;0;0;0;False;0;False;0;0;0;0.1;0;1;FLOAT;0
Node;AmplifyShaderEditor.Vector2Node;4;-3141.228,-1416.445;Inherit;False;Property;_UVP;UV P;2;0;Create;True;0;0;0;False;0;False;0,0;0,0;0;3;FLOAT2;0;FLOAT;1;FLOAT;2
Node;AmplifyShaderEditor.SimpleMultiplyOpNode;5;-3397.228,-1544.445;Inherit;False;2;2;0;FLOAT2;0,0;False;1;FLOAT2;0,0;False;1;FLOAT2;0
Node;AmplifyShaderEditor.Vector2Node;23;-3141.228,-1160.445;Inherit;False;Constant;_Vector0;Vector 0;8;0;Create;True;0;0;0;False;0;False;0,0;0,0;0;3;FLOAT2;0;FLOAT;1;FLOAT;2
Node;AmplifyShaderEditor.FunctionNode;31;-2885.228,-904.4455;Inherit;False;ConstantBiasScale;-1;;1;63208df05c83e8e49a48ffbdce2e43a0;0;3;3;FLOAT2;0,0;False;1;FLOAT;-0.5;False;2;FLOAT;2;False;1;FLOAT2;0
Node;AmplifyShaderEditor.CommentaryNode;56;-4332.979,-187.6994;Inherit;False;1538.791;442.8129;Opacity;12;145;114;73;44;113;8;10;9;107;106;11;111;;0,0,0,1;0;0
Node;AmplifyShaderEditor.LerpOp;22;-2757.228,-1160.445;Inherit;False;3;0;FLOAT2;0,0;False;1;FLOAT2;0,0;False;2;FLOAT;0;False;1;FLOAT2;0
Node;AmplifyShaderEditor.PannerNode;6;-3141.228,-1544.445;Inherit;False;3;0;FLOAT2;0,0;False;2;FLOAT2;0,0;False;1;FLOAT;1;False;1;FLOAT2;0
Node;AmplifyShaderEditor.TexCoordVertexDataNode;11;-4304.443,-82.53802;Inherit;False;0;4;0;5;FLOAT4;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.CommentaryNode;84;-4327.007,281.8066;Inherit;False;1486.067;526.0999;DisolveMaping;13;72;71;70;62;65;63;69;64;66;105;108;109;110;;0.1037736,0.1037736,0.1037736,1;0;0
Node;AmplifyShaderEditor.RegisterLocalVarNode;106;-3988.026,180.6374;Inherit;False;Opacity_VTC_T;-1;True;1;0;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.SimpleAddOpNode;20;-2757.228,-1544.445;Inherit;False;2;2;0;FLOAT2;0,0;False;1;FLOAT2;0,0;False;1;FLOAT2;0
Node;AmplifyShaderEditor.GetLocalVarNode;108;-4309.393,400.2585;Inherit;False;106;Opacity_VTC_T;1;0;OBJECT;;False;1;FLOAT;0
Node;AmplifyShaderEditor.RangedFloatNode;66;-4269.042,566.5135;Inherit;False;Constant;_Float2;Float 2;20;0;Create;True;0;0;0;False;0;False;1;0;0;0;0;1;FLOAT;0
Node;AmplifyShaderEditor.RegisterLocalVarNode;116;-2584.966,-1542.905;Inherit;False;DistortionRegister;-1;True;1;0;FLOAT2;0,0;False;1;FLOAT2;0
Node;AmplifyShaderEditor.CommentaryNode;55;-2286.569,-1590.614;Inherit;False;1896;1537;Color;15;52;49;12;15;16;14;48;53;51;47;50;112;117;1;144;;0,0,0,1;0;0
Node;AmplifyShaderEditor.RegisterLocalVarNode;107;-3809.075,102.2602;Inherit;False;Opacity_VTC_W;-1;True;1;0;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.RangedFloatNode;63;-4103.79,402.1814;Inherit;False;Constant;_Float0;Float 0;20;0;Create;True;0;0;0;False;0;False;0;0;0;0;0;1;FLOAT;0
Node;AmplifyShaderEditor.GetLocalVarNode;117;-2176,-512;Inherit;False;116;DistortionRegister;1;0;OBJECT;;False;1;FLOAT2;0
Node;AmplifyShaderEditor.SimpleSubtractOpNode;65;-4104.889,558.3875;Inherit;False;2;0;FLOAT;0;False;1;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.RangedFloatNode;64;-4097.289,475.3202;Inherit;False;Constant;_Float1;Float 1;20;0;Create;True;0;0;0;False;0;False;1;0;0;0;0;1;FLOAT;0
Node;AmplifyShaderEditor.GetLocalVarNode;109;-3976.759,325.9834;Inherit;False;107;Opacity_VTC_W;1;0;OBJECT;;False;1;FLOAT;0
Node;AmplifyShaderEditor.CommentaryNode;85;-4333.089,-580.5333;Inherit;False;1665.348;371.0714;SecondDistortion;9;103;76;77;74;82;80;81;79;78;;0,0,0,1;0;0
Node;AmplifyShaderEditor.RangedFloatNode;69;-4099.726,664.4172;Inherit;False;Constant;_Float3;Float 3;20;0;Create;True;0;0;0;False;0;False;1;0;0;0;0;1;FLOAT;0
Node;AmplifyShaderEditor.ColorNode;49;-2176,-1024;Inherit;False;Property;_B;B;5;1;[HDR];Create;True;0;0;0;False;0;False;0.5943396,0.259371,0.09812209,0;0.2641509,0.2616589,0.2554289,0;True;0;5;COLOR;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.ColorNode;52;-2176,-768;Inherit;False;Property;_Outline;Outline;6;1;[HDR];Create;True;0;0;0;False;0;False;0.2169811,0.03320287,0.02354041,0;0,0,0,0;True;0;5;COLOR;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.SamplerNode;1;-1920,-512;Inherit;True;Property;_MainTexture;Main Texture;0;0;Create;True;0;0;0;False;3;Space(13);Header(Main Texture);Space(13);False;-1;5e9cda599296bd74a9a45a7b3a63c0a9;6f0bad7c6d47efb4abe92a24bef80f7c;True;0;False;white;Auto;False;Object;-1;Auto;Texture2D;8;0;SAMPLER2D;;False;1;FLOAT2;0,0;False;2;FLOAT;0;False;3;FLOAT2;0,0;False;4;FLOAT2;0,0;False;5;FLOAT;1;False;6;FLOAT;0;False;7;SAMPLERSTATE;;False;5;COLOR;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.Vector2Node;79;-4027.087,-370.1579;Inherit;False;Property;_UVDS;UV D S;11;0;Create;True;0;0;0;False;0;False;1,1;1,1;0;3;FLOAT2;0;FLOAT;1;FLOAT;2
Node;AmplifyShaderEditor.TFHCRemapNode;62;-3850.077,413.3898;Inherit;False;5;0;FLOAT;0;False;1;FLOAT;0;False;2;FLOAT;1;False;3;FLOAT;0;False;4;FLOAT;1;False;1;FLOAT;0
Node;AmplifyShaderEditor.GetLocalVarNode;110;-3784.359,324.6834;Inherit;False;106;Opacity_VTC_T;1;0;OBJECT;;False;1;FLOAT;0
Node;AmplifyShaderEditor.TextureCoordinatesNode;78;-4283.089,-498.1583;Inherit;False;0;-1;2;3;2;SAMPLER2D;;False;0;FLOAT2;1,1;False;1;FLOAT2;0,0;False;5;FLOAT2;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.RegisterLocalVarNode;144;-1536,-512;Inherit;False;mainTex_alpha;-1;True;1;0;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.VertexColorNode;12;-1152,-896;Inherit;False;0;5;COLOR;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.ColorNode;48;-2176,-1280;Inherit;False;Property;_G;G;4;1;[HDR];Create;True;0;0;0;False;0;False;1,0.7230805,0.25,0;1,0.3523919,0,0;True;0;5;COLOR;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.LerpOp;53;-1664,-1152;Inherit;True;3;0;COLOR;0,0,0,0;False;1;COLOR;0,0,0,0;False;2;FLOAT;0;False;1;COLOR;0
Node;AmplifyShaderEditor.RangedFloatNode;9;-4036.67,-74.60577;Inherit;False;Property;_ErosionSmoothness;Erosion Smoothness;9;0;Create;True;0;0;0;False;0;False;0.1;1.57;0.1;15;0;1;FLOAT;0
Node;AmplifyShaderEditor.SimpleMultiplyOpNode;81;-4027.087,-498.1583;Inherit;False;2;2;0;FLOAT2;0,0;False;1;FLOAT2;0,0;False;1;FLOAT2;0
Node;AmplifyShaderEditor.Vector2Node;80;-3771.088,-370.1579;Inherit;False;Property;_UVDP;UV D P;12;0;Create;True;0;0;0;False;0;False;0.1,-0.2;0.1,-0.2;0;3;FLOAT2;0;FLOAT;1;FLOAT;2
Node;AmplifyShaderEditor.SamplerNode;71;-3881.859,585.9556;Inherit;True;Property;_disolveMap;disolveMap;7;0;Create;True;0;0;0;False;3;Space(13);Header(DisolveMapping);Space(13);False;-1;None;None;True;0;False;white;Auto;False;Object;-1;Auto;Texture2D;8;0;SAMPLER2D;;False;1;FLOAT2;0,0;False;2;FLOAT;0;False;3;FLOAT2;0,0;False;4;FLOAT2;0,0;False;5;FLOAT;1;False;6;FLOAT;0;False;7;SAMPLERSTATE;;False;5;COLOR;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.SimpleAddOpNode;70;-3581.946,358.0367;Inherit;False;2;2;0;FLOAT;0;False;1;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.PannerNode;82;-3771.088,-498.1583;Inherit;False;3;0;FLOAT2;0,0;False;2;FLOAT2;0,0;False;1;FLOAT;1;False;1;FLOAT2;0
Node;AmplifyShaderEditor.ColorNode;47;-2176,-1536;Inherit;False;Property;_R;R;3;1;[HDR];Create;True;0;0;0;False;0;False;1,0.9719134,0.5896226,0;0.3679245,0.3679245,0.3679245,0;True;0;5;COLOR;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.LerpOp;51;-1280,-1280;Inherit;True;3;0;COLOR;0,0,0,0;False;1;COLOR;0,0,0,0;False;2;FLOAT;0;False;1;COLOR;0
Node;AmplifyShaderEditor.SimpleAddOpNode;10;-3734.442,2.462128;Inherit;False;2;2;0;FLOAT;0;False;1;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.SmoothstepOpNode;72;-3397.514,460.809;Inherit;False;3;0;FLOAT;0;False;1;FLOAT;0;False;2;FLOAT;1;False;1;FLOAT;0
Node;AmplifyShaderEditor.GetLocalVarNode;145;-3836.597,-149.1651;Inherit;False;144;mainTex_alpha;1;0;OBJECT;;False;1;FLOAT;0
Node;AmplifyShaderEditor.RegisterLocalVarNode;112;-1152,-640;Inherit;False;mainTex_VC_alha;-1;True;1;0;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.GetLocalVarNode;113;-3642.016,-148.0787;Inherit;False;112;mainTex_VC_alha;1;0;OBJECT;;False;1;FLOAT;0
Node;AmplifyShaderEditor.LerpOp;50;-1024,-1536;Inherit;True;3;0;COLOR;0,0,0,0;False;1;COLOR;0,0,0,0;False;2;FLOAT;0;False;1;COLOR;0
Node;AmplifyShaderEditor.RegisterLocalVarNode;105;-3192.814,466.2443;Inherit;False;disolveMapping;-1;True;1;0;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.SmoothstepOpNode;8;-3594.541,-66.93811;Inherit;False;3;0;FLOAT;0;False;1;FLOAT;0;False;2;FLOAT;1;False;1;FLOAT;0
Node;AmplifyShaderEditor.SamplerNode;74;-3429.794,-530.5342;Inherit;True;Property;_DistortionSecond;DistortionSecond;13;1;[Header];Create;True;1;SecondDistortion;0;0;False;1;Space(13);False;-1;None;None;True;0;False;white;Auto;False;Object;-1;Auto;Texture2D;8;0;SAMPLER2D;;False;1;FLOAT2;0,0;False;2;FLOAT;0;False;3;FLOAT2;0,0;False;4;FLOAT2;0,0;False;5;FLOAT;1;False;6;FLOAT;0;False;7;SAMPLERSTATE;;False;5;COLOR;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.RangedFloatNode;77;-3411.708,-312.6792;Inherit;False;Property;_SecondDistortionLerp;SecondDistortionLerp;14;0;Create;True;0;0;0;False;0;False;0.5;0;0.5;1;0;1;FLOAT;0
Node;AmplifyShaderEditor.SimpleAddOpNode;76;-3102.823,-381.4113;Inherit;False;2;2;0;COLOR;0,0,0,0;False;1;FLOAT;0;False;1;COLOR;0
Node;AmplifyShaderEditor.GetLocalVarNode;111;-3476.497,130.1432;Inherit;False;105;disolveMapping;1;0;OBJECT;;False;1;FLOAT;0
Node;AmplifyShaderEditor.SimpleMultiplyOpNode;44;-3408.441,-82.53802;Inherit;False;2;2;0;FLOAT;0;False;1;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.SimpleMultiplyOpNode;16;-640,-1024;Inherit;True;2;2;0;COLOR;0,0,0,0;False;1;COLOR;0,0,0,0;False;1;COLOR;0
Node;AmplifyShaderEditor.RangedFloatNode;15;-1024,-256;Inherit;False;Property;_FlatColor;Flat Color;10;0;Create;True;0;0;0;False;0;False;0;0;0;1;0;1;FLOAT;0
Node;AmplifyShaderEditor.SimpleMultiplyOpNode;73;-3230.983,9.99173;Inherit;False;2;2;0;FLOAT;0;False;1;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.RangedFloatNode;61;-640,0;Inherit;False;Property;_Intensity;Intensity;8;1;[Header];Create;True;1;TextureProps;0;0;False;1;Space(13);False;1;1;0;5;0;1;FLOAT;0
Node;AmplifyShaderEditor.RegisterLocalVarNode;103;-2896.438,-379.3916;Inherit;False;SecondDistortion;-1;True;1;0;COLOR;0,0,0,0;False;1;COLOR;0
Node;AmplifyShaderEditor.LerpOp;14;-640,-512;Inherit;True;3;0;COLOR;0,0,0,0;False;1;COLOR;0,0,0,0;False;2;FLOAT;0;False;1;COLOR;0
Node;AmplifyShaderEditor.SimpleMultiplyOpNode;60;-256,-128;Inherit;False;2;2;0;COLOR;0,0,0,0;False;1;FLOAT;0;False;1;COLOR;0
Node;AmplifyShaderEditor.GetLocalVarNode;104;-640,128;Inherit;False;103;SecondDistortion;1;0;OBJECT;;False;1;COLOR;0
Node;AmplifyShaderEditor.RegisterLocalVarNode;114;-2988.965,7.366943;Inherit;False;OpacityRegister;-1;True;1;0;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.CommentaryNode;155;-2141.765,580.7527;Inherit;False;1194.858;412.5891;Depth Fade;7;152;139;153;147;146;151;148;;0,0,0,1;0;0
Node;AmplifyShaderEditor.CommentaryNode;96;-4326.425,835.4186;Inherit;False;1354.227;543.6159;VertexDisplacement;10;101;95;94;100;93;91;88;90;87;89;;0,0,0,1;0;0
Node;AmplifyShaderEditor.CommentaryNode;37;590,-50;Inherit;False;1243;166;AR;5;32;33;34;35;36;;0,0,0,1;0;0
Node;AmplifyShaderEditor.ScreenPosInputsNode;146;-2091.765,650.4998;Float;False;1;False;0;5;FLOAT4;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.ScreenDepthNode;147;-1886.862,630.7527;Inherit;False;1;True;1;0;FLOAT4;0,0,0,0;False;1;FLOAT;0
Node;AmplifyShaderEditor.RangedFloatNode;153;-1647.717,838.3998;Inherit;False;Property;_Float4;Float 4;21;0;Create;True;0;0;0;False;0;False;0;0;0;1;0;1;FLOAT;0
Node;AmplifyShaderEditor.RegisterLocalVarNode;139;-1194.907,734.3419;Inherit;True;DepthFadeRegister;-1;True;1;0;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.SimpleMultiplyOpNode;152;-1346.331,742.3648;Inherit;False;2;2;0;FLOAT;0;False;1;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.RangedFloatNode;33;896,0;Inherit;False;Property;_ZWrite;ZWrite;23;0;Create;True;0;0;0;True;0;False;0;0;0;0;0;1;FLOAT;0
Node;AmplifyShaderEditor.GetLocalVarNode;115;-640,256;Inherit;False;114;OpacityRegister;1;0;OBJECT;;False;1;FLOAT;0
Node;AmplifyShaderEditor.RegisterLocalVarNode;101;-3210.18,996.3517;Inherit;False;VertexDisplacement;-1;True;1;0;COLOR;0,0,0,0;False;1;COLOR;0
Node;AmplifyShaderEditor.TFHCRemapNode;95;-3418.143,1133.189;Inherit;False;5;0;COLOR;0,0,0,0;False;1;COLOR;0,0,0,0;False;2;COLOR;1,1,1,1;False;3;COLOR;0,0,0,0;False;4;COLOR;1,1,1,1;False;1;COLOR;0
Node;AmplifyShaderEditor.SimpleTimeNode;89;-4199.224,1063.018;Inherit;False;1;0;FLOAT;1;False;1;FLOAT;0
Node;AmplifyShaderEditor.SamplerNode;100;-3738.677,958.5841;Inherit;True;Property;_VertexDistortionNoise_tex;VertexDistortionNoise_tex;17;0;Create;True;0;0;0;False;0;False;-1;None;None;True;0;False;white;Auto;False;Object;-1;Auto;Texture2D;8;0;SAMPLER2D;;False;1;FLOAT2;0,0;False;2;FLOAT;0;False;3;FLOAT2;0,0;False;4;FLOAT2;0,0;False;5;FLOAT;1;False;6;FLOAT;0;False;7;SAMPLERSTATE;;False;5;COLOR;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.RangedFloatNode;87;-4297.325,1153.018;Inherit;False;Property;_VertexDistortion_Speed;VertexDistortion_Speed;19;0;Create;True;0;0;0;False;0;False;0;0;0;10;0;1;FLOAT;0
Node;AmplifyShaderEditor.SimpleAddOpNode;91;-3862.028,1010.218;Inherit;False;2;2;0;FLOAT3;0,0,0;False;1;FLOAT;0;False;1;FLOAT3;0
Node;AmplifyShaderEditor.RangedFloatNode;35;1408,0;Inherit;False;Property;_Src;Src;25;0;Create;True;0;0;0;True;0;False;5;5;0;0;0;1;FLOAT;0
Node;AmplifyShaderEditor.SimpleMultiplyOpNode;94;-3670.027,1185.317;Inherit;False;2;2;0;FLOAT;0;False;1;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.NormalVertexDataNode;90;-4097.229,885.4184;Inherit;False;0;5;FLOAT3;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.RangedFloatNode;32;640,0;Inherit;False;Property;_Cull;Cull;22;0;Create;True;0;0;0;True;3;Space(13);Header(AR);Space(13);False;2;2;0;0;0;1;FLOAT;0
Node;AmplifyShaderEditor.GetLocalVarNode;102;-640,384;Inherit;False;101;VertexDisplacement;1;0;OBJECT;;False;1;COLOR;0
Node;AmplifyShaderEditor.RangedFloatNode;34;1152,-1.372217;Inherit;False;Property;_ZTest;ZTest;24;0;Create;True;0;0;0;True;0;False;2;2;0;0;0;1;FLOAT;0
Node;AmplifyShaderEditor.RangedFloatNode;93;-3959.827,1273.916;Inherit;False;Property;_VertexDistortion_Scale;VertexDistortion_Scale;18;0;Create;True;0;0;0;False;0;False;0;0;-0.1;0.25;0;1;FLOAT;0
Node;AmplifyShaderEditor.RangedFloatNode;36;1664,0;Inherit;False;Property;_Dst;Dst;26;0;Create;True;0;0;0;True;0;False;10;10;0;0;0;1;FLOAT;0
Node;AmplifyShaderEditor.SimpleMultiplyOpNode;88;-4021.426,1095.018;Inherit;False;2;2;0;FLOAT;0;False;1;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.AbsOpNode;151;-1484.254,739.2988;Inherit;False;1;0;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.SimpleMultiplyOpNode;75;-256,128;Inherit;False;2;2;0;COLOR;0,0,0,0;False;1;COLOR;0,0,0,0;False;1;COLOR;0
Node;AmplifyShaderEditor.SimpleSubtractOpNode;148;-1661.386,726.3818;Inherit;False;2;0;FLOAT;0;False;1;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.StandardSurfaceOutputNode;0;0.5699768,96.22454;Float;False;True;-1;2;ASEMaterialInspector;0;0;Unlit;Vefects/SH_Unlit_Flipbook_Advanced;False;False;False;False;False;False;False;False;False;False;False;False;False;False;True;False;False;False;False;False;False;Off;0;True;33;0;True;34;False;0;False;-1;0;False;-1;False;0;Custom;0.5;True;False;0;True;Transparent;;Transparent;All;18;all;True;True;True;True;0;False;-1;False;0;False;-1;255;False;-1;255;False;-1;0;False;-1;0;False;-1;0;False;-1;0;False;-1;0;False;-1;0;False;-1;0;False;-1;0;False;-1;False;2;15;10;25;False;0.5;False;2;5;True;35;10;True;36;5;4;False;-1;1;False;-1;0;False;-1;0;False;-1;0;False;0;0,0,0,0;VertexOffset;True;False;Cylindrical;False;True;Relative;0;;20;-1;-1;-1;0;False;0;0;True;32;-1;0;False;-1;0;0;0;False;0.1;False;-1;0;False;-1;False;15;0;FLOAT3;0,0,0;False;1;FLOAT3;0,0,0;False;2;FLOAT3;0,0,0;False;3;FLOAT;0;False;4;FLOAT;0;False;6;FLOAT3;0,0,0;False;7;FLOAT3;0,0,0;False;8;FLOAT;0;False;9;FLOAT;0;False;10;FLOAT;0;False;13;FLOAT3;0,0,0;False;11;FLOAT3;0,0,0;False;12;FLOAT3;0,0,0;False;14;FLOAT4;0,0,0,0;False;15;FLOAT3;0,0,0;False;0
Node;AmplifyShaderEditor.CommentaryNode;154;1235.397,-360.5068;Inherit;False;304;100;Lush was here! <3;0;Lush was here! <3;0,0,0,1;0;0
WireConnection;25;0;24;0
WireConnection;25;1;26;0
WireConnection;27;0;25;0
WireConnection;27;2;28;0
WireConnection;29;1;27;0
WireConnection;30;0;29;0
WireConnection;5;0;2;0
WireConnection;5;1;3;0
WireConnection;31;3;30;0
WireConnection;22;0;23;0
WireConnection;22;1;31;0
WireConnection;22;2;21;0
WireConnection;6;0;5;0
WireConnection;6;2;4;0
WireConnection;106;0;11;4
WireConnection;20;0;6;0
WireConnection;20;1;22;0
WireConnection;116;0;20;0
WireConnection;107;0;11;3
WireConnection;65;0;108;0
WireConnection;65;1;66;0
WireConnection;1;1;117;0
WireConnection;62;0;109;0
WireConnection;62;1;63;0
WireConnection;62;2;64;0
WireConnection;62;3;65;0
WireConnection;62;4;69;0
WireConnection;144;0;1;4
WireConnection;53;0;52;0
WireConnection;53;1;49;0
WireConnection;53;2;1;3
WireConnection;81;0;78;0
WireConnection;81;1;79;0
WireConnection;70;0;62;0
WireConnection;70;1;110;0
WireConnection;82;0;81;0
WireConnection;82;2;80;0
WireConnection;51;0;53;0
WireConnection;51;1;48;0
WireConnection;51;2;1;2
WireConnection;10;0;11;3
WireConnection;10;1;9;0
WireConnection;72;0;71;1
WireConnection;72;1;62;0
WireConnection;72;2;70;0
WireConnection;112;0;12;4
WireConnection;50;0;51;0
WireConnection;50;1;47;0
WireConnection;50;2;1;1
WireConnection;105;0;72;0
WireConnection;8;0;145;0
WireConnection;8;1;11;3
WireConnection;8;2;10;0
WireConnection;74;1;82;0
WireConnection;76;0;74;0
WireConnection;76;1;77;0
WireConnection;44;0;8;0
WireConnection;44;1;113;0
WireConnection;16;0;12;0
WireConnection;16;1;50;0
WireConnection;73;0;44;0
WireConnection;73;1;111;0
WireConnection;103;0;76;0
WireConnection;14;0;16;0
WireConnection;14;1;12;0
WireConnection;14;2;15;0
WireConnection;60;0;14;0
WireConnection;60;1;61;0
WireConnection;114;0;73;0
WireConnection;147;0;146;0
WireConnection;139;0;152;0
WireConnection;152;0;151;0
WireConnection;152;1;153;0
WireConnection;101;0;95;0
WireConnection;95;0;100;0
WireConnection;95;3;94;0
WireConnection;95;4;93;0
WireConnection;100;1;91;0
WireConnection;91;0;90;0
WireConnection;91;1;88;0
WireConnection;94;0;93;0
WireConnection;88;0;89;0
WireConnection;88;1;87;0
WireConnection;151;0;148;0
WireConnection;75;0;60;0
WireConnection;75;1;104;0
WireConnection;148;0;147;0
WireConnection;148;1;146;4
WireConnection;0;2;75;0
WireConnection;0;9;115;0
ASEEND*/
//CHKSM=9020216237FCFA3D773AF090893112823B84B4EB