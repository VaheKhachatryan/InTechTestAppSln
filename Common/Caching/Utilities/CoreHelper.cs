using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Http.Features;
using Newtonsoft.Json;
using System;
using System.Globalization;
using System.IO;
using System.Net;
using System.Threading.Tasks;

namespace BE.Common.Helpers
{
    public class CoreHelper
    {
        public static readonly JsonSerializerSettings JsonSettings = new JsonSerializerSettings
        {
            ReferenceLoopHandling = ReferenceLoopHandling.Ignore,
            //PreserveReferencesHandling = PreserveReferencesHandling.Objects,
            DateFormatHandling = DateFormatHandling.IsoDateFormat,
            DateTimeZoneHandling = DateTimeZoneHandling.Local
        };


        /// <summary>
        /// Converts object to JSON string
        /// </summary>
        /// <param name="obj"></param>
        /// <returns></returns>
        public static string SerializeJson<T>(T obj)
        {
            if (obj == null)
            {
                return null;
            }

            return JsonConvert.SerializeObject(obj, JsonSettings);
        }
        /// <summary>
        /// Converts JSON string to object
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="jsonData"></param>
        /// <returns></returns>
        public static T DeserializeJson<T>(string jsonData)
        {
            if (jsonData == null)
            {
                return default(T);
            }
            return JsonConvert.DeserializeObject<T>(jsonData, JsonSettings);
        }


        /// <summary>
        /// Get the raw path and full query of request
        /// </summary>
        /// <param name="request">HTTP request</param>
        /// <returns>Raw URL</returns>
        public static string GetRawUrl(HttpRequest request)
        {
            //first try to get the raw target from request feature
            //note: value has not been UrlDecoded
            var rawUrl = request.HttpContext.Features.Get<IHttpRequestFeature>()?.RawTarget;

            //or compose raw URL manually
            if (string.IsNullOrEmpty(rawUrl))
            {
                rawUrl = $"{request.PathBase}{request.Path}{request.QueryString}";
            }

            return rawUrl;
        }

        public static bool IsNumeric(object expression)
        {
            bool isNum = double.TryParse(Convert.ToString(expression), NumberStyles.Any, NumberFormatInfo.InvariantInfo, out _);
            return isNum;
        }

        public static bool IsBoolean(object expression)
        {
            bool isBool = bool.TryParse(Convert.ToString(expression), out _);
            return isBool;
        }

        public static byte[] GetFileBytes(IFormFile file)
        {
            byte[] fileBytes;
            using (var stream = new MemoryStream())
            {
                file.CopyTo(stream);
                fileBytes = stream.ToArray();
            }
            return fileBytes;
        }

        public static async Task<Stream> StringToStreamAsync(string text)
        {
            var stream = new MemoryStream();

            using (var writer = new StreamWriter(stream, leaveOpen: true))
            {
                await writer.WriteAsync(text);
            }

            stream.Position = 0;

            return stream;
        }

        public static bool UrlChecker(string url)
        {
            try
            {
                HttpWebRequest req = (HttpWebRequest)WebRequest.Create(url);
                req.AllowAutoRedirect = false;
                req.UserAgent = "Mozilla/5.0 (Windows; U; Windows NT 5.1; en-US; rv:1.8.0.6) Gecko/20060728 Firefox/1.5";
                HttpWebResponse res = (HttpWebResponse)req.GetResponse();
                return res.StatusCode is HttpStatusCode.OK or HttpStatusCode.Moved;
            }
            catch (Exception)
            {
                return false;
            }
        }
    }
}
