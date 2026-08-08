using Microsoft.Extensions.DependencyInjection;
using MyRecipeBook.Domain.Extensions;
using MyRecipeBook.Infrastructure.DataAccess;
using System.Net.Http.Headers;
using System.Net.Http.Json;

namespace WebApi.Tests;

public abstract class BaseIntegrationTest : IClassFixture<MyRecipeBookApplicationFactory>, IDisposable
{
    internal readonly MyRecipeBookDbContext DbContext;

    private readonly HttpClient _httpClient;
    private readonly IServiceScope _scope;

    public BaseIntegrationTest(MyRecipeBookApplicationFactory factory)
    {
        _httpClient = factory.CreateClient();

        _scope = factory.Services.CreateScope();

        DbContext = _scope.ServiceProvider.GetRequiredService<MyRecipeBookDbContext>();
    }

    protected async Task<HttpResponseMessage> Post(string requestUri, object request, string accessToken = "", string culture = "en-US")
    {
        ChangeRequestCulture(culture);
        AuthorizeRequest(accessToken);

        return await _httpClient.PostAsJsonAsync(requestUri, request);
    }

    protected async Task<HttpResponseMessage> PostFormData(string requestUri, object request, string accessToken = "", Stream? file = null, string fileFieldName = "file", string culture = "en-US")
    {
        ChangeRequestCulture(culture);
        AuthorizeRequest(accessToken);

        var content = BuildFormData(request, file, fileFieldName);

        return await _httpClient.PostAsync(requestUri, content);
    }

    protected async Task<HttpResponseMessage> Put(string requestUri, object request, string accessToken, string culture = "en-US")
    {
        ChangeRequestCulture(culture);
        AuthorizeRequest(accessToken);

        return await _httpClient.PutAsJsonAsync(requestUri, request);
    }

    protected async Task<HttpResponseMessage> PutFormData(string requestUri, Stream file, string accessToken, string fileFieldName = "file", string culture = "en-US")
    {
        ChangeRequestCulture(culture);
        AuthorizeRequest(accessToken);

        var content = new MultipartFormDataContent();
        content.Add(new StreamContent(file), fileFieldName, fileName: fileFieldName);

        return await _httpClient.PutAsync(requestUri, content);
    }

    protected async Task<HttpResponseMessage> Get(string requestUri, string accessToken, string culture = "en-US")
    {
        ChangeRequestCulture(culture);
        AuthorizeRequest(accessToken);

        return await _httpClient.GetAsync(requestUri);
    }

    protected async Task<HttpResponseMessage> Delete(string requestUri, string accessToken, string culture = "en-US")
    {
        ChangeRequestCulture(culture);
        AuthorizeRequest(accessToken);

        return await _httpClient.DeleteAsync(requestUri);
    }

    private void ChangeRequestCulture(string culture)
    {
        _httpClient.DefaultRequestHeaders.AcceptLanguage.Clear();
        _httpClient.DefaultRequestHeaders.AcceptLanguage.ParseAdd(culture);
    }

    private void AuthorizeRequest(string accessToken)
    {
        if (accessToken.IsNotEmpty())
            _httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", accessToken);
    }

    private static MultipartFormDataContent BuildFormData(object request, Stream? file, string fileFieldName)
    {
        var content = new MultipartFormDataContent();

        if (file is not null)
            content.Add(new StreamContent(file), fileFieldName, fileName: fileFieldName);

        foreach (var property in request.GetType().GetProperties())
        {
            var value = property.GetValue(request);
            if (value is null)
                continue;

            if (value is System.Collections.IList list)
            {
                var itemType = list.GetType().GetGenericArguments().Single();
                var index = 0;

                foreach (var item in list)
                {
                    if (itemType.IsClass && itemType != typeof(string))
                    {
                        foreach (var itemProperty in item.GetType().GetProperties())
                        {
                            var itemValue = itemProperty.GetValue(item);
                            content.Add(new StringContent(itemValue!.ToString()!), $"{property.Name}[{index}].{itemProperty.Name}");
                        }

                        index++;
                    }
                    else
                        content.Add(new StringContent(item.ToString()!), property.Name);
                }
            }
            else
                content.Add(new StringContent(value.ToString()!), property.Name);
        }

        return content;
    }

    public void Dispose()
    {
        _scope?.Dispose();
        DbContext?.Dispose();
    }
}
