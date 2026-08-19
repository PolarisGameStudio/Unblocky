Shader "Toony Colors Pro 2/Hybrid Shader 2 with Dissolve and Rim 2 Texuture" {
	Properties {
		_BaseColor ("Color", Vector) = (1,1,1,1)
		[SubColor] _SubColor ("SubColor", Vector) = (1,1,1,1)
		_BaseMap ("Albedo", 2D) = "white" {}
		[SubTexture] _Submap ("Sub", 2D) = "white" {}
		_DissolveAmount ("Dissolve Amount", Range(0, 1)) = 1
		_Direction ("Direction (0=X, 1=Z)", Range(0, 1)) = 0
		_FillDirection ("Fill Direction (1=Normal, 2=Reverse)", Range(1, 2)) = 1
		_SizeX ("Size X", Float) = 1
		_SizeZ ("Size Z", Float) = 1
		[TCP2HeaderToggle(TCP2_RIM_LIGHTING)] _UseRim ("Rim Lighting", Float) = 0
		[TCP2ColorNoAlpha] [HDR] _RimColor ("Color", Vector) = (0.8,0.8,0.8,0.5)
		_RimMin ("Min", Range(0, 2)) = 0.5
		_RimMax ("Max", Range(0, 2)) = 1
		_SubValue ("SubValue", Range(0, 1)) = 0.5
		[Toggle(TCP2_RIM_LIGHTING_LIGHTMASK)] _UseRimLightMask ("Light-based Mask", Float) = 1
	}
	//DummyShaderTextExporter
	SubShader{
		Tags { "RenderType" = "Opaque" }
		LOD 200

		Pass
		{
			HLSLPROGRAM
			#pragma vertex vert
			#pragma fragment frag

			float4x4 unity_ObjectToWorld;
			float4x4 unity_MatrixVP;

			struct Vertex_Stage_Input
			{
				float4 pos : POSITION;
			};

			struct Vertex_Stage_Output
			{
				float4 pos : SV_POSITION;
			};

			Vertex_Stage_Output vert(Vertex_Stage_Input input)
			{
				Vertex_Stage_Output output;
				output.pos = mul(unity_MatrixVP, mul(unity_ObjectToWorld, input.pos));
				return output;
			}

			float4 frag(Vertex_Stage_Output input) : SV_TARGET
			{
				return float4(1.0, 1.0, 1.0, 1.0); // RGBA
			}

			ENDHLSL
		}
	}
	Fallback "Diffuse"
}