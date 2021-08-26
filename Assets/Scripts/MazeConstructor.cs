using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MazeConstructor : MonoBehaviour
{
    public bool displayGrid;
    public bool displayGridValues;
    MazeDataGenerator dataGenerator;
    MazeMeshGenerator meshGenerator;

    [SerializeField] private Material mazeFloorMat;
    [SerializeField] private Material mazeWallMat;

    [Range(10, 50)] public byte mazeHeight;
    [Range(10, 50)] public byte mazeWidth;

    public float hallWidth;
    public float hallHeight;

    byte[,] data;

    byte startRow;
    byte finishRow;

    void Start()
    {
        dataGenerator = new MazeDataGenerator(mazeHeight, mazeWidth);
        meshGenerator = new MazeMeshGenerator(hallWidth, hallHeight);

        startRow = (byte)Random.Range(0, mazeHeight);
        finishRow = (byte)Random.Range(0, mazeHeight);

        data = dataGenerator.BuildMaze(0, 0);
        DisplayMaze();
    }

    void OnGUI()
    {
        if (displayGrid)
        {
            GUI.Label(new Rect(20, 20, 500, 500), PrintMaze());
        }

        if (displayGridValues)
        {
            GUI.Label(new Rect(220, 20, 500, 500), PrintGridValues());
        }
    }

    public string PrintMaze()
    {
        string msg = " ";

        for (int x = 0; x < (data.GetLength(1) * 2 - 1); x++)
        {
            msg += "_ ";
        }

        msg += "\n";

        for (int y = 0; y < data.GetLength(0); y++)
        {
            msg += (y != startRow) ? "|" : " ";

            for (int x = 0; x < data.GetLength(1); x++)
            {
                msg += ((data[y, x] & 2) != 0) ? "   " : " _";

                if ((data[y, x] & 4) != 0)
                {
                    msg += (((data[y, x] | data[y, x + 1]) & 2) != 0) ? "   " : " _";
                }
                else
                {
                    msg += (y == finishRow && x == data.GetLength(0) - 1) ? " " : " |";
                }
            }

            msg += "\n";
        }

        return msg;
    }

    public string PrintGridValues()
    {
        string msg = "";
        msg += "\n";
        for (int y = 0; y < data.GetLength(0); y++)
        {
            for (int x = 0; x < data.GetLength(1); x++)
            {
                msg += " " + data[y, x] + " ";
            }
            msg += "\n";
        }
        return msg;
    }

    private void DisplayMaze()
    {
        GameObject go = new GameObject();
        go.transform.position = Vector3.zero;
        go.name = "Procedural Maze";

        MeshFilter mf = go.AddComponent<MeshFilter>();
        mf.mesh = meshGenerator.BuildMesh(data);

        //MeshCollider mc = go.AddComponent<MeshCollider>();
        MeshCollider mc = go.AddComponent<MeshCollider>();
        mc.sharedMesh = mf.mesh;

        MeshRenderer mr = go.AddComponent<MeshRenderer>();
        mr.materials = new Material[2] { mazeFloorMat, mazeWallMat };
    }
}
