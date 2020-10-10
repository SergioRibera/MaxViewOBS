using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using AForge.Video.DirectShow;
using AForge.Video;

namespace Max_View_OBS
{
    public partial class Form1 : Form
    {
        bool hasDevice;
        FilterInfoCollection devices;
        VideoCaptureDevice captureDevice;
        public Form1()
        {
            InitializeComponent();
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            CloseCam();
            LoadDevices();
        }
        void LoadDevices()
        {
            devices = new FilterInfoCollection(FilterCategory.VideoInputDevice);
            if (devices.Count > 0)
            {
                hasDevice = true;
                for (int i = 0; i < devices.Count; i++)
                    camSelect.Items.Add(devices[i].Name.ToString());
                camSelect.Text = devices[0].Name.ToString();
            }
            else
                hasDevice = false;
        }
        void LoadCam()
        {
            if (hasDevice)
            {
                int i = camSelect.SelectedIndex;
                string nameVideoCam = devices[i].MonikerString;
                captureDevice = new VideoCaptureDevice(nameVideoCam);
                captureDevice.NewFrame += new NewFrameEventHandler(CaptureVideo);
                captureDevice.Start();
            }
            else
                MessageBox.Show("No hay cámaras conectadas", "¡Alerta!", MessageBoxButtons.OK, MessageBoxIcon.Error);
        }
        public void CloseCam()
        {
            if (captureDevice != null && captureDevice.IsRunning)
            {
                captureDevice.SignalToStop();
                captureDevice = null;
            }
        }
        protected override void OnFormClosed(FormClosedEventArgs e)
        {
            base.OnFormClosed(e);
            CloseCam();
        }
        protected override void OnClosed(EventArgs e)
        {
            base.OnClosed(e);
            CloseCam();
        }

        private void camSelect_SelectedIndexChanged(object sender, EventArgs e)
        {
            CloseCam();
            LoadCam();
        }
        void CaptureVideo(object sender, NewFrameEventArgs eventArgs)
        {
            if (hasDevice)
            {
                Bitmap img = (Bitmap)eventArgs.Frame.Clone();
                imagePrev.Image = img;
            }
        }

        private void Form1_FormClosed(object sender, FormClosedEventArgs e) => CloseCam();

        private void button1_Click(object sender, EventArgs e)
        {
            this.ShowInTaskbar = !this.ShowInTaskbar;
            button1.Text = this.ShowInTaskbar ? "Ocultar de la barra de tareas" : "Mostrar en la barra de tareas";
        }
    }
}