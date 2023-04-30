namespace Business.Constants
{
    public static class Messages
    {
        // Public oldugundan PascalCase yaziriq. Private olsaydi 'productAdded' kimi yazardiq
       
        public static readonly string AuthorizationDenied = "No permission!";
        public static readonly string PasswordNotFound = "Password is incorrect!";
        public static readonly string SuccessfulLogin = "Successfully logged in";
        public static readonly string AccessTokenCreated = "Access token created";
        public static readonly string UserAlreadyExists = "User already exists";
        public static readonly string UserRegistered = "User registered successfully";
        public static readonly string UsernameNotFound = "Username is incorrect";
        public static readonly string AccessTokenError= "Token could not be created";
        public static readonly string CategoryNotFound="This category not found from database";
        public static readonly string IncorrectCategory="You don't create this category";
        public static readonly string RequestCreated="Yeni sorğu yaratdı";
    }
}
