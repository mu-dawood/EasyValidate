namespace ConsoleTest.Video;

public class ComplexUser
{
    public string Name { get; set; } = string.Empty;
    public User? UserDetails { get; set; }
    public List<User> Friends { get; set; } = [];
    public int Age { get; set; }
    public string Address { get; set; } = string.Empty;
    public string PhoneNumber { get; set; } = string.Empty;
    public DateTime DateOfBirth { get; set; }
    public bool IsActive { get; set; }
    public string Email { get; set; } = string.Empty;
    public List<string> Tags { get; set; } = [];
    public double Rating { get; set; }
    public Guid Id { get; set; } = Guid.NewGuid();

    // Manual validation: demonstrates complexity and verbosity
    public bool Validate()
    {
        bool isValid = true;

        // Validate Name
        if (string.IsNullOrWhiteSpace(Name))
        {
            Console.WriteLine("Name is required.");
            isValid = false;
        }

        // Validate UserDetails
        if (UserDetails == null)
        {
            Console.WriteLine("UserDetails is required.");
            isValid = false;
        }
        else if (!UserDetails.IsValid())
        {
            Console.WriteLine("UserDetails is invalid.");
            isValid = false;
        }

        // Validate Friends
        if (Friends == null || Friends.Count == 0)
        {
            Console.WriteLine("At least one friend is required.");
            isValid = false;
        }
        else
        {
            int idx = 0;
            foreach (var friend in Friends)
            {
                if (!friend.IsValid())
                {
                    Console.WriteLine($"Friend at index {idx} is invalid.");
                    isValid = false;
                }
                idx++;
            }
        }

        // Validate Age
        if (Age < 0 || Age > 120)
        {
            Console.WriteLine("Age must be between 0 and 120.");
            isValid = false;
        }

        // Validate Address
        if (string.IsNullOrWhiteSpace(Address))
        {
            Console.WriteLine("Address is required.");
            isValid = false;
        }

        // Validate PhoneNumber
        if (string.IsNullOrWhiteSpace(PhoneNumber) || PhoneNumber.Length < 10)
        {
            Console.WriteLine("PhoneNumber is invalid.");
            isValid = false;
        }

        // Validate DateOfBirth
        if (DateOfBirth > DateTime.Now)
        {
            Console.WriteLine("DateOfBirth cannot be in the future.");
            isValid = false;
        }

        // Validate Email
        if (string.IsNullOrWhiteSpace(Email) || !Email.Contains("@"))
        {
            Console.WriteLine("Email is invalid.");
            isValid = false;
        }

        // Validate Tags
        if (Tags == null || Tags.Count == 0)
        {
            Console.WriteLine("At least one tag is required.");
            isValid = false;
        }

        // Validate Rating
        if (Rating < 0.0 || Rating > 5.0)
        {
            Console.WriteLine("Rating must be between 0.0 and 5.0.");
            isValid = false;
        }

        // Validate Id
        if (Id == Guid.Empty)
        {
            Console.WriteLine("Id is required.");
            isValid = false;
        }

        // Validate IsActive (example: must be true for valid user)
        if (!IsActive)
        {
            Console.WriteLine("User must be active.");
            isValid = false;
        }

        return isValid;
    }
}