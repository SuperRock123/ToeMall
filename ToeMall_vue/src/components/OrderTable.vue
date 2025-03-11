<template>
  <div class="order-table-container">
    <el-table :data="orderItems" class="order-table" stripe border>
      <!-- 商品图片 -->
      <el-table-column label="商品图片" width="100">
        <template slot-scope="scope">
          <el-image
            :src="scope.row.picture"
            class="product-image"
            fit="cover"
            v-if="scope.row.picture"
          />
          <span v-else class="no-image">无图片</span>
        </template>
      </el-table-column>

      <!-- 商品ID -->
      <el-table-column prop="productId" label="商品ID" width="100"></el-table-column>
      <!-- 商品名称 -->
      <el-table-column prop="productName" label="商品名称"></el-table-column>
      <!-- 数量 -->
      <el-table-column prop="quantity" label="数量" width="80"></el-table-column>
      <!-- 单价 -->
      <el-table-column prop="price" label="单价" width="100"></el-table-column>
      <!-- 总价 -->
      <el-table-column prop="totalPrice" label="总价" width="100"></el-table-column>
      <!-- 用户信息 -->
      <el-table-column label="用户信息">
        <template slot-scope="scope">
          <div class="user-info">
            <div>用户ID：{{ scope.row.userId }}</div>
            <div>用户名：{{ scope.row.username }}</div>
          </div>
        </template>
      </el-table-column>
    </el-table>
  </div>
</template>

<script>
export default {
  props: {
    // 接收订单对象，默认空对象
    order: {
      type: Object,
      default: () => ({})
    }
  },
  computed: {
    orderItems () {
      // 如果订单对象中包含 items 数组，则直接使用
      if (this.order.items && this.order.items.length > 0) {
        return this.order.items.map(item => ({
          productId: item.product ? item.product.productId : item.productId,
          productName: item.product ? item.product.name : item.productName,
          quantity: item.quantity,
          price: item.product ? item.product.price : item.price,
          totalPrice: item.totalPrice,
          picture: item.product ? item.product.picture : '',
          // 如果每个订单项中未包含用户信息，则统一使用订单的 user
          userId: this.order.user ? this.order.user.userId : '',
          username: this.order.user ? this.order.user.username : ''
        }))
      }
      // 如果订单直接为单个订单对象，则构造成单行数据
      if (this.order && Object.keys(this.order).length > 0) {
        return [{
          productId: this.order.product ? this.order.product.productId : '',
          productName: this.order.product ? this.order.product.name : this.order.productName,
          quantity: this.order.quantity,
          price: this.order.product ? this.order.product.price : this.order.price,
          totalPrice: this.order.totalPrice,
          picture: this.order.product ? this.order.product.picture : '',
          userId: this.order.user ? this.order.user.userId : '',
          username: this.order.user ? this.order.user.username : ''
        }]
      }
      return []
    }
  },
  mounted () {
    console.log('order props:', this.order)
  }
}
</script>

<style scoped>
.order-table-container {
  padding: 20px;
  background: #f7f9fc;
  border-radius: 8px;
  box-shadow: 0 2px 12px rgba(0, 0, 0, 0.1);
  margin: 20px auto;
  max-width: 1200px;
}

.order-table {
  background: #fff;
  border-radius: 4px;
  overflow: hidden;
}

/* 自定义表头样式 */
.el-table th {
  background-color: #409EFF;
  color: #fff;
  font-weight: bold;
  text-align: center;
}

/* 行悬浮效果 */
.el-table .el-table__row:hover {
  background-color: #f2f6fc;
}

/* 商品图片样式 */
.product-image {
  width: 80px;
  height: 80px;
  border-radius: 4px;
  border: 1px solid #eaeaea;
}

/* 无图片时显示样式 */
.no-image {
  display: inline-block;
  width: 80px;
  height: 80px;
  line-height: 80px;
  text-align: center;
  background-color: #f2f2f2;
  border-radius: 4px;
  color: #888;
}

/* 用户信息样式 */
.user-info {
  font-size: 14px;
  color: #606266;
  line-height: 1.5;
}
</style>
