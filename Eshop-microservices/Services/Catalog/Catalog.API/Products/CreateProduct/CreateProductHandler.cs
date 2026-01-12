namespace Catalog.API.Products.CreateProduct
{
    public record CreateProductCommand(string Name , List<string> Category, string Description, string ImageFile,decimal Price) : ICommand<CreateProductResult>;

    public record CreateProductResult(Guid Id);

    public class CreateProductCommandValidator : AbstractValidator<CreateProductCommand>
    {
        public CreateProductCommandValidator()
        {
            RuleFor(x => x.Name).NotEmpty().WithMessage("Name is required");
            RuleFor(x => x.Category).NotEmpty().WithMessage("Category is required");
            RuleFor(x => x.ImageFile).NotEmpty().WithMessage("ImageFile is required");
            RuleFor(x => x.Price).NotEmpty().WithMessage("Price must be greater than 0");
        }
    }

    internal class CreateProductHandler(IDocumentSession session,ILogger<CreateProductHandler> logger) : ICommandHandler<CreateProductCommand, CreateProductResult>
    {
        public async Task<CreateProductResult> Handle(CreateProductCommand requestedCommand, CancellationToken cancellationToken)
        {
            //create product entity from command object
            // save to database
            // return createProductResult result
            var product = new Product
            {
                Name = requestedCommand.Name,
                Category = requestedCommand.Category,
                Description = requestedCommand.Description,
                ImageFile = requestedCommand.ImageFile,
                Price = requestedCommand.Price
            };

            //save to database
            session.Store(product);
            await session.SaveChangesAsync(cancellationToken);

            //return result
            return new CreateProductResult(product.Id);
        }
    }
}
