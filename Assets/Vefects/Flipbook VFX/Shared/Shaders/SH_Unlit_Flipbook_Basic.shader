// Made with Amplify Shader Editor
// Available at the Unity Asset Store - http://u3d.as/y3X 
Shader "Vefects/SH_Unlit_Flipbook_Basic"
{
	Properties
	{
		[Space(13)][Header(Main Texture)][Space(13)]_MainTexture("Main Texture", 2D) = "white" {}
		_FlatColor("Flat Color", Float) = 0
		_UVS("UV S", Vector) = (1,1,0,0)
		_UVP("UV P", Vector) = (0,0,0,0)
		[Space(13)][Header(Distortion)][Space(13)]_DistortionTexture("Distortion Texture", 2D) = "white" {}
		_DistortionLerp("Distortion Lerp", Float) = 0
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
		Tags{ "RenderType" = "Transparent"  "Queue" = "Transparent+0" "IsEmissive" = "true"  }
		Cull [_Cull]
		ZWrite [_ZWrite]
		ZTest [_ZTest]
		Blend [_Src] [_Dst]
		
		CGPROGRAM
		#include "UnityShaderVariables.cginc"
		#pragma target 3.0
		#pragma surface surf Unlit keepalpha noshadow 
		struct Input
		{
			float4 vertexColor : COLOR;
			float2 uv_texcoord;
		};

		uniform float _ZTest;
		uniform float _Src;
		uniform float _ZWrite;
		uniform float _Dst;
		uniform float _Cull;
		uniform sampler2D _MainTexture;
		uniform float2 _UVP;
		uniform float2 _UVS;
		uniform sampler2D _DistortionTexture;
		uniform float2 _UVDP;
		uniform float2 _UVDS;
		uniform float _DistortionLerp;
		uniform float _FlatColor;

		inline half4 LightingUnlit( SurfaceOutput s, half3 lightDir, half atten )
		{
			return half4 ( 0, 0, 0, s.Alpha );
		}

		void surf( Input i , inout SurfaceOutput o )
		{
			float2 panner6 = ( 1.0 * _Time.y * _UVP + ( i.uv_texcoord * _UVS ));
			float2 panner27 = ( 1.0 * _Time.y * _UVDP + ( i.uv_texcoord * _UVDS ));
			float2 lerpResult22 = lerp( float2( 0,0 ) , ( ( (tex2D( _DistortionTexture, panner27 )).rg + -0.5 ) * 2.0 ) , _DistortionLerp);
			float4 tex2DNode1 = tex2D( _MainTexture, ( panner6 + lerpResult22 ) );
			float4 lerpResult14 = lerp( ( i.vertexColor * tex2DNode1.r ) , i.vertexColor , _FlatColor);
			o.Emission = lerpResult14.rgb;
			o.Alpha = saturate( tex2DNode1 ).r;
		}

		ENDCG
	}
	CustomEditor "ASEMaterialInspector"
}
/*ASEBEGIN
Version=18935
7;7;2552;1367;2935.96;304.166;1.3;False;False
Node;AmplifyShaderEditor.CommentaryNode;46;-2803.98,365.7426;Inherit;False;1768;982;Distortion;17;24;26;25;28;27;29;30;3;2;5;23;21;31;4;6;22;20;;0,0,0,1;0;0
Node;AmplifyShaderEditor.TextureCoordinatesNode;24;-2753.978,1055.742;Inherit;False;0;-1;2;3;2;SAMPLER2D;;False;0;FLOAT2;1,1;False;1;FLOAT2;0,0;False;5;FLOAT2;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.Vector2Node;26;-2497.983,1183.742;Inherit;False;Property;_UVDS;UV D S;6;0;Create;True;0;0;0;False;0;False;1,1;1,1;0;3;FLOAT2;0;FLOAT;1;FLOAT;2
Node;AmplifyShaderEditor.SimpleMultiplyOpNode;25;-2497.983,1055.742;Inherit;False;2;2;0;FLOAT2;0,0;False;1;FLOAT2;0,0;False;1;FLOAT2;0
Node;AmplifyShaderEditor.Vector2Node;28;-2241.983,1183.742;Inherit;False;Property;_UVDP;UV D P;7;0;Create;True;0;0;0;False;0;False;0.1,-0.2;0.1,-0.2;0;3;FLOAT2;0;FLOAT;1;FLOAT;2
Node;AmplifyShaderEditor.PannerNode;27;-2241.983,1055.742;Inherit;False;3;0;FLOAT2;0,0;False;2;FLOAT2;0,0;False;1;FLOAT;1;False;1;FLOAT2;0
Node;AmplifyShaderEditor.SamplerNode;29;-1985.984,1055.742;Inherit;True;Property;_DistortionTexture;Distortion Texture;4;0;Create;True;0;0;0;False;3;Space(13);Header(Distortion);Space(13);False;-1;98c3d568d9032a34eb5b038e20fea05d;98c3d568d9032a34eb5b038e20fea05d;True;0;False;white;Auto;False;Object;-1;Auto;Texture2D;8;0;SAMPLER2D;;False;1;FLOAT2;0,0;False;2;FLOAT;0;False;3;FLOAT2;0,0;False;4;FLOAT2;0,0;False;5;FLOAT;1;False;6;FLOAT;0;False;7;SAMPLERSTATE;;False;5;COLOR;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.TextureCoordinatesNode;2;-2113.983,415.7426;Inherit;False;0;-1;2;3;2;SAMPLER2D;;False;0;FLOAT2;1,1;False;1;FLOAT2;0,0;False;5;FLOAT2;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.ComponentMaskNode;30;-1601.983,1055.742;Inherit;False;True;True;False;False;1;0;COLOR;0,0,0,0;False;1;FLOAT2;0
Node;AmplifyShaderEditor.Vector2Node;3;-1857.983,543.7427;Inherit;False;Property;_UVS;UV S;2;0;Create;True;0;0;0;False;0;False;1,1;1,1;0;3;FLOAT2;0;FLOAT;1;FLOAT;2
Node;AmplifyShaderEditor.Vector2Node;23;-1601.983,799.7427;Inherit;False;Constant;_Vector0;Vector 0;8;0;Create;True;0;0;0;False;0;False;0,0;0,0;0;3;FLOAT2;0;FLOAT;1;FLOAT;2
Node;AmplifyShaderEditor.FunctionNode;31;-1345.983,1055.742;Inherit;False;ConstantBiasScale;-1;;1;63208df05c83e8e49a48ffbdce2e43a0;0;3;3;FLOAT2;0,0;False;1;FLOAT;-0.5;False;2;FLOAT;2;False;1;FLOAT2;0
Node;AmplifyShaderEditor.Vector2Node;4;-1601.983,543.7427;Inherit;False;Property;_UVP;UV P;3;0;Create;True;0;0;0;False;0;False;0,0;0,0;0;3;FLOAT2;0;FLOAT;1;FLOAT;2
Node;AmplifyShaderEditor.RangedFloatNode;21;-1601.983,927.7425;Inherit;False;Property;_DistortionLerp;Distortion Lerp;5;0;Create;True;0;0;0;False;0;False;0;0;0;0;0;1;FLOAT;0
Node;AmplifyShaderEditor.SimpleMultiplyOpNode;5;-1857.983,415.7426;Inherit;False;2;2;0;FLOAT2;0,0;False;1;FLOAT2;0,0;False;1;FLOAT2;0
Node;AmplifyShaderEditor.PannerNode;6;-1601.983,415.7426;Inherit;False;3;0;FLOAT2;0,0;False;2;FLOAT2;0,0;False;1;FLOAT;1;False;1;FLOAT2;0
Node;AmplifyShaderEditor.LerpOp;22;-1217.983,799.7427;Inherit;False;3;0;FLOAT2;0,0;False;1;FLOAT2;0,0;False;2;FLOAT;0;False;1;FLOAT2;0
Node;AmplifyShaderEditor.SimpleAddOpNode;20;-1217.983,415.7426;Inherit;False;2;2;0;FLOAT2;0,0;False;1;FLOAT2;0,0;False;1;FLOAT2;0
Node;AmplifyShaderEditor.CommentaryNode;47;-1020.945,-127.4707;Inherit;False;872;385;Color;4;12;15;16;14;;0,0,0,1;0;0
Node;AmplifyShaderEditor.SamplerNode;1;-922.1826,379.6667;Inherit;True;Property;_MainTexture;Main Texture;0;0;Create;True;0;0;0;False;3;Space(13);Header(Main Texture);Space(13);False;-1;None;None;True;0;False;white;Auto;False;Object;-1;Auto;Texture2D;8;0;SAMPLER2D;;False;1;FLOAT2;0,0;False;2;FLOAT;0;False;3;FLOAT2;0,0;False;4;FLOAT2;0,0;False;5;FLOAT;1;False;6;FLOAT;0;False;7;SAMPLERSTATE;;False;5;COLOR;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.VertexColorNode;12;-970.9455,50.5293;Inherit;False;0;5;COLOR;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.RangedFloatNode;15;-580.9459,-35.47072;Inherit;False;Property;_FlatColor;Flat Color;1;0;Create;True;0;0;0;False;0;False;0;1;0;0;0;1;FLOAT;0
Node;AmplifyShaderEditor.CommentaryNode;37;257.0146,-2.145075;Inherit;False;1243;166;AR;5;32;33;34;35;36;;0,0,0,1;0;0
Node;AmplifyShaderEditor.CommentaryNode;48;-418.003,336.9474;Inherit;False;215;161;Opacity;1;45;;0,0,0,1;0;0
Node;AmplifyShaderEditor.SimpleMultiplyOpNode;16;-582.9459,53.5293;Inherit;False;2;2;0;COLOR;0,0,0,0;False;1;FLOAT;0;False;1;COLOR;0
Node;AmplifyShaderEditor.LerpOp;14;-330.946,50.5293;Inherit;False;3;0;COLOR;0,0,0,0;False;1;COLOR;0,0,0,0;False;2;FLOAT;0;False;1;COLOR;0
Node;AmplifyShaderEditor.SaturateNode;45;-368.003,386.947;Inherit;False;1;0;COLOR;0,0,0,0;False;1;COLOR;0
Node;AmplifyShaderEditor.RangedFloatNode;34;819.0151,47.85493;Inherit;False;Property;_ZTest;ZTest;10;0;Create;True;0;0;0;True;0;False;2;2;0;0;0;1;FLOAT;0
Node;AmplifyShaderEditor.RangedFloatNode;35;1075.017,47.85493;Inherit;False;Property;_Src;Src;11;0;Create;True;0;0;0;True;0;False;5;5;0;0;0;1;FLOAT;0
Node;AmplifyShaderEditor.RangedFloatNode;33;563.0142,47.85493;Inherit;False;Property;_ZWrite;ZWrite;9;0;Create;True;0;0;0;True;0;False;0;0;0;0;0;1;FLOAT;0
Node;AmplifyShaderEditor.RangedFloatNode;36;1331.017,47.85493;Inherit;False;Property;_Dst;Dst;12;0;Create;True;0;0;0;True;0;False;10;10;0;0;0;1;FLOAT;0
Node;AmplifyShaderEditor.RangedFloatNode;32;307.0146,47.85493;Inherit;False;Property;_Cull;Cull;8;0;Create;True;0;0;0;True;3;Space(13);Header(AR);Space(13);False;2;2;0;0;0;1;FLOAT;0
Node;AmplifyShaderEditor.StandardSurfaceOutputNode;0;0,0;Float;False;True;-1;2;ASEMaterialInspector;0;0;Unlit;Vefects/SH_Unlit_Flipbook_Basic;False;False;False;False;False;False;False;False;False;False;False;False;False;False;False;False;False;False;False;False;False;Back;0;True;33;0;True;34;False;0;False;-1;0;False;-1;False;0;Custom;0.5;True;False;0;True;Transparent;;Transparent;All;18;all;True;True;True;True;0;False;-1;False;0;False;-1;255;False;-1;255;False;-1;0;False;-1;0;False;-1;0;False;-1;0;False;-1;0;False;-1;0;False;-1;0;False;-1;0;False;-1;False;2;15;10;25;False;0.5;False;2;5;True;35;10;True;36;0;0;False;-1;0;False;-1;0;False;-1;0;False;-1;0;False;0;0,0,0,0;VertexOffset;True;False;Cylindrical;False;True;Relative;0;;13;-1;-1;-1;0;False;0;0;True;32;-1;0;False;-1;0;0;0;False;0.1;False;-1;0;False;-1;False;15;0;FLOAT3;0,0,0;False;1;FLOAT3;0,0,0;False;2;FLOAT3;0,0,0;False;3;FLOAT;0;False;4;FLOAT;0;False;6;FLOAT3;0,0,0;False;7;FLOAT3;0,0,0;False;8;FLOAT;0;False;9;FLOAT;0;False;10;FLOAT;0;False;13;FLOAT3;0,0,0;False;11;FLOAT3;0,0,0;False;12;FLOAT3;0,0,0;False;14;FLOAT4;0,0,0,0;False;15;FLOAT3;0,0,0;False;0
Node;AmplifyShaderEditor.CommentaryNode;49;409.3925,-236.2297;Inherit;False;304;100;Lush was here! <3;0;Lush was here! <3;0,0,0,1;0;0
WireConnection;25;0;24;0
WireConnection;25;1;26;0
WireConnection;27;0;25;0
WireConnection;27;2;28;0
WireConnection;29;1;27;0
WireConnection;30;0;29;0
WireConnection;31;3;30;0
WireConnection;5;0;2;0
WireConnection;5;1;3;0
WireConnection;6;0;5;0
WireConnection;6;2;4;0
WireConnection;22;0;23;0
WireConnection;22;1;31;0
WireConnection;22;2;21;0
WireConnection;20;0;6;0
WireConnection;20;1;22;0
WireConnection;1;1;20;0
WireConnection;16;0;12;0
WireConnection;16;1;1;1
WireConnection;14;0;16;0
WireConnection;14;1;12;0
WireConnection;14;2;15;0
WireConnection;45;0;1;0
WireConnection;0;2;14;0
WireConnection;0;9;45;0
ASEEND*/
//CHKSM=6BD560CD9A25A03FABA275C9A2CA4266B6C73860