using System;
using System.Collections.Generic;
using UnityEngine;

public static class CoinGridSpiralFinder //chat gpt ftw. nothing works but it was soooo beautiful :,|
{
    // Returns first non-empty List<Coin> found in spiral order starting at (startRow, startCol),
    // or null if none found.
    public static List<Coin> FindNearestCoins(List<Coin>[][] grid, int startRow, int startCol)
    {
        if (grid == null) return null;
        int rows = grid.Length;
        if (rows == 0) return null;
        int cols = grid[0]?.Length ?? 0;
        if (cols == 0) return null;

        // Validate start position
        if (startRow < 0 || startRow >= rows || startCol < 0 || startCol >= cols)
            throw new ArgumentOutOfRangeException("Start position is outside the grid.");

        // Check starting cell first
        if (grid[startRow][startCol] != null && grid[startRow][startCol].Count > 0)
            return grid[startRow][startCol];

        // Directions: right, down, left, up
        int[,] dir = new int[,] { { 0, 1 }, { 1, 0 }, { 0, -1 }, { -1, 0 } };

        int r = startRow;
        int c = startCol;
        int stepSize = 1;     // how many steps to take in current leg
        int dirIndex = 0;     // 0..3
        int cellsVisited = 1; // we already checked the start
        int totalCells = rows * cols;

        while (cellsVisited < totalCells)
        {
            // Repeat two legs with the same stepSize (except maybe last partial)
            for (int repeat = 0; repeat < 2; repeat++)
            {
                int dr = dir[dirIndex, 0];
                int dc = dir[dirIndex, 1];

                for (int step = 0; step < stepSize; step++)
                {
                    r += dr;
                    c += dc;

                    // If outside bounds, just skip counting as visited; only count valid cells
                    if (r >= 0 && r < rows && c >= 0 && c < cols)
                    {
                        
                        //if go is inactive -> remove from grid
                        for (int l = grid[r][c].Count-1; l >= 0; l--) {
                            if (grid[r][c][l].gameObject.activeSelf == false)
                            {
                                grid[r][c].RemoveAt(l);
                            }
                        }
                        cellsVisited++;

                        var cell = grid[r][c];
                        if (cell != null && cell.Count > 0)
                        {
                            return cell;
                        }

                        // If we've visited all valid cells we can stop
                        if (cellsVisited >= totalCells)
                            return null;
                    }

                    // If we have visited all cells physically possible, break early
                    // (This check avoids infinite loop when grid is rectangular and spiral steps go beyond)
                    if (cellsVisited >= totalCells)
                        break;
                }

                dirIndex = (dirIndex + 1) % 4;
            }

            stepSize++;
        }

        return null;
    }
}

/* 

    public static class CoinGridSpiralFinder
    {
        // Searches the jagged grid grid[x][y] from (startX, startY) in a spiral.
        // Returns true and out coords + list if a non-empty List<Coin> is found.
        public static bool TryFindFirstNonEmpty(
            List<Coin>[][] grid,
            int startX,
            int startY,
            out int foundX,
            out int foundY,
            out List<Coin> foundList)
        {
            foundX = foundY = -1;
            foundList = null;

            if (grid == null) throw new ArgumentNullException(nameof(grid));
            int width = grid.Length;
            if (width == 0) return false;
            if (startX < 0 || startX >= width) return false;
            if (grid[startX] == null) return false;
            int heightAtStart = grid[startX].Length;
            if (startY < 0 || startY >= heightAtStart) return false;

            // helper to check a jagged cell safely
            bool IsNonEmpty(int x, int y, out List<Coin> list)
            {
                list = null;
                if (x < 0 || x >= grid.Length) return false;
                var col = grid[x];
                if (col == null) return false;
                if (y < 0 || y >= col.Length) return false;
                var l = col[y];
                if (l != null && l.Count > 0)
                {
                    list = l;
                    return true;
                }
                return false;
            }

            // check start
            if (IsNonEmpty(startX, startY, out var startList))
            {
                foundX = startX; foundY = startY; foundList = startList;
                return true;
            }

            // compute max radius needed to reach edges from start (conservative)
            int maxLeft = startX;
            int maxRight = width - 1 - startX;
            int maxUp = startY; // note: height varies per column; use start column height as baseline
            int maxDown = 0;
            // To be safe for jagged arrays, compute maximum possible vertical extents across columns:
            int maxColumnHeightAbove = startY; // distance to top within same column
            int maxColumnHeightBelow = 0;
            for (int x = 0; x < width; x++)
            {
                var col = grid[x];
                if (col == null) continue;
                // treat rows as 0..col.Length-1
                maxColumnHeightAbove = Math.Max(maxColumnHeightAbove, startY); // keep baseline
                maxColumnHeightBelow = Math.Max(maxColumnHeightBelow, col.Length - 1 - startY);
            }
            // Conservative radius: farthest horizontal or vertical distance
            int maxRadius = Math.Max(Math.Max(maxLeft, maxRight), Math.Max(maxColumnHeightAbove, maxColumnHeightBelow));
            // If grid is highly jagged (some columns short), we still iterate full radius, checking bounds for each cell.

            int xPos = startX;
            int yPos = startY;
            int dir = 0;     // 0 = right (y+), 1 = down (x+), 2 = left (y-), 3 = up (x-)
            int stepSize = 1;

            for (int k = 1; k <= maxRadius; k++)
            {
                // move one step right to begin ring (mirrors original pattern)
                yPos += 1;
                if (IsNonEmpty(xPos, yPos, out var list0))
                {
                    foundX = xPos; foundY = yPos; foundList = list0; return true;
                }

                int repeats = (k < maxRadius) ? 2 : 3;
                for (int rep = 0; rep < repeats; rep++)
                {
                    for (int i = 0; i < stepSize; i++)
                    {
                        switch (dir)
                        {
                            case 0: yPos += 1; break; // right
                            case 1: xPos += 1; break; // down
                            case 2: yPos -= 1; break; // left
                            default: xPos -= 1; break; // up
                        }

                        if (IsNonEmpty(xPos, yPos, out var found))
                        {
                            foundX = xPos; foundY = yPos; foundList = found; return true;
                        }
                    }
                    dir = (dir + 1) % 4;
                }

                stepSize++;
            }

            return false;
        }
    }
*/
/*
int x = 0; // current position; x
int y = 0; // current position; y
int d = 0; // current direction; 0=RIGHT, 1=DOWN, 2=LEFT, 3=UP
int c = 0; // counter
int s = 1; // chain size

// starting point
x = ((int)floor(size / 2.0)) - 1;
y = ((int)floor(size / 2.0)) - 1;

for (int k = 1; k <= (size - 1); k++)
{
    for (int j = 0; j < (k < (size - 1) ? 2 : 3); j++)
    {
        for (int i = 0; i < s; i++)
        {
            std::cout << matrix[x][y] << " ";
            c++;

            switch (d)
            {
                case 0: y = y + 1; break;
                case 1: x = x + 1; break;
                case 2: y = y - 1; break;
                case 3: x = x - 1; break;
            }
        }
        d = (d + 1) % 4;
    }
    s = s + 1;
}*/
