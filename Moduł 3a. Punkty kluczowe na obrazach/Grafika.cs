// Kamil Matula, gr. D, 25.03.2020, Algorytm wyszukiwania punktów kluczowych

using System.Collections.Generic;
using System.Linq;
using System.Drawing;

namespace PunktyKluczowe
{
    class Grafika
    {
        public static void BlueRed(string path, bool blue = false)
        {
            // Niebieski czy czerwony?
            Color decision = Color.FromArgb(255, 0, 0);
            if (blue == true) decision = Color.FromArgb(0, 0, 255);

            // Przefiltrowanie i zebranie wartości jasności z pikseli (średniej z kwadratu 3x3)
            Bitmap filtered = filtr(path);
            Bitmap original = new Bitmap(path);

            List<float> brightnesses = new List<float>(); float temp;
            for (int j = 0; j < filtered.Height; j++)
                for (int i = 0; i < filtered.Width; i++)
                {
                    if (i == 0 || i == filtered.Width-1 || j == 0 || j == filtered.Height-1)
                        brightnesses.Add(0);
                    else
                    {
                        temp = 0;
                        for (int a = -1; a <= 1; a++)
                            for (int b = -1; b <= 1; b++)
                                temp += filtered.GetPixel(i + a, j + b).GetBrightness();
                        brightnesses.Add(temp/9);
                    }
                }

            // Pokolorowanie najjaśniejszych pikseli (5% wszystkich pikseli) na oryginalnym obrazku
            int maxIndex = brightnesses.IndexOf(brightnesses.Max());
            int pixelsToChange = filtered.Width * filtered.Height / 20;
            for (int pixelNumber = 0; pixelNumber < pixelsToChange; pixelNumber++)
            {
                original.SetPixel(maxIndex % filtered.Width, maxIndex / filtered.Width, decision);
                brightnesses[maxIndex] = 0;
                maxIndex = brightnesses.IndexOf(brightnesses.Max());
            }

            // Zapis do pliku
            string photoFormat = path.Split('.')[path.Split('.').Length - 1];
            string color = "red"; if (blue == true) color = "blue";
            string outpath = path.Substring(0, path.Length - photoFormat.Length - 1) + "_" + color + "." + photoFormat;
            original.Save(outpath);
        }

        public static Bitmap filtr(string path)
        {
            Bitmap btm = new Bitmap(path);
            Bitmap btmF = new Bitmap(btm.Width, btm.Height);
            double[][] kernel = new double[3][];
            kernel[0] = new double[] { -1, -1, -1 }; 
            kernel[1] = new double[] { -1, 8, -1 }; 
            kernel[2] = new double[] { -1, -1, -1 };

            // Suma wag maski:
            double suma = 0;
            for (int q = 0; q < kernel.Length; q++)
                for (int w = 0; w < kernel[0].Length; w++)
                    suma += kernel[q][w];

            for (int i = 0; i < btm.Width; i++)
                for (int j = 0; j < btm.Height; j++)
                {
                    double R = 0, G = 0, B = 0;
                    if (i == 0 || j == 0 || i == btm.Width - 1 || j == btm.Height - 1) // Krawędzie bez zmian
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
            return btmF;
        }
    }
}