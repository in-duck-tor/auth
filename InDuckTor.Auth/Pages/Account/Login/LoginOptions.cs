namespace InDuckTor.Auth.Pages.Account.Login
{
    public static class LoginOptions
    {
        public static readonly bool AllowLocalLogin = true;
        public static readonly bool AllowRememberLogin = true;
        public static readonly TimeSpan RememberMeLoginDuration = TimeSpan.FromDays(30);
        public static readonly string InvalidCredentialsErrorMessage = "Неверный логин или пароль";
        public static readonly string InactiveUserErrorMessage = "Недействительная учетная запись";
    }
}
