<template>
  <div class="dashboard-container">
    <!-- 仪表盘统计信息卡片 -->
    <el-row :gutter="20">
      <el-col :span="6">
        <el-card class="stat-card">
          <div class="stat-title">总用户数</div>
          <div class="stat-value">{{ statistics.totalUsers }}</div>
        </el-card>
      </el-col>
      <el-col :span="6">
        <el-card class="stat-card">
          <div class="stat-title">总订单数</div>
          <div class="stat-value">{{ statistics.totalOrders }}</div>
        </el-card>
      </el-col>
      <el-col :span="6">
        <el-card class="stat-card">
          <div class="stat-title">总收入</div>
          <div class="stat-value">¥{{ statistics.totalRevenue.toFixed(2) }}</div>
        </el-card>
      </el-col>
      <el-col :span="6">
        <el-card class="stat-card">
          <div class="stat-title">总商品数</div>
          <div class="stat-value">{{ statistics.totalProducts }}</div>
        </el-card>
      </el-col>
    </el-row>

    <!-- 饼图：用户统计 & 商品统计 -->
    <el-row :gutter="20" class="chart-container">
      <el-col :span="12">
        <el-card>
          <div class="chart-title">用户统计</div>
          <div ref="userPieChart" class="chart"></div>
        </el-card>
      </el-col>
      <el-col :span="12">
        <el-card>
          <div class="chart-title">商品统计</div>
          <div ref="productPieChart" class="chart"></div>
        </el-card>
      </el-col>
    </el-row>

    <!-- 折线图：订单趋势 -->
    <el-row :gutter="20">
      <el-col :span="24">
        <el-card>
          <div class="chart-title">订单趋势</div>
          <div ref="orderLineChart" class="chart"></div>
        </el-card>
      </el-col>
    </el-row>

    <!-- 滚动显示最近订单（使用 el-carousel） -->
    <el-row :gutter="20">
      <el-col :span="24">
        <el-card>
          <div class="chart-title">最近订单</div>
          <el-carousel height="200px" indicator-position="none" autoplay interval="3000">
            <el-carousel-item v-for="order in statistics.recentOrders" :key="order.orderId">
              <div class="order-item">
                <span>订单ID: {{ order.orderId }}</span>
                <span>商品: {{ order.productName }}</span>
                <span>金额: ¥{{ order.totalPrice.toFixed(2) }}</span>
                <span>状态: {{ order.orderStatus }}</span>
              </div>
            </el-carousel-item>
          </el-carousel>
        </el-card>
      </el-col>
    </el-row>
  </div>
</template>

<script>
import * as echarts from 'echarts'
import { getDashboardStatistics } from '@/api/statistics'

