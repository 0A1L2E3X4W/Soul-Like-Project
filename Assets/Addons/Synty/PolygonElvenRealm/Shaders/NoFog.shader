// Made with Amplify Shader Editor
// Available at the Unity Asset Store - http://u3d.as/y3X 
Shader "SyntyStudios/NoFog"
{
	Properties
	{
		_Cutoff( "Mask Clip Value", Float ) = 0.5
		_Moon_01("Moon_01", 2D) = "white" {}
		_EmissionPower("EmissionPower", Range( 0 , 20)) = 0
		_EmissionColour("EmissionColour", Color) = (1,0.01415092,0.9477745,0)
		[HideInInspector] _texcoord( "", 2D ) = "white" {}
		[HideInInspector] __dirty( "", Int ) = 1
	}

	SubShader
	{
		Tags{ "RenderType" = "TransparentCutout"  "Queue" = "Geometry+0" "IsEmissive" = "true"  }
		Cull Back
		CGPROGRAM
		#pragma target 3.0
		#pragma surface surf Standard keepalpha noshadow nofog 
		struct Input
		{
			float2 uv_texcoord;
		};

		uniform sampler2D _Moon_01;
		uniform float4 _Moon_01_ST;
		uniform float4 _EmissionColour;
		uniform float _EmissionPower;
		uniform float _Cutoff = 0.5;

		void surf( Input i , inout SurfaceOutputStandard o )
		{
			float2 uv_Moon_01 = i.uv_texcoord * _Moon_01_ST.xy + _Moon_01_ST.zw;
			float4 tex2DNode2 = tex2D( _Moon_01, uv_Moon_01 );
			o.Albedo = tex2DNode2.rgb;
			o.Emission = ( ( tex2DNode2 * _EmissionColour ) * _EmissionPower ).rgb;
			o.Alpha = 1;
			clip( tex2DNode2.a - _Cutoff );
		}

		ENDCG
	}
	CustomEditor "ASEMaterialInspector"
}
/*ASEBEGIN
Version=18909
294;607;1409;798;948;59;1;True;True
Node;AmplifyShaderEditor.SamplerNode;2;-449,-28;Inherit;True;Property;_Moon_01;Moon_01;1;0;Create;True;0;0;0;False;0;False;-1;49932ae08a9c9074693121a4946c4212;49932ae08a9c9074693121a4946c4212;True;0;False;white;Auto;False;Object;-1;Auto;Texture2D;8;0;SAMPLER2D;;False;1;FLOAT2;0,0;False;2;FLOAT;0;False;3;FLOAT2;0,0;False;4;FLOAT2;0,0;False;5;FLOAT;1;False;6;FLOAT;0;False;7;SAMPLERSTATE;;False;5;COLOR;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.ColorNode;5;-348,454;Inherit;False;Property;_EmissionColour;EmissionColour;3;0;Create;True;0;0;0;False;0;False;1,0.01415092,0.9477745,0;0,0,0,0;True;0;5;COLOR;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.SimpleMultiplyOpNode;6;-181,179;Inherit;False;2;2;0;COLOR;0,0,0,0;False;1;COLOR;0,0,0,0;False;1;COLOR;0
Node;AmplifyShaderEditor.RangedFloatNode;4;-533,298;Inherit;False;Property;_EmissionPower;EmissionPower;2;0;Create;True;0;0;0;False;0;False;0;0;0;20;0;1;FLOAT;0
Node;AmplifyShaderEditor.SimpleMultiplyOpNode;3;-71,281;Inherit;False;2;2;0;COLOR;0,0,0,0;False;1;FLOAT;0;False;1;COLOR;0
Node;AmplifyShaderEditor.StandardSurfaceOutputNode;0;120,-61;Float;False;True;-1;2;ASEMaterialInspector;0;0;Standard;SyntyStudios/NoFog;False;False;False;False;False;False;False;False;False;True;False;False;False;False;False;False;False;False;False;False;False;Back;0;False;-1;0;False;-1;False;0;False;-1;0;False;-1;False;0;Custom;0.5;True;False;0;False;TransparentCutout;;Geometry;All;16;all;True;True;True;True;0;False;-1;False;0;False;-1;255;False;-1;255;False;-1;0;False;-1;0;False;-1;0;False;-1;0;False;-1;0;False;-1;0;False;-1;0;False;-1;0;False;-1;False;2;15;10;25;False;0.5;False;0;0;False;-1;0;False;-1;0;0;False;-1;0;False;-1;0;False;-1;0;False;-1;0;False;0;0,0,0,0;VertexOffset;True;False;Cylindrical;False;Relative;0;;0;-1;-1;-1;0;False;0;0;False;-1;-1;0;False;-1;0;0;0;False;0.1;False;-1;0;False;-1;False;16;0;FLOAT3;0,0,0;False;1;FLOAT3;0,0,0;False;2;FLOAT3;0,0,0;False;3;FLOAT;0;False;4;FLOAT;0;False;5;FLOAT;0;False;6;FLOAT3;0,0,0;False;7;FLOAT3;0,0,0;False;8;FLOAT;0;False;9;FLOAT;0;False;10;FLOAT;0;False;13;FLOAT3;0,0,0;False;11;FLOAT3;0,0,0;False;12;FLOAT3;0,0,0;False;14;FLOAT4;0,0,0,0;False;15;FLOAT3;0,0,0;False;0
WireConnection;6;0;2;0
WireConnection;6;1;5;0
WireConnection;3;0;6;0
WireConnection;3;1;4;0
WireConnection;0;0;2;0
WireConnection;0;2;3;0
WireConnection;0;10;2;4
ASEEND*/
//CHKSM=218FC2437915D627147001ECCAAD939ABD9C489B