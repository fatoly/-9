using System;
using System.Drawing;
using System.Windows.Forms;




namespace lab9
{
    public partial class Form1 : Form
    {
        Bitmap img;
        Bitmap changedImg;
       
        public Form1()
        {
            InitializeComponent();
        }


        private void openButton_Click(object sender, EventArgs e)
        {
            OpenFileDialog dialog = new OpenFileDialog();                       //описываем и порождаем объект dialog класса OpenFileDialog
            dialog.Filter = "Image files (*.BMP, *.JPG, *.GIF, *.TIF, *.PNG, *.ICO, *.EMF, *.WMF)|*.bmp;*.jpg;*.gif; *.tif; *.png; *.ico; *.emf; *.wmf";  //задаем расширения файлов 
            if (dialog.ShowDialog() == DialogResult.OK)                         //вызываем диалоговое окно и проверяем выбран ли файл
            {
                //Открываем файл картинки...
                Image streamImg = Image.FromFile(dialog.FileName); //Загружаем в image изображение из выбранного файла
                //Помещаем исходное изображение в PictureBox1
                pictureBox1.Image = streamImg;
                img = new Bitmap(streamImg);
                changedImg = new Bitmap(streamImg);

                Red.Enabled = true;
                Blue.Enabled = true;
                Green.Enabled = true;
                normal.Enabled = true;
                Negative.Enabled = true;
                Colorless.Enabled = true;
            }
        }



        private void saveButton_Click(object sender, EventArgs e)
        {
            SaveFileDialog savedialog = new SaveFileDialog();//описываем и порождаем объект savedialog 
            //задаем свойства для savedialog
            savedialog.Title = "Сохранить картинку как ...";
            savedialog.OverwritePrompt = true;
            savedialog.CheckPathExists = true;
            savedialog.Filter =
            "Bitmap File(*.bmp)|*.bmp|" +
            "GIF File(*.gif)|*.gif|" +
            "JPEG File(*.jpg)|*.jpg|" +
            "TIF File(*.tif)|*.tif|" +
            "PNG File(*.png)|*.png";
            savedialog.ShowHelp = true;
            // If selected, save
            if (savedialog.ShowDialog() == DialogResult.OK)//вызываем диалоговое окно и проверяем задано ли имя файла
            {
                // в строку fileName записываем указанный в savedialog полный путь к файлу
                string fileName = savedialog.FileName;
                // Убираем из имени три последних символа (расширение файла)
                string strFilExtn = fileName.Remove(0, fileName.Length - 3);
                // Сохраняем файл в нужном формате и с нужным расширением
                switch (strFilExtn)
                {
                    case "bmp":
                        img.Save(fileName, System.Drawing.Imaging.ImageFormat.Bmp);
                        break;
                    case "jpg":
                        img.Save(fileName, System.Drawing.Imaging.ImageFormat.Jpeg);
                        break;
                    case "gif":
                        img.Save(fileName, System.Drawing.Imaging.ImageFormat.Gif);
                        break;
                    case "tif":
                        img.Save(fileName, System.Drawing.Imaging.ImageFormat.Tiff);
                        break;
                    case "png":
                        img.Save(fileName, System.Drawing.Imaging.ImageFormat.Png);
                        break;
                    default:
                        break;
                }
            }
        }

        private Bitmap MakeGray(Bitmap bmp)
        {
            Bitmap shiftedImg = (Bitmap)bmp.Clone();
            //циклы для перебора всех пикселей на изображении
            for (int i = 0; i < shiftedImg.Width; i++)
            {
                for (int j = 0; j < bmp.Height; j++)
                {
                    int R = shiftedImg.GetPixel(i, j).R; //извлекаем в R значение красного цвета в текущей точке
                    int G = shiftedImg.GetPixel(i, j).G; //извлекаем в G значение зеленого цвета в текущей точке
                    int B = shiftedImg.GetPixel(i, j).B; //извлекаем в B значение синего цвета в текущей точке
                    int Gray = (R = G + B) / 3; // высчитываем среденее арифметическое трех каналов
                    Color p = Color.FromArgb(255, Gray, Gray, Gray); //переводим int в значение цвета. 255 - показывает степень прозрачности. остальные значения одинаковы для трех каналов R,G,B
                    shiftedImg.SetPixel(i, j, p); //записываме полученный цвет в текущую точку
                }
            }
            Refresh(); //вызываем функцию перерисовки окна


            return shiftedImg;
        }

        private Bitmap GetRedChannel(Bitmap source)
        {
            Bitmap result = new Bitmap(source.Width, source.Height);
            for (int i = 0; i < source.Width; i++)
            {
                for (int j = 0; j < source.Height; j++)
                {
                    Color color = source.GetPixel(i, j);
                    result.SetPixel(i, j, Color.FromArgb(color.A, color.R, 0, 0));
                }
            }
            return result;
        }

        private Bitmap GetBlueChannel(Bitmap source)
        {
            Bitmap result = new Bitmap(source.Width, source.Height);
            for (int i = 0; i < source.Width; i++)
            {
                for (int j = 0; j < source.Height; j++)
                {
                    Color color = source.GetPixel(i, j);
                    result.SetPixel(i, j, Color.FromArgb(color.A, 0, 0, color.B));
                }
            }
            return result;
        }

        private Bitmap GetGreenChannel(Bitmap source)
        {
            Bitmap result = new Bitmap(source.Width, source.Height);
            for (int i = 0; i < source.Width; i++)
            {
                for (int j = 0; j < source.Height; j++)
                {
                    Color color = source.GetPixel(i, j);
                    result.SetPixel(i, j, Color.FromArgb(color.A, 0, color.G, 0));
                }
            }
            return result;
        }

        public Bitmap negative(Bitmap sourceimage)
        {
            Bitmap result = new Bitmap(sourceimage.Width, sourceimage.Height);
            Color c;
            for (int i = 0; i < result.Width; i++)
            {
                for (int j = 0; j < result.Height; j++)
                {
                    c = sourceimage.GetPixel(i, j);
                    c = Color.FromArgb(255 - c.R, 255 - c.G, 255 - c.B);
                    result.SetPixel(i, j, c);
                }
            }
            return result;
        }


        private void Colorless_Click(object sender, EventArgs e)
        {
            changedImg = MakeGray(img);
            pictureBox1.Image = changedImg;
        }

        private void normal_Click(object sender, EventArgs e)
        {
            pictureBox1.Image = img;
            Refresh();
        }
        private void Red_Click(object sender, EventArgs e)
        {
            changedImg = GetRedChannel(img);
            pictureBox1.Image = changedImg;
        }

        private void Blue_Click(object sender, EventArgs e)
        {
            changedImg = GetBlueChannel(img);
            pictureBox1.Image = changedImg;
        }

        private void Green_Click(object sender, EventArgs e)
        {
            changedImg = GetGreenChannel(img);
            pictureBox1.Image = changedImg;

        }

        private void Negative_Click(object sender, EventArgs e)
        {
            changedImg = negative(img);
            pictureBox1.Image = changedImg;
        }

        private void pictureBox1_Click(object sender, EventArgs e)
        {

        }
    }
}
