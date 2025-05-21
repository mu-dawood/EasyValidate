// See https://aka.ms/new-console-template for more information


using EasyValidate.TestApp;

var user = new Dto { Name = "", Age = 10, Email = "bad-email" };
var errors = user.Validate();

// foreach (var error in errors)
// {
    // Console.WriteLine($"Property: {error}");
    // foreach (var message in error.ErrorMessages)
    // {
    //     Console.WriteLine($"  Error: {message}");
    // }
// }