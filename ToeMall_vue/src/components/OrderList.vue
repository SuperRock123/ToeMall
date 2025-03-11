<template>
  <div>
    <el-row>
      <el-col :span="8">
        <el-input v-model="searchKeyword" placeholder="搜索订单"></el-input>
      </el-col>
      <el-col :span="8">
        <el-select v-model="searchStatus" placeholder="选择订单状态">
          <el-option label="所有状态" value=""></el-option>
          <el-option label="未支付" value="Unpaid"></el-option>
          <el-option label="已支付" value="Paid"></el-option>
          <el-option label="已取消" value="Cancelled"></el-option>
        </el-select>
      </el-col>
      <el-col :span="8" class="text-right">
        <el-button type="primary" @click="searchOrders">搜索</el-button>
      </el-col>
    </el-row>
    <el-table :data="orders" style="width: 100%">
      <el-table-column prop="orderId" label="订单ID"></el-table-column>
      <el-table-column prop="orderStatus" label="状态"></el-table-column>
      <el-table-column prop="productName" label="商品名称"></el-table-column>
      <el-table-column prop="totalPrice" label="总价"></el-table-column>
      <el-table-column label="操作">
        <template slot-scope="scope">
          <el-button size="mini" @click="viewOrder(scope.row)">查看</el-button>
          <el-button size="mini" type="danger" @click="confirmCancelOrder(scope.row)">取消</el-button>
        </template>
      </el-table-column>
    </el-table>
    <el-pagination
      background
      layout="prev, pager, next"
      :total="totalOrders"
      :page-size="pageSize"
      @current-change="handlePageChange"
    ></el-pagination>
    <el-dialog :visible.sync="orderDialogVisible" title="订单详情">
      <order-table :order="selectedOrder"></order-table>
    </el-dialog>
    <el-dialog
      title="确认取消订单"
      :visible.sync="cancelDialogVisible"
      width="30%"
      @close="cancelDialogVisible = false"
    >
      <span>确定要取消这个订单吗？</span>
      <span slot="footer" class="dialog-footer">
        <el-button @click="cancelDialogVisible = false">取消</el-button>
        <el-button type="primary" @click="cancelOrder">确定</el-button>
      </span>
    </el-dialog>
  </div>
</template>

<script>
import { getOrders, searchOrders, cancelOrder, getOrderDetail } from '@/api/orders'
import OrderTable from '@/components/OrderTable.vue'

export default {
  components: { OrderTable },
  data () {
    return {
      orders: [],
      searchKeyword: '',
      searchStatus: '',
      orderDialogVisible: false,
      cancelDialogVisible: false,
      selectedOrder: null,
      orderToCancel: null,
      currentPage: 1,
      pageSize: 20,
      totalOrders: 0
    }
  },
  methods: {
    fetchOrders () {
      getOrders(this.currentPage, this.pageSize).then(response => {
        this.orders = response.data.orders
        this.totalOrders = response.data.pagination.totalCount
      })
    },
    searchOrders () {
      searchOrders(this.searchKeyword, this.searchStatus, this.currentPage, this.pageSize).then(response => {
        this.orders = response.data.orders
        this.totalOrders = response.data.pagination.totalCount
      })
    },
    viewOrder (order) {
      getOrderDetail(order.orderId).then(response => {
        this.selectedOrder = response.data
        this.orderDialogVisible = true
      })
    },
    confirmCancelOrder (order) {
      this.orderToCancel = order
      this.cancelDialogVisible = true
    },
    cancelOrder () {
      cancelOrder(this.orderToCancel.orderId).then(() => {
        this.fetchOrders()
        this.cancelDialogVisible = false
      })
    },
    handlePageChange (page) {
      this.currentPage = page
      this.fetchOrders()
    }
  },
  mounted () {
    this.fetchOrders()
  }
}
</script>
