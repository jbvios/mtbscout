using System;
using System.Collections.Generic;
using System.Xml;
using System.Globalization;
using System.IO;
using ICSharpCode.SharpZipLib.Zip;
using System.Diagnostics;
using System.Security;
using System.Web;

namespace MTBScout
{

    /// <summary>
    /// Summary description for GpxParser
    /// </summary
    /// >
    public class GpxParser
    {
        private double maxEle = 0.0;
        private double linearDistance = 0.0;
        private double distance3D = 0.0;
        private double totalClimb = 0.0;
        private double minEle = 2000000.0;
        private bool photoLoaded = false;
        private XmlDocument doc = new XmlDocument();

        private string zippedFile = null;
        public string ZippedFile
        {
            get
            {
                if (zippedFile == null)
                {
                    lock (this)
                    {
                        if (zippedFile == null)
                        {
                            zippedFile = PathFunctions.GetWorkingPath(Path.ChangeExtension(sourceFile, ".zip"));
                            if (!File.Exists(zippedFile))
                            {
                                string folder = Path.GetDirectoryName(zippedFile);
                                if (!Directory.Exists(folder))
                                    Directory.CreateDirectory(folder);
                                using (FileStream fs = new FileStream(zippedFile, FileMode.Create))
                                {
                                    using (ZipOutputStream stream = new ZipOutputStream(fs))
                                    {
                                        ZipEntry entry = new ZipEntry(Path.GetFileName(sourceFile));

                                        entry.DateTime = DateTime.Now;
                                        stream.PutNextEntry(entry);

                                        byte[] buff = File.ReadAllBytes(sourceFile);
                                        stream.Write(buff, 0, buff.Length);

                                        stream.Finish();
                                    }
                                }
                            }
                        }
                    }
                }
                return zippedFile;
            }
        }

        GenericPoint mediumPoint = null;
        public GenericPoint MediumPoint
        {
            get
            {
                if (mediumPoint == null)
                {
                    lock (this)
                    {
                        if (mediumPoint == null)
                        {
                            double lat = 0.0, lon = 0.0;
                            int points = 0;
                            foreach (Track t in Tracks)
                                foreach (TrackSegment seg in t.Segments)
                                {
                                    foreach (TrackPoint tp in seg.Points)
                                    {
                                        lat += tp.lat;
                                        lon += tp.lon;
                                    }
                                    points += seg.Points.Count;
                                }
                            mediumPoint = new GenericPoint();
                            mediumPoint.lat = lat / points;
                            mediumPoint.lon = lon / points;
                        }
                    }
                }
                return mediumPoint;
            }
        }

        private int? countryCode = null;
        public int CountryCode
        {
            get
            {
                if (countryCode == null)
                {
                    lock (this)
                    {
                        if (countryCode == null)
                        {
                            countryCode = Helper.GetCountryCode(MediumPoint.lat, MediumPoint.lon);
                        }
                    }
                }
                return countryCode.Value;
            }
        }
        private List<Track> tracks = new List<Track>();
        public List<Track> Tracks { get { return tracks; } }

        private List<WayPoint> wayPoints = new List<WayPoint>();
        public List<WayPoint> WayPoints { get { return wayPoints; } }

        private string sourceFile = "";
        public string SourceFile { get { return sourceFile; } }
        public double MaxElevation { get { return maxEle; } }
        public double MinElevation { get { return minEle; } }
        public double LinearDistance { get { return linearDistance; } }
        public double Distance3D { get { return distance3D; } }
        public double TotalClimb { get { return totalClimb; } }

        public GpxParser()
        {

        }

        private static string GetParserKey(string routeName)
        {
            return routeName + "_GPX";
        }
        public static GpxParser FromSession(string routeName)
        {
            return HttpContext.Current.Session[GetParserKey(routeName)] as GpxParser;
        }
        public static void RemoveFromSession(string routeName)
        {
            HttpContext.Current.Session[GetParserKey(routeName)]  = null;
        }
        public void ToSession(string routeName)
        {
            HttpContext.Current.Session[GetParserKey(routeName)] = this;
        }

