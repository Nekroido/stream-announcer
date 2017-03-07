using Newtonsoft.Json;
using System.Collections.Generic;

namespace Announcer.Models.Responses.Vk
{
    public class Error
    {
        [JsonProperty("error_code")]
        public int ErrorCode { get; set; }

        [JsonProperty("error_msg")]
        public string Message { get; set; }

        [JsonProperty("error_text")]
        public string Text { get; set; }

        [JsonProperty("request_params")]
        public List<RequestParameter> Parameters { get; set; }

        [JsonProperty("redirect_uri")]
        public string RedirectUri { get; set; }

        [JsonProperty("captcha_sid")]
        public string CaptchaSid { get; set; }

        [JsonProperty("captcha_img")]
        public string CaptchaImage { get; set; }
    }
}
