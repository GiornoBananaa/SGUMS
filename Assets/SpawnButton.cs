using System;
using System.Collections.Generic;
using TeamSystem;
using TMPro;
using UnitSystem;
using UnitSystem.UnitFactories;
using UnityEngine;
using UnityEngine.UI;
using Zenject;

public class SpawnButton : MonoBehaviour
{
    private UnitSpawner _unitSpawner;
    [SerializeField] private TMP_Dropdown _typeDropdown;
    [SerializeField] private TMP_Dropdown _teamDropdown;
    [SerializeField] private TMP_InputField _xInputField;
    [SerializeField] private TMP_InputField _zInputField;

    private void Awake()
    {
        _typeDropdown.options = new List<TMP_Dropdown.OptionData>();
        for(int i = 0; i < Enum.GetNames(typeof(UnitType)).Length; i++)
        {
            _typeDropdown.options.Add(new TMP_Dropdown.OptionData(Enum.GetName(typeof(UnitType), i)));
        }
        _teamDropdown.options = new List<TMP_Dropdown.OptionData>();
        for(int i = 0; i < Enum.GetNames(typeof(TeamColor)).Length; i++)
        {
            _teamDropdown.options.Add(new TMP_Dropdown.OptionData(Enum.GetName(typeof(TeamColor), i)));
        }
    }

    [Inject]
    public void Construct(UnitSpawner unitSpawner)
    {
        _unitSpawner = unitSpawner;
    }
    
    public void Spawn()
    {
        _unitSpawner.SpawnUnit(new Vector3(int.Parse(_xInputField.text), 0, int.Parse(_zInputField.text)),(UnitType)_typeDropdown.value,(TeamColor)_teamDropdown.value);
    }
}