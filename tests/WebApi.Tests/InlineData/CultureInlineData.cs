using System.Collections;

namespace WebApi.Tests.InlineData;

public class CultureInlineData : IEnumerable<object[]>
{
    public IEnumerator<object[]> GetEnumerator()
    {
        yield return new object[] { "en-US" };
        yield return new object[] { "pt-BR" };
        yield return new object[] { "es" };
    }

    IEnumerator IEnumerable.GetEnumerator() => GetEnumerator();
}
