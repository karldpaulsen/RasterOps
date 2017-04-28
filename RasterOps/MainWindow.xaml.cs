﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using System.Device.Location;

namespace RasterOps
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        TileSet mySet = new TileSet(4, 4);

        public MainWindow()
        {
            InitializeComponent();

            string[,] tileNames = new string[,] {
                    { "g:/Sectionals/slc/slc-000-010.tif",
                      "g:/Sectionals/slc/slc-000-011.tif",
                      "g:/Sectionals/slc/slc-000-012.tif",
                      "g:/Sectionals/slc/slc-000-013.tif" },
                    { "g:/Sectionals/slc/slc-001-010.tif",
                      "g:/Sectionals/slc/slc-001-011.tif",
                      "g:/Sectionals/slc/slc-001-012.tif",
                      "g:/Sectionals/slc/slc-001-013.tif" },
                    { "g:/Sectionals/slc/slc-002-010.tif",
                      "g:/Sectionals/slc/slc-002-011.tif",
                      "g:/Sectionals/slc/slc-002-012.tif",
                      "g:/Sectionals/slc/slc-002-013.tif" },
                    { "g:/Sectionals/slc/slc-003-010.tif",
                      "g:/Sectionals/slc/slc-003-011.tif",
                      "g:/Sectionals/slc/slc-003-012.tif",
                      "g:/Sectionals/slc/slc-003-013.tif" }
                };

            for (int row = 0; row < 4; row++)
            {
                for (int column = 0; column < 4; column++)
                {
                    Tile tile = new Tile(tileNames[row, column]);
                    mySet.addTile(column, row, tile);

                    Image image = getCell(column, row, RasterGrid);
                    image.Source = tile.Source;
                }
            }
        }

        private Image getCell(int col, int row, Grid grid)
        {
            Image image = (Image)grid.Children.Cast<UIElement>().
                  First(el => Grid.GetRow(el) == row && Grid.GetColumn(el) == col);

            return image;
        }

        private void MainWindow_MouseMove(object sender, MouseEventArgs e)
        {
            Point p = e.GetPosition(this);
            Point g = TranslatePoint(p, RasterGrid);
            DisplayXY.Text = p.X.ToString("F0") + "," + p.Y.ToString("F0");
            GridXY.Text = g.X.ToString("F0") + "," + g.Y.ToString("F0");
        }

        private int getRow(Point p)
        {
            int row = 0;
            double accumulatedHeight = 0.0;

            foreach (var rowDefinition in RasterGrid.RowDefinitions)
            {
                accumulatedHeight += rowDefinition.ActualHeight;
                if (accumulatedHeight >= p.Y)
                    break;
                row++;
            }
            return row;
        }

        private int getColumn(Point p)
        {
            int column = 0;
            double accumulatedWidth = 0.0;

            foreach (var columnDefinition in RasterGrid.ColumnDefinitions)
            {
                accumulatedWidth += columnDefinition.ActualWidth;
                if (accumulatedWidth >= p.X)
                    break;
                column++;
            }
            return column;
        }

        private void Grid_MouseMove(object sender, MouseEventArgs e)
        {
            Point p = e.GetPosition(RasterGrid);
            int row = getRow(p);
            int column = getColumn(p);
            GridRowColumn.Text = column.ToString() + "," + row.ToString();
        }

        private void Image_MouseMove(object sender, MouseEventArgs e)
        {
            Image image = sender as Image;
            Point p = e.GetPosition(image);
            RasterXY.Text = p.X.ToString("F0") + "," + p.Y.ToString("F0");

            p = e.GetPosition(RasterGrid);
            int row = getRow(p);
            int col = getColumn(p);
            Tile tile = mySet.getTile(col, row);
            Point c = tile.px2coord(p);
            GeoCoordinate ll = tile.coord2LatLon(c);
            LatLon.Text = ll.Latitude.ToString("F6") + "," + ll.Longitude.ToString("F6");
        }

        private void RasterGrid_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            Point p = e.GetPosition(RasterGrid);
            int row = getRow(p);
            int column = getColumn(p);
        }


        private void Image_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            Point p = e.GetPosition(RasterGrid);
            int row = getRow(p);
            int column = getColumn(p);

            Tile tile = new Tile("g:/Sectionals/slc/slc-040-043.tif");
            mySet.addTile(column, row, tile);

            Image image = getCell(column, row, RasterGrid);
            image.Source = tile.Source;

            Point c = tile.px2coord(p);
            GeoCoordinate ll = tile.coord2LatLon(c);
            LatLon.Text = ll.Latitude.ToString("F6") + "," + ll.Longitude.ToString("F6");

        }
    }
}