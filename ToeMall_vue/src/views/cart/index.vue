<!-- filepath: d:\jobApplications\ToeMallDemo\toemall-dev\src\views\cart\index.vue -->
<template>
  <div class="cart">
    <h1>购物车</h1>
    <el-table
      :data="cartItems"
      style="width: 100%"
      @selection-change="handleSelectionChange"
      ref="cartTable"
    >
      <el-table-column type="selection" width="55"></el-table-column>
      <el-table-column label="图片" width="120">
        <template slot-scope="scope">
          <img :src="scope.row.picture" class="product-image">
        </template>
      </el-table-column>
      <el-table-column prop="name" label="商品名称" width="180"></el-table-column>
      <el-table-column prop="price" label="价格" width="100">
        <template slot-scope="scope">
          <span>{{ scope.row.price | currency }}</span>
        </template>
      </el-table-column>
      <el-table-column label="数量" width="120">
        <template slot-scope="scope">
          <el-input-number v-model="scope.row.quantity" @change="updateQuantity(scope.row)" :min="1"></el-input-number>
        </template>
      </el-table-column>
      <el-table-column label="操作" width="120">
        <template slot-scope="scope">
          <el-button type="danger" @click="removeItem(scope.row.productId)">删除</el-button>
        </template>
      </el-table-column>
    </el-table>
    <div class="cart-summary">
      <el-checkbox v-model="selectAll" @change="toggleSelectAll">全选</el-checkbox>
      <div class="total">总计: {{ total | currency }}</div>
      <el-button type="primary" @click="checkout" class="checkout-btn">结算</el-button>
    </div>
  </div>
</template>

<script>
import { mapGetters, mapActions } from 'vuex'
import { createOrder } from '@/api/orders'

export default {
  name: 'CartView',
  data () {
    return {
      selectedItems: [],
      selectAll: false
    }
  },
  computed: {
    ...mapGetters('cart', ['cartItems', 'cartTotalPrice']),
    total () {
      return this.selectedItems.reduce((sum, item) => sum + item.price * item.quantity, 0)
    }
  },
  methods: {
    ...mapActions('cart', ['removeProductFromCart', 'clearCart', 'updateProductQuantity']),
    updateQuantity (item) {
      this.updateProductQuantity({ productId: item.productId, quantity: item.quantity })
    },
    removeItem (productId) {
      this.removeProductFromCart(productId)
    },
    handleSelectionChange (val) {
      this.selectedItems = val
    },
    toggleSelectAll () {
      if (this.selectAll) {
        this.$refs.cartTable.toggleAllSelection()
      } else {
        this.selectedItems = []
      }
    },
    async checkout () {
      if (this.selectedItems.length === 0) {
        this.$message.warning('请选择要结算的商品')
        return
      }
      try {
        const orderPromises = this.selectedItems.map(item => createOrder(item.productId, item.quantity))
        await Promise.all(orderPromises)
        this.selectedItems.forEach(item => this.removeProductFromCart(item.productId))
      } catch (error) {
        console.error('订单创建失败', error)
      }
    }
  }
}
</script>

<style scoped>
.cart {
  padding: 20px;
}
.product-image {
  width: 100px;
  height: 100px;
  object-fit: cover;
}
.cart-summary {
  margin-top: 20px;
  text-align: right;
}
.checkout-btn {
  margin-top: 10px;
}
</style>
