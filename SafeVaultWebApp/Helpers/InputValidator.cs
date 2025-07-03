using System.Text.RegularExpressions;

namespace SafeVaultWebApp.Helpers
{
    public static class ValidationHelpers
    {
        public static (bool isValid, string sanitizedUsername, string sanitizedEmail, string error) ValidateUserInput(string username, string email)
        {
            if (string.IsNullOrWhiteSpace(username) || string.IsNullOrWhiteSpace(email))
            {
                return (false, null, null, "Username and email are required.");
            }

            if (Regex.Match(username, @"[<>,/!@#$%^&*()~{}\[\],;:\']").Success || Regex.Match(email, @"[<>/!#$%^&*()~{}\[\]],;:\'").Success)
            {
                return (false, null, null, "Username or email contain invalid charsacters.");
            }

            // Remove potentially malicious characters (basic sanitization)
            string sanitizedUsername = Regex.Replace(username, @"[^a-zA-Z0-9_\-]", "");
            string sanitizedEmail = Regex.Replace(email, @"[^a-zA-Z0-9@._\-]", "");

            // Email format validation
            if (!Regex.IsMatch(sanitizedEmail, @"^[^@\s]+@[^@\s]+\.[^@\s]+$"))
            {
                return (false, sanitizedUsername, sanitizedEmail, "Invalid email format.");
            }

            // Optionally, check username length and allowed characters
            if (sanitizedUsername.Length < 3 || sanitizedUsername.Length > 20)
            {
                return (false, sanitizedUsername, sanitizedEmail, "Username must be between 3 and 20 characters.");
            }

            return (true, sanitizedUsername, sanitizedEmail, null);
        }
    }
}
