<template>
  <div class="search">
    <el-container>
      <!-- 顶部导航栏 -->
      <el-header class="search-header">
        <el-input
          v-model="searchQuery"
          :placeholder="`搜索 ${$route.query.q}`"
          clearable
          @keydown.enter.native="handleSearch"
          class="search-input"
        ></el-input>
        <el-dropdown @command="handleCommand" class="dropdown">
          <span class="el-dropdown-link">
            {{ activeTab === 'products' ? '商品' : '用户' }} <i class="el-icon-arrow-down el-icon--right"></i>
          </span>
          <el-dropdown-menu slot="dropdown">
            <el-dropdown-item command="products">商品</el-dropdown-item>
            <el-dropdown-item command="users">用户</el-dropdown-item>
          </el-dropdown-menu>
        </el-dropdown>
        <el-dropdown @command="handleSortCommand" class="dropdown">
          <span class="el-dropdown-link">
            排序: {{ sortLabel }} <i class="el-icon-arrow-down el-icon--right"></i>
          </span>
          <el-dropdown-menu slot="dropdown">
            <el-dropdown-item command="createdAt">时间</el-dropdown-item>
            <el-dropdown-item command="price">价格</el-dropdown-item>
            <el-dropdown-item command="stockQuantity">库存</el-dropdown-item>
          </el-dropdown-menu>
        </el-dropdown>
      </el-header>
      <!-- 主内容区域 -->
      <el-main>
        <div v-if="activeTab === 'products'">
          <!-- 商品列表 -->
          <el-row :gutter="20">
            <el-col :span="6" v-for="product in products" :key="product.productId">
              <el-card :body-style="{ padding: '20px' }">
                <img :src="product.picture" class="image">
                <div style="padding: 14px;">
                  <span>{{ product.name }}</span>
                  <div class="bottom clearfix">
                    <span class="price">{{ product.price | currency }}</span>
                    <el-button type="primary" icon="el-icon-shopping-cart-2" size="mini" @click="addToCart(product)">
                      加入购物车
                    </el-button>
                  </div>
                </div>
              </el-card>
            </el-col>
          </el-row>
        </div>
        <div v-else-if="activeTab === 'users'">
          <!-- 用户列表 -->
          <el-table :data="users" style="width: 100%">
            <el-table-column prop="avatar" label="头像" width="100">
              <template slot-scope="scope">
                <img :src="scope.row.avatar" class="avatar">
              </template>
            </el-table-column>
            <el-table-column prop="username" label="用户名" width="180"></el-table-column>
            <el-table-column prop="email" label="邮箱" width="200"></el-table-column>
            <el-table-column prop="pointsBalance" label="积分" width="100"></el-table-column>
          </el-table>
        </div>
        <el-pagination
          background
          layout="prev, pager, next"
          :total="pagination.totalCount"
          :page-size="pagination.pageSize"
          :current-page.sync="pagination.currentPage"
          @current-change="fetchResults">
        </el-pagination>
      </el-main>
    </el-container>
  </div>
</template>

<script>
import { searchProducts } from '@/api/products'
import { searchUsers } from '@/api/users'

export default {
  name: 'search',
  data () {
    return {
      searchQuery: this.$route.query.q || '',
      activeTab: 'products', // 默认显示商品
      sortBy: 'createdAt',
      sortLabel: '时间',
      products: [],
      users: [],
      pagination: {
        currentPage: 1,
        pageSize: 20,
        totalCount: 0
      }
    }
  },
  watch: {
    '$route.query.q': 'fetchResults'
  },
  methods: {
    // 回车或点击搜索下拉选项时触发
    handleSearch () {
      // this.$router.replace({ path: '/search', query: { q: this.searchQuery } })
      this.fetchResults()
    },
    // 下拉菜单切换搜索类型（商品、用户）
    handleCommand (command) {
      this.activeTab = command
      this.fetchResults()
    },
    // 排序下拉菜单
    handleSortCommand (command) {
      this.sortBy = command
      this.sortLabel = command === 'createdAt' ? '时间' : command === 'price' ? '价格' : '库存'
      this.fetchResults()
    },
    async fetchResults (page = 1) {
      this.pagination.currentPage = page
      if (this.activeTab === 'products') {
        console.log(this.sortBy)
        const response = await searchProducts(this.searchQuery, page, this.pagination.pageSize, null, this.sortBy)
        this.products = response.data.products
        this.pagination.totalCount = response.data.pagination.totalCount
      } else if (this.activeTab === 'users') {
        const response = await searchUsers(this.searchQuery, page, this.pagination.pageSize)
        this.users = response.data.users
        this.pagination.totalCount = response.data.pagination.totalCount
      }
    },
    addToCart (product) {
      // 此处添加加入购物车逻辑
      console.log('加入购物车', product)
    }
  },
  created () {
    this.fetchResults()
  }
}
</script>

<style scoped>
.search-header {
  display: flex;
  justify-content: center;
  align-items: center;
  padding: 20px;
  background-color: #fff;
  box-shadow: 0 2px 4px rgba(0,0,0,0.1);
}

.search-input {
  width: 400px;
  margin-right: 20px;
}

.dropdown {
  margin-left: 20px;
  cursor: pointer;
}

.image {
  width: 100%;
  height: 200px;
  object-fit: cover;
  border-radius: 4px;
}

.price {
  color: #f56c6c;
  font-size: 18px;
  font-weight: bold;
}

.bottom {
  margin-top: 10px;
  display: flex;
  justify-content: space-between;
  align-items: center;
}

.avatar {
  width: 40px;
  height: 40px;
  border-radius: 50%;
}

/* 提升 Element UI 弹出层层级，防止被遮挡 */
.el-popper {
  z-index: 3000 !important;
}
</style>
