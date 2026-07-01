using System.IO;
using System.Net.Http;
using System.Net.Http.Json;
using System.Text.Json;

namespace SalesPro.Wpf.Services;

public sealed class ApiClientService
{
    private readonly HttpClient _httpClient;
    private readonly JsonSerializerOptions _jsonOptions = new(JsonSerializerDefaults.Web);

    public ApiClientService()
    {
        _httpClient = new HttpClient
        {
            BaseAddress = new Uri(ReadBaseUrl())
        };
    }

    public async Task<T> GetAsync<T>(string path, CancellationToken cancellationToken = default)
    {
        var response = await _httpClient.GetAsync(path, cancellationToken);
        await EnsureSuccessAsync(response, cancellationToken);
        return await response.Content.ReadFromJsonAsync<T>(_jsonOptions, cancellationToken)
               ?? throw new InvalidOperationException("La respuesta del API vino vacía.");
    }

    public async Task<TResponse> PostAsync<TRequest, TResponse>(string path, TRequest request, CancellationToken cancellationToken = default)
    {
        var response = await _httpClient.PostAsJsonAsync(path, request, _jsonOptions, cancellationToken);
        await EnsureSuccessAsync(response, cancellationToken);
        return await response.Content.ReadFromJsonAsync<TResponse>(_jsonOptions, cancellationToken)
               ?? throw new InvalidOperationException("La respuesta del API vino vacía.");
    }

    public async Task<TResponse> PutAsync<TRequest, TResponse>(string path, TRequest request, CancellationToken cancellationToken = default)
    {
        var response = await _httpClient.PutAsJsonAsync(path, request, _jsonOptions, cancellationToken);
        await EnsureSuccessAsync(response, cancellationToken);
        return await response.Content.ReadFromJsonAsync<TResponse>(_jsonOptions, cancellationToken)
               ?? throw new InvalidOperationException("La respuesta del API vino vacía.");
    }

    public async Task DeleteAsync(string path, CancellationToken cancellationToken = default)
    {
        var response = await _httpClient.DeleteAsync(path, cancellationToken);
        await EnsureSuccessAsync(response, cancellationToken);
    }

    private static string ReadBaseUrl()
    {
        var path = Path.Combine(AppContext.BaseDirectory, "appsettings.json");
        if (!File.Exists(path))
        {
            return "http://localhost:5294/";
        }

        using var document = JsonDocument.Parse(File.ReadAllText(path));
        return document.RootElement.TryGetProperty("ApiBaseUrl", out var property)
            ? property.GetString() ?? "http://localhost:5294/"
            : "http://localhost:5294/";
    }

    private static async Task EnsureSuccessAsync(HttpResponseMessage response, CancellationToken cancellationToken)
    {
        if (response.IsSuccessStatusCode)
        {
            return;
        }

        var body = await response.Content.ReadAsStringAsync(cancellationToken);
        var message = TryReadApiMessage(body) ?? $"Error HTTP {(int)response.StatusCode}: {response.ReasonPhrase}";
        throw new InvalidOperationException(message);
    }

    private static string? TryReadApiMessage(string body)
    {
        if (string.IsNullOrWhiteSpace(body))
        {
            return null;
        }

        try
        {
            using var document = JsonDocument.Parse(body);
            return document.RootElement.TryGetProperty("message", out var message)
                ? message.GetString()
                : body;
        }
        catch (JsonException)
        {
            return body;
        }
    }
}
