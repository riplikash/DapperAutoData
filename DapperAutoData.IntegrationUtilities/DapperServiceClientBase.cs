using System.Net.Http.Headers;
using System.Net.Mime;
using System.Text.Json;
using System.Text;

namespace DapperAutoData.IntegrationUtilities;

public class DapperServiceClientBase
{
    protected readonly TokenProvider _tokenProvider;
    protected readonly JsonSerializerOptions _jsonSerializerOptions;
    public HttpClient _client;

    public DapperServiceClientBase(HttpClient httpclient, JsonSerializerOptions jsonSerializerOptions, TokenProvider tokenProvider)
    {
        _client = httpclient;
        _jsonSerializerOptions = jsonSerializerOptions;
        _tokenProvider = tokenProvider;
    }

    #region Delete

    public async Task<TResponse> Delete<TResponse>(string endpoint, Dictionary<string, string> headers, CancellationToken token)
    {
        SetHeaderRequest(headers);

        var response = await _client.DeleteAsync(endpoint, token).ConfigureAwait(false);
        var responseContent = await response.Content.ReadAsStringAsync().ConfigureAwait(false);
        var model = JsonSerializer.Deserialize<TResponse>(responseContent, _jsonSerializerOptions);

        return model;
    }

    public TResponse DeleteSync<TResponse>(string endpoint, Dictionary<string, string> headers, CancellationToken token)
    {
        SetHeaderRequest(headers);

        var webRequest = new HttpRequestMessage(HttpMethod.Delete, endpoint);

        var response = _client.Send(webRequest);

        using var reader = new StreamReader(response.Content.ReadAsStream());
        string responseContent = reader.ReadToEnd();
        var model = JsonSerializer.Deserialize<TResponse>(responseContent, _jsonSerializerOptions);

        return model;
    }

    #endregion

    #region Get

    /// <summary>
    /// Utility for making async Get requests with url and query parameters
    /// </summary>
    public async Task<TResponse> Get<TResponse>(string endpoint, Dictionary<string, string> headers, CancellationToken token)
    {

        SetHeaderRequest(headers);
        HttpResponseMessage response = await _client.GetAsync(endpoint, token).ConfigureAwait(false);
        var responseContent = await response.Content.ReadAsStringAsync().ConfigureAwait(false);

        var model = JsonSerializer.Deserialize<TResponse>(responseContent, _jsonSerializerOptions);

        return model;
    }

    public TResponse GetSync<TResponse>(string endpoint, Dictionary<string, string> headers)
    {

        SetHeaderRequest(headers);
        var webRequest = new HttpRequestMessage(HttpMethod.Get, endpoint);

        var response = _client.Send(webRequest);

        using var reader = new StreamReader(response.Content.ReadAsStream());
        string responseContent = reader.ReadToEnd();
        var model = JsonSerializer.Deserialize<TResponse>(responseContent, _jsonSerializerOptions);

        return model;
    }

    #endregion

    #region Post    
    public async Task<TResponse> Post<TRequest, TResponse>(string endpoint, TRequest request, Dictionary<string, string> headers, CancellationToken token)
    {

        var requestContent = new StringContent(JsonSerializer.Serialize(request), Encoding.UTF8, MediaTypeNames.Application.Json);
        SetHeaderRequest(headers);


        var response = await _client.PostAsync(endpoint, requestContent, token).ConfigureAwait(false);

        var responseContent = await response.Content.ReadAsStringAsync().ConfigureAwait(false);
        var model = JsonSerializer.Deserialize<TResponse>(responseContent, _jsonSerializerOptions);

        return model;
    }