        public void Save(string file)
        {
            doc.Save(file);
        }
        public void Parse(string gpxFile)
        {
            this.sourceFile = gpxFile;
            using (FileStream fs = new FileStream(gpxFile, FileMode.Open, FileAccess.Read))
                Parse(fs);

        }
        public void Parse(Stream stream)
        {
            doc.Load(stream);
            foreach (XmlElement wpElement in doc.DocumentElement.GetElementsByTagName("wpt"))
            {
                WayPoint wp = new WayPoint();
                wp.Parse(wpElement);
                wayPoints.Add(wp);

            }
            foreach (XmlElement trackElement in doc.DocumentElement.GetElementsByTagName("trk"))
            {
                Track trk = new Track();
                trk.Parse(trackElement);
                tracks.Add(trk);
                maxEle = Math.Max(maxEle, trk.MaxElevation);
                minEle = Math.Min(minEle, trk.MinElevation);
                linearDistance += trk.LinearDistance;
                distance3D += trk.Distance3D;
                totalClimb += trk.TotalClimb;
            }


        }

        public TrackPoint GetPoint(DateTime time, int distanceSeconds)
        {
            foreach (Track t in tracks)
                foreach (TrackSegment s in t.Segments)
                    foreach (TrackPoint tp in s.Points)
                    {
                        if ((time - tp.time) <= TimeSpan.FromSeconds(distanceSeconds))
                            return tp;
                    }
            return null;
        }


        public void LoadPhothos()
        {
            if (photoLoaded)
                return;
            string original = PathFunctions.GetImagePathFromGpx(sourceFile);
            if (Directory.Exists(original))
            {
                int prog = 0;
                foreach (string file in Directory.GetFiles(original, "*.jpg"))
                {
                    if (AlreadyAvailable(file))
                        continue;
                    try
                    {
                        DateTime photoTime = Helper.GetCreationDate(file);
                        TrackPoint tp = GetPoint(photoTime, 1);
                        if (tp != null)
                        {
                            WayPoint wp = new WayPoint(tp);
                            wp.link = file;
                            wp.name = Helper.GetImageTitle(file);
                            if (string.IsNullOrEmpty(wp.name))
                                wp.name = Helper.GetImageCaption(prog++, wp.link);
                            wayPoints.Add(wp);
                        }
                    }
                    catch
                    {
                    }

                }
            }
            photoLoaded = true;
        }

        private bool AlreadyAvailable(string file)
        {
            foreach (WayPoint wp in WayPoints)
                if (string.Compare(wp.link, file, StringComparison.InvariantCultureIgnoreCase) == 0)
                    return true;
            return false;
        }

        // codice preso da http://www.codeproject.com/KB/cs/Douglas-Peucker_Algorithm.aspx

        /// <summary>
        /// Uses the Douglas Peucker algorithm to reduce the number of points.
        /// </summary>
        /// <param name="Points">The points.</param>
        /// <param name="Tolerance">The tolerance.</param>
        /// <returns></returns>
        public static List<TrackPoint> DouglasPeuckerReduction(
            List<TrackPoint> points,
            double tolerance)
        {
            try
            {
                if (points == null || points.Count < 3)
                    return points;

                int firstPoint = 0;
                int lastPoint = points.Count - 1;
                List<int> pointIndexsToKeep = new List<int>();

                //Add the first and last index to the keepers
                pointIndexsToKeep.Add(firstPoint);
                pointIndexsToKeep.Add(lastPoint);

                //The first and the last point cannot be the same
                while (points[firstPoint].Equals(points[lastPoint]))
                    lastPoint--;

                DouglasPeuckerReduction(points, firstPoint, lastPoint, tolerance, ref pointIndexsToKeep);

                List<TrackPoint> returnPoints = new List<TrackPoint>();
                pointIndexsToKeep.Sort();
                foreach (int index in pointIndexsToKeep)
                    returnPoints.Add(points[index]);

                return returnPoints;
            }
            catch
            {
                return points;
            }
        }

        /// <summary>
        /// Douglases the peucker reduction.
        /// </summary>
        /// <param name="points">The points.</param>
        /// <param name="firstPoint">The first point.</param>
        /// <param name="lastPoint">The last point.</param>
        /// <param name="tolerance">The tolerance.</param>
        /// <param name="pointIndexsToKeep">The point index to keep.</param>
        private static void DouglasPeuckerReduction(
            List<TrackPoint> points,
            int firstPoint,
            int lastPoint,
            Double tolerance,
            ref List<int> pointIndexsToKeep)
        {
            double maxDistance = 0;
            int indexFarthest = 0;

            for (int index = firstPoint; index < lastPoint; index++)
            {
                double distance = PerpendicularDistance(points[firstPoint], points[lastPoint], points[index]);
                if (distance > maxDistance)
                {
                    maxDistance = distance;
                    indexFarthest = index;
                }
            }

            if (maxDistance > tolerance && indexFarthest != 0)
            {
                //Add the largest point that exceeds the tolerance
                pointIndexsToKeep.Add(indexFarthest);

                DouglasPeuckerReduction(points, firstPoint, indexFarthest, tolerance, ref pointIndexsToKeep);
                DouglasPeuckerReduction(points, indexFarthest, lastPoint, tolerance, ref pointIndexsToKeep);
            }
        }

