using HKSDEImporter.Core.Models.Domain;
using HKSDEImporter.Core.Validation;

namespace HKSDEImporter.Tests;

public sealed class ValidatorTests
{
    [Fact]
    public void CategoryValidator_ReturnsInvalidForZeroId()
    {
        var validator = new CategoryValidator();

        var result = validator.Validate(new Category(0, "Bad", true, null));

        Assert.False(result.IsValid);
    }

    [Fact]
    public void GroupValidator_ReturnsInvalidForMissingCategoryReference()
    {
        var validator = new GroupValidator();

        var result = validator.Validate(new Group(5, 0, "Group", true));

        Assert.False(result.IsValid);
    }

    [Fact]
    public void TypeValidator_ReturnsValidForNormalType()
    {
        var validator = new TypeValidator();

        var result = validator.Validate(new TypeItem(10, 20, "Type", "Desc", true, 1, null, null, null, null));

        Assert.True(result.IsValid);
    }
}
