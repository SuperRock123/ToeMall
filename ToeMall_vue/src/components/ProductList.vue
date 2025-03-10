<template>
  <div class="product-list">
    <div v-infinite-scroll="loadMore" :infinite-scroll-disabled="loading" :infinite-scroll-distance="10">
      <el-row :gutter="20">
        <el-col :span="6" v-for="product in products" :key="product.productId">
          <el-card class="product-card" shadow="hover" :body-style="{ padding: '10px' }">
            <!-- 商品名称作为卡片头部 -->
            <div slot="header" class="card-header">
              <span class="card-title">{{ product.name }}</span>
            </div>
            <!-- 商品图片 -->
            <img :src="product.picture" class="image">
            <!-- 卡片内容 -->
            <div class="card-content">
              <p class="description">{{ product.description }}</p>
              <p class="stock">库存: {{ product.stockQuantity }}</p>
              <div class="bottom clearfix">
                <span class="price">{{ product.price | currency }}</span>
                <el-button type="primary" icon="el-icon-shopping-cart-2" size="mini" @click="openDialog(product)">
                  加入购物车
                </el-button>
              </div>
            </div>
          </el-card>
        </el-col>
      </el-row>
    </div>

    <!-- 数量选择对话框 -->
    <el-dialog title="选择数量" :visible.sync="dialogVisible">
      <div>
        <el-input-number v-model="selectedQuantity" :min="1" :max="selectedProduct?.stockQuantity"></el-input-number>
      </div>
      <span slot="footer" class="dialog-footer">
        <el-button @click="dialogVisible = false">取消</el-button>
        <el-button type="primary" @click="addToCart">确定</el-button>
      </span>
    </el-dialog>
  </div>
</template>

<script>
import { getProducts } from '@/api/products'
import { mapActions } from 'vuex'

export default {
  name: 'ProductList',
  props: {
    category: {
      type: String,
      default: null
    }
  },
  data () {
    return {
      products: [],
      page: 1,
      pageSize: 20,
      loading: false,
      dialogVisible: false,
      selectedProduct: null,
      selectedQuantity: 1
    }
  },
  watch: {
    category: 'fetchProducts'
  },
  methods: {
    ...mapActions('cart', ['addProductToCart']),
    async fetchProducts () {
      this.page = 1
      this.products = []
      await this.loadMore()
    },
    async loadMore () {
      if (this.loading) return
      this.loading = true
      try {
        // 请求商品数据
        let response = null
        if (this.category === 'all') {
          response = await getProducts(this.page, this.pageSize)
        } else {
          response = await getProducts(this.page, this.pageSize, this.category)
        }
        this.products = [...this.products, ...response.data.products]
        this.page += 1
      } catch (error) {
      } finally {
        this.loading = false
      }
    },
    openDialog (product) {
      this.selectedProduct = product
      this.selectedQuantity = 1
      this.dialogVisible = true
    },
    addToCart () {
      this.addProductToCart({ ...this.selectedProduct, quantity: this.selectedQuantity })
      this.dialogVisible = false
    }
  },
  mounted () {
    this.fetchProducts()
  }
}
</script>

<style scoped>
.product-list {
  padding: 20px;
}

.product-card {
  transition: transform 0.3s, box-shadow 0.3s;
}
.product-card:hover {
  transform: translateY(-5px);
  box-shadow: 0 8px 16px rgba(0, 0, 0, 0.2);
}

.image {
  width: 100%;
  height: 200px;
  object-fit: cover;
  border-radius: 4px;
}

.card-header {
  font-weight: bold;
  font-size: 16px;
  background-color: #f5f7fa;
  padding: 10px;
  border-bottom: 1px solid #ebeef5;
}

.card-title {
  color: #333;
}

.card-content {
  padding: 10px 0;
}

.description {
  color: #666;
  margin: 0;
  font-size: 14px;
  min-height: 40px;
}

.stock {
  color: #999;
  font-size: 12px;
  margin: 5px 0;
}

.bottom {
  margin-top: 10px;
  display: flex;
  justify-content: space-between;
  align-items: center;
}

.price {
  color: #f56c6c;
  font-size: 18px;
  font-weight: bold;
}
</style>
