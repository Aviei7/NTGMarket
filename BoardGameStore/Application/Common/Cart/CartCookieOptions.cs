
using Microsoft.AspNetCore.Http;


namespace Application.Common.Cart
{
    public class CartCookieOptions
    {
        public  string TokenName { get; set; }  = "guest_cart";
        public string Path { get; set; } = "/";
        public bool HttpOnly { get; set; } = true;
        public bool Secure { get; set; } = true;
        public SameSiteMode SameSite { get; set; } = SameSiteMode.None;
        public int ExpireDays { get; set; } = 30;
    }
}
