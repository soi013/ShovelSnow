using FluentAssertions;
using JPLab2.Infrastructure;
using NUnit.Framework;
using System.Linq;

namespace JPLab2.Tests
{
    public class StringExtension_Test
    {
        [Test]
        public void ConcatenateString_Success()
        {
            var concatedText = Enumerable.Range(1, 5)
                            .ConcatenateString(",");

            concatedText.Should().Be("1,2,3,4,5");
        }
    }
}
