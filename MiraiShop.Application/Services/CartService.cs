using MiraiShop.Application.Interfaces;
using MiraiShop.Domain.Entities;
using MiraiShop.Domain.Exceptions;
using MiraiShop.Domain.Interfaces;

namespace MiraiShop.Application.Services;

public class CartService : ICartService
{
    private readonly IProductRepository _productRepository;
    private readonly ICartRepository _cartRepository;

    public CartService(IProductRepository productRepository, ICartRepository cartRepository)
    {
        _productRepository = productRepository;
        _cartRepository = cartRepository;
    }
    public Task<IReadOnlyList<Cart>> GetCartAsync(Guid memberId)
    {
        throw new NotImplementedException();
    }

    public Task<Cart?> GetItemAsync(Guid memberId, Guid productId)
    {
        return _cartRepository.GetItemAsync(memberId, productId);
    }
    // 加入購物車：先完成驗證，再新增或更新明細。
    public async Task AddAsync(Cart cart)
    {
        if (cart.Quantity <= 0)
        {
            throw new CartException("商品數量必須大於 0！");
        }

        Product? product = await _productRepository.GetByIdAsync(cart.ProductId);
        if (product is null)
        {
            throw new CartException("找不到商品！");
        }

        if (product.Stock <= 0)
        {
            throw new CartException("商品已完售！");
        }

        Cart? cartItem = await _cartRepository.GetItemAsync(cart.MemberId, cart.ProductId);

        // 先檢查累加後的數量，避免驗證失敗時已修改原本的明細。
        long totalQuantity = (long)(cartItem?.Quantity ?? 0) + cart.Quantity;
        if (totalQuantity > product.Stock)
        {
            throw new CartException("商品庫存不足！");
        }

        if (cartItem is not null)
        {
            // 已有商品：更新為累加後的數量。
            cartItem.Quantity = (int)totalQuantity;
            cartItem.UpdatedAt = DateTime.UtcNow;
            await _cartRepository.UpdateAsync(cartItem);
        }
        else
        {
            // 尚無商品：新增購物車明細。
            cart.UpdatedAt = DateTime.UtcNow;
            await _cartRepository.AddAsync(cart);
        }

        // 加入購物車不扣庫存；結帳時再重新確認庫存。
    }

    public Task UpdateAsync(Cart cart)
    {
        throw new NotImplementedException();
    }

    public Task DeleteAsync(Guid memberId, Guid productId)
    {
        throw new NotImplementedException();
    }
    
}