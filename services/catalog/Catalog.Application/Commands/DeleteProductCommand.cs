using MediatR;

namespace Catalog.Application.Commands
{
    public class DeleteProductCommand : IRequest<bool>
    {
        public string Id { get; set; } = string.Empty;

        public DeleteProductCommand(string id)
        {
            Id = id;
        }
    }
}