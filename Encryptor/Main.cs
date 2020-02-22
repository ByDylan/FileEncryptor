using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Security.Cryptography;
using System.IO;
using System.Threading;

namespace Encryptor
{
    public partial class Main : Form
    {
        public Main()
        {
            InitializeComponent();
        }

        private void Main_Load(object sender, EventArgs e)
        {
            Thread.Sleep(2300);
            Console.Beep();
            this.FormBorderStyle = FormBorderStyle.FixedSingle;
        }

        private void button2_Click(object sender, EventArgs e)
        {
            OpenFileDialog FDG = new OpenFileDialog();
            FDG.Filter ="All Files|*.*";
            if (FDG.ShowDialog() == DialogResult.OK)
            {
                string path = FDG.FileName.ToString();
                textBox1.Text = path;
            }
        }

        public byte[] AES_Encrypt(byte[] bytesToBeEncrypted, byte[] passwordBytes)
        {
            byte[] encryptedBytes = null;

            byte[] saltBytes = new byte[] { 1, 2, 38, 6, 4, 5, 6, 3, 4, 9, 6, 34, 4, 9, 4, 3, 3, 46, 54, 64, 5, 31, 24, 65, 1, 63, 4, 56, 46, 3, 7, 3 };

            using (MemoryStream ms = new MemoryStream())
            {
                using (RijndaelManaged AES = new RijndaelManaged()) 
                {
                    AES.KeySize = 256;
                    AES.BlockSize = 128;

                    var key = new Rfc2898DeriveBytes(passwordBytes, saltBytes, 1000);
                    AES.Key = key.GetBytes(AES.KeySize / 8);
                    AES.IV = key.GetBytes(AES.BlockSize / 8);

                    AES.Mode = CipherMode.CBC;

                    using (var cs = new CryptoStream(ms, AES.CreateEncryptor(), CryptoStreamMode.Write))
                    {
                        cs.Write(bytesToBeEncrypted, 0, bytesToBeEncrypted.Length);
                        cs.Close();
                    }
                    encryptedBytes = ms.ToArray();
                }
            }

            return encryptedBytes;
        }

        private void button1_Click(object sender, EventArgs e)
        {
            byte[] decrypted = File.ReadAllBytes(textBox1.Text);
            byte[] encrypted = AES_Encrypt(decrypted, Encoding.UTF8.GetBytes("WET43QIOahijbnfjkasiHIOFOI"));
            string path = textBox1.Text.Substring(0, textBox1.Text.Length) + "encrypt";
            File.WriteAllBytes(path, encrypted);
            Thread.Sleep(2300);
            MessageBox.Show("Success");
            Close();
        }
    }

}