        /// <summary>
        /// The distance of a point from a line made from point1 and point2.
        /// </summary>
        /// <param name="pt1">The PT1.</param>
        /// <param name="pt2">The PT2.</param>
        /// <param name="p">The p.</param>
        /// <returns></returns>
        public static double PerpendicularDistance(TrackPoint Point1, TrackPoint Point2, TrackPoint Point)
        {
            //Area = |(1/2)(x1y2 + x2y3 + x3y1 - x2y1 - x3y2 - x1y3)|   *Area of triangle
            //Base = v((x1-x2)²+(x1-x2)²)                               *Base of Triangle*
            //Area = .5*Base*H                                          *Solve for height
            //Height = Area/.5/Base

            double area = Math.Abs(.5 * (Point1.lat * Point2.lon + Point2.lat *
            Point.lon + Point.lat * Point1.lon - Point2.lat * Point1.lon - Point.lat *
            Point2.lon - Point1.lat * Point.lon));
            double bottom = Math.Sqrt(Math.Pow(Point1.lat - Point2.lat, 2) +
            Math.Pow(Point1.lon - Point2.lon, 2));
            double height = area / bottom * 2;

            return height;

            //Another option
            //double A = Point.lat - Point1.lat;
            //double B = Point.lon - Point1.lon;
            //double C = Point2.lat - Point1.lat;
            //double D = Point2.lon - Point1.lon;

            //double dot = A * C + B * D;
            //double len_sq = C * C + D * D;
            //double param = dot / len_sq;

            //double xx, yy;

            //if (param < 0)
            //{
            //    xx = Point1.lat;
            //    yy = Point1.lon;
            //}
            //else if (param > 1)
            //{
            //    xx = Point2.lat;
            //    yy = Point2.lon;
            //}
            //else
            //{
            //    xx = Point1.lat + param * C;
            //    yy = Point1.lon + param * D;
            //}

            //double d = DistanceBetweenOn2DPlane(Point, new Point(xx, yy));
        }

    }
    public class Track
    {
        private double maxEle = 0.0;
        private double linearDistance = 0.0;
        private double minEle = 2000000.0;
        private double distance3D = 0.0;
        private double totalClimb = 0.0;
        private string name = "";
        private string description = "";
        private List<TrackSegment> segments = new List<TrackSegment>();
        public List<TrackSegment> Segments { get { return segments; } }
        public double MaxElevation { get { return maxEle; } }
        public double MinElevation { get { return minEle; } }
        public double LinearDistance { get { return linearDistance; } }
        public string Name { get { return name; } }
        public string Description { get { return description; } }
        public double Distance3D { get { return distance3D; } }
        public double TotalClimb { get { return totalClimb; } }

        internal void Parse(XmlElement trackElement)
        {
            XmlNodeList list = trackElement.GetElementsByTagName("name");
            if (list.Count == 1)
                name = list[0].InnerText;
            list = trackElement.GetElementsByTagName("desc");
            if (list.Count == 1)
                description = list[0].InnerText;

            foreach (XmlElement trackSegment in trackElement.GetElementsByTagName("trkseg"))
            {
                TrackSegment seg = new TrackSegment();
                seg.Parse(trackSegment);
                segments.Add(seg);
                maxEle = Math.Max(maxEle, seg.MaxElevation);
                minEle = Math.Min(minEle, seg.MinElevation);
                linearDistance += seg.LinearDistance;
                distance3D += seg.Distance3D;
                totalClimb += seg.TotalClimb;
            }
        }
    }

    public class TrackSegment
    {
        private List<TrackPoint> points = new List<TrackPoint>();
        private double maxEle = 0.0;
        private double linearDistance = 0.0;
        private double minEle = 2000000.0;
        private double distance3D = 0.0;
        private double totalClimb = 0.0;

