using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MazeMeshGenerator
{
    float hallWidth;
    float hallHeight;

    public MazeMeshGenerator(float hallWidth, float hallHeight)
    {
        this.hallWidth = hallWidth;
        this.hallHeight = hallHeight;
    }

    public Mesh BuildMesh(byte[,] data)
    {
        Mesh maze = new Mesh();

        List<Vector3> newVertices = new List<Vector3>();
        List<Vector2> newUVs = new List<Vector2>();

        maze.subMeshCount = 2;
        List<int> floorTriangles = new List<int>();
        List<int> wallTriangles = new List<int>();

        int rMax = data.GetLength(0);
        int cMax = data.GetLength(1);

        //Debug.Log("row: " + rMax);
        //Debug.Log("column: " + cMax);

        float halfH = hallHeight * 0.5f;

        for (int i = 0; i < rMax; i++)
        {
            for (int j = 0; j < cMax; j++)
            {
                // Outer walls
                if (i - 1 < 0)
                {
                    AddQuad(Matrix4x4.TRS(
                        new Vector3(j * hallWidth, halfH, -1 * (i - 0.5f) * hallWidth),
                        Quaternion.LookRotation(Vector3.forward),
                        new Vector3(hallWidth, hallHeight, 1)
                    ), ref newVertices, ref newUVs, ref wallTriangles);
                }

                if (i + 1 == rMax)
                {
                    AddQuad(Matrix4x4.TRS(
                        new Vector3(j * hallWidth, halfH, -1 * (i + 0.5f) * hallWidth),
                        Quaternion.LookRotation(Vector3.back),
                        new Vector3(hallWidth, hallHeight, 1)
                    ), ref newVertices, ref newUVs, ref wallTriangles);
                }

                if (j - 1 < 0)
                {
                    AddQuad(Matrix4x4.TRS(
                        new Vector3((j - 0.5f) * hallWidth, halfH, -1 * i * hallWidth),
                        Quaternion.LookRotation(Vector3.left),
                        new Vector3(hallWidth, hallHeight, 1)
                    ), ref newVertices, ref newUVs, ref wallTriangles);
                }

                if (j + 1 == cMax)
                {
                    AddQuad(Matrix4x4.TRS(
                        new Vector3((j + 0.5f) * hallWidth, halfH, -1 * i * hallWidth),
                        Quaternion.LookRotation(Vector3.right),
                        new Vector3(hallWidth, hallHeight, 1)
                    ), ref newVertices, ref newUVs, ref wallTriangles);
                }

                if (data[i, j] != 0)
                {
                    // floor
                    AddQuad(Matrix4x4.TRS(
                        new Vector3(j * hallWidth, 0, -1 * i * hallWidth),
                        Quaternion.LookRotation(Vector3.up),
                        new Vector3(hallWidth, hallWidth, 1)
                    ), ref newVertices, ref newUVs, ref floorTriangles);

                    // ceiling
                    AddQuad(Matrix4x4.TRS(
                        new Vector3(j * hallWidth, hallHeight, -1 * i * hallWidth),
                        Quaternion.LookRotation(Vector3.down),
                        new Vector3(hallWidth, hallWidth, 1)
                    ), ref newVertices, ref newUVs, ref floorTriangles);

                    switch (data[i, j])
                    {
                        // North segment
                        case 1:
                            AddQuad(Matrix4x4.TRS(
                                new Vector3((j + 0.5f) * hallWidth, halfH, -1 * i * hallWidth),
                                Quaternion.LookRotation(Vector3.left),
                                new Vector3(hallWidth, hallHeight, 1)
                            ), ref newVertices, ref newUVs, ref wallTriangles);

                            AddQuad(Matrix4x4.TRS(
                                new Vector3(j * hallWidth, halfH, -1 * (i + 0.5f) * hallWidth),
                                Quaternion.LookRotation(Vector3.forward),
                                new Vector3(hallWidth, hallHeight, 1)
                            ), ref newVertices, ref newUVs, ref wallTriangles);

                            AddQuad(Matrix4x4.TRS(
                                new Vector3((j - 0.5f) * hallWidth, halfH, -1 * i * hallWidth),
                                Quaternion.LookRotation(Vector3.right),
                                new Vector3(hallWidth, hallHeight, 1)
                            ), ref newVertices, ref newUVs, ref wallTriangles);

                            break;

                        // South segment
                        case 2:
                            AddQuad(Matrix4x4.TRS(
                                new Vector3((j - 0.5f) * hallWidth, halfH, -1 * i * hallWidth),
                                Quaternion.LookRotation(Vector3.right),
                                new Vector3(hallWidth, hallHeight, 1)
                            ), ref newVertices, ref newUVs, ref wallTriangles);

                            AddQuad(Matrix4x4.TRS(
                                new Vector3(j * hallWidth, halfH, -1 * (i - 0.5f) * hallWidth),
                                Quaternion.LookRotation(Vector3.back),
                                new Vector3(hallWidth, hallHeight, 1)
                            ), ref newVertices, ref newUVs, ref wallTriangles);

                            AddQuad(Matrix4x4.TRS(
                                new Vector3((j + 0.5f) * hallWidth, halfH, -1 * i * hallWidth),
                                Quaternion.LookRotation(Vector3.left),
                                new Vector3(hallWidth, hallHeight, 1)
                            ), ref newVertices, ref newUVs, ref wallTriangles);

                            break;

                        // North - South segment
                        case 3:
                            AddQuad(Matrix4x4.TRS(
                                new Vector3((j + 0.5f) * hallWidth, halfH, -1 * i * hallWidth),
                                Quaternion.LookRotation(Vector3.left),
                                new Vector3(hallWidth, hallHeight, 1)
                            ), ref newVertices, ref newUVs, ref wallTriangles);

                            AddQuad(Matrix4x4.TRS(
                                new Vector3((j - 0.5f) * hallWidth, halfH, -1 * i * hallWidth),
                                Quaternion.LookRotation(Vector3.right),
                                new Vector3(hallWidth, hallHeight, 1)
                            ), ref newVertices, ref newUVs, ref wallTriangles);

                            break;

                        // East segment
                        case 4:
                            AddQuad(Matrix4x4.TRS(
                                new Vector3(j * hallWidth, halfH, -1 * (i + 0.5f) * hallWidth),
                                Quaternion.LookRotation(Vector3.forward),
                                new Vector3(hallWidth, hallHeight, 1)
                            ), ref newVertices, ref newUVs, ref wallTriangles);

                            AddQuad(Matrix4x4.TRS(
                                new Vector3((j - 0.5f) * hallWidth, halfH, -1 * i * hallWidth),
                                Quaternion.LookRotation(Vector3.right),
                                new Vector3(hallWidth, hallHeight, 1)
                            ), ref newVertices, ref newUVs, ref wallTriangles);

                            AddQuad(Matrix4x4.TRS(
                                new Vector3(j * hallWidth, halfH, -1 * (i - 0.5f) * hallWidth),
                                Quaternion.LookRotation(Vector3.back),
                                new Vector3(hallWidth, hallHeight, 1)
                            ), ref newVertices, ref newUVs, ref wallTriangles);

                            break;

                        // North - East segment
                        case 5:
                            AddQuad(Matrix4x4.TRS(
                                new Vector3(j * hallWidth, halfH, -1 * (i + 0.5f) * hallWidth),
                                Quaternion.LookRotation(Vector3.forward),
                                new Vector3(hallWidth, hallHeight, 1)
                            ), ref newVertices, ref newUVs, ref wallTriangles);

                            AddQuad(Matrix4x4.TRS(
                                new Vector3((j - 0.5f) * hallWidth, halfH, -1 * i * hallWidth),
                                Quaternion.LookRotation(Vector3.right),
                                new Vector3(hallWidth, hallHeight, 1)
                            ), ref newVertices, ref newUVs, ref wallTriangles);

                            break;

                        // South - East segment
                        case 6:
                            AddQuad(Matrix4x4.TRS(
                                new Vector3((j - 0.5f) * hallWidth, halfH, -1 * i * hallWidth),
                                Quaternion.LookRotation(Vector3.right),
                                new Vector3(hallWidth, hallHeight, 1)
                            ), ref newVertices, ref newUVs, ref wallTriangles);

                            AddQuad(Matrix4x4.TRS(
                                new Vector3(j * hallWidth, halfH, -1 * (i - 0.5f) * hallWidth),
                                Quaternion.LookRotation(Vector3.back),
                                new Vector3(hallWidth, hallHeight, 1)
                            ), ref newVertices, ref newUVs, ref wallTriangles);

                            break;

                        // North - South - East segment
                        case 7:
                            AddQuad(Matrix4x4.TRS(
                                new Vector3((j - 0.5f) * hallWidth, halfH, -1 * i * hallWidth),
                                Quaternion.LookRotation(Vector3.right),
                                new Vector3(hallWidth, hallHeight, 1)
                            ), ref newVertices, ref newUVs, ref wallTriangles);

                            break;

                        // West segment
                        case 8:
                            AddQuad(Matrix4x4.TRS(
                                new Vector3(j * hallWidth, halfH, -1 * (i - 0.5f) * hallWidth),
                                Quaternion.LookRotation(Vector3.back),
                                new Vector3(hallWidth, hallHeight, 1)
                            ), ref newVertices, ref newUVs, ref wallTriangles);

                            AddQuad(Matrix4x4.TRS(
                                new Vector3((j + 0.5f) * hallWidth, halfH, -1 * i * hallWidth),
                                Quaternion.LookRotation(Vector3.left),
                                new Vector3(hallWidth, hallHeight, 1)
                            ), ref newVertices, ref newUVs, ref wallTriangles);

                            AddQuad(Matrix4x4.TRS(
                                new Vector3(j * hallWidth, halfH, -1 * (i + 0.5f) * hallWidth),
                                Quaternion.LookRotation(Vector3.forward),
                                new Vector3(hallWidth, hallHeight, 1)
                            ), ref newVertices, ref newUVs, ref wallTriangles);

                            break;

                        // North - West segment
                        case 9:
                            AddQuad(Matrix4x4.TRS(
                                new Vector3((j + 0.5f) * hallWidth, halfH, -1 * i * hallWidth),
                                Quaternion.LookRotation(Vector3.left),
                                new Vector3(hallWidth, hallHeight, 1)
                            ), ref newVertices, ref newUVs, ref wallTriangles);

                            AddQuad(Matrix4x4.TRS(
                                new Vector3(j * hallWidth, halfH, -1 * (i + 0.5f) * hallWidth),
                                Quaternion.LookRotation(Vector3.forward),
                                new Vector3(hallWidth, hallHeight, 1)
                            ), ref newVertices, ref newUVs, ref wallTriangles);

                            break;

                        // South - West segment
                        case 10:
                            AddQuad(Matrix4x4.TRS(
                                new Vector3(j * hallWidth, halfH, -1 * (i - 0.5f) * hallWidth),
                                Quaternion.LookRotation(Vector3.back),
                                new Vector3(hallWidth, hallHeight, 1)
                            ), ref newVertices, ref newUVs, ref wallTriangles);

                            AddQuad(Matrix4x4.TRS(
                                new Vector3((j + 0.5f) * hallWidth, halfH, -1 * i * hallWidth),
                                Quaternion.LookRotation(Vector3.left),
                                new Vector3(hallWidth, hallHeight, 1)
                            ), ref newVertices, ref newUVs, ref wallTriangles);

                            break;

                        // North - South - West segment
                        case 11:
                            AddQuad(Matrix4x4.TRS(
                                new Vector3((j + 0.5f) * hallWidth, halfH, -1 * i * hallWidth),
                                Quaternion.LookRotation(Vector3.left),
                                new Vector3(hallWidth, hallHeight, 1)
                            ), ref newVertices, ref newUVs, ref wallTriangles);

                            break;

                        // East - West segment
                        case 12:
                            AddQuad(Matrix4x4.TRS(
                                new Vector3(j * hallWidth, halfH, -1 * (i - 0.5f) * hallWidth),
                                Quaternion.LookRotation(Vector3.back),
                                new Vector3(hallWidth, hallHeight, 1)
                            ), ref newVertices, ref newUVs, ref wallTriangles);

                            AddQuad(Matrix4x4.TRS(
                                new Vector3(j * hallWidth, halfH, -1 * (i + 0.5f) * hallWidth),
                                Quaternion.LookRotation(Vector3.forward),
                                new Vector3(hallWidth, hallHeight, 1)
                            ), ref newVertices, ref newUVs, ref wallTriangles);

                            break;

                        // North - East - West segment
                        case 13:
                            AddQuad(Matrix4x4.TRS(
                                new Vector3(j * hallWidth, halfH, -1 * (i + 0.5f) * hallWidth),
                                Quaternion.LookRotation(Vector3.forward),
                                new Vector3(hallWidth, hallHeight, 1)
                            ), ref newVertices, ref newUVs, ref wallTriangles);

                            break;

                        // South - East - West segment
                        case 14:
                            AddQuad(Matrix4x4.TRS(
                                new Vector3(j * hallWidth, halfH, -1 * (i - 0.5f) * hallWidth),
                                Quaternion.LookRotation(Vector3.back),
                                new Vector3(hallWidth, hallHeight, 1)
                            ), ref newVertices, ref newUVs, ref wallTriangles);

                            break;

                        default:
                            break;
                    }
                }
            }
        }

        maze.vertices = newVertices.ToArray();
        maze.uv = newUVs.ToArray();

        maze.SetTriangles(floorTriangles.ToArray(), 0);
        maze.SetTriangles(wallTriangles.ToArray(), 1);

        maze.RecalculateNormals();

        return maze;
    }

    private void AddQuad(
        Matrix4x4 matrix,
        ref List<Vector3> newVertices,
        ref List<Vector2> newUVs,
        ref List<int> newTriangles)
    {
        int index = newVertices.Count;

        Vector3 vert1 = new Vector3(-0.5f, -0.5f, 0);
        Vector3 vert2 = new Vector3(-0.5f, 0.5f, 0);
        Vector3 vert3 = new Vector3(0.5f, 0.5f, 0);
        Vector3 vert4 = new Vector3(0.5f, -0.5f, 0);

        newVertices.Add(matrix.MultiplyPoint3x4(vert1));
        newVertices.Add(matrix.MultiplyPoint3x4(vert2));
        newVertices.Add(matrix.MultiplyPoint3x4(vert3));
        newVertices.Add(matrix.MultiplyPoint3x4(vert4));

        newUVs.Add(new Vector2(1, 0));
        newUVs.Add(new Vector2(1, 1));
        newUVs.Add(new Vector2(0, 1));
        newUVs.Add(new Vector2(0, 0));

        newTriangles.Add(index + 2);
        newTriangles.Add(index + 1);
        newTriangles.Add(index);

        newTriangles.Add(index + 3);
        newTriangles.Add(index + 2);
        newTriangles.Add(index);
    }
}
