using System.Collections.Generic;
using System.Drawing;

namespace Skynet.Converter
{
    /// <summary>
    /// Ковентер цветных изображений в черно-белые
    /// </summary>
    public class ImgConvert
    {
        /// <summary>
        /// [int] - пороговая величина для перекраски пикселей в черный цвет (0-127) и белый (128-255)
        /// </summary>
        public int Threshold { get; set; } = 128;
        /// <summary>
        /// [int] - Ширина последнего загруженного изображения
        /// </summary>
        private int Width { get;  set; }
        /// <summary>
        /// [int] - Высота последнего загруженного изображения
        /// </summary>
        private int Height { get;  set; }
        /// <summary>
        /// Конверт цветного изображения в список черных и белых пикселей
        /// false - черный, true - белый
        /// </summary>
        public float[] Convert(string path)
        {
            var result = new List<float>();

            Bitmap img = new Bitmap(path);
            var resizeImg = new Bitmap(img, new Size(50, 50));

            Width = resizeImg.Width;
            Height = resizeImg.Height;

            for (int y = 0; y < resizeImg.Height; y++)
            {
                for (int x = 0; x < resizeImg.Width; x++)
                {
                    var pixel = resizeImg.GetPixel(x, y);
                        
                    result.Add(Brightness(pixel));
                }
            }

            return result.ToArray();
        }

        /// <summary>
        /// Сохранение конечного изображения после обработки [string] - путь к сохранению, [int, int] - ширина и высота изображения, [List] - список черно-белых пикселей
        /// </summary>
        public void Save (string pathToSave, List<float> pixels)
        {
            var img = new Bitmap(Width, Height);

            for (int y = 0; y < Height; y++)
            {
                for (int x = 0; x < Width; x++)
                {
                    var color = pixels[y * Width + x] == 1f ? Color.White : Color.Black;
                    img.SetPixel(x, y, color);
                }
            }

            img.Save(pathToSave);
        }

        /// <summary>
        /// Конверт одного пикеселя в черно-белый вариант
        /// </summary>
        private float Brightness(Color pixel)
        {
           
            var result = (float)(0.299 * pixel.R + 0.587 * pixel.G + 0.114 * pixel.B);

            return result < Threshold ? 0f : 1f;
           
        }
    }
}
