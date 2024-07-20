using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace TeamSystem
{
    [CreateAssetMenu(fileName = "TeamsData", menuName = "SO/TeamsData")]
    public class TeamsDataSO : ScriptableObject
    {
        [field: SerializeField] public TeamData[] Teams { get; private set; }
        private Dictionary<TeamColor, TeamData> _teamByTeamColor;
        
        public Dictionary<TeamColor, TeamData> TeamByTeamColor
        {
            get
            {
                if (_teamByTeamColor == null)
                {
                    _teamByTeamColor = new Dictionary<TeamColor, TeamData>();
                    _teamByTeamColor = Teams.ToDictionary(t=>t.TeamColor);
                }

                return _teamByTeamColor;
            }
        }
    }
}