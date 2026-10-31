
namespace HttpServerDemo
{
    internal class Token
    {
        public string Value { get; set; } = "";
        public int UserId { get; set; }
        public DateTime ExpiresAt { get; set; }
    }
}
