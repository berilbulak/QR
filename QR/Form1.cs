using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.IO;
using AForge;
using AForge.Video;
using AForge.Video.DirectShow;
using ZXing;
using ZXing.Aztec;
using System.Net.Http;
using ClosedXML.Excel;
using IronOcr;

namespace QR
{
    public partial class Form1 : Form
    {
        private FilterInfoCollection CaptureDevice;
        private VideoCaptureDevice FinalFrame;
        private bool hasRedirected ;
        private readonly HttpClient _httpClient = new HttpClient();
        private readonly IronTesseract ocr = new IronTesseract();

        public Form1()
        {
            InitializeComponent();
            
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            CaptureDevice = new FilterInfoCollection(FilterCategory.VideoInputDevice);
            if (CaptureDevice.Count == 0)
            {
                MessageBox.Show("No video devices found.");
                return;
            }
            foreach (FilterInfo Device in CaptureDevice)
            {
                comboBox1.Items.Add(Device.Name);
            }
            comboBox1.SelectedIndex = 0;
            FinalFrame = new VideoCaptureDevice();

        }

        private void button1_Click(object sender, EventArgs e)
        {
            try
            {
               
                FinalFrame = new VideoCaptureDevice(CaptureDevice[comboBox1.SelectedIndex].MonikerString);
                
                FinalFrame.NewFrame += new NewFrameEventHandler(FinalFrame_NewFrame);
               
                FinalFrame.Start();
            }
            catch
            {
                MessageBox.Show("Bir hata meydana geldi: ", "hata", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            
        
        }


        private void FinalFrame_NewFrame(object sender, NewFrameEventArgs eventArgs)
        {
            try
            {
                
                    pictureBox1.Image?.Dispose();
                

                using (Bitmap frame = (Bitmap)eventArgs.Frame.Clone())
                {
                    pictureBox1.Image = new Bitmap(frame);
                }

                
                
                    ProcessQRCodeOrBarcode();
                
            }
            catch (Exception ex)
            {
                MessageBox.Show("Görüntülenirken bir hata oluştu: " + ex.Message);
            }
        }

        private string targetUrl = "https://monitoring.e-kassa.gov.az/#/index?doc=BdcQ9LPwqcNy2gSyQYnpgRvodhY14c8ig1zvMjDMPzyx";
        private string redirectUrl = "https://monitoring.e-kassa.gov.az/pks-monitoring/2.0.0/documents/BdcQ9LPwqcNy2gSyQYnpgRvodhY14c8ig1zvMjDMPzyx";

        private void ProcessQRCodeOrBarcode()
        {
            if (pictureBox1.Image == null)
            {
                return;
            }
            Bitmap processedImage = new Bitmap(pictureBox1.Image);

            try
            {
                using (var input = new OcrInput())
                {
                    input.LoadImage(processedImage);
                    var result = ocr.Read(input);

                    if (result.Barcodes.Any())
                    {
                        foreach (var barcode in result.Barcodes)
                        {
                            textBox1.Text = barcode.Value;

                            if (barcode.Value.Equals(targetUrl, StringComparison.OrdinalIgnoreCase) && !hasRedirected)
                            {
                                System.Diagnostics.Process.Start(new System.Diagnostics.ProcessStartInfo
                                {
                                    FileName = redirectUrl,
                                    UseShellExecute = true
                                });

                                hasRedirected = true;
                                ProcessImageFromWeb("https://monitoring.e-kassa.gov.az/pks-monitoring/2.0.0/documents/BdcQ9LPwqcNy2gSyQYnpgRvodhY14c8ig1zvMjDMPzyx", @"C:\path\to\output.xlsx");
                            }
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error decoding QR code or Barcode: " + ex.Message);
            }
            finally
            {
                processedImage.Dispose();
            }
        }



        private void ProcessImageFromWeb(string imageUrl, string excelFilePath)
        {
            // Resmi indir
            Bitmap image = DownloadImage(imageUrl);

            // Resmi OCR ile işle
            string extractedText = PerformOCRWithIronTesseract(image);


            // Çıktıyı Excel dosyasına yaz
            WriteToExcel(extractedText, excelFilePath);
        }

      

        private Bitmap DownloadImage(string url)
        {
            var response = _httpClient.GetAsync(url).Result;
            response.EnsureSuccessStatusCode();

            using (Stream stream = response.Content.ReadAsStreamAsync().Result)
            {
                return (Bitmap)Image.FromStream(stream);
            }
        }


        private string PerformOCRWithIronTesseract(Bitmap image)
        {
            string extractedText = string.Empty;
            {
                using (var input = new OcrInput())
                {
                    input.LoadImage(image);
                    var result = ocr.Read(input);
                    extractedText = result.Text;
                }
            }
            return extractedText;
        }

        private void WriteToExcel(string text, string filePath)
        {
            using (var workbook = new XLWorkbook())
            {
                var worksheet = workbook.Worksheets.Add("ExtractedData");
                worksheet.Cell(1, 1).Value = "Extracted Text";
                worksheet.Cell(2, 1).Value = text;

                workbook.SaveAs(filePath);
            }
        }

        private void Form1_FormClosing(object sender, FormClosingEventArgs e)
        {
           if (FinalFrame != null && FinalFrame.IsRunning)
           {
                FinalFrame.Stop();
           }
        }
        
    }
}
