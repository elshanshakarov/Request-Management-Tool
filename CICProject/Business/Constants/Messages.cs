using Entities.Concrete;

namespace Business.Constants
{
    public static class Messages
    {
        // Public oldugundan PascalCase yaziriq. Private olsaydi 'productAdded' kimi yazardiq
        public const string UserNotFound = "User not found!";
        public const string AuthorizationDenied = "No permission!";
        public const string PasswordNotFound = "Password is incorrect!";
        public const string SuccessfulLogin = "Successfully logged in";
        public const string AccessTokenCreated = "Access token created";
        public const string UserAlreadyExists = "User already exists";
        public const string UserRegistered = "User registered successfully";
        public const string UsernameNotFound = "Username is incorrect";
        public const string AccessTokenError = "Token could not be created";
        public const string CategoryNotFound = "This category not found from database";
        public const string IncorrectCategory = "You don't create this category";
        
        public const string RequestNotFound = "Request not found";

        public const string StatusSuccess = "Status changed successfully.";
        public const string StatusError = "The status wasn't changed.";

        public const string RequestCreated = "Yeni sorğu yaratdı";
        public const string LockMessage = "Sorğunu öz üzərinə götürdü";
        public const string RejectMessage = "Sorğunu imtina etdi";
        public const string CloseMessage = "Sorğunu qapatdı";
        public const string WaitMessage = "Sorğunu gözləməyə saldı";
        public const string ConfirmMessage = "Sorğunu təsdiqlədi";
        public const string PasswordIncorrect= "The password was repeated incorrectly.";
    }
}
