using GMap.NET.WindowsForms.Markers;
using GMap.NET.WindowsForms.ToolTips;
using GMap.NET.WindowsForms;
using GMap.NET;
using static System.Net.Mime.MediaTypeNames;
using GMap.NET.WindowsPresentation;

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

            return d;
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
            GMapOverlay routes = new GMapOverlay("routes"); //Создаем объект наложения (Overlay)
            List<PointLatLng> points = new List<PointLatLng>(); //Создаем лист, где будут наши точки пути.

            List<Marker> markers_copy = markers.ToList();

            points.Add(new PointLatLng(markers[0].Get_latitude(), markers[0].Get_longitude()));
            markers_copy.Remove(markers[0]);

            for (int i = 0; i < markers.Count - 1; i++)
            {
                List<double> distances = new List<double>();
                int res = 0;

                for (int j = i + 1; j < markers.Count; j++)
                {
                    distances.Add(Get_distance(markers[i], markers[j]));
                }

                if (distances.Count >= 2)
                {
                    for (int j = i + 1; j < markers.Count; j++)
                    {
                        if (distances[j-i] < distances[res])
                        { res = j-i; }
                    }
                    res += i;
                    points.Add(new PointLatLng(markers[res].Get_latitude(), markers[res].Get_longitude()));
                    markers_copy.Remove(markers[res]);
                }
            }
            points.Add(new PointLatLng(markers_copy.Last().Get_latitude(), markers_copy.Last().Get_longitude()));

            gMapControl1.Overlays.Clear();

            GMap.NET.WindowsForms.GMapRoute route = new GMap.NET.WindowsForms.GMapRoute(points, "First"); //Создаем из полученных точнек маршрут и даем ей имя.
            route.Stroke = new Pen(Color.Red, 3); //Задаем цвет и ширину линии
            routes.Routes.Add(route); //Добавляем на наш Overlay маршрут
            gMapControl1.Overlays.Add(routes); //Накладываем Overlay на карту.

            GMapOverlay markersOverlay = new GMapOverlay("marker"); //Создаем Overlay
            GMarkerGoogle markerStart = new GMarkerGoogle(points.FirstOrDefault(), GMarkerGoogleType.blue); //Создаем новую точку и даем ей координаты первого элемента из листа координат и синий цвет
            GMarkerGoogle markerEnd = new GMarkerGoogle(points.LastOrDefault(), GMarkerGoogleType.red); //Тоже самое, но красный цвет и последний из списка координат.
            markerStart.ToolTip = new GMapRoundedToolTip(markerStart); //Указываем тип всплывающей подсказки для точки старта
            markerEnd.ToolTip = new GMapBaloonToolTip(markerEnd); //Другой тип подсказки для точки окончания (для теста)

            markersOverlay.Markers.Add(markerStart); //Добавляем точки
            markersOverlay.Markers.Add(markerEnd); //В наш оверлей маркеров

            gMapControl1.Overlays.Add(markersOverlay); //Добавляем оверлей на карту
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
}