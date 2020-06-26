using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class UIManager : MonoBehaviour
{

    public Slider columnas;
    public Text columnasTxt;
    public Slider filas;
    public Text filasTxt;

    private void Start()
    {
        columnasTxt.text = "" + columnas.value;
        filasTxt.text = "" + filas.value;
    }

    public void ValueChanged()
    {
        columnasTxt.text = "" + columnas.value;
        filasTxt.text = "" + filas.value;
    }

    public void CreateMaze()
    {
        MazeGenerator.singleton.xSize = (int)columnas.value;
        MazeGenerator.singleton.ySize = (int)filas.value;

        MazeGenerator.singleton.CreateWalls();

        float higher = (filas.value > columnas.value) ? filas.value : columnas.value;
        Camera.main.orthographicSize = higher / 2 + higher / 10;
    }

    public void ResetGame()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }
}
