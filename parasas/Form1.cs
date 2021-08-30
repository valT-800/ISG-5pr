using System;
using System.Data;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.Security.Cryptography;
namespace parasas
{
    public partial class Form1 : Form
    {
        ASCIIEncoding ByteConverter = new ASCIIEncoding();
        RSACryptoServiceProvider RSAalg = new RSACryptoServiceProvider();
        private byte[] originalData;
        RSAParameters Key;
        public Form1()
        {
            InitializeComponent();
        }
        
       
        public static byte[] HashAndSignBytes(byte[] DataToSign, RSAParameters Key)
        {
            try
            {     
                RSACryptoServiceProvider RSAalg = new RSACryptoServiceProvider();
                RSAalg.ImportParameters(Key);
                return RSAalg.SignData(DataToSign, SHA256.Create());
            }
            catch (CryptographicException e)
            {
                MessageBox.Show(e.Message);

                return null;
            }
        }
        
        private void Form1_Load(object sender, EventArgs e)
        {

        }

        private void button1_Click(object sender, EventArgs e)
        {
            string dataString = textBox1.Text;
            Key = RSAalg.ExportParameters(true);
            originalData = ByteConverter.GetBytes(dataString);

            var signedData = HashAndSignBytes(originalData, Key);

            richTextBox1.Text = String.Join(" ", signedData);

        }
        private void button2_Click(object sender, EventArgs e)
        {
            richTextBox2.Text = richTextBox1.Text;
        }

        public static bool VerifySignedHash(byte[] DataToVerify, byte[] SignedData, RSAParameters Key)
        {
            try
            {
                RSACryptoServiceProvider RSAalg = new RSACryptoServiceProvider();

                RSAalg.ImportParameters(Key);

                return RSAalg.VerifyData(DataToVerify, SHA256.Create(), SignedData);
            }
            catch (CryptographicException e)
            {
                Console.WriteLine(e.Message);

                return false;
            }
        }

        private void button3_Click(object sender, EventArgs e)
        {
            try
            {
                var changedString = richTextBox2.Text;
                var array  = changedString.Split(' ');
                var changedData = array.Select(byte.Parse).ToArray();

                if (VerifySignedHash(originalData, changedData, Key))
                {
                    MessageBox.Show("The data was verified");
                }
                else
                {
                    MessageBox.Show("The data does not match the signature");
                }

            }
            catch (ArgumentNullException)
            {
                MessageBox.Show("The data was not signed or verified");
            }
        }
    }
}
