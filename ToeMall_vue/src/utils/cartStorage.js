const CART_KEY = 'cart_items'

/**
 * 保存购物车数据到 localStorage
 * @param {Array} items - 购物车商品列表
 */
export function saveCart (items) {
  localStorage.setItem(CART_KEY, JSON.stringify(items))
}

/**
 * 从 localStorage 加载购物车数据
 * @returns {Array} - 购物车商品列表
 */
export function loadCart () {
  const items = localStorage.getItem(CART_KEY)
  return items ? JSON.parse(items) : []
}

/**
 * 清空购物车数据
 */
export function clearCart () {
  localStorage.removeItem(CART_KEY)
}
