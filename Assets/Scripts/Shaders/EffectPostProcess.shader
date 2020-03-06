Shader "Custom/OutlineEffectPostProcess"
{
    Properties{
        // outline effect properties
        _MainTex("_MainTex",2D) = "white"{}
		_LineTex("LineTex",2D) = "white"{}
		_VerticalLineTex("_VerticalLineTex",2D) = "white"{}
		_OutlineWidth("OutlineWidth",int) = 3
		[MaterialToggle]_DepthEffectOnVerticalLineTex("_DepthEffectOnVerticalLineTex", Float) = 0
	}
    SubShader
    {
        
        Pass
        {
            CGPROGRAM
            #pragma vertex vert_img
            #pragma fragment frag
            #include "UnityCG.cginc"
            #pragma target 3.5
            
            // outline effect variables
            float4 _MainTex_TexelSize;
			int _OutlineWidth;
			float _DepthThreshold;
			float _DepthNormalThreshold;
			float _DepthNormalThresholdScale;
			float _NormalThreshold;
            sampler2D _CameraDepthTexture;
            sampler2D _MainTex,_LineTex,_VerticalLineTex;
			int _DepthEffectOnVerticalLineTex;
            
            sampler2D _params;
            int _MaskArrayCount;
            UNITY_DECLARE_TEX2DARRAY(_MaskArray);
            
            
            
            struct OutlineEffectReturn{
                float4 mask : SV_Target0;
                float4 effect : SV_Target1;
            };
            
            OutlineEffectReturn outlineEffect(v2f_img input,int i, float index)
            {
                float rowCount = 3;
                float4 row1 = tex2D(_params, float2(index, 0 / (rowCount - 1)));
                float4 row2 = tex2D(_params, float2(index, 1 / (rowCount - 1)));
                float4 row3 = tex2D(_params, float2(index, 2 / (rowCount - 1)));
                
                float _LineNumber = row1.r * 150.0;
                float _PixelStep = row1.g * 255.0;
                float _EdgeAlpha = row1.b;
                float _ColorMaskAlpha = row1.a;
                float4 _LineColor = row2;
                float4 _GridColor = row3;
                

				float depthIn = SAMPLE_DEPTH_TEXTURE(_CameraDepthTexture, input.uv).r;
				float depth = 1.0-(saturate(depthIn)) * 1000;
				depth *= 0.2;
                int condition = max(sign(-_OutlineWidth - floor(depth)), 0);
                float width = _OutlineWidth * condition + depth * (1-condition);
				
				float pixelX = _ScreenParams.x / _PixelStep;
				float pixelY = _ScreenParams.y / _PixelStep;
				
				float2 pixelUV = float2(floor(pixelX * input.uv.x) / pixelX, floor(pixelY * input.uv.y) / pixelY);
				float2 m = _MainTex_TexelSize;
				
				float2 bottomLeftUV =   pixelUV + float2(-m.x, -m.y) * width;
				float2 topRightUV =     pixelUV + float2(m.x ,  m.y) * width;  
				float2 bottomRightUV =  pixelUV + float2(m.x , -m.y) * width;
				float2 topLeftUV =      pixelUV + float2(-m.x,  m.y) * width;
//				float2 BottomUV =       pixelUV + float2(0, -m.y) * width * 2;
//				float2 TopUV =          pixelUV + float2(0,  m.y) * width * 2;  
//				float2 RightUV =        pixelUV + float2(m.x , 0) * width * 2;
//				float2 LeftUV =         pixelUV + float2(-m.x, 0) * width * 2;

                float mask = UNITY_SAMPLE_TEX2DARRAY(_MaskArray, float3(input.uv, i));
				float objMask0 = UNITY_SAMPLE_TEX2DARRAY(_MaskArray, float3(bottomLeftUV, i)).r;
				float objMask1 = UNITY_SAMPLE_TEX2DARRAY(_MaskArray, float3(topRightUV, i)).r;
				float objMask2 = UNITY_SAMPLE_TEX2DARRAY(_MaskArray, float3(bottomRightUV, i)).r;
				float objMask3 = UNITY_SAMPLE_TEX2DARRAY(_MaskArray, float3(topLeftUV, i)).r;
//				float objMask4 = UNITY_SAMPLE_TEX2DARRAY(_MaskArray, float3(BottomUV, i)).r;
//				float objMask5 = UNITY_SAMPLE_TEX2DARRAY(_MaskArray, float3(TopUV, i)).r;
//				float objMask6 = UNITY_SAMPLE_TEX2DARRAY(_MaskArray, float3(RightUV, i)).r;
//				float objMask7 = UNITY_SAMPLE_TEX2DARRAY(_MaskArray, float3(LeftUV, i)).r;

				float maskEdge = 1 - objMask0 * objMask1 * objMask2 * objMask3 /* * objMask4 * objMask5 * objMask6 * objMask7*/;
				float edge = saturate(objMask0 + objMask1 + objMask2 + objMask3 /*+ objMask4 + objMask5 + objMask6 + objMask7*/);
				edge *= maskEdge;

				float4 edgeColor = float4(_LineColor.rgb, _LineColor.a * edge);
					
				float2 uv = input.uv * float2(pixelX,pixelY);
				float4 grid = tex2D(_LineTex,uv) * 4;
				grid.rgb *= _GridColor.rgb * _GridColor.a;

				float verticalLineUV = (input.uv.g*10.0);
				float offset = (verticalLineUV+_Time.g);
				float4 noise = tex2D(_VerticalLineTex,pixelUV*(_LineNumber) + half2(0, _Time.y));

				float4 FinalEdge =  saturate(edge*edgeColor*_EdgeAlpha);
				float lerper = clamp(pow(clamp(depthIn * 30,0,1),3), 0.3,.5) * _DepthEffectOnVerticalLineTex;
				float4 Inside = mask * lerp(_LineColor/2, _LineColor, lerper)*noise/5;
				
				OutlineEffectReturn o;
				o.mask = edge * _ColorMaskAlpha;
				o.effect = FinalEdge + grid * objMask0 + Inside * lerp(_LineColor.a/3, _LineColor.a, lerper) * 3;
				return o;
            }
            
            
            float4 frag(v2f_img input) : COLOR
            { 
                float4 color = tex2D(_MainTex, input.uv);
                float4 effect = 0;
                float colorMask = 0;
                for(int i=0; i<_MaskArrayCount; i++){
                    float paramsIndex = float(i)/float(_MaskArrayCount-1);
                    OutlineEffectReturn e = outlineEffect(input, i, paramsIndex);
                    effect += e.effect;
                    colorMask = saturate(colorMask + e.mask);
                }
                effect = saturate(lerp(color, 0, colorMask) + effect);
                //effect = clamp(effect, 0, 1);
                return effect;
            }
            ENDCG
        }
    }
}