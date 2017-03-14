using System;
using System.Collections.Generic;
using System.Net;
using System.Threading;
using System.Threading.Tasks;

namespace Common.Helpers
{
    public static class WebClientHelper
    {
        public static async Task<TResponse> PostDataAsync<TParam, TResponse>(string url, TParam parameter, int? moveTimeout = null)
        {
            var webClient = CreateWebClient();
            var response = webClient.UploadStringTaskAsync(new Uri(url), JsonHelper.Serialize(parameter));

            var awaitedTasks = new List<Task>();
            awaitedTasks.Add(response);

            if(moveTimeout.HasValue)
            {
                var cancelationTokenSource = new CancellationTokenSource();
                awaitedTasks.Add(DelayHelper.DelayAsync(moveTimeout.Value, cancelationTokenSource.Token));
                cancelationTokenSource.CancelAfter(moveTimeout.Value);
            }

            await Task.WhenAny(awaitedTasks.ToArray());

            if (!response.IsCompleted)
                webClient.CancelAsync();

            if (response.IsCompleted)
                return JsonHelper.Deserialize<TResponse>(response.Result);
            else throw new InvalidOperationException("Post data timeout");
        }

        public static async Task<TResponse> GetDataAsync<TResponse>(string url)
        {
            var webClient = CreateWebClient();
            var downloadedString = await webClient.DownloadStringTaskAsync(url);
            return JsonHelper.Deserialize<TResponse>(downloadedString);
        }

        private static WebClient CreateWebClient()
        {
            var webClient = new WebClient();
            webClient.Headers.Add("Accept", "application/json");
            webClient.Headers.Add("Content-Type", "application/json");
            return webClient;
        }
    }
}