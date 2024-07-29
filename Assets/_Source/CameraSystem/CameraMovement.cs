using System;
using UnityEngine;

namespace CameraSystem
{
    public class CameraMovement : MonoBehaviour
    {
        [SerializeField] private Transform _camera;

        private const float UP_BORDER = 430f;
        private const float DOWN_BORDER = 87f;
        private const float X_BORDER = 910f;
        private const float Z_BORDER = 750f;

        private float _scrollSensitivity = 10f;
        private float _cameraSpeed = 50f;

        private bool _xMove;
        private bool _yMove;
        private bool _zMove;
        private bool _sprint;
        
        private void LateUpdate()
        {
            MoveCamera();
        }

        private void MoveCamera()
        {
            float x = 0;
            float y = 0;
            float z = 0;

            if (Input.GetKey(KeyCode.W))
                z += _cameraSpeed * Time.deltaTime;
            if (Input.GetKey(KeyCode.S))
                z -= _cameraSpeed * Time.deltaTime;
            if (Input.GetKey(KeyCode.D))
                x += _cameraSpeed * Time.deltaTime;
            if (Input.GetKey(KeyCode.A))
                x -= _cameraSpeed * Time.deltaTime;
            if (Input.GetAxis("Mouse ScrollWheel") > 0f)
                y += _scrollSensitivity;
            if (Input.GetAxis("Mouse ScrollWheel") < 0f)
                y -= _scrollSensitivity;
            if (Input.GetKey(KeyCode.LeftShift)) 
            {
                x *= 2;
                z *= 2;
            }
            
            Vector3 newPos = _camera.localPosition + new Vector3(
                Math.Abs(_camera.localPosition.x + x) < X_BORDER ? x : 0, 
                0, 
                Math.Abs(_camera.localPosition.z + z) < Z_BORDER ? z : 0);
            _camera.localPosition = newPos;

            newPos = _camera.position + _camera.forward * ((_camera.position + _camera.forward * y).y < UP_BORDER && (_camera.position + _camera.forward * y).y > DOWN_BORDER ? y:0);
            _camera.position = newPos;
        }
    }
}
