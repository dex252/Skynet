using System.Collections.Generic;
using System.Drawing;

namespace Skynet.Converter
{
    /// <summary>
    /// Ковентер цветных изображений в черно-белые
    /// </summary>
    public class ImgConvert
    {
        public int Threshold { get; set; } = 128;
        /// <summary>
        /// Конверт цветного изображения в список черных и белых пикселей
        /// false - черный, true - белый
        /// </summary>
        public List<bool> Convert(string path)
        {
            var result = new List<bool>();

            Bitmap img = new Bitmap(path);

            for (int x = 0; x < img.Width; x++)
            {
                for (int y = 0; y < img.Height; y++)
                {
                    var pixel = img.GetPixel(x, y);

                    result.Add(Brightness(pixel));
                }
            }

            return result;
        }

        /// <summary>
        /// Сохранение конечного изображения после обработки [string] - путь к сохранению, [int, int] - ширина и высота изображения, [List] - список черно-белых пикселей
        /// </summary>
        public void Save (string pathToSave, int width, int height, List<bool> pixels)
        {
            var img = new Bitmap(width, height);

            for (int x = 0; x < width; x++)
            {
                for (int y = 0; y < height; y++)
                {
                    var color = pixels[y * width + x] == true ? Color.White : Color.Black;
                    img.SetPixel(x, y, color);
                }
            }

            img.Save(pathToSave);
        }

        /// <summary>
        /// Конверт одного пикеселя в черно-белый вариант
        /// </summary>
        private bool Brightness(Color pixel)
        {
           
            var result = (0.299 * pixel.R + 0.587 * pixel.G + 0.114 * pixel.B);

            return !(result < Threshold);
        }
    }
}
