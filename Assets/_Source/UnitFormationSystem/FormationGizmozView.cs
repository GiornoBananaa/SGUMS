using System.Collections.Generic;
using UnityEngine;
using Zenject;

public class FormationGizmozView : MonoBehaviour
{
    private static List<List<Vector2>> _bounds;
    private static List<List<Vector2>> _positions;
    
    [Inject]
    public void Construct()
    {
        _bounds = new List<List<Vector2>>();
        _positions = new List<List<Vector2>>();
    }
    
    private void OnDrawGizmos()
    {
        DrawBounds();
        DrawPositions();
    }
    
    private void DrawPositions()
    {
        if(_positions == null) return;
        Gizmos.color = Color.yellow;
        foreach (var positions in _positions)
        {
            float count = 0;
            foreach (var position in positions)
            {
                Gizmos.DrawSphere(new Vector3(position.x, 0.3f, position.y), 0.1f);
                Gizmos.color = Color.Lerp(Color.yellow,Color.red,count/positions.Count);
                count++;
            }
        }
    }
    
    private void DrawBounds()
    {
        if(_bounds == null) return;
        Gizmos.color = Color.yellow;
        foreach (var bounds in _bounds)
        {
            for (int i = 0; i < bounds.Count; i++)
            {
                Vector3 point = new Vector3(bounds[i].x, 0.3f, bounds[i].y);
                
                if(i != bounds.Count-1)
                {
                    Vector3 pointNext = new Vector3(bounds[i+1].x, 0.3f, bounds[i+1].y);
                    Gizmos.DrawLine(point, pointNext);
                }
                else
                {
                    Vector3 firstPoint = new Vector3(bounds[0].x, 0.3f, bounds[0].y);
                    Gizmos.DrawLine(point, firstPoint);
                }
            }
        }
    }

    public static void DrawPoints(List<Vector2> points, Vector2 size)
    {
        List<Vector2> drawPoints = new List<Vector2>();
        foreach (var point in points)
        {
            drawPoints.Add(new Vector2(point.x + size.x/2, point.y + size.y/2));
        }
        _positions.Add(drawPoints);
    }
    
    public static void DrawFigure(IEnumerable<Vector2> points, float size)
    {
        List<Vector2> bounds = new List<Vector2>();
        foreach (var point in points)
        {
            bounds.Add(new Vector2(point.x, point.y) * size);
        }
        
        _bounds.Add(bounds);
    }
}
