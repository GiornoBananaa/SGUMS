using System;
using System.Collections.Generic;
using UnitGroupingSystem;
using UnityEngine;

namespace UnitFormationSystem
{
    public class FormationSetter
    {
        public Action<Formation> OnFormation;
        private const float UNIT_SPACING = 1.5f;
        
        public void EnterFormation(Vector2[] formationBounds, Crowd crowd)
        {
            Formation formation = DistributePoints(formationBounds, crowd.Units.Count);
            
            if(formation == null) return;
            
            int pointIndex = 0;
            crowd.Formation = formation;
            foreach (var unit in crowd.Units)
            {
                unit.PathOffset = formation.Positions[pointIndex];
                pointIndex++;
            }
            
            OnFormation?.Invoke(formation);
        }
        
        public Formation DistributePoints(Vector2[] boundaryPoints, int numPoints)
        {
            float width = boundaryPoints[0].x;
            float height = boundaryPoints[0].y;
            
            float lowestXPoint = boundaryPoints[0].x;
            float lowestYPoint = boundaryPoints[0].y;
            
            foreach (var point in boundaryPoints)
            {
                if (point.x < lowestXPoint)
                    lowestXPoint = point.x;
                if (point.y < lowestYPoint)
                    lowestYPoint = point.y;
                if(point.x > width)
                    width = point.x;
                if(point.y > height)
                    height = point.y;
            }
            
            width -= lowestXPoint;
            height -= lowestYPoint;
            
            if (width == 0 || height == 0) return null;

            float diagonalLength = Mathf.Sqrt(Mathf.Pow(width,2) + Mathf.Pow(height,2));
            
            width /= diagonalLength;
            height /= diagonalLength;
            
            for (int i = 0; i < boundaryPoints.Length; i++)
            {
                boundaryPoints[i] = new Vector2(boundaryPoints[i].x - lowestXPoint, boundaryPoints[i].y - lowestYPoint) / diagonalLength;
            }
            
            List<Vector2> points = new List<Vector2>();
            float sideRatio = width / height;
            int intersectionsY = Mathf.CeilToInt(Mathf.Sqrt(numPoints / sideRatio));
            int intersectionsX = Mathf.CeilToInt(intersectionsY * sideRatio);
            int loops = 0;
            float cellSize = height / (intersectionsY - 1);
            
            while (points.Count < numPoints && loops < 200)
            {
                if(width < height && loops != 0)
                {
                    intersectionsY++;
                    intersectionsX = Mathf.CeilToInt(intersectionsY * sideRatio);
                    cellSize = height / (intersectionsY-1);
                }
                else if(loops != 0)
                {
                    intersectionsX++;
                    intersectionsY = Mathf.CeilToInt(intersectionsX * (1 / sideRatio));
                    cellSize = height / (intersectionsY-1);
                }
                points.Clear();
                int spiralLoop = 0;
                foreach (var coordinates in SpiralTraversal(intersectionsX, intersectionsY))
                {
                    spiralLoop++;
                    if (spiralLoop > 200) break;
                    (int i, int j) = coordinates;
                    Vector2 point = new Vector2(i * cellSize, j * cellSize);
                    if (IsPointInPolygon(point, boundaryPoints))
                    {
                        points.Add(point * (UNIT_SPACING / cellSize) - new Vector2((intersectionsX - 1) / 2f, (intersectionsY - 1) / 2f) * UNIT_SPACING);
                    }

                    if (points.Count >= numPoints)
                        break;
                }
                loops++;
            }
            FormationGizmozView.DrawFigure(boundaryPoints, intersectionsX * UNIT_SPACING);
            FormationGizmozView.DrawPoints(points, new Vector2(intersectionsX - 1, intersectionsY - 1) * UNIT_SPACING);
            return new Formation(points, boundaryPoints, new Vector2(intersectionsX - 1, intersectionsY - 1) * UNIT_SPACING);
        }
        
        
        private bool IsPointInPolygon(Vector2 point, Vector2[] polygon)
        {
            int numPoints = polygon.Length;
            bool isInside = false;
            
            for (int i = 0, j = numPoints - 1; i < numPoints; j = i++)
            {
                // Check if the point is on the edge of the polygon
                if ((polygon[i].x - point.x) * (polygon[j].y - point.y) - (polygon[j].x - point.x) * (polygon[i].y - point.y) == 0 &&
                    (polygon[i].x - point.x) * (polygon[j].x - point.x) <= 0 &&
                    (polygon[i].y - point.y) * (polygon[j].y - point.y) <= 0)
                {
                    return true;
                }

                // Check if the point is inside the polygon
                if ((polygon[i].y <= point.y && point.y < polygon[j].y) || (polygon[j].y <= point.y && point.y < polygon[i].y))
                {
                    if (point.x < (polygon[j].x - polygon[i].x) * (point.y - polygon[i].y) / (polygon[j].y - polygon[i].y) + polygon[i].x)
                    {
                        isInside = !isInside;
                    }
                }
            }
            
            return isInside;
        }
        
        public static IEnumerable<(int,int)> SpiralTraversal(int width, int height)
        {
            int rows = width;
            int cols = height;
            int centerRow = rows / 2;
            int centerCol = cols / 2;
            int radius = Math.Min((rows + 1) / 2, (cols + 1) / 2) ;
            
            yield return (centerRow,centerCol);
            for (int r = 0; r <= radius; r++)
            {
                
                // Traverse top row
                for (int i = centerCol - r + 1; i <= centerCol + r; i++)
                    yield return (centerRow - r, i);
                
                // Traverse right column
                for (int i = centerRow - r + 1; i <= centerRow + r; i++)
                    yield return (i, centerCol + r);
                
                // Traverse bottom row (if there are any elements left)
                if (centerRow + r <= rows - 1)
                {
                    for (int i = centerCol + r - 1; i >= centerCol - r; i--)
                        yield return (centerRow + r, i);
                }
                
                // Traverse left column (if there are any elements left)
                if (centerCol - r >= 0)
                {
                    for (int i = centerRow + r - 1; i > centerRow - r-1; i--)
                        yield return (i, centerCol - r);
                }
            }
        }
    }
}
