using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Threading.Tasks;

namespace IOS_MCL
{
	public class URLHandler
    {
        public static HttpResponseMessage result;
        public static async Task<string> PostURL(List<KeyValuePair<string, string>> list)
        {
            HttpClient client = new HttpClient();
            try
            {
                KeyValuePair<string, string>[] arr = list.ToArray();
                FormUrlEncodedContent content = new FormUrlEncodedContent(arr);
                result = await client.PostAsync("http://69.207.170.153:8237/restsrv/RestController.php?", content);
                string output = result.Content.ReadAsStringAsync().Result;
                Console.WriteLine(output);
                return output;
            }
            catch (Exception e)
            {
                Console.WriteLine("Error: " + e.Message);
            }
            return null;
        }
    }
}
