using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MazeGenerator : MonoBehaviour
{

    public GameObject wall;
    public GameObject wallHolder;
    public static MazeGenerator singleton;

    public float wallLength = 1.0f;
    public int xSize = 5;
    public int ySize = 5;

    public Cell[] cells;
    private int currentCell = 1;
    private int totalCells;
    private int visitedCells = 0;
    private bool startedBuilding = false;
    private int currentNeighbour = 0;
    private List<int> lastCells;
    private int backingUp = 0;
    private int wallToBreak = 0;

    private Vector3 iniPos;

    // Use this for initialization
    void Start()
    {
        singleton = this;
    }

    public void CreateWalls()
    {
        iniPos = new Vector3((-xSize / 2) + wallLength / 2, 0, (-ySize / 2) + wallLength / 2);
        Vector3 myPos = iniPos;
        for (int i = 0; i < ySize; i++)
        {
            for (int j = 0; j <= xSize; j++)
            {
                myPos = new Vector3(iniPos.x + (j * wallLength) - wallLength / 2, 0, iniPos.z + (i * wallLength) - wallLength / 2);
                Instantiate(wall, myPos, Quaternion.identity).transform.SetParent(wallHolder.transform);
            }
        }

        for (int i = 0; i <= ySize; i++)
        {
            for (int j = 0; j < xSize; j++)
            {
                myPos = new Vector3(iniPos.x + (j * wallLength), 0, iniPos.z + (i * wallLength) - wallLength);
                Instantiate(wall, myPos, Quaternion.Euler(0, 90, 0)).transform.SetParent(wallHolder.transform);
            }
        }

        CreateCells();
    }

    void CreateCells()
    {
        lastCells = new List<int>();
        lastCells.Clear();
        totalCells = xSize * ySize;
        int childsAmount = wallHolder.transform.childCount;
        GameObject[] walls = new GameObject[childsAmount];
        cells = new Cell[totalCells];
        int westEastCount = 0;
        int southNorthCount = 0;
        int horizontalCount = 0;

        //Get Walls
        for (int i = 0; i < childsAmount; i++)
        {
            walls[i] = wallHolder.transform.GetChild(i).gameObject;
        }

        //Assign walls to cells
        for (int i = 0; i < cells.Length; i++)
        {
            cells[i] = new Cell();
            cells[i].westWall = walls[westEastCount];
            cells[i].southWall = walls[southNorthCount + (xSize + 1) * ySize];
            if (horizontalCount == xSize)
            {
                westEastCount += 2;
                horizontalCount = 0;
            }
            else
            {
                westEastCount++;
            }
            horizontalCount++;
            southNorthCount++;
            cells[i].eastWall = walls[westEastCount];
            cells[i].northWall = walls[(southNorthCount + (xSize + 1) * ySize) + xSize - 1];

        }
        CreateMaze();
    }

    void CreateMaze()
    {
        while (visitedCells < totalCells)
        {
            if (startedBuilding)
            {
                Neighbour();
                if (!cells[currentNeighbour].visited && cells[currentCell].visited)
                {
                    BreakWall();
                    cells[currentNeighbour].visited = true;
                    visitedCells++;
                    lastCells.Add(currentCell);
                    currentCell = currentNeighbour;
                    if (lastCells.Count > 0)
                    {
                        backingUp = lastCells.Count - 1;
                    }
                }
            }
            else
            {
                currentCell = Random.Range(0, totalCells);
                cells[currentCell].visited = true;
                visitedCells++;
                startedBuilding = true;
            }
        }

        MazeSolver.singleton.Initialize();
    }

    void BreakWall()
    {
        switch (wallToBreak)
        {
            case 1:
                Destroy(cells[currentCell].northWall);
                break;
            case 2: Destroy(cells[currentCell].westWall);
                break;
            case 3: Destroy(cells[currentCell].eastWall);
                break;
            case 4: Destroy(cells[currentCell].southWall);
                break;
        }
    }

    void Neighbour()
    {
        int length = 0;
        int[] neighbours = new int[4];
        int[] connectingWall = new int[4];
        int check = 0;
        check = (currentCell + 1) / xSize;
        check -= 1;
        check *= xSize;
        check += xSize;

        //west
        if (currentCell + 1 < totalCells && (currentCell + 1) != check)
        {
            if (!cells[currentCell + 1].visited)
            {
                neighbours[length] = currentCell + 1;
                connectingWall[length] = 3;
                length++;
            }
        }

        //east
        if (currentCell - 1 >= 0 && currentCell != check)
        {
            if (!cells[currentCell - 1].visited)
            {
                neighbours[length] = currentCell - 1;
                connectingWall[length] = 2;
                length++;
            }
        }

        //north
        if (currentCell + xSize < totalCells)
        {
            if (!cells[currentCell + xSize].visited)
            {
                neighbours[length] = currentCell + xSize;
                connectingWall[length] = 1;
                length++;
            }
        }

        //south
        if (currentCell - xSize >= 0)
        {
            if (!cells[currentCell - xSize].visited)
            {
                neighbours[length] = currentCell - xSize;
                connectingWall[length] = 4;
                length++;
            }
        }

        if (length != 0)
        {
            int chosenCell = Random.Range(0, length);
            currentNeighbour = neighbours[chosenCell];
            wallToBreak = connectingWall[chosenCell];
        }
        else
        {
            if (backingUp > 0)
            {
                currentCell = lastCells[backingUp];
                backingUp--;
            }
        }
    }

    // Update is called once per frame
    void Update()
    {

    }
}
