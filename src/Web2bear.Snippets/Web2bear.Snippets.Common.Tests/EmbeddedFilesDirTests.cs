using System;
using FluentAssertions;
using Web2bear.Snippets.Common.Tests.Data;
using Xunit;

namespace Web2bear.Snippets.Common.Tests
{
    public class EmbeddedFilesDirTests
    {
        private readonly EmbeddedFilesDir _embeddedFilesDir;

        public EmbeddedFilesDirTests()
        {
            _embeddedFilesDir = EmbeddedFilesDir.Create<DataDir>();
        }

        [Fact]
        public void EmbeddedTextFileShouldBeReaded()
        {
            var sampleText = _embeddedFilesDir.ReadTextFile("hello.txt");
            sampleText.Should().Be("Hello");
        }

        [Fact]
        public void EmbeddedJsonObjectShouldBeReaded()
        {
            var sample = _embeddedFilesDir.ReadJsonObject<Sample>("sample.json");
            sample.Id.Should().Be(100);
            sample.Name.Should().Be("Alex");
        }


    }
}
