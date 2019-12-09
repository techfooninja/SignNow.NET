using Newtonsoft.Json;
using SignNow.Net.Exceptions;
using SignNow.Net.Interfaces;
using SignNow.Net.Internal.Helpers;
using SignNow.Net.Internal.Model;
using SignNow.Net.Model;
using System;
using System.Net;
using System.Linq;
using System.Net.Http;
using System.Threading;
using System.Threading.Tasks;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.IO;

namespace SignNow.Net.Internal.Service
{
    class SignNowClient : ISignNowClient
    {
        private HttpClient HttpClient { get; }

        /// <summary>
        /// client_name>/version (OS_type OS_release; platform; arch) runtime/version
        /// </summary>
        private static string sdkUserAgentString;

        private static string SdkUserAgentString
        {
            get { return sdkUserAgentString; }
            set
            {
                if (sdkUserAgentString == null)
                    sdkUserAgentString = BuildUserAgentString();
            }
        }

        /// <summary>
        /// Initialize a new instance of SignNow Client
        /// </summary>
        /// <param name="httpClient">
        /// If <c>null</c>, an HTTP client will be created with default parameters.
        /// </param>
        public SignNowClient(HttpClient httpClient = null)
        {
#if NET45
            // With .NET Framework 4.5, it's necessary to manually enable support for TLS 1.2.
            ServicePointManager.SecurityProtocol |= SecurityProtocolType.Tls12;
#endif
            this.HttpClient = httpClient ?? new HttpClient();
        }

        /// <inheritdoc />
        public async Task<TResponse> RequestAsync<TResponse>(RequestOptions requestOptions, CancellationToken cancellationToken = default)
        {
            return await RequestAsync(
                requestOptions,
                new HttpContentToObjectAdapter<TResponse>(new HttpContentToStringAdapter()),
                HttpCompletionOption.ResponseContentRead,
                cancellationToken
                ).ConfigureAwait(false);
        }

        /// <inheritdoc />
        public async Task<Stream> RequestAsync(RequestOptions requestOptions, CancellationToken cancellationToken = default)
        {
            return await RequestAsync(
                requestOptions,
                new HttpContentToStreamAdapter(),
                HttpCompletionOption.ResponseContentRead,
                cancellationToken
                ).ConfigureAwait(false);
        }

        /// <summary>
        /// Process Request with request options and returns result object.
        /// </summary>
        /// <typeparam name="TResponse"></typeparam>
        /// <param name="requestOptions"></param>
        /// <param name="adapter"></param>
        /// <param name="completionOption"></param>
        /// <param name="cancellationToken"></param>
        /// <returns></returns>
        public async Task<TResponse> RequestAsync<TResponse>(RequestOptions requestOptions, IHttpContentAdapter<TResponse> adapter = default, HttpCompletionOption completionOption = default, CancellationToken cancellationToken = default)
        {
            using (var request = CreateHttpRequest(requestOptions))
            {
                var response = await HttpClient.SendAsync(request, completionOption, cancellationToken).ConfigureAwait(false);

                await ProcessErrorResponse(response).ConfigureAwait(false);

                return await adapter.Adapt(response.Content).ConfigureAwait(false);
            }
        }

        /// <summary>
        /// Process Error Response to prepare SignNow Exception
        /// </summary>
        /// <param name="response"></param>
        /// <exception cref="SignNowException">SignNow Exception.</exception>
        /// <returns></returns>
        [SuppressMessage("Microsoft.Performance", "CA1825:Unnecessary zero-length array allocation", Justification = "Solution Array.Empty<>() works only for .NetStandard2.0, no significant memory or performance improvement")]
        private async static Task ProcessErrorResponse(HttpResponseMessage response)
        {
            if (!response.IsSuccessStatusCode)
            {
                var context = await response.Content.ReadAsStringAsync().ConfigureAwait(false);
                var apiError = context;
                var snException = new SignNowException[0];
                try
                {
                    var converter = new HttpContentToObjectAdapter<ErrorResponse>(new HttpContentToStringAdapter());
                    var errorResponse = await converter.Adapt(response.Content).ConfigureAwait(false);

                    if (errorResponse.Errors?.Count > 1)
                    {
                        snException = errorResponse.Errors.Select(e => new SignNowException(e.Message)).ToArray();
                    }

                    apiError = errorResponse.GetErrorMessage();
                }
#pragma warning disable CA1031 // Do not catch general exception types
                catch (JsonSerializationException)
                {
                }
#pragma warning restore CA1031 // Do not catch general exception types

                throw new SignNowException(apiError, snException)
                {
                    RawHeaders = response.Headers.ToDictionary(a => a.Key, a => a.Value),
                    RawResponse = response.Content.ReadAsStringAsync().Result,
                    HttpStatusCode = response.StatusCode
                };
            }
        }

        /// <summary>
        /// Creates Http Request from <see cref="SignNow.Net.Model.RequestOptions"/> class.
        /// </summary>
        /// <param name="requestOptions"></param>
        /// <exception cref="ArgumentException">The <paramref name="requestOptions">RequestUrl</paramref> argument is a null.</exception>
        /// <returns>Request Message <see cref="System.Net.Http.HttpRequestMessage"/></returns>
        private static HttpRequestMessage CreateHttpRequest(RequestOptions requestOptions)
        {
            if (requestOptions.RequestUrl == null)
            {
                throw new ArgumentException(ExceptionMessages.RequestUrlIsNull);
            }

            var requestMessage = new HttpRequestMessage(requestOptions.HttpMethod, requestOptions.RequestUrl.ToString());

            requestMessage.Headers.Add("User-Agent", sdkUserAgentString);

            if (requestOptions.Token != null)
            {
                requestMessage.Headers.Add("Authorization", requestOptions.Token.GetAuthorizationHeaderValue());
            }

            requestMessage.Content = requestOptions.Content?.GetHttpContent();

            return requestMessage;
        }

        /// <summary>
        /// Creates pre-formatted string with SDK, OS, Runtime information
        /// </summary>
        /// <returns></returns>
        private static string BuildUserAgentString()
        {
            return $"{UserAgentSdkHeaders.ClientName()}/{UserAgentSdkHeaders.SdkVersion()} ({UserAgentSdkHeaders.OsDetails()}) {UserAgentSdkHeaders.RuntimeInfo()}";
        }
    }
}
