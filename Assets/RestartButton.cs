using System;
using System.Threading.Tasks;
using LandscapeSystem;
using UnityEngine;
using UnityEngine.SceneManagement;
using Zenject;

public class RestartButton : MonoBehaviour
{
    private LandscapeGenerator _landscapeGenerator;

    private async void Start()
    {
        await Loading();
    }

    private async Task Loading()
    {
        await _landscapeGenerator.GenerateTerrain();
        _landscapeGenerator.GenerateTerrainObjects();
        _landscapeGenerator.BuildNavMesh();
    }
    
    [Inject]
    public void Construct(LandscapeGenerator landscapeGenerator)
    {
        _landscapeGenerator = landscapeGenerator;
    }

    public void Restart()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }
}