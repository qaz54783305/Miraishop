import { Component } from '@angular/core';
import { AUTH_STORAGE_KEY } from '../interceptors/auth.interceptor';


@Component({
  selector: 'app-welcome',
  templateUrl: './welcome.component.html',
  styleUrls: ['./welcome.component.css']
})
export class WelcomeComponent {
  readonly products = Array.from({ length: 8 }, (_, index) => ({
    id: `demo-${index + 1}`, name: `商品名稱 ${index + 1}`, price: (index + 1) * 199
  }));
  cartItems: { id: string; name: string; price: number; quantity: number }[] = [];
  cartOpen = false;
  cartMessage = '';

  get cartCount(): number {
    return this.cartItems.reduce((count, item) => count + item.quantity, 0);
  }

  get cartTotal(): number {
    return this.cartItems.reduce((total, item) => total + item.price * item.quantity, 0);
  }

  addToCart(product: { id: string; name: string; price: number }): void {
    const item = this.cartItems.find(item => item.id === product.id);
    if (item) {
      if (item.quantity >= 99) {
        this.cartMessage = '每項商品最多可加入 99 件。';
        return;
      }
      item.quantity++;
    } else {
      this.cartItems.push({ ...product, quantity: 1 });
    }
    this.cartMessage = `已加入 ${product.name}，購物車共 ${this.cartCount} 件商品。`;
  }

  changeQuantity(id: string, delta: number): void {
    const item = this.cartItems.find(item => item.id === id);
    if (!item) return;
    item.quantity = Math.max(1, Math.min(99, item.quantity + delta));
  }

  removeItem(id: string): void {
    this.cartItems = this.cartItems.filter(item => item.id !== id);
  }

  openCart(dialog: HTMLDialogElement): void {
    this.cartOpen = true;
    dialog.showModal();
  }

  closeCart(dialog: HTMLDialogElement): void {
    dialog.close();
    this.cartOpen = false;
  }


  private get jwtPayload(): Record<string, string> {
    const raw = localStorage.getItem(AUTH_STORAGE_KEY);
    if (!raw) return {};
    try {
      const auth = JSON.parse(raw) as { token: string };
      // base64url → base64 → Uint8Array → UTF-8 string
      const base64 = auth.token.split('.')[1].replace(/-/g, '+').replace(/_/g, '/');
      const bytes = Uint8Array.from(atob(base64), c => c.charCodeAt(0));
      return JSON.parse(new TextDecoder('utf-8').decode(bytes));
    } catch { return {}; }
  }

  logout() {
    localStorage.removeItem(AUTH_STORAGE_KEY);
    window.location.href = '/login';
  }


  get memberName(): string  { return this.jwtPayload['name']  ?? ''; }
  get memberEmail(): string { return this.jwtPayload['email'] ?? ''; }

  get isAdmin(): boolean {
    const role = (this.jwtPayload as Record<string, unknown>)['role']
      ?? (this.jwtPayload as Record<string, unknown>)['http://schemas.microsoft.com/ws/2008/06/identity/claims/role'];
    return Array.isArray(role) ? role.includes('Admin') : role === 'Admin';
  }

}
