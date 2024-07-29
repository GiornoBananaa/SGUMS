using System;
using System.Linq;
using InputSystem;
using SelectionSystem;
using UnitFormationSystem;
using UnitGroupingSystem;
using UnitSystem;
using UnitSystem.MovementSystem;
using UnityEngine;

namespace OrderSystem
{
    public class FormationDrawOrder : IOrder
    {
        private readonly GroupSelection _groupSelection;
        private readonly FormationDrawer _formationDrawer;
        private readonly FormationSetter _formationSetter;
        private readonly InputListener _inputListener;
        private readonly UnitMover _unitMover;
        private Group _selectedGroup;
        
        public Orders OrderType => Orders.FormationDrawOrder;
        public bool Activated => _groupSelection.Selected.Count() == 1;
        
        public FormationDrawOrder(GroupSelection groupSelection, FormationDrawer formationDrawer, 
            FormationSetter formationSetter, InputListener inputListener, UnitMover unitMover)
        {
            _groupSelection = groupSelection;
            _formationDrawer = formationDrawer;
            _formationSetter = formationSetter;
            _inputListener = inputListener;
            _unitMover = unitMover;
        }
        
        public void Execute()
        {
            _selectedGroup = _groupSelection.Selected.First();
            _formationDrawer.OnLineDrawn += OnFormationDrawn;
            _inputListener.EnableFormationDrawing();
        }

        private void OnFormationDrawn(LineRenderer lineRenderer)
        {
            _formationDrawer.OnLineDrawn -= OnFormationDrawn;
            _inputListener.DisableFormationDrawing();
            Vector3[] linePositions = new Vector3[lineRenderer.positionCount];
            lineRenderer.GetPositions(linePositions);
            
            if(lineRenderer.positionCount < 3) return;
            Vector2[] linePositionsConverted = Array.ConvertAll(linePositions, i => new Vector2(i.x, i.z));
            _formationSetter.EnterFormation(linePositionsConverted, _selectedGroup);
            
            Vector3 sum = Vector3.zero;
            foreach (var position in linePositions)
            {
                sum += position;
            }
            var center = sum/linePositions.Length;
            
            _unitMover.MoveToPoint(_selectedGroup.Units, center);
        }
    }
}