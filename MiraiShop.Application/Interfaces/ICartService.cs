using MiraiShop.Domain.Entities;

namespace MiraiShop.Application.Interfaces;
    
public interface ICartService
{
    Task<IReadOnlyList<Cart>> GetCartAsync(Guid memberId);

    Task<Cart?> GetItemAsync(Guid memberId, Guid productId);

    Task AddAsync(Cart cart);

    Task UpdateAsync(Cart cart);

    Task DeleteAsync(Guid memberId, Guid productId);
}