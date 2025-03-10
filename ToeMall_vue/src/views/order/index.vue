<!-- filepath: d:\jobApplications\ToeMallDemo\toemall-dev\src\views\order\index.vue -->
<template>
  <div class="order">
    <h1>订单列表</h1>
    <el-table :data="orders" style="width: 100%">
      <el-table-column prop="orderId" label="订单 ID" width="100"></el-table-column>
      <el-table-column prop="productName" label="商品名称" width="180"></el-table-column>
      <el-table-column prop="price" label="价格" width="100">
        <template slot-scope="scope">
          <span>{{ scope.row.price | currency }}</span>
        </template>
      </el-table-column>
      <el-table-column prop="quantity" label="数量" width="100"></el-table-column>
      <el-table-column prop="totalPrice" label="总价" width="100">
        <template slot-scope="scope">
          <span>{{ scope.row.totalPrice | currency }}</span>
        </template>
      </el-table-column>
      <el-table-column prop="orderStatus" label="订单状态" width="120"></el-table-column>
      <el-table-column prop="createdAt" label="创建时间" width="180">
        <template slot-scope="scope">
          <span>{{ new Date(scope.row.createdAt).toLocaleString() }}</span>
        </template>
      </el-table-column>
      <el-table-column label="操作" width="200">
        <template slot-scope="scope">
          <el-button type="primary" @click="viewOrderDetail(scope.row.orderId)">查看详情</el-button>
          <el-button type="success" @click="payOrder(scope.row.orderId)" v-if="scope.row.orderStatus === 'Unpaid'">支付</el-button>
          <el-button type="danger" @click="cancelOrder(scope.row.orderId)" v-if="scope.row.orderStatus === 'Unpaid' || scope.row.orderStatus === 'Paid'">取消</el-button>
        </template>
      </el-table-column>
    </el-table>
    <el-pagination
      background
      layout="prev, pager, next"
      :total="pagination.totalCount"
      :page-size="pagination.pageSize"
      :current-page.sync="pagination.currentPage"
      @current-change="fetchOrders">
    </el-pagination>

    <!-- 订单详情对话框 -->
    <el-dialog title="订单详情" :visible.sync="dialogVisible" width="50%">
      <div v-if="selectedOrder">
        <p><strong>订单 ID:</strong> {{ selectedOrder.orderId }}</p>
        <p><strong>商品名称:</strong> {{ selectedOrder.productName }}</p>
        <p><strong>价格:</strong> {{ selectedOrder.price | currency }}</p>
        <p><strong>数量:</strong> {{ selectedOrder.quantity }}</p>
        <p><strong>总价:</strong> {{ selectedOrder.totalPrice | currency }}</p>
        <p><strong>订单状态:</strong> {{ selectedOrder.orderStatus }}</p>
        <p><strong>创建时间:</strong> {{ new Date(selectedOrder.createdAt).toLocaleString() }}</p>
        <p><strong>用户名:</strong> {{ selectedOrder.user.username }}</p>
        <img :src="selectedOrder.product.picture" alt="商品图片" class="product-image">
      </div>
      <span slot="footer" class="dialog-footer">
        <el-button @click="dialogVisible = false">关闭</el-button>
      </span>
    </el-dialog>
  </div>
</template>

<script>
import { getOrders, getOrderDetail, payOrder, cancelOrder as cancelOrderApi } from '@/api/orders'

export default {
  name: 'OrderView',
  data () {
    return {
      orders: [],
      pagination: {
        currentPage: 1,
        pageSize: 10,
        totalCount: 0
      },
      loading: false,
      dialogVisible: false,
      selectedOrder: null
    }
  },
  methods: {
    async fetchOrders (page = 1) {
      this.loading = true
      try {
        const response = await getOrders(page, this.pagination.pageSize)
        this.orders = response.data.orders
        this.pagination.totalCount = response.data.pagination.totalCount
      } catch (error) {
        console.error('获取订单列表失败', error)
      } finally {
        this.loading = false
      }
    },
    async viewOrderDetail (orderId) {
      try {
        const response = await getOrderDetail(orderId)
        this.selectedOrder = response.data
        this.dialogVisible = true
      } catch (error) {
        console.error('获取订单详情失败', error)
      }
    },
    async payOrder (orderId) {
      try {
        const response = await payOrder(orderId)
        if (response.data.orderStatus === 'Paid') {
          this.fetchOrders(this.pagination.currentPage)
        }
      } catch (error) {

      }
    },
    async cancelOrder (orderId) {
      try {
        const response = await cancelOrderApi(orderId)
        if (response.data.orderStatus === 'Cancelled') {
          this.fetchOrders(this.pagination.currentPage)
        }
      } catch (error) {

      }
    }
  },
  created () {
    this.fetchOrders()
  }
}
</script>

<style scoped>
.order {
  padding: 20px;
}
.product-image {
  width: 100px;
  height: 100px;
  object-fit: cover;
}
</style>
