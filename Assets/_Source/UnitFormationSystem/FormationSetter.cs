using System;
using System.Collections.Generic;
using System.Linq;
using SelectionSystem;
using UnitSystem;
using UnityEngine;

namespace UnitFormationSystem
{
    public class FormationSetter
    {
        private UnitSelection _unitSelection;

        public Action<List<Vector2>, float> OnFormation;
        private float _formationSize;
        
        public FormationSetter(UnitSelection unitSelection)
        {
            _unitSelection = unitSelection;
        }
        
        public void EnterFormation(Vector2[] formation, IEnumerable<Unit> units = null)
        {
            units ??= _unitSelection.Selected;
            List<Vector2> points = DistributePoints(formation, units.Count());
            int pointIndex = 0;
            foreach (var unit in units)
            {
                unit.PathOffset = points[pointIndex];
                pointIndex++;
            }
            OnFormation?.Invoke(formation.ToList(), _formationSize);
        }
        
        public List<Vector2> DistributePoints(Vector2[] boundaryPoints, int numPoints, float unitsSpacing = 1.5f)
        {
            float width = 0;
            float height = 0;

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

            float maxValue = width < height ? height : width;
            
            for (int i = 0; i < boundaryPoints.Length; i++)
            {
                boundaryPoints[i] = new Vector2((boundaryPoints[i].x - lowestXPoint) / maxValue, (boundaryPoints[i].y - lowestYPoint) / maxValue);
            }

            width /= maxValue;
            height /= maxValue;
            
            List<Vector2> points = new List<Vector2>();
            int cellsCount = Mathf.CeilToInt(Mathf.Sqrt(numPoints));
            float cellSize = 0;
            int numCellsX = 0;
            int numCellsY = 0;
            int bugBuff = 0;
            
            while (points.Count < numPoints && bugBuff < 200)
            {
                bugBuff++;
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

            float sizeModifier = unitsSpacing / cellSize;
            _formationSize = sizeModifier;
            
            for (int i = 0; i < points.Count; i++)
            {
                points[i] = (points[i] - new Vector2(width/2, height/2)) * sizeModifier;
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
    }
}
