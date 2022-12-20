Shader "CustomRenderTexture/WormShader"
{
    Properties
    {
        _Tint ("Tint", Color) = (1,1,1,1)
        _AngleOffset ("Angle Offset", float) = 0
        _CellDensity ("Cell Density", float) = 1
        _CellSparsity ("Cell Sparsity", float) = 1
    }

    SubShader
    {
        Lighting Off
        Blend One Zero

        Pass
        {
            CGPROGRAM
            #include "UnityCustomRenderTexture.cginc"
            #pragma vertex CustomRenderTextureVertexShader
            #pragma fragment frag
            #pragma target 3.0

            float4 _Tint;
            
            float _AngleOffset;
            float _CellDensity;
            float _CellSparsity;

            inline float2 randomVector(float2 uv, float offset)
            {
                float2x2 m = float2x2(15.27, 47.63, 99.41, 89.98);
                uv = frac(sin(mul(uv, m)) * 46839.32);
                return float2(sin(uv.y*+ offset) * 0.5 + 0.5, cos(uv.x * offset) * 0.5 + 0.5);
            }

            float2 voronoi(float2 uv, float angleOffset, float cellDensity)
            {
                float2 g = floor(uv * cellDensity);
                float2 f = frac(uv * cellDensity);
                float3 res = float3(8.0, 0.0, 0.0);

                for(int y = -1; y <= 1; y++)
                {
                    for(int x = -1; x <= 1; x++)
                    {
                        float2 lattice = float2(x, y);
                        float2 offset = randomVector(lattice + g, angleOffset);
                        float d = distance(lattice + offset, f);
                        if(d < res.x)
                        {
                            res = float3(d, offset.x, offset.y);
                        }
                    }
                }
                return res;
            }
                        
            float4 frag(v2f_customrendertexture IN) : COLOR
            {
                float2 v = voronoi(IN.globalTexcoord, _AngleOffset + _Time * 20, _CellDensity * _CellSparsity);
                float4 col = 1 - v.x / _CellSparsity;
                
                col.a = 1;
                
                return col * _Tint;
            }
            ENDCG
        }
    }
}