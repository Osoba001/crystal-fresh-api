using Microsoft.Extensions.DependencyInjection;
using System.Text.Json.Serialization;

namespace Share.MediatKO
{
    public interface IMediator
    {
        Task<ActionResponse> SendAsync<TRequest>(TRequest request, CancellationToken cancellationToken = default)
            where TRequest : Request;
    }

    public abstract class Request
    {
        [JsonIgnore]
        public virtual Guid UserIdentifier { get; set; }
        public virtual ActionResponse Validate() => new();
    }
    public interface IRequestHandler<TRequest> where TRequest : Request
    {
        Task<ActionResponse> HandleAsync(TRequest request, CancellationToken cancellationToken = default);
    }


    public class Mediator(IServiceProvider serviceProvider) : IMediator
    {
        private readonly IServiceProvider _serviceProvider = serviceProvider;

        public Task<ActionResponse> SendAsync<TRequest>(TRequest request, CancellationToken cancellationToken = default) where TRequest : Request
        {
            var handler = _serviceProvider.GetRequiredService<IRequestHandler<TRequest>>();
            return handler.HandleAsync(request, cancellationToken);
        }
    }

}

