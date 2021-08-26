/*
MIT License

Copyright (c) 2020 Damjan Tomic

Permission is hereby granted, free of charge, to any person obtaining a copy
of this software and associated documentation files (the "Software"), to deal
in the Software without restriction, including without limitation the rights
to use, copy, modify, merge, publish, distribute, sublicense, and/or sell
copies of the Software, and to permit persons to whom the Software is
furnished to do so, subject to the following conditions:

The above copyright notice and this permission notice shall be included in all
copies or substantial portions of the Software.

THE SOFTWARE IS PROVIDED "AS IS", WITHOUT WARRANTY OF ANY KIND, EXPRESS OR
IMPLIED, INCLUDING BUT NOT LIMITED TO THE WARRANTIES OF MERCHANTABILITY,
FITNESS FOR A PARTICULAR PURPOSE AND NONINFRINGEMENT. IN NO EVENT SHALL THE
AUTHORS OR COPYRIGHT HOLDERS BE LIABLE FOR ANY CLAIM, DAMAGES OR OTHER
LIABILITY, WHETHER IN AN ACTION OF CONTRACT, TORT OR OTHERWISE, ARISING FROM,
OUT OF OR IN CONNECTION WITH THE SOFTWARE OR THE USE OR OTHER DEALINGS IN THE
SOFTWARE.
 */

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MazeDataGenerator
{
    byte N;
    byte S;
    byte E;
    byte W;

    Dictionary<string, byte> sides;
    Dictionary<string, sbyte> dX;
    Dictionary<string, sbyte> dY;
    Dictionary<string, byte> opposite;

    byte[,] grid;

    public MazeDataGenerator(byte height, byte width)
    {
        N = 1;
        S = 2;
        E = 4;
        W = 8;

        sides = new Dictionary<string, byte>() { { "N", 1 }, { "S", 2 }, { "E", 4 }, { "W", 8 } };
        dX = new Dictionary<string, sbyte>() { { "E", 1 }, { "W", -1 }, { "N", 0 }, { "S", 0 } };
        dY = new Dictionary<string, sbyte>() { { "E", 0 }, { "W", 0 }, { "N", -1 }, { "S", 1 } };
        opposite = new Dictionary<string, byte>() { { "E", W }, { "W", E }, { "N", S }, { "S", N } };

        grid = new byte[height, width];
    }

    private void RandomiseDirections(string[] array)
    {
        for (int i = array.Length - 1; i > 0; i--)
        {
            int j = Random.Range(0, array.Length - 1);
            string temp = array[i];
            array[i] = array[j];
            array[j] = temp;
        }
    }

    public byte[,] BuildMaze(byte cx, byte cy)
    {
        return MazeGeneration(cx, cy, grid);
    }

    byte[,] MazeGeneration(byte cx, byte cy, byte[,] grid)
    {
        string[] directions = new string[] { "N", "S", "E", "W" };

        RandomiseDirections(directions);

        for (int i = 0; i < directions.Length; i++)
        {
            string direction = directions[i];

            byte nx = (byte)(cx + dX[direction]);
            byte ny = (byte)(cy + dY[direction]);

            if (ny >= 0 && ny < grid.GetLength(0) && nx >= 0 && nx < grid.GetLength(1) && grid[ny, nx] == 0)
            {
                grid[cy, cx] |= sides[direction];
                grid[ny, nx] |= opposite[direction];
                MazeGeneration(nx, ny, grid);
            }
        }

        return grid;
    }
}
