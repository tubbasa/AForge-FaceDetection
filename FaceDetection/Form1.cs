using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using AForge;

namespace FaceDetection
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            FindFace(pictureBox1);
        }
      
        public void FindFace(PictureBox pb )
        {
            AForge.Video.DirectShow.VideoCaptureDevice FinalVideoSource;
            AForge.Video.DirectShow.FilterInfoCollection VideoCaptuerDevices;
            VideoCaptuerDevices = new AForge.Video.DirectShow.FilterInfoCollection(AForge.Video.DirectShow.FilterCategory.VideoInputDevice);
            FinalVideoSource = new AForge.Video.DirectShow.VideoCaptureDevice(VideoCaptuerDevices[0].MonikerString);
     
            FinalVideoSource.NewFrame += new AForge.Video.NewFrameEventHandler((sender, eventArgs) =>
            {
                Bitmap image = (Bitmap)eventArgs.Frame.Clone();
                pb.Image = image;
            });

            FinalVideoSource.Start();
            FinalVideoSource.NewFrame += new AForge.Video.NewFrameEventHandler((sender, eventArgs) =>
            {
                Bitmap image = (Bitmap)eventArgs.Frame.Clone();
               //görüntüde yüz aranacağı belirtilir.
                Accord.Vision.Detection.Cascades.FaceHaarCascade cascade = new Accord.Vision.Detection.Cascades.FaceHaarCascade();
             //görüntü üzerinde arama yapar. İkinci parametresi minimum arama alanıdır.
                Accord.Vision.Detection.HaarObjectDetector detector = new Accord.Vision.Detection.HaarObjectDetector(cascade, 1);

                detector.SearchMode = Accord.Vision.Detection.ObjectDetectorSearchMode.Single;
                detector.ScalingFactor = 1.5f;
                detector.ScalingMode = Accord.Vision.Detection.ObjectDetectorScalingMode.GreaterToSmaller;
                detector.UseParallelProcessing = true;
                detector.Suppression = 3;
                Rectangle[] faces = detector.ProcessFrame(image);
                Graphics g = Graphics.FromImage(image);
                foreach (var face in faces)
                {
                    Pen p = new Pen(Color.Red, 10f);
                    g.DrawRectangle(p, face);
                }
                g.Dispose();

                pb.Image = image;

            });
        }
        
    }
}
