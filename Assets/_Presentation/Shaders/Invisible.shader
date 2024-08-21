Shader "Unlit/Invisible"
{
    Properties 
    {
        _Color ("Color", Color) = (1, 1, 1, 1)
    }
    SubShader
    {
        Tags { "RenderType"="Opaque"}
        
        Blend Zero One
        ZWrite Off
        Pass{}
    }
}