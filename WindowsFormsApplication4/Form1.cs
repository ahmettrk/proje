using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using AForge.Video;
using AForge.Video.DirectShow;
using AForge;
using AForge.Imaging;
using AForge.Imaging.Filters;
using System.Drawing.Imaging;
using System.IO.Ports;

namespace WindowsFormsApplication4
{
    public partial class Form1 : Form
    {
        private FilterInfoCollection VideoCaptureDevices;
        private VideoCaptureDevice FinalVideo;

        int R;
        int G;
        int B;

        public Form1()
        {
            InitializeComponent();
            {
                comboBox2.DataSource = SerialPort.GetPortNames();
                VideoCaptureDevices = new FilterInfoCollection(FilterCategory.VideoInputDevice);
                foreach (FilterInfo VideoCaptureDevice in VideoCaptureDevices)
                {
                    comboBox1.Items.Add(VideoCaptureDevice.Name);
                }
                comboBox1.SelectedIndex = 0;
            }
        }

        private void button1_Click(object sender, EventArgs e)
        {
            FinalVideo = new VideoCaptureDevice(VideoCaptureDevices[comboBox1.SelectedIndex].MonikerString);
            FinalVideo.NewFrame += new NewFrameEventHandler(FinalVideo_NewFrame);
            FinalVideo.Start();
        }
        void FinalVideo_NewFrame(object sender, NewFrameEventArgs eventArgs)
        {
            Bitmap video = (Bitmap)eventArgs.Frame.Clone();
            pictureBox1.Image = video;
            Bitmap image = (Bitmap)eventArgs.Frame.Clone();
            Bitmap image1 = (Bitmap)eventArgs.Frame.Clone();
            pictureBox1.Image = image;

            if (Kırmızı.Checked)
            {
                EuclideanColorFiltering filter = new EuclideanColorFiltering();
                filter.CenterColor = new RGB(Color.FromArgb(215, 0, 0));
                filter.Radius = 100;
                filter.ApplyInPlace(image1);
                nesnebul(image1);
            }

            if (Maviş.Checked)
            {
                EuclideanColorFiltering filter = new EuclideanColorFiltering();
                filter.CenterColor = new RGB(Color.FromArgb(30, 144, 255));
                filter.Radius = 100;
                filter.ApplyInPlace(image1);
                nesnebul(image1);
            }

            if (Yeşil.Checked)
            {
                EuclideanColorFiltering filter = new EuclideanColorFiltering();
                filter.CenterColor = new RGB(Color.FromArgb(0, 215, 0));
                filter.Radius = 100;
                filter.ApplyInPlace(image1);
                nesnebul(image1);
            }

            if (Elayar.Checked)
            {
                EuclideanColorFiltering filter = new EuclideanColorFiltering();
                filter.CenterColor = new RGB(Color.FromArgb(R, G, B));
                filter.Radius = 100;
                filter.ApplyInPlace(image1);
                nesnebul(image1);
            }
        }
        private void button2_Click(object sender, EventArgs e)
        {
            FinalVideo.Stop();
        }

        private void trackBar1_Scroll(object sender, EventArgs e)
        {
            R = trackBar1.Value;
        }

        private void trackBar2_Scroll(object sender, EventArgs e)
        {
            G = trackBar2.Value;
        }

        private void trackBar3_Scroll(object sender, EventArgs e)
        {
            B = trackBar3.Value;
        }
        private System.Drawing.Point[] ToPointsArray(List<IntPoint> points)
        {
            System.Drawing.Point[] array = new System.Drawing.Point[points.Count];

            for (int i = 0, n = points.Count; i < n; i++)
            {
                array[i] = new System.Drawing.Point(points[i].X, points[i].Y);
            }
            return array;
        }
        public void nesnebul(Bitmap image)
        {
            BlobCounter blobCounter = new BlobCounter();
            blobCounter.MinWidth = 10;
            blobCounter.MinHeight = 10;
            blobCounter.FilterBlobs = true;
            blobCounter.ObjectsOrder = ObjectsOrder.Size;

            BitmapData objectsData = image.LockBits(new Rectangle(0, 0, image.Width, image.Height), ImageLockMode.ReadOnly, image.PixelFormat);
            Grayscale grayscaleFilter = new Grayscale(0.2125, 0.7154, 0.0721);
            UnmanagedImage grayImage = grayscaleFilter.Apply(new UnmanagedImage(objectsData));
            image.UnlockBits(objectsData);

            blobCounter.ProcessImage(image);
            Rectangle[] rects = blobCounter.GetObjectsRectangles();
            Blob[] blobs = blobCounter.GetObjectsInformation();
            try
            {
                pictureBox2.Image = image;
            }
            catch { }

            if (objeyakala.Checked)
            {
                foreach (Rectangle recs in rects)
                {
                    if (rects.Length > 0)
                    {
                        Rectangle objectRect = rects[0];
                        Graphics g = pictureBox1.CreateGraphics();
                        using (Pen pen = new Pen(Color.FromArgb(160, 255, 160), 5))
                        {
                            g.DrawRectangle(pen, objectRect);
                        }
                        int objectX = objectRect.X + (objectRect.Width / 2);
                        int objectY = objectRect.Y + (objectRect.Height / 2);

                        g.Dispose();

                        if (kordinat.Checked)
                        {
                            Invoke((MethodInvoker)delegate
                            {
                                richTextBox1.Text = objectRect.Location.ToString() + "\n" + richTextBox1.Text + "\n"; ;
                            });

                            if (objectX > 0 && objectX < 108 && objectY < 80)
                            {
                                serialPort1.Write("1");
                            }
                            if (objectX > 108 && objectX < 216 && objectY < 80)
                            {
                                serialPort1.Write("2");
                            }
                            if (objectX > 216 && objectX < 326 && objectY < 80)
                            {
                                serialPort1.Write("3");
                            }
                            if (objectX > 0 && objectX < 108 && objectY > 80 && objectY < 160)
                            {
                                serialPort1.Write("4");
                            }
                            if (objectX > 108 && objectX < 216 && objectY > 80 && objectY < 160)
                            {
                                serialPort1.Write("5");
                            }
                            if (objectX > 216 && objectX < 326 && objectY > 80 && objectY < 160)
                            {
                                serialPort1.Write("6");
                            }
                            if (objectX > 0 && objectX < 108 && objectY > 160 && objectY < 240)
                            {
                                serialPort1.Write("7");
                            }
                            if (objectX > 108 && objectX < 216 && objectY > 160 && objectY < 240)
                            {
                                serialPort1.Write("8");
                            }
                            if (objectX > 216 && objectX < 326 && objectY > 160 && objectY < 240)
                            {
                                serialPort1.Write("9");
                            }
                        }
                    }
                }
            }
        }

        private void button3_Click(object sender, EventArgs e)
        {
            serialPort1.BaudRate = 9600;
            serialPort1.PortName = comboBox2.SelectedItem.ToString();
            serialPort1.Open();
            if (serialPort1.IsOpen)
            {
                MessageBox.Show("Port Açık");
            }
    }

        private void button4_Click(object sender, EventArgs e)
        {
            serialPort1.Close();
            MessageBox.Show("Port Kapalı");
        }
    }
}

