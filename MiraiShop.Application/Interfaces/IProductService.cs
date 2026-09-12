using MiraiShop.Domain.Entities;

namespace MiraiShop.Application.Interfaces;

public interface IProductService
{
    Task<Product?> GetByIdAsync(Guid id);
}
