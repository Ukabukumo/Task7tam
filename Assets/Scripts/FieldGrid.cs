using UnityEngine;
using System;

public class FieldGrid : MonoBehaviour
{
    private Vector2[,] _grid;

    private void Awake()
    {
        _grid = CreateGrid();
    }

    private Vector2[,] CreateGrid()
    {
        Vector2[,] coords = new Vector2[9, 17];

        // Creating an array of cell coordinates
        for (int i = 0; i < 9; i++)
        {
            for (int j = 0; j < 17; j++)
            {
                float x = -7.8f + j - 0.1f * i;
                float y = 3.5f - 0.9f * i;
                coords[i, j] = new Vector2(x, y);
            }
        }

        return coords;
    }

    public Vector2 GetNearestCell(float x, float y, string type="Coords")
    {
        //int nY = Math.Abs((y - 3.6f) % 0.9f) > 0.45f ? (int)(Math.Abs(y - 3.6f) / 0.9f) + 1 : (int)(Math.Abs(y - 3.6f) / 0.9f);

        // Search for nearest coords
        int nY = (int)Math.Round(Math.Abs(y - 3.6f) / 0.9f);
        int nX = (int)Math.Round(x + 7.9f + 0.1f * nY);

        if (type == "Numbers")
        {
            return new Vector2(nX, nY);
        }

        else
        {
            return _grid[nY, nX];
        }
    }
}
