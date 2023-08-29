using GMap.NET.WindowsForms.Markers;
using GMap.NET.WindowsForms.ToolTips;
using GMap.NET.WindowsForms;
using GMap.NET;
using static System.Net.Mime.MediaTypeNames;
using GMap.NET.WindowsPresentation;
using System.Text;

namespace travelling_salesman_problem
{
    public partial class Form1 : Form
    {
        private List<Marker> markers = new() { };
        private double Deg2Rad(double deg)
        {
            //from degree to radius
            return deg * (Math.PI / 180d);
        }
        private int Get_lat_and_long(String text, ref double latitude, ref double longitude)
        {
            //Request to the Google Geocoding API.
            string url = string.Format(
            "https://maps.googleapis.com/maps/api/geocode/xml?address={0}&key=AIzaSyDrLdfidigCgTYWTN4l6T2N0J6pNRqZArY&sensor=true_or_false&language=ru",
            Uri.EscapeDataString(text));

            //Making a request to a Uniform Resource Identifier (URI).
            System.Net.HttpWebRequest request =
            (System.Net.HttpWebRequest)System.Net.WebRequest.Create(url);

            //We receive a response from an Internet resource.
            System.Net.WebResponse response =
            request.GetResponse();

            //An instance of the System.IO.Stream class
            //to read data from an Internet resource.
            System.IO.Stream dataStream =
            response.GetResponseStream();

            //Initializing a new class instance
            //System.IO.StreamReader for the specified stream.
            System.IO.StreamReader sreader =
            new System.IO.StreamReader(dataStream);

            //Reads the stream from the current position to the end.
            string responsereader = sreader.ReadToEnd();

            //Closing the response stream.
            response.Close();

            System.Xml.XmlDocument xmldoc =
            new System.Xml.XmlDocument();

            xmldoc.LoadXml(responsereader);

            if (xmldoc.GetElementsByTagName("status")[0].ChildNodes[0].InnerText == "OK")
            {

                //Getting latitude and longitude.
                System.Xml.XmlNodeList nodes =
                xmldoc.SelectNodes("//location");
                //Get latitude and longitude.
                foreach (System.Xml.XmlNode node in nodes)
                {
                    latitude =
                    System.Xml.XmlConvert.ToDouble(node.SelectSingleNode("lat").InnerText.ToString());
                    longitude =
                    System.Xml.XmlConvert.ToDouble(node.SelectSingleNode("lng").InnerText.ToString());
                }
                return 1;
            }
            return 0;

        }

        private double Get_distance(in Marker marker_1, in Marker marker_2)
        {
            double latitude_1 = marker_1.Get_latitude();
            double longitude_1 = marker_1.Get_longitude();

            double latitude_2 = marker_2.Get_latitude();
            double longitude_2 = marker_2.Get_longitude();


            var R = 6371d;
            var dLat = Deg2Rad(latitude_2 - latitude_1);
            var dLon = Deg2Rad(longitude_2 - longitude_1);
            var a =
                Math.Sin(dLat / 2d) * Math.Sin(dLat / 2d) +
                Math.Cos(Deg2Rad(latitude_1)) * Math.Cos(Deg2Rad(latitude_2)) *
                Math.Sin(dLon / 2d) * Math.Sin(dLon / 2d);
            var c = 2d * Math.Atan2(Math.Sqrt(a), Math.Sqrt(1d - a));
            var d = R * c;
            
            return Math.Abs(d); ;
        }

        public Form1()
        {
            InitializeComponent();
        }

        private void Form1_Load(object sender, EventArgs e)
        {

        }
        private void gMapControl1_Load(object sender, EventArgs e)
        {
            GMap.NET.GMaps.Instance.Mode = GMap.NET.AccessMode.ServerAndCache; //choice of map loading - online or from resources
            gMapControl1.MapProvider = GMap.NET.MapProviders.GoogleMapProvider.Instance; //which map provider is used (in our case, Google) 
            gMapControl1.MinZoom = 2; //minimum zoom
            gMapControl1.MaxZoom = 24; //maximum zoom
            gMapControl1.Zoom = 4; // what zoom is used when opening
            gMapControl1.MouseWheelZoomType = GMap.NET.MouseWheelZoomType.MousePositionAndCenter; // how it zooms in (just to the center of the map or by mouse position)
            gMapControl1.CanDragMap = true; // dragging the map with the mouse
            gMapControl1.DragButton = MouseButtons.Left; // which button is used for dragging
            gMapControl1.ShowCenter = false; //show or hide the red cross in the center
            gMapControl1.ShowTileGridLines = false; //show or hide tiles
        }

