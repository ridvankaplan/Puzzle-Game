
using System;
using System.Drawing;
using System.IO;
using System.Windows.Forms;
using Image = System.Drawing.Image;

namespace Puzzle_Oyunu
{
    public partial class Form1 : System.Windows.Forms.Form
    {
        String filepath = null;
        Bitmap[,] originalimage = new Bitmap[4, 4];
        Bitmap[,] randomimage = new Bitmap[4, 4];
        Bitmap[,] tempimage = new Bitmap[4, 4];
        int right=0,tempright=0;
        int point = 60;       
        int times;

        public Form1()
        {
            InitializeComponent();
        }

        private void mixbutton_Click(object sender, EventArgs e)
        {
            if (filepath == null)
            {
                MessageBox.Show("Öncelikle Resim Seçmeniz Gereklidir!!!");
                return;
            }
            Image image = Image.FromFile(filepath);

            this.point = 60;

            int widthThird = (int)((double)image.Width / 4.0);
            int heightThird = (int)((double)image.Height / 4.0);

            for (int i = 0; i < 4; i++)
                for (int j = 0; j < 4; j++)
                {
                    originalimage[i, j] = new Bitmap(widthThird, heightThird);
                    Graphics g = Graphics.FromImage(originalimage[i, j]);
                    g.DrawImage(image, new Rectangle(0, 0, widthThird, heightThird), new Rectangle(j * widthThird, i * heightThird, widthThird, heightThird), GraphicsUnit.Pixel);
                    g.Dispose();
                }

            randomControl();
            setPictureBox();
            pictureControl(); //En az 1 resmin dogru yere gelmesini kontrol eder

        }

        private void findMax()
        {
            try { 

            StreamReader reader = File.OpenText("C:\\Users\\Ridvan\\Desktop\\enyuksekskor.txt");
            string line;
            int[] points = new int[100];
            int i = 1;
            line = reader.ReadLine();
                try
                {
                    points[i] = Int32.Parse(line);
                    Console.Write(points[i]);
                }
                catch(FormatException)
                {                }
            
            while (line != null)
            {
                    try
                    {
                        points[i] = Int32.Parse(line);
                    }
                    catch (FormatException)
                    { }

                i++;
                line = reader.ReadLine();
            }

            int max= points[0];
            for (i=1;i<points.Length ; i++)
            {
                if(max < points[i])
                {
                    max = points[i];
                }
            }
            maxpointLabel.Text = "En Yüksek Puan : " + max;
            reader.Close();
            }
            catch (FileNotFoundException ex)
            { return; }
        }

        private void chooseImage_Click(object sender, EventArgs e)
    {
        OpenFileDialog file = new OpenFileDialog();
        file.Filter = "Resim Dosyaları| *.jpg;*.jpeg;*.png"; //Jpg, jpeg, png uzantılarına izin verir
        file.InitialDirectory = ".";
        if (file.ShowDialog() == DialogResult.OK)
        {
            this.filepath = file.FileName;
        }
    }

        private void randomControl() {

            int[,] control = new int[4, 4];
            int i, j;
            for (i = 0; i < 4; i++)
            {
                for (j = 0; j < 4; j++)
                    control[i, j] = 0;
            }
            Random rand = new Random();
            int index1, index2;
            i = 0; j = 0;
            while (i < 4)
            {
                while (j < 4)
                {
                    index1 = rand.Next(0, 4);
                    index2 = rand.Next(0, 4);

                    if (control[index1, index2] == 0)
                    {
                        randomimage[index1, index2] = originalimage[i, j];
                        control[index1, index2] = 1;
                        j++;
                    }
                }
                i++; j = 0;
            }
        }

        //Random olarak picturebox'lara resimleri atiyor
        private void setPictureBox()
       {
              pictureBox.Image = randomimage[0, 0];
              pictureBox1.Image = randomimage[0, 1];
              pictureBox2.Image = randomimage[0, 2];
              pictureBox3.Image = randomimage[0, 3];
              pictureBox4.Image = randomimage[1, 0];
              pictureBox5.Image = randomimage[1, 1];
              pictureBox6.Image = randomimage[1, 2];
              pictureBox7.Image = randomimage[1, 3];
              pictureBox8.Image = randomimage[2, 0];
              pictureBox9.Image = randomimage[2, 1];
              pictureBox10.Image = randomimage[2, 2];
              pictureBox11.Image = randomimage[2, 3];
              pictureBox12.Image = randomimage[3, 0];
              pictureBox13.Image = randomimage[3, 1];
              pictureBox14.Image = randomimage[3, 2];
              pictureBox15.Image = randomimage[3, 3];

            pictureControl();
        }
        
        private void pictureControl()
    {

            right = 0;
            for (int i = 0; i < 4; i++)
            {
                for (int j = 0; j < 4; j++)
                {
                    var ms = new System.IO.MemoryStream();
                    randomimage[i, j].Save(ms, System.Drawing.Imaging.ImageFormat.Jpeg);
                    String base64_1 = Convert.ToBase64String(ms.ToArray());
                    ms = new System.IO.MemoryStream();
                    originalimage[i, j].Save(ms, System.Drawing.Imaging.ImageFormat.Jpeg);
                    String base64_2 = Convert.ToBase64String(ms.ToArray());

                    if (base64_1 == base64_2)
                        right++;

                }
            }
            rightlabel.Text = "Yerinde olan parça adedi : " + right;
            if (right == 0)
            {
                MessageBox.Show("En az 1 resim doğru gelmelidir. Tekrar Deneyiniz!");
              //  Environment.Exit(0);           
            }
            else if (right == 16)
            {
                MessageBox.Show("Mükemmel ! Tüm parça yerinde!");
                saveFile();
                Environment.Exit(0);
            }

            pointControl(right);
            findMax();
        }

