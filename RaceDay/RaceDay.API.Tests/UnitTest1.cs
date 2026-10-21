using Xunit;

namespace RaceDay.API.Tests;

public class BasicTests
{
    [Theory]
    [InlineData("Participant", true)]
    [InlineData("Organiser", true)]
    [InlineData("Admin", false)]
    [InlineData("", false)]
    public void RoleNames_AreValidated(
        string role, bool expected)
    {
        var valid = role == "Participant" || role == "Organiser";

        Assert.Equal(expected, valid);
    }

    [Theory]
    [InlineData("runner@example.com", true)]
    [InlineData("not-an-email", false)]
    public void EmailFormat_IsValidated(
        string email, bool expected)
    {
        var valid = new System.ComponentModel.DataAnnotations
            .EmailAddressAttribute().IsValid(email);

        Assert.Equal(expected, valid);
    }
}