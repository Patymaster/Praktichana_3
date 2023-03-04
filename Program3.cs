using System;
using System.Drawing;
using System.IO;


namespace ConsoleApplication1
{
    class Program
    {
        static void Main(string[] args)
        {
            // Задаємо шлях до папки з зображеннями
            string path = @"C:\Images\";

            // Оголошуємо делегати для операцій обробки зображення та відображення оброблених зображень
            Func<Bitmap, Bitmap> processingDelegate = InvertColors;
            Action<Bitmap> displayDelegate = DisplayImage;

            // Застосовуємо делегати до кожного зображення в папці
            foreach (string file in Directory.GetFiles(path, "*.jpg"))
            {
                Bitmap originalImage = new Bitmap(file);

                // Обробляємо зображення
                Bitmap processedImage = processingDelegate(originalImage);

                // Відображаємо оброблене зображення
                displayDelegate(processedImage);
            }
        }

        // Операція обернення кольорів
        static Bitmap InvertColors(Bitmap original)
        {
            Bitmap result = new Bitmap(original.Width, original.Height);

            for (int x = 0; x < original.Width; x++)
            {
                for (int y = 0; y < original.Height; y++)
                {
                    Color originalColor = original.GetPixel(x, y);
                    Color invertedColor = Color.FromArgb(
                        255 - originalColor.R,
                        255 - originalColor.G,
                        255 - originalColor.B
                    );
                    result.SetPixel(x, y, invertedColor);
                }
            }

            return result;
        }

        // Операція відображення зображення на екрані
        static void DisplayImage(Bitmap image)
        {
            image.Show();
        }
    }
}