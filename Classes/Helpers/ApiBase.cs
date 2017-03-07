using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;

namespace Announcer.Classes.Helpers
{
    public abstract class ApiBase
    {
        public virtual HttpClient GetClient()
        {
            return new HttpClient();
        }

        public virtual NameValueCollection GetParameters(NameValueCollection parameters)
        {
            return parameters ?? new NameValueCollection();
        }

        public virtual async Task<T> GetRequest<T>(string baseUrl, NameValueCollection parameters)
        {
            var response = await GetRequest(baseUrl, parameters);

            try
            {
                var result = JsonConvert.DeserializeObject<T>(response);
                return result.Equals(default(T)) ? default(T) : result;
            }
            catch
            {
                return default(T);
            }
        }

        public virtual async Task<string> GetRequest(string baseUrl, NameValueCollection parameters)
        {
            parameters = GetParameters(parameters);

            var url = new UriBuilder(baseUrl)
            {
                Query = BuildQuery(parameters)
            };
            return await GetRequest(url.ToString());
        }

        protected async Task<string> GetRequest(string url)
        {
            try
            {
                using (var c = GetClient())
                {
                    var response = await c.GetStringAsync(url);

                    return response;
                }
            }
            catch (Exception e)
            {
                // Log and continue
                Logger.Log($"{this.GetType().Name}: {e.Message} ({url}).", Logger.Severity.error);
            }
            finally
            {
                if (Config.Load().Debug)
                {
                    Logger.Log($"{this.GetType().Name}: ({url}).", Logger.Severity.debug);
                }
            }

            return "";
        }

        public virtual async Task<T> PostRequest<T>(string baseUrl, Dictionary<string, object> parameters, NameValueCollection files)
        {
            var response = await PostRequest(baseUrl, parameters, files);

            try
            {
                var result = JsonConvert.DeserializeObject<T>(response);
                return result.Equals(default(T)) ? default(T) : result;
            }
            catch
            {
                return default(T);
            }
        }

        public virtual async Task<string> PostRequest(string url, Dictionary<string, object> parameters, NameValueCollection files)
        {
            var payload = new MultipartFormDataContent();

            addParametersToPayload(ref payload, parameters);

            foreach (string parameterName in files)
            {
                if (File.Exists(files[parameterName]))
                {
                    payload.Add(new ByteArrayContent(File.ReadAllBytes(files[parameterName])), String.Format("\"{0}\"", parameterName), String.Format("\"{0}\"", Path.GetFileName(files[parameterName])));
                }
            }

            return await PostRequest(url, payload);
        }

        private void addParametersToPayload(ref MultipartFormDataContent payload, Dictionary<string, object> parameters, string parentParameter = null)
        {
            foreach (var parameter in parameters)
            {
                if (parameter.Value.GetType() == typeof(Dictionary<string, object>))
                    addParametersToPayload(ref payload, (Dictionary<string, object>)parameter.Value);
                else
                    payload.Add(new StringContent(parameter.Value.ToString(), Encoding.UTF8), String.Format("\"{0}\"", parameter.Key.ToString()));
            }
        }

        protected async Task<string> PostRequest(string url, MultipartFormDataContent payload)
        {
            try
            {
                using (var c = GetClient())
                {
                    var response = await c.PostAsync(url, payload);

                    response.EnsureSuccessStatusCode();

                    var content = await response.Content.ReadAsByteArrayAsync();

                    return Encoding.ASCII.GetString(content);
                }
            }
            catch (Exception e)
            {
                // Log and continue
                Logger.Log($"{this.GetType().Name}: {e.Message} ({url}).", Logger.Severity.error);
            }
            finally
            {
                if (Config.Load().Debug)
                {
                    Logger.Log($"{this.GetType().Name}: ({url}).", Logger.Severity.debug);
                }
            }

            return null;
        }

        protected async Task<string> JsonRequest(string url, StringContent payload)
        {
            try
            {
                using (var c = GetClient())
                {
                    var response = await c.PostAsync(url, payload);

                    response.EnsureSuccessStatusCode();

                    var content = await response.Content.ReadAsByteArrayAsync();

                    return Encoding.ASCII.GetString(content);
                }
            }
            catch (Exception e)
            {
                // Log and continue
                Logger.Log($"{this.GetType().Name}: {e.Message} ({url}).", Logger.Severity.error);
            }
            finally
            {
                if (Config.Load().Debug)
                {
                    Logger.Log($"{this.GetType().Name}: ({url}).", Logger.Severity.debug);
                }
            }

            return null;
        }

        protected static string BuildQuery(NameValueCollection parameters)
        {
            return string.Join("&", parameters.AllKeys.Select(key => string.Join("&", parameters.GetValues(key).Select(val => string.Format("{0}={1}", WebUtility.UrlEncode(key.ToString()), WebUtility.UrlEncode(val?.ToString()))))));
        }
    }
}
