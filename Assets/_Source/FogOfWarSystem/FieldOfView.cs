using UnityEngine;

namespace FogOfWarSystem
{
    public class FieldOfView : MonoBehaviour
    {
        private const float MASK_Y_SIZE = 150;
        
        [SerializeField] private MeshRenderer _meshRenderer;

        public void SetSize(float size)
        {
            _meshRenderer.transform.localScale = new Vector3(size, MASK_Y_SIZE, size);
        }
    }
}