using UnityEngine;

namespace SelectionSystem
{
    public class GUIAreaSelectionView : MonoBehaviour, IAreaSelectionView
    {
        private Texture2D _whiteTexture;
        private bool _isDrawing;
        private Vector2 _areaStart;
        private Vector2 _dragPoint;
        
        private Texture2D WhiteTexture
        {
            get
            {
                if (_whiteTexture == null)
                {
                    _whiteTexture = new Texture2D(1, 1);
                    _whiteTexture.SetPixel(0, 0, Color.white);
                    _whiteTexture.Apply();
                }

                return _whiteTexture;
            }
        }
        
        private void OnGUI()
        {
            if (_isDrawing)
            {
                var rect = GetScreenRect(_areaStart, _dragPoint);
                DrawScreenRect(rect, new Color(0.8f, 0.8f, 0.95f, 0.1f));
                DrawScreenRectBorder(rect, 1, Color.green);
            }
        }
        
        public void EnableView(Vector2 startPoint)
        {
            _isDrawing = true;
            _areaStart = startPoint;
            _dragPoint = startPoint;
        }

        public void SetDragPoint(Vector2 dragPoint)
        {
            _dragPoint = dragPoint;
        }

        public void DisableView()
        {
            _isDrawing = false;
        }
        
        public Rect GetScreenRect(Vector3 screenPosition1, Vector3 screenPosition2)
        {
            screenPosition1.y = Screen.height - screenPosition1.y;
            screenPosition2.y = Screen.height - screenPosition2.y;
            
            return Rect.MinMaxRect(screenPosition1.x, screenPosition1.y, screenPosition2.x, screenPosition2.y);
        }
        
        public void DrawScreenRect(Rect rect, Color color)
        {
            GUI.color = color;
            GUI.DrawTexture(rect, WhiteTexture);
        }

        public void DrawScreenRectBorder(Rect rect, float thickness, Color color)
        {
            //Top
            DrawScreenRect(new Rect(rect.xMin, rect.yMin, rect.width, thickness), color);
            // Left
            DrawScreenRect(new Rect(rect.xMin, rect.yMin, thickness, rect.height), color);
            // Right
            DrawScreenRect(new Rect(rect.xMax - thickness, rect.yMin, thickness, rect.height), color);
            // Bottom
            DrawScreenRect(new Rect(rect.xMin, rect.yMax - thickness, rect.width, thickness), color);
        }
    }
}