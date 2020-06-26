using UnityEngine;

[System.Serializable]
public class Cell{
    public bool visited;
    public GameObject northWall;
    public GameObject westWall;
    public GameObject eastWall;
    public GameObject southWall;

    public bool solvingVisited;
    public Cell northNeighbour;
    public Cell eastNeighbour;
    public Cell westNeighbour;
    public Cell southNeighbour;

    public Vector3 position;
}
