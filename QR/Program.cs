using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace QR
{
    internal static class Program
    {
  
        [STAThread]
        static void Main()
        {
            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);
            try
            {
                using (Form2 form2 = new Form2())
                {
                    // Form2'yi dialog olarak göster
                    if (form2.ShowDialog() == DialogResult.OK)
                    {
                        // Form2 OK ile kapandıysa Form1'i başlat
                        Application.Run(new Form1());
                    }
                    // Form2 OK ile kapatılmadıysa uygulama doğal olarak kapanır
                }
            }
            catch (Exception ex)
            {
                // Hata olursa mesaj göster
                MessageBox.Show("Bir hata meydana geldi: " + ex.Message, "Hata", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }
    }
}
   

