using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using Parse;
using GMap.NET.WindowsForms.Markers;
using GMap.NET.MapProviders;
using GMap.NET.WindowsForms;
using System.Threading;


namespace MissionControl
{
    public partial class Form1 : Form
    {
        GMapOverlay m_markersOverlay = new GMapOverlay();
        CancellationTokenSource cts = new CancellationTokenSource();

        const string PARSE_OBJ_NAME = "GPS";
        const string PARSE_COL_NAME_LON = "lon";
        const string PARSE_COL_NAME_LAT = "lat";
        const int QUERY_MAX_TO_SHOW = 50;


        public Form1()
        {
            InitializeComponent();
            ParseClient.Initialize("5M2u8zCYhBxJJIQOjQvEM5L6cgly6oD6KWaG4R1L", "BCI8zYHe2kKP5HEe3VyST4a68rgDzsIgsjYerzcb");
        }

        private void Form1_Load(object sender, EventArgs e)
        {

            //full screen
            gMapControl1.SetBounds(0, 0, ClientRectangle.Width, ClientRectangle.Height);
            pictureBox1.SetBounds(0, 0, ClientRectangle.Width, ClientRectangle.Height);

            //Image
            gMapControl1.MapProvider = GMap.NET.MapProviders.ArcGIS_Imagery_World_2D_MapProvider.Instance;
            GMap.NET.GMaps.Instance.Mode = GMap.NET.AccessMode.ServerOnly;

            gMapControl1.MinZoom = 1;
            //gMapControl1.MaxZoom = 11;

            //Map Control
            pictureBox1.Parent = gMapControl1;
            //gMapControl1.GrayScaleMode = true;

            gMapControl1.Zoom = 11;
            
            //Picture Box2
            pictureBox2.Parent = gMapControl1;

            // Retrieve Initial Location
            receiveLocation();

            PeriodicGetLocAsync(cts.Token);

        }

        async private void receiveLocation()
        {
            //Parse Test
            // Build a query
            var query = ParseObject.GetQuery("GPS");
            query.Limit(QUERY_MAX_TO_SHOW);
            IEnumerable<ParseObject> gpsInfos = await query.FindAsync();
            if (gpsInfos != null)
            {
                gMapControl1.Position = new GMap.NET.PointLatLng(gpsInfos.ElementAt(0).Get<double>(PARSE_COL_NAME_LAT), gpsInfos.ElementAt(0).Get<double>(PARSE_COL_NAME_LON));
                foreach (ParseObject gpsloc in gpsInfos)
                {
                    setLocationMark(gpsloc.Get<double>(PARSE_COL_NAME_LAT), gpsloc.Get<double>(PARSE_COL_NAME_LON));
                }
            }
            else
            {
                gMapControl1.Position = new GMap.NET.PointLatLng(43.389758, -80.405068);
            }

        }

        private void pictureBox2_Click(object sender, EventArgs e)
        {

        }
        private void pictureBox1_Click(object sender, EventArgs e)
        {

        }

        private void button1_Click(object sender, EventArgs e)
        {

        }

        private void pictureBox2_Click_1(object sender, EventArgs e)
        {
            receiveLocation();
        }

        private void setLocationMark(double latitude, double longitude)
        {
            GMarkerGoogle marker = new GMarkerGoogle(new GMap.NET.PointLatLng(latitude, longitude), GMarkerGoogleType.red_dot);
            m_markersOverlay.Markers.Add(marker);
            gMapControl1.Overlays.Add(m_markersOverlay);
        }

        public async Task PeriodicGetLocAsync(CancellationToken cancellationToken)
        {
            const int REFRESH_RATE = 10000;

            while (true)
            {
                receiveLocation();
                await Task.Delay(REFRESH_RATE, cancellationToken);
            }
        }

    }
}