        private void address_button_Click(object sender, EventArgs e)
        {
            if (addressBox.Text != null)
            {
                double latitude = 0;
                double longitude = 0;
                Get_lat_and_long(addressBox.Text, ref latitude, ref longitude);

                markers.Add(new Marker());
                markers.Last().Set(addressBox.Text, latitude, longitude);
                Address_lable.Text = markers.Count+" address added";
            }
        }

        private void solve_button_Click(object sender, EventArgs e)
        {
            GMapOverlay routes = new GMapOverlay("routes"); 
            List<PointLatLng> points = new List<PointLatLng>(); 
            double[,] mat = new double[markers.Count, markers.Count];

            for(int i = 0;i< markers.Count; i++) 
            {
                for (int j = 0; j < markers.Count; j++)
                {
                    if(i== j) continue;
                    mat[i, j] = Get_distance(markers[i], markers[j]);
                }
            }
            string indexes = string.Join(Environment.NewLine, Waybill.GetWaybills(mat).OrderBy(wb => wb.Length));

            indexes = indexes.Replace(",","");
            indexes = indexes.Replace(" ", "");
            indexes = indexes.Remove(indexes.IndexOf("="));
            foreach (char c in indexes) 
            {
                int num = c;
                num -= 48;
            points.Add(new PointLatLng(markers[num].Get_latitude(), markers[num].Get_longitude()));
            }

            gMapControl1.Overlays.Clear();

            GMap.NET.WindowsForms.GMapRoute route = new GMap.NET.WindowsForms.GMapRoute(points, "First"); 
            route.Stroke = new Pen(Color.Red, 3);
            routes.Routes.Add(route);
            gMapControl1.Overlays.Add(routes);

            GMapOverlay markersOverlay = new GMapOverlay("marker");
            for(int i = 0; i < points.Count-1; i++) 
            {
                GMarkerGoogle marker_new = new GMarkerGoogle(points[i], GMarkerGoogleType.blue);
                markersOverlay.Markers.Add(marker_new);
            }


            gMapControl1.Overlays.Add(markersOverlay);
        }
    }
    public class Marker
    {
        string text;
        double latitude;
        double longitude;

        public void Set(string text, double latitude, double longitude)
        {
            this.text = text;
            this.latitude = latitude;
            this.longitude = longitude;
        }
        public string Get_text()
        {
            return text;
        }
        public double Get_latitude()
        {
            return latitude;
        }
        public double Get_longitude()
        {
            return longitude;
        }
    }
    public class Waybill
    {

        public IEnumerable<int> Indexes { get; }

        public double Length { get; }

        public Waybill(IEnumerable<int> indexes, double length)
        {
            Indexes = indexes;
            Length = length;
        }

        private Waybill() { Indexes = Enumerable.Empty<int>(); }

        public static Waybill Empty = new Waybill();

        
        public Waybill Append(int index, double length)
        {
            if (length < 0)
                throw new ArgumentOutOfRangeException("Distance cannot be negative");

            return new Waybill(Indexes.Append(index), Length + length);
        }

        
        public Waybill Prepend(int index, double length)
        {
            if (length < 0)
                throw new ArgumentOutOfRangeException("Distance cannot be negative");

            return new Waybill(Indexes.Prepend(index), Length + length);
        }

        public override string ToString()
        {
            return $"{string.Join(", ", Indexes)} = {Length};";
        }
        public static IEnumerable<Waybill> GetWaybills(double[,] distances, int first = 0, int end = 0, IList<int> enabledIndexes = null)
        {
            int rows = distances.GetLength(0);

            if (enabledIndexes == null)
            {
                enabledIndexes = Enumerable.Range(0, rows).ToList();
                enabledIndexes.Remove(first);
            }

            if (enabledIndexes.Count == 0)
            {

                Waybill waybill = Waybill.Empty.Append(first, 0);

                if (first != end)
                    yield return waybill.Append(end, distances[first, end]);

                yield break;
            }

            foreach (int index in enabledIndexes)
            {
                double length = distances[first, index];
                List<int> nextIndexes = enabledIndexes.ToList();
                nextIndexes.Remove(index);
                foreach (Waybill waybill in GetWaybills(distances, index, end, nextIndexes))
                    yield return waybill.Prepend(first, length); ;
            }
        }
    }

    
}