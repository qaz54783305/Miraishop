using Microsoft.EntityFrameworkCore;
using MiraiShop.Domain.Entities;
using MiraiShop.Domain.Interfaces;
using MiraiShop.Infrastructure.Persistence;

namespace MiraiShop.Infrastructure.Repositories;

public class CartRepository : ICartRepository
{
    private readonly MiraiShopDbContext _context;

    public CartRepository(MiraiShopDbContext context)
    {
        _context = context;
    }

    public Task<Cart?> GetItemAsync(Guid memberId, Guid productId)
    {
        return _context.Carts.FirstOrDefaultAsync(cart =>
            cart.MemberId == memberId && cart.ProductId == productId);
    }

    public async Task<IReadOnlyList<Cart>> GetCartAsync(Guid memberId)
    {
        return await _context.Carts
            .Where(cart => cart.MemberId == memberId)
            .ToListAsync();
    }

    public async Task AddAsync(Cart cart)
    {
        _context.Carts.Add(cart);
        await _context.SaveChangesAsync();
    }

    public async Task UpdateAsync(Cart cart)
    {
        _context.Carts.Update(cart);
        await _context.SaveChangesAsync();
    }

    public async Task DeleteAsync(Guid memberId, Guid productId)
    {
        var cart = await GetItemAsync(memberId, productId);
        if (cart is null) return;

        _context.Carts.Remove(cart);
        await _context.SaveChangesAsync();
    }
}
