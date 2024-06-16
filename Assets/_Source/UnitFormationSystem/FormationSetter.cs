using System;
using System.Collections.Generic;
using SelectionSystem;
using UnityEngine;

namespace UnitFormationSystem
{
    public class FormationSetter
    {
        private UnitSelection _unitSelection;

        public FormationSetter(UnitSelection unitSelection)
        {
            _unitSelection = unitSelection;
        }
        
        public void EnterFormation(Vector2[] formation)
        {
            List<Vector2> points = DistributePoints(formation, _unitSelection.SelectedCount);
            int pointIndex = 0;
            
            foreach (var unit in _unitSelection.Selected)
            {
                unit.PathOffset = points[pointIndex];
                pointIndex++;
            }
        }
        
        public List<Vector2> DistributePoints(Vector2[] boundaryPoints, int numPoints, float minDistance = 1)
        {
            float width = 0;
            float height = 0;
            foreach (var point in boundaryPoints)
            {
                if(point.x > width)
                    width = point.x;
                if(point.y > height)
                    height = point.y;
            }
            
            List<Vector2> points = new List<Vector2>();
            int cellsCount = Mathf.CeilToInt(Mathf.Sqrt(numPoints));
            float cellSize = 0;
            int numCellsX = 0;
            int numCellsY = 0;

            while (points.Count < numPoints)
            {
                points.Clear();
                if(width < height)
                {
                    cellSize = width / cellsCount;
                    numCellsX = cellsCount;
                    numCellsY = (int)(height / cellSize);
                }
                else
                {
                    cellSize = height / cellsCount;
                    numCellsX = Mathf.CeilToInt(width / cellSize);
                    numCellsY = (int)(height / cellSize);
                }
                
                for (int i = 0; i < numCellsX; i++)
                {
                    for (int j = 0; j < numCellsY; j++)
                    {
                        Vector2 point = new Vector2(i * cellSize, j * cellSize);
                        if (IsPointInPolygon(point, boundaryPoints))
                        {
                            points.Add(point);
                        }
                    }
                }
                
                cellsCount++;
            }
            
            for (int i = 0; i < points.Count; i++)
            {
                points[i] = points[i] * (1+cellSize/minDistance) - new Vector2(width/2, height/2);
            }
            
            return points;
        }

        private bool IsPointInPolygon(Vector2 point, Vector2[] polygon)
        {
            bool isInside = false;
            for (int i = 0, j = polygon.Length - 1; i < polygon.Length; j = i++)
            {
                if (((polygon[i].y > point.y) != (polygon[j].y > point.y)) &&
                    (point.x < (polygon[j].x - polygon[i].x) * (point.y - polygon[i].y) / (polygon[j].y - polygon[i].y) + polygon[i].x))
                {
                    isInside = !isInside;
                }
            }
            return isInside;
        }
        
        public float CalculateArea(Vector2[] vertices)
        {
            float area = 0;
            int j = vertices.Length - 1;

            for (int i = 0; i < vertices.Length; i++)
            {
                area += (vertices[j].x + vertices[i].x) * (vertices[j].y - vertices[i].y);
                j = i;
            }

            return Math.Abs(area / 2);
        }
    }
}
