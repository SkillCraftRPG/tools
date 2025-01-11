using GraphQL;
using MediatR;

namespace SkillCraft.Tools.GraphQL;

internal static class ResolveFieldContextExtensions
{
  public static async Task<T> ExecuteAsync<T>(this IResolveFieldContext context, IRequest<T> request)
  {
    IServiceProvider serviceProvider = context.RequestServices ?? throw new ArgumentException($"The {nameof(context.RequestServices)} are required.", nameof(context));
    IMediator mediator = serviceProvider.GetRequiredService<IMediator>();
    return await mediator.Send(request, context.CancellationToken);
  }
}
