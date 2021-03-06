﻿using System;

using System.Windows.Controls;
using OSGeo.GDAL;
using OSGeo.OSR;
using System.Device.Location;
using System.Windows;
using System.Windows.Media.Imaging;

namespace RasterOps
{
    public class Tile : Image
    {
         Dataset ds;

        double[] GeoTransform = new double[6];

        public double height;
        public double width;
        public Point origin;
        public double pixelSizeX;
        public double pixelSizeY;

        CoordinateTransformation toLatLon;
        CoordinateTransformation toCoord;

        SpatialReference srsLCC = new SpatialReference("");
        SpatialReference srsLatLon = new SpatialReference("");

        public Tile(string file) : base()
        {
            Gdal.AllRegister();

            ds = Gdal.Open(file, Access.GA_ReadOnly);

            ds.GetGeoTransform(GeoTransform);

            height = ds.RasterYSize;
            width = ds.RasterXSize;

            origin = new Point(GeoTransform[1], GeoTransform[3]);

            pixelSizeX = GeoTransform[1];
            pixelSizeY = GeoTransform[5];

            string s = ds.GetProjection();

            srsLCC = new SpatialReference(s);
            srsLatLon.SetWellKnownGeogCS("WGS84");

            toLatLon = new CoordinateTransformation(srsLCC, srsLatLon);
            toCoord = new CoordinateTransformation(srsLatLon, srsLCC);

            BitmapImage img = new BitmapImage();
            img.BeginInit();
            img.UriSource = new Uri(file);
            img.EndInit();
            this.Source = img;
        }

        # region Coordinate Transformations

        // Does a transformation between display points
        // and east,north point in the rasters spatial
        // system

        public Point px2coord(Point mp)
        {
            Point v = new Point();
            v.X = GeoTransform[0] + (mp.X * GeoTransform[1]);
            v.Y = GeoTransform[3] + (mp.Y * GeoTransform[5]);

            return v;
        }


        // Does a transformation from spatial reference system
        // to raster x,y

        public Point coord2px(Point p)
        {
            Point v = new Point();
            v.X = ((p.X - GeoTransform[0]) / GeoTransform[1]);
            v.Y = ((p.Y - GeoTransform[3]) / GeoTransform[5]);
            return v;
        }

        // Does a transformation from spatial reference system
        // to Lat/Lon

        public GeoCoordinate coord2LatLon(Point p)
        {
            double[] points = new double[3];

            points[0] = p.X;
            points[1] = p.Y;
            points[2] = 0;

            toLatLon.TransformPoint(points);

            return new GeoCoordinate(points[1], points[0]);
        }

        // Does a transformation from Lat/Lon to 
        // spatial reference system

        public Point LatLon2coord(GeoCoordinate latlon)
        {
            double[] p = new double[3];

            p[0] = latlon.Longitude;
            p[1] = latlon.Latitude;
            p[2] = 0;

            toCoord.TransformPoint(p);

            return new Point(p[0], p[1]);
        }

        #endregion

    }
}