        public List<TrackPoint> Points { get { return points; } }
        public TrackPoint[] ReducedPoints
        {
            get
            {
                if (reducedPoints == null)
                {
                    lock (this)
                    {
                        if (reducedPoints == null)
                            reducedPoints = GpxParser.DouglasPeuckerReduction(Points, 0.00005).ToArray();
                    }
                }
                return reducedPoints;
            }
        }
        public double MaxElevation { get { return maxEle; } }
        public double MinElevation { get { return minEle; } }
        public double LinearDistance { get { return linearDistance; } }
        public double Distance3D { get { return distance3D; } }
        public double TotalClimb { get { return totalClimb; } }

        TrackPoint[] reducedPoints = null;

        internal void Parse(XmlElement trackSegment)
        {
            TrackPoint prevPoint = null;
            foreach (XmlElement trackPoint in trackSegment.GetElementsByTagName("trkpt"))
            {
                TrackPoint point = new TrackPoint();
                point.Parse(trackPoint);

                maxEle = Math.Max(point.ele, maxEle);
                minEle = Math.Min(point.ele, minEle);

                if (prevPoint != null)
                {
                    double seg = point - prevPoint;

                    linearDistance += seg;
                    double h = point.ele - prevPoint.ele;
                    if (h > 0)
                        totalClimb += h;
                    distance3D += Math.Sqrt(seg * seg + h * h);

                }
                prevPoint = point;
                points.Add(point);
            }
        }


    }

    public class GenericPoint
    {
        public double ele = double.NaN;
        public double lat;
        public double lon;
        public string link;

        public GenericPoint()
        {

        }
        public GenericPoint(GenericPoint gp)
        {
            ele = gp.ele;
            lat = gp.lat;
            lon = gp.lon;
            link = gp.link;
        }

        public override bool Equals(object obj)
        {
            GenericPoint gp = obj as GenericPoint;
            if (gp == null)
                return false;

            return ele == gp.ele && lat == gp.lat && lon == gp.lon;
        }

        public override int GetHashCode()
        {
            return ele.GetHashCode() + lat.GetHashCode() + lon.GetHashCode();
        }
        public virtual void Parse(XmlElement trackPoint)
        {
            lat = double.Parse(trackPoint.GetAttribute("lat"), CultureInfo.InvariantCulture);
            lon = double.Parse(trackPoint.GetAttribute("lon"), CultureInfo.InvariantCulture);

            XmlNodeList elevations = trackPoint.GetElementsByTagName("ele");
            if (elevations.Count == 1)
            {
                ele = double.Parse(elevations[0].InnerText, CultureInfo.InvariantCulture);
            }
        }
        public static double operator -(GenericPoint a, GenericPoint b)
        {
            if (a.lat == b.lat && a.lon == b.lon)
                return 0.0;

            return CalculateGreatCircleDistance(
                            Radians(a.lat),
                            Radians(a.lon),
                            Radians(b.lat),
                            Radians(b.lon)
                            );

        }

        public static double Radians(double degrees)
        {
            return degrees * Math.PI / 180;
        }
        public static double CalculateGreatCircleDistance(double lat1, double long1, double lat2, double long2)
        {
            double radius = 6378137.0;
            return radius * Math.Acos(
                Math.Sin(lat1) * Math.Sin(lat2)
                + Math.Cos(lat1) * Math.Cos(lat2) * Math.Cos(long2 - long1));
        }


    }

    public class TrackPoint : GenericPoint
    {
        public DateTime time;

        public TrackPoint()
        {

        }
        public TrackPoint(TrackPoint tp)
            : base(tp)
        {
            time = tp.time;
        }
        public override void Parse(XmlElement trackPoint)
        {
            base.Parse(trackPoint);
            XmlNodeList times = trackPoint.GetElementsByTagName("time");
            if (times.Count == 1)
            {
                DateTime.TryParse(times[0].InnerText, out time);
            }
        }
    }

    public class WayPoint : GenericPoint
    {
        public string name = "";
        public string description = "";

        public WayPoint()
        {

        }
        public WayPoint(WayPoint wp)
            : base(wp)
        {
            name = wp.name;
            description = wp.description;
        }

        public WayPoint(GenericPoint gp)
            : base(gp)
        {

        }
        public override void Parse(XmlElement trackPoint)
        {
            base.Parse(trackPoint);
            XmlNodeList list = trackPoint.GetElementsByTagName("name");
            if (list.Count == 1)
                name = list[0].InnerText;
            list = trackPoint.GetElementsByTagName("cmt");
            if (list.Count == 1)
                description = list[0].InnerText;
        }
    }
}