using CartingService.BLL.Interfaces;
using CartingService.DTOs;
using CartingService.Protos;
using Grpc.Core;
using Microsoft.Extensions.Logging;

namespace CartingService.GrpcServices;

public class CartServiceGrpc : CartService.CartServiceBase
{
    private readonly ICartBL _cartBL;
    private readonly ILogger<CartServiceGrpc> _logger;

    public CartServiceGrpc(ICartBL cartBL, ILogger<CartServiceGrpc> logger)
    {
        _cartBL = cartBL;
        _logger = logger;
    }

    public override async Task<CartResponse> GetCartItems(CartRequest request, ServerCallContext context)
    {
        _logger.LogInformation("Unary RPC: Getting cart for userId={UserId}", request.UserId);

        if (context.CancellationToken.IsCancellationRequested)
        {
            _logger.LogInformation("Unary RPC was cancelled.");
            throw new RpcException(new Status(StatusCode.Cancelled, "Client cancelled request"));
        }

        var cart = await _cartBL.GetCartAsync(request.UserId);
        var response = new CartResponse();

        if (cart?.Items != null)
        {
            response.Items.AddRange(cart.Items.Select(MapToProto));
        }

        return response;
    }

    public override async Task StreamCartItems(CartRequest request, IServerStreamWriter<CartItem> responseStream, ServerCallContext context)
    {
        _logger.LogInformation("Server Streaming RPC: Streaming cart items for userId={UserId}", request.UserId);

        var cart = await _cartBL.GetCartAsync(request.UserId);
        if (cart?.Items != null)
        {
            foreach (var item in cart.Items)
            {
                if (context.CancellationToken.IsCancellationRequested)
                {
                    _logger.LogInformation("Server streaming cancelled.");
                    break;
                }

                await responseStream.WriteAsync(MapToProto(item));
            }
        }
    }

    public override async Task<CartResponse> AddItemsToCart(IAsyncStreamReader<CartItem> requestStream, ServerCallContext context)
    {
        _logger.LogInformation("Client Streaming RPC: Adding items to cart.");

        string? userId = context.RequestHeaders.GetValue("user-id");
        if (string.IsNullOrWhiteSpace(userId))
            throw new RpcException(new Status(StatusCode.InvalidArgument, "Missing 'user-id' in metadata."));

        var items = new List<ItemDto>();

        await foreach (var grpcItem in requestStream.ReadAllAsync(context.CancellationToken))
        {
            items.Add(MapToDto(grpcItem));
        }

        foreach (var item in items)
        {
            await _cartBL.AddItemToCartAsync(userId, item);
        }

        var updatedCart = await _cartBL.GetCartAsync(userId);
        var response = new CartResponse();
        if (updatedCart?.Items != null)
            response.Items.AddRange(updatedCart.Items.Select(MapToProto));

        return response;
    }

    public override async Task ChatAddItems(IAsyncStreamReader<CartItem> requestStream, IServerStreamWriter<CartResponse> responseStream, ServerCallContext context)
    {
        _logger.LogInformation("Bidirectional RPC: Adding items and responding with updated cart.");

        string? userId = context.RequestHeaders.GetValue("user-id");
        if (string.IsNullOrWhiteSpace(userId))
            throw new RpcException(new Status(StatusCode.InvalidArgument, "Missing 'user-id' in metadata."));

        await foreach (var grpcItem in requestStream.ReadAllAsync(context.CancellationToken))
        {
            await _cartBL.AddItemToCartAsync(userId, MapToDto(grpcItem));

            var updatedCart = await _cartBL.GetCartAsync(userId);
            var response = new CartResponse();
            if (updatedCart?.Items != null)
                response.Items.AddRange(updatedCart.Items.Select(MapToProto));

            await responseStream.WriteAsync(response);
        }
    }

    // Mapping helpers

    private static CartItem MapToProto(ItemDto item) => new()
    {
        Id = item.Id.ToString(),
        Name = item.Name,
        Quantity = item.Quantity,
        Price = (double)item.Price
    };

    private static ItemDto MapToDto(CartItem item) => new()
    {
        Id = int.TryParse(item.Id, out var idVal) ? idVal : 0,
        Name = item.Name,
        Quantity = item.Quantity,
        Price = (decimal)item.Price
    };
}
