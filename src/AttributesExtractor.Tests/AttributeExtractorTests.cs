using System;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Reflection;
using System.Threading.Tasks;
using AttributesExtractor.Playground;
using AttributesExtractor.SourceGenerator;
using AttributesExtractor.Tests.Data;
using AttributesExtractor.Tests.Utils;
using Microsoft.CodeAnalysis;
using Xunit;

namespace AttributesExtractor.Tests
{
    public class AttributeExtractorTests
    {
        [Fact]
        public async Task CompiledWithoutErrors()
        {
            var project = TestProject.Project;
            var assembly = await project.CompileToRealAssembly();
        }

        [Fact]
        public async Task BasicExtractionFromClass()
        {
            var expectedEntries = new[]
            {
                new PropertyInfo<User, string>("FirstName", new[] { new AttributeData(typeof(RequiredAttribute)) }),
                new PropertyInfo<User, string>("LastName", new[] { new AttributeData(typeof(RequiredAttribute)) }),
            };

            var project = await TestProject.Project
                .ReplacePartOfDocumentAsync("Program.cs", "// place to replace 1",
                    @"var attributes = user.GetAttributes();");

            var entries = await project.ExecuteTest();

            Assert.True(expectedEntries.Select(TestExtensions.Stringify)
                .SequenceEqual(entries!.Select(TestExtensions.Stringify)));
        }

        [Fact]
        public async Task ExtractionFromClassWithArguments()
        {
            var expectedEntries = new[]
            {
                new PropertyInfo<User, string>("FirstName",
                    new[]
                    {
                        new AttributeData(typeof(RequiredAttribute)),
                        new AttributeData(typeof(DescriptionAttribute), "Some first name")
                    }),
                new PropertyInfo<User, string>("LastName", new[] { new AttributeData(typeof(RequiredAttribute)) }),
            };

            var project = await TestProject.Project
                .ReplacePartOfDocumentAsync(
                    (TestProject.Project.Name, "Program.cs", "// place to replace 1", @"var attributes = user.GetAttributes();"),
                    (TestProject.Core.Name, "User.cs", "// place to replace 2", @"[System.ComponentModel.Description(""Some first name"")]"));
            
            var entries = await project.ExecuteTest();

            Assert.True(expectedEntries.Select(TestExtensions.Stringify)
                .SequenceEqual(entries!.Select(TestExtensions.Stringify)));
        }
    }
}