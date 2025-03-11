<template>
  <div class="admin-panel">
    <el-container>
      <el-aside width="200px">
        <el-menu
          default-active="dashboard"
          class="el-menu-vertical-demo"
          @select="navigateTo"
        >
          <el-menu-item index="dashboard">统计</el-menu-item>
          <el-menu-item index="product-list">商品管理</el-menu-item>
          <el-menu-item index="user-list">用户管理</el-menu-item>
          <el-menu-item index="order-list">订单管理</el-menu-item>
        </el-menu>
      </el-aside>
      <el-main>
        <component :is="currentView"></component>
      </el-main>
    </el-container>
  </div>
</template>

<script>
import Dashboard from '@/components/Dashboard.vue'
import ProductList from '@/components/AdminProductList.vue'
import UserList from '@/components/UserList.vue'
import OrderList from '@/components/OrderList.vue'

export default {
  data () {
    return {
      currentView: 'Dashboard'
    }
  },
  components: {
    Dashboard,
    ProductList,
    UserList,
    OrderList
  },
  methods: {
    navigateTo (view) {
      this.currentView = view
    }
  }
}
</script>

<style scoped>
.admin-panel {
  height: 100vh;
}
</style>
