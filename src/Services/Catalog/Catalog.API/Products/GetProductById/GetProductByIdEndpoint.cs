namespace Catalog.API.Products.GetProductById
{
    public record GetProductByIdResponse(Product Product);

    public class GetProductByIdEndpoint : ICarterModule
    {
        public void AddRoutes(IEndpointRouteBuilder app)
        {
            app.MapGet("/products/{id}", async (Guid id, ISender sender) =>
            {
                GetProductByIdResult result = await sender.Send(new GetProductByIdQuery(id));
                GetProductByIdResponse response = result.Adapt<GetProductByIdResponse>();
                return Results.Ok(response);
            })
            .WithName("GetPropductById")
            .Produces<GetProductByIdResponse>(StatusCodes.Status200OK)
            .ProducesProblem(StatusCodes.Status400BadRequest)
            .WithSummary("Get Product By Id")
            .WithDescription("get Product By Id");
        }
    }
}
