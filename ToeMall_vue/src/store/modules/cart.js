import request from '@/utils/request'
import { saveCart, loadCart, clearCart } from '@/utils/cartStorage'

const state = {
  items: loadCart(), // 从 localStorage 加载购物车数据
  checkoutStatus: null
}

const getters = {
  cartItems: state => state.items,
  cartTotalPrice: state => {
    return state.items.reduce((total, item) => {
      return total + item.price * item.quantity
    }, 0)
  }
}

const actions = {
  /**
   * 添加商品到购物车
   * @param {Object} context - Vuex 上下文对象
   * @param {Object} product - 商品对象
   */
  async addProductToCart ({ state, commit }, product) {
    const cartItem = state.items.find(item => item.productId === product.productId)
    if (!cartItem) {
      commit('pushProductToCart', { productId: product.productId, price: product.price, name: product.name, quantity: product.quantity, picture: product.picture })
    } else {
      commit('incrementItemQuantity', { cartItem, quantity: product.quantity })
    }
    saveCart(state.items) // 保存购物车数据到 localStorage
  },
  /**
   * 从购物车移除商品
   * @param {Object} context - Vuex 上下文对象
   * @param {number} productId - 商品 ID
   */
  async removeProductFromCart ({ commit, state }, productId) {
    commit('removeProductFromCart', productId)
    saveCart(state.items) // 保存购物车数据到 localStorage
  },
  /**
   * 结账
   * @param {Object} context - Vuex 上下文对象
   */
  async checkout ({ state, commit }) {
    try {
      await request.post('/api/Orders/create', {
        items: state.items
      })
      commit('emptyCart')
      commit('setCheckoutStatus', 'success')
      clearCart() // 清空 localStorage 中的购物车数据
    } catch (error) {
      commit('setCheckoutStatus', 'fail')
    }
  }
}

const mutations = {
  /**
   * 将商品添加到购物车
   * @param {Object} state - Vuex 状态对象
   * @param {Object} payload - 商品信息
   */
  pushProductToCart (state, { productId, price, name, quantity, picture }) {
    state.items.push({
      productId,
      price,
      name,
      quantity,
      picture
    })
  },
  /**
   * 增加购物车中商品的数量
   * @param {Object} state - Vuex 状态对象
   * @param {Object} payload - 购物车商品对象和数量
   */
  incrementItemQuantity (state, { cartItem, quantity }) {
    cartItem.quantity += quantity
  },
  /**
   * 从购物车中移除商品
   * @param {Object} state - Vuex 状态对象
   * @param {number} productId - 商品 ID
   */
  removeProductFromCart (state, productId) {
    state.items = state.items.filter(item => item.productId !== productId)
  },
  /**
   * 清空购物车
   * @param {Object} state - Vuex 状态对象
   */
  emptyCart (state) {
    state.items = []
  },
  /**
   * 设置结账状态
   * @param {Object} state - Vuex 状态对象
   * @param {string} status - 结账状态
   */
  setCheckoutStatus (state, status) {
    state.checkoutStatus = status
  }
}

export default {
  namespaced: true,
  state,
  getters,
  actions,
  mutations
}