        //Dogru resim oldugunde 4 puan arttırılıyor yanlis oldugunda 8 puan eksiltiliyor
        private void pointControl(int right)
        {
            if (tempright < right)
            {
                point += 4;
            }
            else
            {
                point -= 8;
            }
            tempright = right;

            if (point > 100)
                point = 100;
            else if (point < 0)
                point = 0;
    
            pointLabel.Text = "Güncel Puanınız : "+ point;

        }

        private void saveFile()
        {
            string path = @"C:\\Users\\Ridvan\\Desktop\\enyuksekskor.txt";
            System.IO.File.AppendAllText(path, point+"\n");
        }

        private void swapPicture()
        {
            int a=-1, b=-1;
            int c=-1, d=-1;
            int control = 0;

            for (int i = 0; i < 4; i++)
            {
                for(int j=0;j<4;j++)
                {
                    if(tempimage[i,j]!=null && control==0)
                    {
                        a = i;b = j;
                        c = i;d = j;
                        control = 1;
                    }
                    else if(tempimage[i, j] != null && control == 1)
                    {
                        c = i; d = j;
                    }
                }
            }

            randomimage[a, b] = tempimage[c, d];
            randomimage[c,d] = tempimage[a,b];

            times = 0;
            setPictureBox();
            clearTempImage();
            }
        
        private void timesControl()
        {
            if (times == 2)
                swapPicture();
        }

        private void clearTempImage()
        {
            for (int i = 0; i < 4; i++)
            {
                for (int j = 0; j < 4; j++)
                {
                    tempimage[i, j] = null;
                }
            }
        }

        private void setTempImage(int count)
        {
            switch (count)
            {
                case (0):
                    tempimage[0, 0] = randomimage[0, 0];
                    break;
                case (1):
                    tempimage[0, 1] = randomimage[0, 1];
                    break;
                case (2):
                    tempimage[0, 2] = randomimage[0, 2];
                    break;
                case (3):
                    tempimage[0, 3] = randomimage[0, 3];
                    break;
                case (4):
                    tempimage[1, 0] = randomimage[1, 0];
                    break;
                case (5):
                    tempimage[1, 1] = randomimage[1, 1];
                    break;
                case (6):
                    tempimage[1, 2] = randomimage[1, 2];
                    break;
                case (7):
                    tempimage[1, 3] = randomimage[1, 3];
                    break;
                case (8):
                    tempimage[2, 0] = randomimage[2, 0];
                    break;
                case (9):
                    tempimage[2, 1] = randomimage[2, 1];
                    break;
                case (10):
                    tempimage[2, 2] = randomimage[2, 2];
                    break;
                case (11):
                    tempimage[2, 3] = randomimage[2, 3];
                    break;
                case (12):
                    tempimage[3, 0] = randomimage[3, 0];
                    break;
                case (13):
                    tempimage[3, 1] = randomimage[3, 1];
                    break;
                case (14):
                    tempimage[3, 2] = randomimage[3, 2];
                    break;
                case (15):
                    tempimage[3, 3] = randomimage[3, 3];
                    break;
            }
            times++;
            timesControl();
        }

        private void pictureBox_Click(object sender, EventArgs e)
        { setTempImage(0); }       
        private void pictureBox1_Click(object sender, EventArgs e)
        {   setTempImage(1);        }
        private void pictureBox2_Click(object sender, EventArgs e)
        {setTempImage(2);       }
        private void pictureBox3_Click(object sender, EventArgs e)
        {    setTempImage(3);       }
        private void pictureBox4_Click(object sender, EventArgs e)
        {  setTempImage(4);        }
        private void pictureBox5_Click(object sender, EventArgs e)
        {  setTempImage(5);        }
        private void pictureBox6_Click(object sender, EventArgs e)
        { setTempImage(6);        }
        private void pictureBox7_Click(object sender, EventArgs e)
        { setTempImage(7);        }
        private void pictureBox8_Click(object sender, EventArgs e)
        { setTempImage(8);        }
        private void pictureBox9_Click(object sender, EventArgs e)
        { setTempImage(9);        }
        private void pictureBox10_Click(object sender, EventArgs e)
        {   setTempImage(10);        }
        private void pictureBox11_Click(object sender, EventArgs e)
        {setTempImage(11);        }
        private void pictureBox12_Click(object sender, EventArgs e)
        {setTempImage(12);        }
        private void pictureBox13_Click(object sender, EventArgs e)
        {setTempImage(13);        }
        private void pictureBox14_Click(object sender, EventArgs e)
        { setTempImage(14);        }
        private void pictureBox15_Click(object sender, EventArgs e)
        { setTempImage(15);        }

    }

    }
 