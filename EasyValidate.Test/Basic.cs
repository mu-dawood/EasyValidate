
using EasyValidate.Test.Models;
using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp;

namespace EasyValidate.Test;

public class Basic
{
    [Fact]
    public void TestMethod1()
    {

        var user = new Dto { Name = "", Age = 10, Email = "bad-email" };
        var errors = user.Validate();
    }

}