    /// <summary>
    /// Utility for making sync Post requests with body, url, and query parameters
    /// </summary>
    public TResponse PostSync<TRequest, TResponse>(string endpoint, TRequest request, Dictionary<string, string> headers)
    {
        SetHeaderRequest(headers);

        var webRequest = new HttpRequestMessage(HttpMethod.Post, endpoint)
        {
            Content = new StringContent(JsonSerializer.Serialize(request), Encoding.UTF8, MediaTypeNames.Application.Json)
        };

        var response = _client.Send(webRequest);

        using var reader = new StreamReader(response.Content.ReadAsStream());
        string responseContent = reader.ReadToEnd();
        var model = JsonSerializer.Deserialize<TResponse>(responseContent, _jsonSerializerOptions);

        return model;
    }

    #endregion

    #region Put

    public async Task<TResponse> Put<TRequest, TResponse>(string endpoint, TRequest request, Dictionary<string, string> headers, CancellationToken token)
    {

        SetHeaderRequest(headers);

        var requestContent = new StringContent(JsonSerializer.Serialize(request), Encoding.UTF8, MediaTypeNames.Application.Json);

        var response = await _client.PutAsync(endpoint, requestContent, token).ConfigureAwait(false);
        var responseContent = await response.Content.ReadAsStringAsync().ConfigureAwait(false);
        var model = JsonSerializer.Deserialize<TResponse>(responseContent, _jsonSerializerOptions);

        return model;
    }

    /// <summary>
    /// Utility for making sync Post requests with body, url, and query parameters
    /// </summary>
    public TResponse PutSync<TRequest, TResponse>(string endpoint, TRequest request, Dictionary<string, string> headers)
    {
        SetHeaderRequest(headers);

        var webRequest = new HttpRequestMessage(HttpMethod.Put, endpoint)
        {
            Content = new StringContent(JsonSerializer.Serialize(request), Encoding.UTF8, MediaTypeNames.Application.Json)
        };

        var response = _client.Send(webRequest);

        using var reader = new StreamReader(response.Content.ReadAsStream());
        string responseContent = reader.ReadToEnd();
        var model = JsonSerializer.Deserialize<TResponse>(responseContent, _jsonSerializerOptions);

        return model;
    }


    #endregion

    #region Patch

    public async Task<TResponse> Patch<TRequest, TResponse>(string endpoint, TRequest request, Dictionary<string, string> headers, CancellationToken token)
    {

        SetHeaderRequest(headers);

        var requestContent = new StringContent(JsonSerializer.Serialize(request), Encoding.UTF8, MediaTypeNames.Application.Json);

        var response = await _client.PatchAsync(endpoint, requestContent, token).ConfigureAwait(false);
        var responseContent = await response.Content.ReadAsStringAsync().ConfigureAwait(false);
        var model = JsonSerializer.Deserialize<TResponse>(responseContent, _jsonSerializerOptions);

        return model;
    }

    /// <summary>
    /// Utility for making sync Post requests with body, url, and query parameters
    /// </summary>
    public TResponse PatchSync<TRequest, TResponse>(string endpoint, TRequest request, Dictionary<string, string> headers)
    {
        SetHeaderRequest(headers);

        var webRequest = new HttpRequestMessage(HttpMethod.Patch, endpoint)
        {
            Content = new StringContent(JsonSerializer.Serialize(request), Encoding.UTF8, MediaTypeNames.Application.Json)
        };

        var response = _client.Send(webRequest);

        using var reader = new StreamReader(response.Content.ReadAsStream());
        string responseContent = reader.ReadToEnd();
        var model = JsonSerializer.Deserialize<TResponse>(responseContent, _jsonSerializerOptions);

        return model;
    }


    #endregion

    public void SetHeaderRequest(Dictionary<string, string> headers)
    {

        _client.DefaultRequestHeaders.Clear();

        if (_client.DefaultRequestHeaders.Authorization == null && _tokenProvider.AccessToken != null)
            _client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", _tokenProvider.AccessToken);

        if (headers != null && headers.Any())
        {
            foreach (var item in headers)
            {
                _client.DefaultRequestHeaders.Add(item.Key, item.Value);
            }
        }

    }
}
