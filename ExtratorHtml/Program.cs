using HtmlAgilityPack;
using System;
using System.IO;
using System.Net;
using System.Text;


namespace ExtratorHtml
{
    class Program
    {
        public static void Main(string[] args)
        {
            
            string formUrl = "https://login.aliexpress.com/"; // NOTE: This is the URL the form POSTs to, not the URL of the form (you can find this in the "action" attribute of the HTML's form tag
            //string formParams = string.Format("fm-login-id={0}&fm-login-password={1}", "jonathangobato@hotmail.com", "crw21k9u");
            string formParams = string.Format("loginId={0}&password={1}", "lingsearcher@gmail.com", "lingse@rcher"); 
            string cookieHeader;
            HttpWebRequest req = (HttpWebRequest)WebRequest.Create(formUrl);
            req.ContentType = "application/x-www-form-urlencoded";
            req.Method = "POST";
            byte[] bytes = Encoding.ASCII.GetBytes(formParams);
            req.ContentLength = bytes.Length;
            using (Stream os = req.GetRequestStream())
            {
                os.Write(bytes, 0, bytes.Length);
            }

            HttpWebResponse resp = (HttpWebResponse)req.GetResponse();

            cookieHeader = resp.Headers["Set-cookie"];

            string url = "https://pt.aliexpress.com/wholesale?SearchText=";
            url = url + "iphone+x";
            //string url = "https://br.gearbest.com/xiaomi-mi-9-_gear/";

            HttpWebRequest request = (HttpWebRequest)WebRequest.Create(url);
            request.Headers.Add("Cookie", cookieHeader);

            HttpWebResponse response = (HttpWebResponse)request.GetResponse();

            string data = String.Empty;

            if (response.StatusCode == HttpStatusCode.OK)
            {
                Stream receiveStream = response.GetResponseStream();
                StreamReader readStream = null;

                if (response.CharacterSet == null)
                {
                    readStream = new StreamReader(receiveStream);
                }
                else
                {
                    readStream = new StreamReader(receiveStream, Encoding.GetEncoding(response.CharacterSet));
                }

                data = readStream.ReadToEnd();

                response.Close();
                readStream.Close();
            }

            var doc = new HtmlDocument();
            doc.LoadHtml(data);

            string priceRange = doc.DocumentNode.SelectSingleNode("(//span[contains(@class,'price price-m')]//span[contains(@itemprop,'price')])[1]").InnerText;

            //string priceRange = doc.DocumentNode.SelectSingleNode("(//div[contains(@class,'gbGoodsItem_outBox')]//p[contains(@itemprop,'gbGoodsItem_price js-currency js-asyncPrice')])[1]").InnerText;
            //string fullName = doc.DocumentNode.SelectSingleNode("(//span[contains(@class,'price price-m')]//span[contains(@itemprop,'price')])[1]").InnerText;

            Console.WriteLine("Produto: iPhone X");
            Console.WriteLine("Preço do produto: " + priceRange);
            Console.WriteLine("Digite qualquer tecla para continuar...");
            Console.ReadKey();
        }
    }
}
