<template>
    <div class="dashboard">
      <!-- 四个统计卡片 -->
      <el-row :gutter="20">
        <el-col :span="6" v-for="(item, index) in stats" :key="index">
          <el-card class="dashboard-card">
            <div class="card-content">
              <h3>{{ item.title }}</h3>
              <p>{{ item.value }}</p>
            </div>
          </el-card>
        </el-col>
      </el-row>

      <!-- 最近订单列表 -->
      <el-card class="recent-orders-card" style="margin-top: 20px;">
        <h3>最近订单</h3>
        <el-table :data="dashboardData.recentOrders" stripe style="width: 100%">
          <el-table-column prop="orderId" label="订单ID" width="100"></el-table-column>
          <el-table-column prop="productName" label="产品名称"></el-table-column>
          <el-table-column prop="totalPrice" label="总价" width="100"></el-table-column>
          <el-table-column prop="orderStatus" label="状态" width="100"></el-table-column>
          <el-table-column prop="createdAt" label="下单时间" width="180"></el-table-column>
        </el-table>
      </el-card>

      <!-- 一周订单统计 -->
      <el-card class="weekly-orders-card" style="margin-top: 20px;">
        <h3>近一周订单情况</h3>
        <el-table :data="orderStats.dailyStats" stripe style="width: 100%">
          <el-table-column prop="date" label="日期" width="180"></el-table-column>
          <el-table-column prop="generatedOrdersCount" label="生成订单数" width="150"></el-table-column>
          <el-table-column prop="completedOrdersCount" label="完成订单数" width="150"></el-table-column>
          <el-table-column prop="totalRevenue" label="总收入" width="150"></el-table-column>
        </el-table>
      </el-card>
    </div>
  </template>

<script>
import { getDashboardStatistics, getOrderStatistics } from '@/api/statistics' // 根据实际项目路径调整

export default {
  name: 'AdminDashboard',
  data () {
    return {
      dashboardData: {
        totalUsers: 0,
        totalOrders: 0,
        totalRevenue: 0,
        totalProducts: 0,
        recentOrders: []
      },
      orderStats: {
        dailyStats: []
      }
    }
  },
  computed: {
    // 便于渲染四个统计卡片的数据
    stats () {
      return [
        { title: '用户总数', value: this.dashboardData.totalUsers },
        { title: '订单总数', value: this.dashboardData.totalOrders },
        { title: '收入总额', value: this.dashboardData.totalRevenue },
        { title: '产品总数', value: this.dashboardData.totalProducts }
      ]
    }
  },
  created () {
    this.fetchDashboardData()
    this.fetchOrderStats()
  },
  methods: {
    // 获取仪表盘数据
    fetchDashboardData () {
      getDashboardStatistics()
        .then(response => {
          if (response.statusCode === 200) {
            this.dashboardData = response.data
          } else {
            this.$message.error(response.message || '获取仪表盘数据失败')
          }
        })
        .catch(error => {
          console.error('获取仪表盘数据异常：', error)
          this.$message.error('获取仪表盘数据异常')
        })
    },
    // 获取一周订单统计数据
    fetchOrderStats () {
      getOrderStatistics()
        .then(response => {
          if (response.statusCode === 200) {
            this.orderStats.dailyStats = response.data.dailyStats
          } else {
            this.$message.error(response.message || '获取订单统计数据失败')
          }
        })
        .catch(error => {
          console.error('获取订单统计数据异常：', error)
          this.$message.error('获取订单统计数据异常')
        })
    }
  }
}
</script>

  <style scoped>
  .dashboard {
    padding: 20px;
  }
  .dashboard-card {
    text-align: center;
  }
  .card-content h3 {
    margin: 0;
    font-size: 16px;
    color: #333;
  }
  .card-content p {
    margin: 10px 0 0;
    font-size: 24px;
    font-weight: bold;
    color: #409EFF;
  }
  .recent-orders-card,
  .weekly-orders-card {
    margin-top: 20px;
  }
  </style>
