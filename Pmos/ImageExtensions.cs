using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web.Mvc;
using System.Windows.Media.Imaging;

namespace Pmos
{
    public static class ImageExtensions
    {
        public static Image[] ToPng(this Icon icon)
        {
            MemoryStream iconStream = new MemoryStream();

            icon.Save(iconStream);

            var decoder = new IconBitmapDecoder(
                iconStream,
                BitmapCreateOptions.PreservePixelFormat,
                BitmapCacheOption.None);

            List<Image> images = new List<Image>();

            foreach (var frame in decoder.Frames)
            {
                // save file as PNG
                BitmapEncoder encoder = new PngBitmapEncoder();
                encoder.Frames.Add(frame);
                var size = frame.PixelHeight;

                MemoryStream png = new MemoryStream();
                encoder.Save(png);
                Image i = Image.FromStream(png);

                images.Add(i);

            }

            return images.ToArray();
        }

        public static MvcHtmlString PngFromBytes(this HtmlHelper html, byte[] bytes)
        {
            return new MvcHtmlString(bytes.GetImageTag().ToString());
        }

        public static TagBuilder GetImageTag(this byte[] bytes, object htmlAttributes = null, string content = "image/png")
        {
            TagBuilder img = new TagBuilder("img");

            img.Attributes["src"] = string.Format("data:{0};base64, {1}", content, Convert.ToBase64String(bytes));

            if(htmlAttributes != null)
            {

                
                var customAttributes = HtmlHelper.AnonymousObjectToHtmlAttributes(htmlAttributes);

                foreach (KeyValuePair<string, object> customAttribute in customAttributes)
                {
                    img.MergeAttribute(customAttribute.Key.ToString(), customAttribute.Value.ToString());
                }


            }

            return img;
        }


    }
}
