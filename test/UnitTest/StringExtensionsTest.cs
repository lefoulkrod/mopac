using System.Runtime.InteropServices.ComTypes;
using Mopac.Library.Extensions;
using Xunit;
using FluentAssertions;

namespace Mopac.Library.UnitTest
{
    public class StringExtensionsTest
    {
        [Fact]
        public void ConcatOrBegin_should_concatinate_two_strings()
        {
            "hello".ConcatOrBegin("+", "world").Should().Be("hello+world");
        }

        [Theory]
        [InlineData(null, "hello", "hello")]
        [InlineData("", "hello", "hello")]
        public void ConcatOrBegin_should_return_the_specified_string_if_the_original_string_is_null_or_empty(string original, string concat, string expected)
        {
            original.ConcatOrBegin("+", concat).Should().Be(expected);
        }
    }
}
