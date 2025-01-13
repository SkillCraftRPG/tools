using Logitar.Portal.Contracts.Tokens;
using Logitar.Portal.Contracts.Users;

namespace SkillCraft.Tools.Core.Identity.Models;

public record IdentityModel
{
  public string? Subject { get; set; }
  public EmailPayload? Email { get; set; }
  public List<ClaimModel> Claims { get; set; } = [];
}
