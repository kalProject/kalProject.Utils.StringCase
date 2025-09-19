using System.Globalization;
using FluentAssertions;
using Xunit;
using StringCaseConvert = kalProject.Utils.Convert;

namespace kalProject.Utils.StringCase.Tests
{
    public class ConvertTests
    {
        [Theory]
        [InlineData("HelloWorld", "hello_world")]
        [InlineData("HTTPServer2", "http_server_2")]
        [InlineData("hello world", "hello_world")]
        public void ToSnakeCase_Works(string input, string expected)
        {
            var inv = CultureInfo.InvariantCulture;
            StringCaseConvert.ToSnakeCase(input, inv).Should().Be(expected);
        }

        [Theory]
        [InlineData("hello_world", "hello-world")]
        [InlineData("HelloWorld", "hello-world")]
        public void ToKebabCase_Works(string input, string expected)
        {
            var inv = CultureInfo.InvariantCulture;
            StringCaseConvert.ToKebabCase(input, inv).Should().Be(expected);
        }

        [Fact]
        public void Detect_Returns_Expected()
        {
            StringCaseConvert.DetectCaseType("hello-world").ToString().Should().Be("KebabCase");
        }
    }
}
