//Kamil Matula gr. D, 12.03.2020
using System.Drawing;

namespace Wprowadzenie
{
    class Grafika
    {
        public static void greyPicture(string path) //Tworzenie obrazu w odcieniach szarości
        {
            Bitmap btm = new Bitmap(path);
            for (int i = 0; i < btm.Width; i++)
                for (int j = 0; j < btm.Height; j++)
                {
                    Color pxl = btm.GetPixel(i, j);
                    int avg = (pxl.R + pxl.G + pxl.B) / 3;
                    btm.SetPixel(i, j, Color.FromArgb(avg, avg, avg));
                }
            string photoFormat = path.Split('.')[path.Split('.').Length - 1];
            string outpath = path.Substring(0, path.Length - photoFormat.Length - 1) + "_grey." + photoFormat;
            btm.Save(outpath);
        }

        public static void filtr(string path) //Zastosowanie maski do przefiltrowania grafiki
        {
            Bitmap btm = new Bitmap(path);
            Bitmap btmF = new Bitmap(btm.Width, btm.Height);

            double[][] kernel = new double[3][];
            //mean removal (wyostrza):
            kernel[0] = new double[] { -1, -1, -1 }; kernel[1] = new double[] { -1, 9, -1 }; kernel[2] = new double[] { -1, -1, -1 };
            //filtr uwypuklający wschodni
            //kernel[0] = new double[] { -1, 0, 1 }; kernel[1] = new double[] { -1, 1, 1 }; kernel[2] = new double[] { -1, 0, 1 };
            //LAPL1:
            //kernel[0] = new double[] { 0, -1, 0 }; kernel[1] = new double[] { -1, 4, -1 }; kernel[2] = new double[] { 0, -1, 0 };

            double suma = 0;
            for (int q = 0; q < kernel.Length; q++) //suma wag maski
                for (int w = 0; w < kernel[0].Length; w++)
                    suma += kernel[q][w];

            for (int i = 0; i < btm.Width; i++)
                for (int j = 0; j < btm.Height; j++)
                {
                    double R = 0, G = 0, B = 0;
                    if (i == 0 || j == 0 || i == btm.Width - 1 || j == btm.Height - 1)
                        btmF.SetPixel(i, j, Color.FromArgb(btm.GetPixel(i, j).R, btm.GetPixel(i, j).G, btm.GetPixel(i, j).B));
                    else
                    {
                        for (int a = -1; a < kernel.Length - 1; a++)
                            for (int b = -1; b < kernel[0].Length - 1; b++)
                            {
                                Color pxl = btm.GetPixel(i + a, j + b);
                                R += kernel[a + 1][b + 1] * pxl.R;
                                G += kernel[a + 1][b + 1] * pxl.G;
                                B += kernel[a + 1][b + 1] * pxl.B;
                            }
                        if (suma != 0) { R /= suma; G /= suma; B /= suma; } //zapobiega to zmianie jasności obrazu
                        if (R > 255) R = 255; if (R < 0) R = 0;
                        if (G > 255) G = 255; if (G < 0) G = 0;
                        if (B > 255) B = 255; if (B < 0) B = 0;
                        btmF.SetPixel(i, j, Color.FromArgb((int)R, (int)G, (int)B));
                    }
                }
            string photoFormat = path.Split('.')[path.Split('.').Length - 1];
            string outpath = path.Substring(0, path.Length - photoFormat.Length - 1) + "_filtred." + photoFormat;
            btmF.Save(outpath);
        }
    }
}