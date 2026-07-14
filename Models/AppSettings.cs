using SQLite;

namespace SwiftScan.Models
{
    [Table("AppSettings")]
    public class AppSettings
    {
        [PrimaryKey]
        public string Key { get; set; } = string.Empty;

        public string Value { get; set; } = string.Empty;
    }
}
