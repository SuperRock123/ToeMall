<!--
  购物车页面
  功能：
  1. 显示购物车中的商品列表
  2. 支持修改商品数量
  3. 支持删除商品
  4. 计算总价
  5. 提交订单

  状态管理：
  - cartItems: 购物车商品列表（Vuex）
  - total: 计算购物车总价

  API 接口：
  - POST /api/orders: 创建新订单
  - PUT /api/cart: 更新购物车
  - DELETE /api/cart/{id}: 删除购物车商品
-->

<template>
  <div class="cart">
    <h1>购物车</h1>
    <div class="cart-items">
      <!-- 购物车商品列表将在这里渲染 -->
    </div>
    <div class="cart-summary">
      <div class="total">总计: ¥{{ total }}</div>
      <button @click="checkout" class="checkout-btn">结算</button>
    </div>
  </div>
</template>

<script>
export default {
  name: 'CartView',
  computed: {
    total () {
      return 0 // 这里将实现计算总价的逻辑
    }
  },
  methods: {
    checkout () {
      // 这里将实现结算逻辑
    }
  }
}
</script>

<style scoped>
.cart {
  padding: 20px;
}
.cart-summary {
  margin-top: 20px;
  text-align: right;
}
.checkout-btn {
  margin-top: 10px;
}
</style>
