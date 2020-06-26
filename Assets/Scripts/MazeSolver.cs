using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MazeSolver : MonoBehaviour
{
    public static MazeSolver singleton;
    public Cell[] cells;
    public int xSize;
    public int ySize;
    public Cell firstCell;
    public Cell lastCell;
    public GameObject startPlace;
    public GameObject endPlace;
    public GameObject track;

    private bool finished = false;

    private void Awake()
    {
        singleton = this;
    }

    public void Initialize()
    {
        cells = MazeGenerator.singleton.cells;
        xSize = MazeGenerator.singleton.xSize;
        ySize = MazeGenerator.singleton.ySize;
        firstCell = cells[0];
        lastCell = cells[cells.Length - 1];

        //Define las posiciones centrales de cada celda
        int xC = 0;
        int yC = 0;
        Vector3 iniPos = firstCell.westWall.transform.position;
        iniPos.x += 0.5f;
        for (int i = 0; i < cells.Length; i++)
        {
            cells[i].position = new Vector3(iniPos.x + xC, 0, iniPos.z + yC);
            if (xC == xSize - 1)
            {
                xC = 0;
                yC++;
            }
            else
            {
                xC++;
            }
        }

        //Posiciona el Comienzo y el Fin.
        Instantiate(startPlace, firstCell.position + Vector3.down, Quaternion.identity);
        Instantiate(endPlace, lastCell.position + Vector3.down, Quaternion.identity);

        CreateNeighbours();
    }

    public void CreateNeighbours()
    {
        int xC = 0;
        int yC = 0;
        int actualPos;
        int i = 0;

        //Define los vecinos para cada Celda
        while (yC < ySize)
        {
            actualPos = xC + (xSize * yC);

            //Define el vecino Arriba
            if (yC < ySize - 1) cells[i].northNeighbour = cells[actualPos + xSize];

            //Define el vecino Abajo
            if (yC > 0) cells[i].southNeighbour = cells[actualPos - xSize];

            //Define el vecino Izquierda
            if (xC > 0) cells[i].westNeighbour = cells[actualPos - 1];

            //Define el vecino Derecha
            if (xC < xSize - 1) cells[i].eastNeighbour = cells[actualPos + 1];

            if (xC == xSize - 1)
            {
                xC = 0;
                yC++;
            }
            else xC++;
            i++;
        }
    }

    public void StartSolving()
    {
        SolveMaze(firstCell);
    }

    public void SolveMaze(Cell cell)
    {
        if ((cell != null && !cell.solvingVisited) && !finished)
        {
            if (cell != lastCell)
            {
                cell.solvingVisited = true;
                GameObject trackPoint = Instantiate(track, cell.position, Quaternion.identity);
                if (cell.northWall == null) SolveMaze(cell.northNeighbour);
                if (cell.eastWall == null) SolveMaze(cell.eastNeighbour);
                if (cell.southWall == null) SolveMaze(cell.southNeighbour);
                if (cell.westWall == null) SolveMaze(cell.westNeighbour);

                if (!finished) DestroyImmediate(trackPoint);
                return;
            }
            else
            {
                GameObject trackPoint = Instantiate(track, cell.position, Quaternion.identity);
                finished = true;
                return;
            }
        }
        else
        {
            return;
        }
    }

}
