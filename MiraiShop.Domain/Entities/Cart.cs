namespace MiraiShop.Domain.Entities;

/// <summary>
/// 購物車明細：每筆代表一位會員加入的一項商品。
/// PK / FK 與數量約束由 Infrastructure 層的 MiraiShopDbContext 設定。
/// </summary>
public class Cart
{
    // 複合主鍵 PK（MemberId + ProductId）；外鍵 FK → Member.Id。
    public Guid MemberId { get; set; }

    // 複合主鍵 PK（MemberId + ProductId）；外鍵 FK → Product.Id。
    public Guid ProductId { get; set; }

    // 數量須大於 0；同一會員再次加入同商品時累加數量。
    public int Quantity { get; set; } = 1;

    // 每次新增或修改明細時，更新為目前 UTC 時間。
    public DateTime UpdatedAt { get; set; } = DateTime.UtcNow;
}
