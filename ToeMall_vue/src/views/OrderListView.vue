<!--
  订单列表页面
  功能：
  1. 显示用户的所有订单
  2. 支持按订单状态过滤（未支付、已支付、配送中、已取消）
  3. 显示订单详情（商品列表、总价、状态）
  4. 支持订单支付（积分支付）

  状态管理：
  - orders: 订单列表数据
  - statusFilter: 当前选中的状态过滤器

  API 接口：
  - GET /api/orders: 获取订单列表
  - GET /api/orders/{id}: 获取订单详情
  - POST /api/orders/{id}/pay: 支付订单
-->

<template>
  <div class="order-list">
    <h1>我的订单</h1>
    <div class="order-filters">
      <!-- 订单状态过滤器将在这里实现 -->
    </div>
    <div class="orders">
      <!-- 订单列表将在这里渲染 -->
    </div>
  </div>
</template>

<script>
export default {
  name: 'OrderListView',
  data () {
    return {
      orders: [],
      statusFilter: 'all'
    }
  },
  created () {
    // 这里将实现获取订单列表的逻辑
  }
}
</script>

<style scoped>
.order-list {
  padding: 20px;
}
</style>