export default {
  name: 'Dashboard',
  data () {
    return {
      statistics: {
        totalUsers: 0,
        activeUsers: 0,
        newUsersToday: 0,
        totalOrders: 0,
        totalRevenue: 0,
        totalProducts: 0,
        lowStockProducts: 0,
        categoriesCount: 0,
        recentOrders: [],
        dailyStats: []
      }
    }
  },
  async mounted () {
    await this.fetchStatistics()
    this.initCharts()
    window.addEventListener('resize', this.resizeCharts)
  },
  beforeDestroy () {
    window.removeEventListener('resize', this.resizeCharts)
  },
  methods: {
    async fetchStatistics () {
      const response = await getDashboardStatistics()
      if (response.statusCode === 200) {
        // 兼容后端可能缺失某些字段，提供默认值
        const data = response.data
        this.statistics = {
          totalUsers: data.totalUsers || 0,
          activeUsers: data.activeUsers || 0,
          newUsersToday: data.newUsersToday || 0,
          totalOrders: data.totalOrders || 0,
          totalRevenue: data.totalRevenue || 0,
          totalProducts: data.totalProducts || 0,
          lowStockProducts: data.lowStockProducts || 0,
          categoriesCount: data.categoriesCount || 0,
          recentOrders: data.recentOrders || [],
          dailyStats: data.dailyStats || []
        }
      }
    },
    initCharts () {
      this.initUserPieChart()
      this.initProductPieChart()
      this.initOrderLineChart()
    },
    initUserPieChart () {
      const chart = echarts.init(this.$refs.userPieChart)
      const option = {
        title: { text: '用户统计', left: 'center', top: 10 },
        tooltip: { trigger: 'item' },
        legend: { orient: 'vertical', left: 'left' },
        color: ['#5470C6', '#91CC75', '#EE6666'],
        series: [{
          name: '用户统计',
          type: 'pie',
          radius: '50%',
          data: [
            { value: this.statistics.totalUsers, name: '总用户' },
            { value: this.statistics.activeUsers, name: '活跃用户' },
            { value: this.statistics.newUsersToday, name: '今日新增' }
          ],
          emphasis: {
            itemStyle: {
              shadowBlur: 10,
              shadowOffsetX: 0,
              shadowColor: 'rgba(0, 0, 0, 0.5)'
            }
          }
        }]
      }
      chart.setOption(option)
      this.userPieChart = chart
    },
    initProductPieChart () {
      const chart = echarts.init(this.$refs.productPieChart)
      const option = {
        title: { text: '商品统计', left: 'center', top: 10 },
        tooltip: { trigger: 'item' },
        legend: { orient: 'vertical', left: 'left' },
        color: ['#73C0DE', '#FAC858', '#EE6666'],
        series: [{
          name: '商品统计',
          type: 'pie',
          radius: '50%',
          data: [
            { value: this.statistics.totalProducts, name: '总商品' },
            { value: this.statistics.lowStockProducts, name: '低库存商品' },
            { value: this.statistics.categoriesCount, name: '商品分类' }
          ],
          emphasis: {
            itemStyle: {
              shadowBlur: 10,
              shadowOffsetX: 0,
              shadowColor: 'rgba(0, 0, 0, 0.5)'
            }
          }
        }]
      }
      chart.setOption(option)
      this.productPieChart = chart
    },
    initOrderLineChart () {
      const chart = echarts.init(this.$refs.orderLineChart)
      const dates = this.statistics.dailyStats ? this.statistics.dailyStats.map(d => d.date) : []
      const generatedOrders = this.statistics.dailyStats ? this.statistics.dailyStats.map(d => d.generatedOrdersCount) : []
      const completedOrders = this.statistics.dailyStats ? this.statistics.dailyStats.map(d => d.completedOrdersCount) : []
      const option = {
        title: { text: '订单趋势', left: 'center', top: 10 },
        tooltip: { trigger: 'axis' },
        legend: { data: ['总订单', '已完成订单'], top: 40 },
        xAxis: { type: 'category', data: dates },
        yAxis: { type: 'value' },
        series: [
          { name: '总订单', type: 'line', data: generatedOrders, smooth: true },
          { name: '已完成订单', type: 'line', data: completedOrders, smooth: true }
        ]
      }
      chart.setOption(option)
      this.orderLineChart = chart
    },
    resizeCharts () {
      if (this.userPieChart) this.userPieChart.resize()
      if (this.productPieChart) this.productPieChart.resize()
      if (this.orderLineChart) this.orderLineChart.resize()
    }
  }
}
</script>

<style scoped>
/* 整体容器样式 */
.dashboard-container {
  padding: 30px;
  background-color: #f0f2f5;
  min-width: 1200px; /* 保证在大屏幕上显示宽敞 */
}

/* 每行间距 */
.el-row {
  margin-bottom: 20px;
}

/* 仪表盘卡片样式 */
.stat-card {
  text-align: center;
  padding: 20px;
  font-size: 28px;
  font-weight: bold;
  color: #333;
  border-radius: 8px;
  background-color: #ffffff;
  box-shadow: 0 2px 12px rgba(0, 0, 0, 0.1);
  transition: transform 0.2s ease;
}
.stat-card:hover {
  transform: translateY(-5px);
}

/* 图表容器 */
.chart-container {
  margin-top: 20px;
}

/* 图表整体样式 */
.chart {
  width: 100%;
  height: 350px;
  border: 1px solid #ebeef5;
  border-radius: 4px;
  background-color: #fff;
  position: relative;
}

/* 图表标题样式 */
.chart-title {
  text-align: center;
  font-size: 18px;
  font-weight: bold;
  margin-bottom: 10px;
  color: #333;
  padding-top: 10px;
}

/* 订单项样式（滚动显示的） */
.order-item {
  display: flex;
  justify-content: space-between;
  align-items: center;
  padding: 10px 20px;
  border-bottom: 1px solid #ebeef5;
  font-size: 16px;
  color: #606266;
  transition: background-color 0.3s ease;
}
.order-item:hover {
  background-color: #f5f7fa;
}

/* Carousel 容器样式 */
.el-carousel {
  border: 1px solid #ebeef5;
  border-radius: 4px;
  background: #ffffff;
}

/* 调整 Element UI 卡片内边距 */
.el-card {
  padding: 10px;
}

/* 响应式调整，适配屏幕宽度较小时的情况 */
@media (max-width: 1400px) {
  .dashboard-container {
    min-width: 1000px;
  }
  .stat-card {
    font-size: 24px;
  }
  .chart {
    height: 300px;
  }
}
</style>
