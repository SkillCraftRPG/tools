using FluentValidation;

namespace SkillCraft.Tools.Models.Account;

internal class GetTokenValidator : AbstractValidator<GetTokenPayload>
{
  public GetTokenValidator()
  {
    RuleFor(x => x).Must(x => (x.RefreshToken == null && x.Credentials != null) || (x.RefreshToken != null && x.Credentials == null))
      .WithErrorCode("GetTokenValidator")
      .WithMessage(x => $"Exactly one of the following must be provided: {nameof(x.RefreshToken)}, {nameof(x.Credentials)}.");
  }
}
