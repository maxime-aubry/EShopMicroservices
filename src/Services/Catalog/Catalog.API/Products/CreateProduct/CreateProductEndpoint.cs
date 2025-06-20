﻿namespace Catalog.API.Products.CreateProduct
{
    public record CreateProductRequest(string Name, List<string> Category, string Description, string ImageFile, decimal Price);

    public record CreateProductResponse(Guid Id);

    public class CreateProductEndpoint : ICarterModule
    {
        public void AddRoutes(IEndpointRouteBuilder app)
        {
            app.MapPost("/products",
                async (CreateProductRequest request, ISender sender) =>
                {
                    CreateProductCommand command = request.Adapt<CreateProductCommand>();
                    CreateProductResult result = await sender.Send(command);
                    CreateProductResponse response = result.Adapt<CreateProductResponse>();
                    return Results.Created($"/products/{response.Id}", response);
                }
            )
            .WithName("CreateProduct")
            .Produces<CreateProductResponse>(StatusCodes.Status201Created)
            .ProducesProblem(StatusCodes.Status400BadRequest)
            .WithSummary("Create Product")
            .WithDescription("Create Product");

            string t = default!;
            t.DoSomething(0);
        }
    }

    public static class StringMethods
    {
        public static void DoSomething(this string str, int nb)
        {

        }
    }
}
