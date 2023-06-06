namespace IDM.Shared.Models.Account
{
    public class AccountModel
    {
        public string Email { get; set; }
        public string Password { get; set; }
        public bool IsPersistent { get; set; }
        public string ReferelUrl { get; set; }
    }
}
