namespace WebApp.Helper;

public static class ClientExtension
{
    public async static Task<T> GetOperationAsync<T>(this HttpClient client, T result, string request)
    {
        HttpResponseMessage response = await client.GetAsync(request);
        if(response.IsSuccessStatusCode)
            result = await response.Content.ReadAsAsync<T>();

        return result;
    }

    public async static Task<T> PostOperationAsync<T,V>(this HttpClient client, T result, V data, string request)
    {
        HttpResponseMessage response = await client.PostAsJsonAsync(request, data);
        if (response.IsSuccessStatusCode)
            result = await response.Content.ReadAsAsync<T>();

        return result;
    }

    public async static Task PutOperationAsync<T> (this HttpClient client, T data, string request)
    {
        HttpResponseMessage response = await client.PutAsJsonAsync(request, data);
        response.EnsureSuccessStatusCode();
    }
}
