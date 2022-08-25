Shader "Custom/test2"
{
    Properties
    {
       _faceColor("Face Color : ", Color) = (0.0,0.0,0.0,1)
       _edgeColor("Edge Color : ", Color) = (1,1,1,1)

    }
    SubShader
    {
       
        CGPROGRAM
            //#pragma gives instructions to the compiler -- set options, take actions, override defaults, etc.
            //syntax of #pragma is
            //#pragma surface surfaceFunction lightModel [optionalparams]
            //here, the surface modifier means that it shades the surface.
            //the surf modifier calls the surf function
            //Lambert calls Unity's Lambert lighting model.
            #pragma surface surf Lambert
              fixed4 _faceColor;
            
           /* #pragma vertex vert
            #pragma fragment frag

            struct VertInput
            {
                float4 pos: POSITION;
            };

            struct vertOutput
            {
                float4 pos : SV_POSITION;
            }

            vertOutput vert(vertInput input)
            {
                vertOutput o;
                o.pos = mul(UNITY+MATRIX_MVP, input.pos);
                return o
            }

            float4 frag(vertOutput output) : COLOR
            {
                return float4(0.0f,0.0f,0.0f,1.0f));
            }
*/
            struct Input
            {
                float2 uv_myTex;
                float2 uv_vert;
            };
        
        void surf(Input IN, inout SurfaceOutput o)
        {
            o.Albedo.rgb = _faceColor.rgb;
            //o.Texture = uv_myTex;
            //o.Normal.rgb = _edgeColor.rgb;
        }
        ENDCG
    }
    FallBack "Diffuse"
}
