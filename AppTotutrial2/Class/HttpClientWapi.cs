﻿using AppTotutrial2;
using AppTotutrial2.Class;
using Newtonsoft.Json;
using System;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace FTA_Demo.Class
{
    public class HttpClientWapi : IDisposable
    {
        /// <summary>
        /// Inner declaration
        /// </summary>
        CancellationTokenSource cancelSource;
        CancellationToken cancelToken;

        /// <summary>
        /// Public declararion
        /// </summary>
        public string lastErrorDesc;
        public bool isSuccessStatusCode;

        readonly string APP_JSON = "application/json";

        /// <summary>
        /// The constructor
        /// </summary>
        public HttpClientWapi()
        {
            lastErrorDesc = string.Empty;
        }

        /// <summary>
        /// Dispose code
        /// </summary>
        public void Dispose()
        {
            cancelSource = null;
            GC.Collect();
        }

        /// <summary>
        /// Handler normal opr as sales, clock in and other
        /// </summary>
        /// <param name="cioRequest"></param>
        /// <returns></returns>
        public async Task<string> RequestSvrAsync(string server_addr, Cio cioRequest)
        {
            string repliedContent = string.Empty;
            try
            {
                cancelSource = new CancellationTokenSource();
                cancelToken = cancelSource.Token;
                
                HttpClient httpClient = App.client; // reference to the single http client

                // added 20200311T1124
                // add in the bearer token for request resource
                //httpClient.DefaultRequestHeaders.Authorization =
                //            new AuthenticationHeaderValue("Bearer", cioRequest.token);


                string json = JsonConvert.SerializeObject(cioRequest);
                StringContent stringContent = new StringContent(json, Encoding.UTF8, APP_JSON);
                Uri uri = new Uri(server_addr);

                cancelToken.ThrowIfCancellationRequested(); // <-- to detect any cancellation by the user                
                HttpResponseMessage response = await httpClient.PostAsync(uri, stringContent, cancelToken);
                isSuccessStatusCode = response.IsSuccessStatusCode;

                cancelToken.ThrowIfCancellationRequested();
                repliedContent = await response.Content.ReadAsStringAsync();
            }
            catch (Exception excep)
            {
                lastErrorDesc = excep.ToString();
            }
            return repliedContent;
        }

        ///// <summary>
        ///// Handler dynamic query path of the server
        ///// like super admin add, update company, users and group
        ///// </summary>
        ///// <param name="cioRequest"></param>
        ///// <param name="query"></param>
        ///// <returns></returns>
        //public async Task<string> RequestSvrAsync_mgt(Cio cioRequest, string query)
        //{
        //    string repliedContent = string.Empty;
        //    try
        //    {
        //        cancelSource = new CancellationTokenSource();
        //        cancelToken = cancelSource.Token;
        //        ShareUltilities shareUltilities = App.su;
        //        HttpClient httpClient = App.client; // reference to the single http client

        //        // added 20200311T1124
        //        // add in the bearer token for request resource
        //        httpClient.DefaultRequestHeaders.Authorization =
        //                    new AuthenticationHeaderValue("Bearer", App.bearerToken.access_token);

        //        string json = JsonConvert.SerializeObject(cioRequest);
        //        StringContent stringContent = new StringContent(json, Encoding.UTF8, shareUltilities.APP_JSON);
        //        Uri uri = new Uri(shareUltilities.WEBAPI_HOST + query);

        //        cancelToken.ThrowIfCancellationRequested(); // <-- to detect any cancellation by the user                
        //        HttpResponseMessage response = await httpClient.PostAsync(uri, stringContent, cancelToken);
        //        isSuccessStatusCode = response.IsSuccessStatusCode;

        //        cancelToken.ThrowIfCancellationRequested();
        //        repliedContent = await response.Content.ReadAsStringAsync();
        //    }
        //    catch (Exception excep)
        //    {
        //        lastErrorDesc = excep.ToString();
        //    }
        //    return repliedContent;
        //}

        /// <summary>
        /// Cancel the request
        /// </summary>
        public void CancelRequest()
        {
            if (!cancelSource.IsCancellationRequested)
                cancelSource.Cancel();
        }

    }
}
