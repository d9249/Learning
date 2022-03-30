Shader "Custom/InvisibleShadowCaster" {
	SubShader{
		UsePass "VertexLit/SHADOWCOLLECTOR"
		UsePass "VertexLit/SHADOWCASTER"
	}
		FallBack off
}
