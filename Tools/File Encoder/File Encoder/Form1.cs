namespace File_Encoder
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
        }

        private void Form1_Load(object sender, EventArgs e)
        {

        }

        private void button1_Click(object sender, EventArgs e)
        {
            string filePath = textBox2.Text;   

            try
            {
            
                byte[] fileBytes = File.ReadAllBytes(filePath);

           
                string base64Encoded = Convert.ToBase64String(fileBytes);


                textBox1.Text = base64Encoded;
                
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message + " " + ex.StackTrace);
            }
        }
    }
}
