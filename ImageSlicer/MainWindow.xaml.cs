using System;
using System.Collections;
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

namespace ImageSlicer
{
    /// <summary>
    /// MainWindow.xaml에 대한 상호 작용 논리
    /// </summary>
    public partial class MainWindow : Window
    {
        ArrayList arrLines = new ArrayList();
        string strPath = string.Empty;
        string strFileName = string.Empty;

        public MainWindow()
        {
            InitializeComponent();

            this.Loaded += MainWindow_Loaded;
        }

        private void MainWindow_Loaded(object sender, RoutedEventArgs e)
        {
            InitControl();
        }

        private void InitControl()
        {
            this.btnSelectImage.Click += BtnSelectImage_Click;
            this.btnAddLine.Click += BtnAddLine_Click;
            this.btnSplit.Click += BtnSplit_Click;
        }

        private void BtnSelectImage_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                Microsoft.Win32.OpenFileDialog ofd = new Microsoft.Win32.OpenFileDialog();

                if(ofd.ShowDialog() == true)
                {
                    this.tbImageName.Text = ofd.FileName;

                    BitmapImage bitmap = new BitmapImage();
                    bitmap.BeginInit();
                    bitmap.UriSource = new Uri(tbImageName.Text);
                    bitmap.EndInit();

                    imgMain.Source = bitmap;

                    imgMain.Width = bitmap.Width;

                    strPath = ofd.FileName.Replace(ofd.SafeFileName, "");
                    string[] sprit = ofd.SafeFileName.Split('.');

                    strFileName = ofd.SafeFileName.Remove(ofd.SafeFileName.Length - sprit[sprit.Length - 1].Length - 1, sprit[sprit.Length-1].Length + 1);

                    arrLines.Clear();

                    for (int i = canvasImage.Children.Count -1; i >= 1; i--)
                    {
                        if (!canvasImage.Children[i].GetType().Name.Equals("Image"))
                        {
                            canvasImage.Children.Remove(canvasImage.Children[i]);
                        }
                    }
                }
            }
            catch(Exception ex)
            {
                LOG.ex(ex);
            }
        }

        private void BtnAddLine_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                UCLine newLine = new UCLine();

                canvasImage.Children.Add(newLine);

                newLine.Width = imgMain.Width;
                newLine.Height = 30;

                Canvas.SetTop(newLine, scrImage.VerticalOffset + scrImage.ActualHeight / 2);

                newLine.setY();

                arrLines.Add(newLine);

            }
            catch (Exception ex)
            {
                LOG.ex(ex);
            }
        }

        private void BtnSplit_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                int oldTop = 0;

                btnAddLine.IsEnabled = false;
                btnSelectImage.IsEnabled = false;
                btnSplit.IsEnabled = false;

                for(int i = 0; i < arrLines.Count; i++)
                {
                    UCLine ucLine = arrLines[i] as UCLine;

                    RenderTargetBitmap rtb = new RenderTargetBitmap((int)imgMain.Width, (int)imgMain.ActualHeight, 96d, 96d, System.Windows.Media.PixelFormats.Default);
                    rtb.Render(imgMain);

                    var crop = new CroppedBitmap(rtb, new Int32Rect(0, oldTop, (int)imgMain.Width, ucLine.Y - oldTop));

                    BitmapEncoder pngEncorder = new PngBitmapEncoder();
                    pngEncorder.Frames.Add(BitmapFrame.Create(crop));

                    using (var fs = System.IO.File.OpenWrite(strPath + strFileName + i.ToString() + ".png"))
                    {
                        pngEncorder.Save(fs);
                    }

                    oldTop = ucLine.Y;
                }

                btnAddLine.IsEnabled = true;
                btnSelectImage.IsEnabled = true;
                btnSplit.IsEnabled = true;

                MessageBox.Show("Complete");
            }
            catch(Exception ex)
            {
                LOG.ex(ex);
            }
        }
    }
}
