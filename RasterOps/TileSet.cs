using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RasterOps
{
    class TileSet
    {
        Tile[,] grid;

        public TileSet(int w, int h)
        {
            grid = new Tile[w, h];
        }

        public void addTile(int col, int row, string s)
        {
            grid[col, row] = new Tile(s);
        }

        public void addTile(int col, int row, Tile tile)
        {
            grid[col, row] = tile;
        }

        public Tile getTile(int col, int row)
        {
            return grid[col, row];
        }
    }
}
