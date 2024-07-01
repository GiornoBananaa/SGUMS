using System;
using System.Linq;
using InputSystem;
using SelectionSystem;
using UnitFormationSystem;
using UnitSystem;
using UnitSystem.MovementSystem;
using UnityEngine;

namespace OrderSystem
{
    public class FormationDrawOrder : IOrder
    {
        private readonly UnitSelection _unitSelection;
        private readonly FormationDrawer _formationDrawer;
        private readonly FormationSetter _formationSetter;
        private readonly InputListener _inputListener;
        private readonly UnitMover _unitMover;
        
        
        public Orders OrderType => Orders.FormationDrawOrder;
        public bool Activated => _unitSelection.Selected.Count() > 1;
        
        public FormationDrawOrder(UnitSelection unitSelection, FormationDrawer formationDrawer, 
            FormationSetter formationSetter, InputListener inputListener, UnitMover unitMover)
        {
            _unitSelection = unitSelection;
            _formationDrawer = formationDrawer;
            _formationSetter = formationSetter;
            _inputListener = inputListener;
            _unitMover = unitMover;
        }
        
        public void Execute()
        {
            _formationDrawer.OnLineDrawn += OnFormationDrawn;
            _inputListener.EnableFormationDrawing();
        }

        private void OnFormationDrawn(LineRenderer lineRenderer)
        {
            _formationDrawer.OnLineDrawn -= OnFormationDrawn;
            Vector3[] linePositions = new Vector3[lineRenderer.positionCount];
            lineRenderer.GetPositions(linePositions);
            Vector2[] linePositionsConverted = Array.ConvertAll(linePositions, i => new Vector2(i.x, i.z));
            _inputListener.DisableFormationDrawing();
            _formationSetter.EnterFormation(linePositionsConverted);
            
            Vector3 sum = Vector3.zero;
            foreach (var unit in _unitSelection.Selected)
            {
                sum += unit.transform.position;
            }
            var center = sum/_unitSelection.SelectedCount;
            
            _unitMover.MoveToPoint(center);
        }
    }
}