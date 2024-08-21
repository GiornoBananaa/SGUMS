using UnityEngine;
using UnityEngine.Rendering;
using UnityEngine.UI;

namespace FogOfWarSystem.UI
{
    public class CutoutMaskRawImage : Image
    {
        private static readonly int _stencilComp = Shader.PropertyToID("_StencilComp");

        public override Material materialForRendering
        {
            get
            {
                Material material = new Material(base.materialForRendering);
                material.SetInt(_stencilComp, (int)CompareFunction.NotEqual);
                return material;
            }
        }
    }
}
