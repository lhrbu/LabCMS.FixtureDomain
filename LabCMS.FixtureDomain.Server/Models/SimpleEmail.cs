
namespace LabCMS.FixtureDomain.Server.Models;
public record SimpleEmail(
    string FromAddress,
    IEnumerable<string> ToAddresses,
    string Subject,
    string HtmlBody);
