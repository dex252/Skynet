using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Skynet.Converter.Tests
{
    [TestClass()]
    public class ImgConvertTests
    {
        [TestMethod()]
        public void ConvertTest()
        {
            var conventer = new ImgConvert();

            //var inputs = conventer.Convert("/images/clear.png");

            var inputs = conventer.Convert(@"C:\Users\Slava\Desktop\Skynet\Skynet\SkynetTests\images\paras.png");

           // conventer.Save("D:\\image.png", inputs);
        }
    }
}