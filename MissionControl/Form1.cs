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

namespace MissionControl
{
    public partial class Form1 : Form
    {
        GMapOverlay m_markersOverlay = new GMapOverlay();

        public Form1()
        {
            InitializeComponent();
            ParseClient.Initialize("5M2u8zCYhBxJJIQOjQvEM5L6cgly6oD6KWaG4R1L", "BCI8zYHe2kKP5HEe3VyST4a68rgDzsIgsjYerzcb");
        }

        private void Form1_Load(object sender, EventArgs e)
        {

            //full screen
            gMapControl1.SetBounds(0, 0, ClientRectangle.Width-1, ClientRectangle.Height);
            pictureBox1.SetBounds(0, 0, ClientRectangle.Width-1, ClientRectangle.Height);

            //Image
            gMapControl1.MapProvider = GMap.NET.MapProviders.ArcGIS_Imagery_World_2D_MapProvider.Instance;
            GMap.NET.GMaps.Instance.Mode = GMap.NET.AccessMode.ServerOnly;

            gMapControl1.MinZoom = 1;
            gMapControl1.MaxZoom = 11;

            //Map Control
            pictureBox1.Parent = gMapControl1;
            gMapControl1.GrayScaleMode = true;

            gMapControl1.Position = new GMap.NET.PointLatLng(43.389758, -80.405068);
            gMapControl1.Zoom = 11;
            
            //Picture Box2
            pictureBox2.Parent = gMapControl1;

            parseTest();
           
        }

        async private void parseTest()
        {
            //Parse Test
            var testObject = new ParseObject("TestObject");
            testObject["foo"] = "bar";
            await testObject.SaveAsync();
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
            //Drop a way point at my location give or take
            var r = new Random();

            double randomLat = 43.389758 + 0.0001 * r.Next(-100, 100);
            double randomLon = - 80.405068 + 0.0001 * r.Next(-100, 100);

            GMarkerGoogle marker = new GMarkerGoogle(new GMap.NET.PointLatLng(randomLat, randomLon), GMarkerGoogleType.red_pushpin);
            m_markersOverlay.Markers.Add(marker);
            gMapControl1.Overlays.Add(m_markersOverlay);
        }
    }
}
