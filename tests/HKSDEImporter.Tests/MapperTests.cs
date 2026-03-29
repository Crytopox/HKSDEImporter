using HKSDEImporter.Core.Mapping;
using HKSDEImporter.Core.Models.Raw;

namespace HKSDEImporter.Tests;

public sealed class MapperTests
{
    [Fact]
    public void CategoryMapper_MapsEnglishName()
    {
        var mapper = new CategoryMapper();
        var raw = new RawCategory
        {
            Key = 42,
            IconId = 9,
            Name = new RawLocalizedText { En = "Ships" },
            Published = true
        };

        var category = mapper.Map(raw);

        Assert.Equal(42, category.CategoryId);
        Assert.Equal("Ships", category.Name);
        Assert.Equal(9, category.IconId);
        Assert.True(category.Published);
    }

    [Fact]
    public void TypeMapper_MapsDescriptionEnglishOnly()
    {
        var mapper = new TypeMapper();
        var raw = new RawType
        {
            Key = 100,
            GroupId = 200,
            Description = new RawLocalizedText { En = "Desc" },
            Name = new RawLocalizedText { En = "Type Name" },
            PortionSize = 1,
            Published = true
        };

        var type = mapper.Map(raw);

        Assert.Equal(100, type.TypeId);
        Assert.Equal(200, type.GroupId);
        Assert.Equal("Type Name", type.Name);
        Assert.Equal("Desc", type.Description);
    }
}
