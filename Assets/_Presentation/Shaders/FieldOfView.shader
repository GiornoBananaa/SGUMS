Shader "Unlit/FieldOfView"
{
    SubShader 
    {
        Tags { "RenderType"="Opaque"}
        
        Blend Zero One
        ZWrite Off
        
        Pass 
        {
            Stencil
            {
                Ref 5
                Comp Always
                Pass Replace
            }
        }
    }
}
