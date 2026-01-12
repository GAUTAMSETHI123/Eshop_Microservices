using JasperFx.CodeGeneration.Frames;
using Microsoft.Extensions.Logging;

namespace Catalog.API.Products.GetProducts
{
    public record GetProductQuery() : IQuery<GetProductsResult>;
    public record GetProductsResult(IEnumerable<Product> Products);
    internal class GetProductsQueryHandler(IDocumentSession session, ILogger<GetProductsQueryHandler> Logger) : IQueryHandler<GetProductQuery, GetProductsResult>
    {
        public async Task<GetProductsResult> Handle(GetProductQuery requestedQuery, CancellationToken cancellationToken)
        {
            Logger.LogInformation("GetProductsQueryHandler.Handle called with {@Query}", requestedQuery);
            var products = await session.Query<Product>().ToListAsync(cancellationToken);
            return new GetProductsResult(products);
        }
    }
}
