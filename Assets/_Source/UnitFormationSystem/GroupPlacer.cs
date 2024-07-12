using System.Collections.Generic;
using System.Linq;
using UnitGroupingSystem;
using UnityEngine;

namespace UnitFormationSystem
{
    public class GroupPlacer
    {
        private const float RATIO = 2/1f;
        private const float SPACING = 2.5f;
        
        public void PlaceFormations(IEnumerable<Crowd> groups)
        {
            List<List<Crowd>> placedGroups = new List<List<Crowd>>();
            SortedList<int, float> rowsWidths = new SortedList<int, float>();
            List<float> rowsHeights = new List<float>();
            placedGroups.Add(new List<Crowd>());
            rowsHeights.Add(0);
            rowsWidths.Add(0,0);
            float width = 0;
            float height = 0;
            bool firstElement = true;
            
            // distribution by ratio
            foreach (var group in groups)
            {
                int i;
                if (firstElement)
                {
                    i = 0;
                    
                    firstElement = false;
                }
                else if (width / height < RATIO)
                {
                    i = rowsWidths.Keys.Last();
                }
                else
                {
                    i = placedGroups.Count;
                    placedGroups.Add(new List<Crowd>());
                    rowsHeights.Add(0);
                    rowsWidths.Add(i,0);
                }
                
                placedGroups[i].Add(group);
                rowsWidths[i] += group.Formation.Size.x;
                
                if(rowsHeights[i] < group.Formation.Size.y)
                {
                    height += group.Formation.Size.y - rowsHeights[i];
                    rowsHeights[i] = group.Formation.Size.y;
                }
                if (rowsWidths[i] > width)
                    width = rowsWidths[i];
            }
            
            // rows alignment
            Vector2 lastPosition = new Vector2(0, -height/2);
            
            for (int i = 0; i < placedGroups.Count; i++)
            {
                lastPosition = new Vector2(-rowsWidths[i]/2, lastPosition.y);
                for (int j = 0; j < placedGroups[i].Count; j++)
                {
                    placedGroups[i][j].Offset = lastPosition + new Vector2(placedGroups[i][j].Formation.Size.x/2, placedGroups[i][j].Formation.Size.y/2);
                    lastPosition += new Vector2(placedGroups[i][j].Formation.Size.x + SPACING, 0);
                }
                if(rowsHeights.Count > i + 1)
                    lastPosition = new Vector2(0, lastPosition.y + rowsHeights[i]/2 + rowsHeights[i+1]/2+SPACING);
            }
        }
    }
}