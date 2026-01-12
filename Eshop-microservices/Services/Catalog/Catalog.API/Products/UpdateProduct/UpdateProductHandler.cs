namespace Catalog.API.Products.UpdateProduct
{
    public record UpdateProductCommand(Guid Id,string Name, List<string> Category, string Description, string ImageFile, decimal Price) : ICommand<UpdateProductResult>;

    public record UpdateProductResult(bool IsSuccess);

    public class UpdateProductCommandValidator : AbstractValidator<UpdateProductCommand>
    {
        public UpdateProductCommandValidator()
        {
            RuleFor(command => command.Id).NotEmpty().WithMessage("Product ID is required");

            RuleFor(command => command.Name)
                .NotEmpty().WithMessage("Name is required")
                .Length(2, 150).WithMessage("Name must be between 2 and 150 characters");

            RuleFor(command => command.Price)
                .GreaterThan(0).WithMessage("Price must be greater than 0");
        }
    }

    internal class UpdateProductCommandHandler(IDocumentSession session, ILogger<UpdateProductCommandHandler> logger): ICommandHandler<UpdateProductCommand, UpdateProductResult>
    {
        public async Task<UpdateProductResult> Handle(UpdateProductCommand requestedCommand, CancellationToken cancellationToken)
        {
            logger.LogInformation("UpdateProductHandler.handle called with {@Command}", requestedCommand);
            var product = await session.LoadAsync<Product>(requestedCommand.Id, cancellationToken);
            if (product == null)
            {
                throw new ProductNotFoundException();
            }
            product.Category = requestedCommand.Category;
            product.Price = requestedCommand.Price;
            product.Description = requestedCommand.Description;
            product.ImageFile = requestedCommand.ImageFile;
            product.Name = requestedCommand.Name;

            session.Update(product);
            await session.SaveChangesAsync(cancellationToken);

            return new UpdateProductResult(true);
        }
    }
}
