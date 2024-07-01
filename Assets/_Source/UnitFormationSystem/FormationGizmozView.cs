using System.Collections.Generic;
using UnitFormationSystem;
using UnityEngine;
using Zenject;

public class FormationGizmozView : MonoBehaviour
{
    private FormationSetter _formationSetter;

    private List<List<Vector2>> _formations;
    
    [Inject]
    public void Construct(FormationSetter formationSetter)
    {
        _formationSetter = formationSetter;
        _formationSetter.OnFormation += AddFormation;
        _formations = new List<List<Vector2>>();
    }
    
    private void OnDrawGizmos()
    {
        if(_formations == null) return;
        Gizmos.color = Color.yellow;
        foreach (var formation in _formations)
        {
            for (int i = 0; i < formation.Count; i++)
            {
                Vector3 point = new Vector3(formation[i].x, 0.3f, formation[i].y);
                
                if(i != formation.Count-1)
                {
                    Vector3 pointNext = new Vector3(formation[i+1].x, 0.3f, formation[i+1].y);
                    Gizmos.DrawLine(point, pointNext);
                }
                else
                {
                    Vector3 firstPoint = new Vector3(formation[0].x, 0.3f, formation[0].y);
                    Gizmos.DrawLine(point, firstPoint);
                }
            }
        }
    }

    private void AddFormation(List<Vector2> formation, float size)
    {
        for (int i = 0; i < formation.Count; i++)
        {
            formation[i] *= size;
        }
        _formations.Add(formation);
    }
    
    private void OnDestroy()
    {
        _formationSetter.OnFormation -= AddFormation;
    }
}
