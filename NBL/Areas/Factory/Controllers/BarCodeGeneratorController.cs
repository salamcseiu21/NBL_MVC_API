using System;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Imaging;
using System.IO;
using System.Linq;
using System.Text.RegularExpressions;
using System.Web;
using System.Web.Mvc;
using NBL.BLL.Contracts;
using OnBarcode.Barcode.BarcodeScanner;

namespace NBL.Areas.Factory.Controllers
{
    [Authorize(Roles = "Factory")]
    public class BarCodeGeneratorController : Controller
    {
        private ICommonManager _iCommonManager;

        public BarCodeGeneratorController(ICommonManager iCommonManager)
        {
            _iCommonManager = iCommonManager;
        }
        // GET: BarCodeDemo
        public ActionResult GenerateBarCode()
        {

           
            
            return View();
        }

        [HttpPost]
        public ActionResult GenerateBarCode(string barcode)
        {

            var barcodeList = _iCommonManager.GetAllTestBarcode();
            foreach (string s in barcodeList.Take(Convert.ToInt32(barcode)))
            {
                GenerateBarCodeFromaGivenString(Regex.Replace(s, @"\t|\n|\r", ""));
            }
            return View();
        }

        private void GenerateBarCodeFromaGivenString(string barcode)
        {
            using (MemoryStream memoryStream = new MemoryStream())
            {
                using (Bitmap bitMap = new Bitmap(barcode.Length * 40, 80))
                {
                    using (Graphics graphics = Graphics.FromImage(bitMap))
                    {
                        Font oFont = new Font("IDAutomationHC39M", 16);
                        PointF point = new PointF(2f, 2f);
                        SolidBrush whiteBrush = new SolidBrush(Color.White);
                        graphics.FillRectangle(whiteBrush, 0, 0, bitMap.Width, bitMap.Height);
                        SolidBrush blackBrush = new SolidBrush(Color.Black);
                        graphics.DrawString("*" + barcode + "*", oFont, blackBrush, point);
                    }

                    bitMap.Save(memoryStream, ImageFormat.Jpeg);

                    ViewBag.BarcodeImage = "data:image/png;base64," + Convert.ToBase64String(memoryStream.ToArray());
                    var filePath = Server.MapPath("~/Areas/Factory/Images/BarCodes/" + barcode + ".jpg");
                    Image img = System.Drawing.Image.FromStream(memoryStream);
                    img.Save(filePath, ImageFormat.Jpeg);
                }
            }
        }

        public ActionResult BarCodeRead()
        {
            return View();
        }


        [HttpPost]
        public ActionResult BarCodeRead(HttpPostedFileBase barCodeUpload)
        {


            String localSavePath = "~/Areas/Factory/Images/BarCodes/";
            string str = string.Empty;
            string strImage = string.Empty;
            string strBarCode = string.Empty;

            if (barCodeUpload != null)
            {
                String fileName = barCodeUpload.FileName;
                localSavePath += fileName;
                barCodeUpload.SaveAs(Server.MapPath(localSavePath));

                Bitmap bitmap = null;
                try
                {
                    bitmap = new Bitmap(barCodeUpload.InputStream);
                }
                catch (Exception ex)
                {
                    ex.ToString();
                }

                if (bitmap == null)
                {

                    str = "Your file is not an image";

                }
                else
                {
                    strImage = "http://localhost:" + Request.Url.Port + "/Areas/Factory/Images/BarCodes/" + fileName;

                    strBarCode = ReadBarcodeFromFile(Server.MapPath(localSavePath));

                }
            }
            else
            {
                str = "Please upload the bar code Image.";
            }
            ViewBag.ErrorMessage = str;
            ViewBag.BarCode = strBarCode;
            ViewBag.BarImage = strImage;
            return View();
        }
        private String ReadBarcodeFromFile(string _Filepath)
        {
            String[] barcodes = BarcodeScanner.Scan(_Filepath, BarcodeType.Code39);
            return barcodes[0];
        }
    }
}