Shader "Hidden/OutlineEffectObject"
{
    Properties {}
	SubShader
	{
		Pass
		{
			CGPROGRAM
			#pragma vertex vert
			#pragma fragment frag
			#include "UnityCG.cginc"
			
            sampler2D_float _CameraDepthTexture;

			struct vertexInput
			{
				float4 vertex : POSITION;
				float3 texCoord : TEXCOORD0;
			};

			struct vertexOutput
			{
				float4 pos : SV_POSITION;
				float3 texCoord : TEXCOORD0;
                float linearDepth : TEXCOORD1;
                float4 screenPos : TEXCOORD2;
			};

			vertexOutput vert(vertexInput input)
			{
				vertexOutput output;
				output.pos = UnityObjectToClipPos(input.vertex);
                output.texCoord = input.texCoord;
                
                output.screenPos = ComputeScreenPos(output.pos);
                output.linearDepth = -(UnityObjectToViewPos(input.vertex).z * _ProjectionParams.w);

                return output;
			}
		
			float4 frag(vertexOutput input) : COLOR
			{
                float4 c = 0;
				float2 uv = input.screenPos.xy / input.screenPos.w;
				float camDepth = SAMPLE_DEPTH_TEXTURE(_CameraDepthTexture, uv);
				camDepth = Linear01Depth (camDepth); 
                float diff = saturate(input.linearDepth - camDepth);
                if(diff < 0.000001)
                    c = 1;
                return c;
			}

			ENDCG
		}
    }
}