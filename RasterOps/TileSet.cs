using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;

namespace RasterOps
{
    public class TileSet : Grid
    {
        const int CELLWDITH = 256;
        const int CELLHEIGHT = 256;

        public int cellHeight { get; set; }
        public int cellWidth { get; set; }
        public int rows { get; set; }
        public int columns { get; set; }

         public TileSet() : base()
        {
            cellWidth = CELLWDITH;
            cellHeight = CELLHEIGHT;

            this.Loaded += new RoutedEventHandler(Initialize);
        }

        public TileSet(int w, int h) : base()
        {
            cellWidth = CELLWDITH;
            cellHeight = CELLHEIGHT;

            buildGrid(h, w);

        }

        protected void Initialize(Object sender, RoutedEventArgs e)
        {
            buildGrid(columns, rows);
        }

        private void buildGrid(int rows, int columns)
        {
            ColumnDefinition columnDef;
            RowDefinition rowDef;

            for (int r = 0; r < rows; r++)
            {
                rowDef = new RowDefinition();
                rowDef.Height = new GridLength(cellHeight);
                this.RowDefinitions.Add(rowDef);
            }

            for (int c = 0; c < columns; c++)
            {
                columnDef = new ColumnDefinition();
                columnDef.Width = new GridLength(cellWidth);
                this.ColumnDefinitions.Add(columnDef);
            }
        }

        public void addTile(int col, int row, string s)
        {
            Tile tile = new Tile(s);
       
            Grid.SetRow(tile, row);
            Grid.SetColumn(tile, col);
            this.Children.Add(tile);
        }

        public void addTile(int col, int row, Tile tile)
        {
            Grid.SetRow(tile, row);
            Grid.SetColumn(tile, col);
            this.Children.Add(tile);
        }

        public void removeTile(int col, int row)
        {
            this.Children.Remove(getTile(col, row));
        }

        public void replaceTile(int col, int row, Tile tile)
        {
            removeTile(col, row);
            addTile(col, row, tile);
        }

        public Tile getTile(int col, int row)
        {
            if (this.Children.Count != 0)
                return (Tile)this.Children.Cast<UIElement>().
                     First(el => Grid.GetRow(el) == row && Grid.GetColumn(el) == col);
            else
                return null;
        }
    }
}
