<!-- filepath: d:\jobApplications\ToeMallDemo\toemall-dev\src\views\home\index.vue -->
<template>
  <div class="home">
    <el-container style="height: 100vh;">
      <!-- 顶部导航栏 -->
      <el-header>
        <nav class="navbar">
          <div class="nav-left">
            <router-link to="/" class="nav-brand">ToeMall</router-link>
          </div>
          <div class="nav-center">
            <el-input
              v-model="searchQuery"
              placeholder="搜索商品..."
              clearable
              @keyup.native.enter="handleSearch"
              class="search-input"
            ></el-input>
          </div>
          <div class="nav-right">
            <template v-if="isAuthenticated">
              <div class="user-info" @click="drawer = true">
                <el-avatar :src="user.avatar" />
                <span class="username">{{ user.username }}</span>
              </div>
              <el-drawer
                title="个人中心"
                :visible.sync="drawer"
                direction="rtl"
                :with-header="false"
              >
                <div class="drawer-content">
                  <el-avatar :src="user.avatar" size="large" />
                  <p class="drawer-username">{{ user.username }}</p>
                  <p class="drawer-email">{{ user.email }}</p>
                  <p class="drawer-points">积分: {{ user.pointsBalance }}</p>
                  <router-link to="/cart" class="cart-link">
                    <el-button type="primary" icon="el-icon-shopping-cart">购物车</el-button>
                  </router-link>
                  <el-button type="primary" @click="openEditDialog">修改个人信息</el-button>
                  <router-link to="/orders" class="orders-link">
                    <el-button type="info" icon="el-icon-document">订单管理</el-button>
                  </router-link>
                  <el-button type="danger" @click="handleLogout">退出登录</el-button>
                  <template v-if="user.role === 'Admin'">
                    <router-link to="/admin" class="admin-link">
                      <el-button type="warning">管理员</el-button>
                    </router-link>
                  </template>
                </div>
              </el-drawer>
            </template>
            <template v-else>
              <router-link to="/login" class="auth-link">登录</router-link>
              <router-link to="/register" class="auth-link">注册</router-link>
            </template>
          </div>
        </nav>
      </el-header>

      <el-container>
        <!-- 左侧菜单：二级分类菜单，不做跳转，使用组件匹配 -->
        <el-aside width="200px" style="background-color: #F5F7FA;">
          <el-menu :default-active="activeIndex" @select="handleCategorySelect">
            <el-submenu index="1">
              <template #title>分类</template>
              <el-menu-item :index="'all'">全部</el-menu-item>
              <el-menu-item
                v-for="category in categories"
                :key="category.categoryId"
                :index="category.categoryId.toString()"
              >
                {{ category.name }}
              </el-menu-item>
            </el-submenu>
          </el-menu>
        </el-aside>

        <!-- 主内容区域 -->
        <el-main>
          <product-list :category="selectedCategory"></product-list>
        </el-main>
      </el-container>

      <!-- 回到顶部按钮 -->
      <back-to-top></back-to-top>

      <!-- 底部 -->
      <el-footer style="background-color: #909399; color: white; text-align: center;">
        © 2025 ToeMall All Rights Reserved.
      </el-footer>
    </el-container>

    <!-- 修改个人信息对话框 -->
    <el-dialog title="修改个人信息" :visible.sync="editDialogVisible">
      <el-form :model="editForm" label-width="100px">
        <el-form-item label="用户名">
          <el-input v-model="editForm.username"></el-input>
        </el-form-item>
        <el-form-item label="邮箱">
          <el-input v-model="editForm.email"></el-input>
        </el-form-item>
        <el-form-item label="头像">
          <el-input v-model="editForm.avatar"></el-input>
        </el-form-item>
        <el-form-item label="密码">
          <el-input type="password" v-model="editForm.password"></el-input>
        </el-form-item>
        <el-form-item label="积分">
          <el-input v-model="editForm.pointsBalance" disabled></el-input>
        </el-form-item>
      </el-form>
      <div slot="footer" class="dialog-footer">
        <el-button @click="editDialogVisible = false">取消</el-button>
        <el-button type="primary" @click="updateUserInfo">保存</el-button>
      </div>
    </el-dialog>
  </div>
</template>

<script>
import BackToTop from '@/components/BackToTop.vue'
import ProductList from '@/components/ProductList.vue'
import { getToken, removeToken } from '@/utils/CookieUtil'
import { getUserInfo, updateUser } from '@/api/users'
import { getAllCategories } from '@/api/category'

export default {
  name: 'home',
  components: {
    ProductList,
    BackToTop
  },
  data () {
    return {
      drawer: false,
      editDialogVisible: false,
      activeIndex: 'all',
      selectedCategory: null,
      searchQuery: '',
      user: {
        username: '',
        avatar: '',
        email: '',
        pointsBalance: 0,
        role: ''
      },
      editForm: {
        username: '',
        avatar: '',
        email: '',
        password: '',
        pointsBalance: 0
      },
      categories: []
    }
  },
  computed: {
    isAuthenticated () {
      return getToken() !== null
    }
  },
  methods: {
    async fetchUserInfo () {
      if (this.isAuthenticated) {
        try {
          const response = await getUserInfo()
          this.user = response.data
          this.editForm = { ...response.data, password: '' }
        } catch (error) {
        }
      }
    },
    async fetchCategories () {
      try {
        const response = await getAllCategories()
        this.categories = response.data
      } catch (error) {
      }
    },
    handleCategorySelect (index) {
      this.selectedCategory = index
    },
    handleSearch () {
      // 回车后跳转到 /search，并传递查询参数
      this.$router.push({ path: '/search', query: { q: this.searchQuery } })
    },
    handleLogout () {
      // console.log('handleLogout')
      removeToken()
      this.$router.replace('/login')
    },
    openEditDialog () {
      this.editDialogVisible = true
    },
    async updateUserInfo () {
      try {
        await updateUser(this.user.userId, this.editForm.username, this.editForm.email, this.editForm.avatar, this.user.role, this.user.pointsBalance, this.editForm.password)
        this.editDialogVisible = false
        this.handleLogout()
      } catch (error) {
      }
    }
  },
  created () {
    this.fetchUserInfo()
    this.fetchCategories()
  }
}
</script>

<style scoped>
/* 顶部导航栏 */
.navbar {
  padding: 1rem 2rem;
  background: linear-gradient(to right, #ffffff, #f0f0f0);
  box-shadow: 0 2px 4px rgba(0,0,0,0.1);
  display: flex;
  align-items: center;
  justify-content: space-between;
  border-bottom: 1px solid #e0e0e0;
}

/* 分区容器 */
.nav-left,
.nav-center,
.nav-right {
  display: flex;
  align-items: center;
}

/* 品牌样式 */
.nav-brand {
  font-size: 1.8rem;
  font-weight: bold;
  color: #333;
  text-decoration: none;
}

/* 搜索框 */
.search-input {
  width: 400px;
  border-radius: 20px;
  box-shadow: 0 2px 6px rgba(0,0,0,0.1);
}

/* 右侧区域 */
.nav-right {
  gap: 1rem;
}

/* 登录/注册链接 */
.auth-link {
  text-decoration: none;
  color: #333;
  padding: 0.5rem 1rem;
  transition: background-color 0.3s ease;
}
.auth-link:hover {
  background-color: #f5f5f5;
  border-radius: 4px;
}

/* 用户信息 */
.user-info {
  display: flex;
  align-items: center;
  cursor: pointer;
  transition: transform 0.3s ease;
}
.user-info:hover {
  transform: scale(1.05);
}
.user-info .username {
  margin-left: 8px;
  font-size: 1.1rem;
  color: #333;
}

/* Drawer 弹出层内样式 */
.drawer-content {
  display: flex;
  flex-direction: column;
  align-items: center;
  padding: 20px;
}
.drawer-username {
  margin-top: 10px;
  font-size: 1.4rem;
  font-weight: bold;
  color: #333;
}
.drawer-email,
.drawer-points {
  margin-top: 5px;
  font-size: 1rem;
  color: #666;
}

/* 左侧菜单 */
.el-aside {
  background-color: #f9f9f9;
  border-right: 1px solid #e0e0e0;
  padding: 10px;
}
.el-menu {
  border: none;
}

/* 商品卡片 */
.image {
  width: 100%;
  height: 200px;
  object-fit: cover;
  border-radius: 6px;
  transition: transform 0.3s ease;
}
.image:hover {
  transform: scale(1.05);
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

/* 底部 */
.el-footer {
  background-color: #909399;
  color: #fff;
  text-align: center;
  padding: 1rem;
}

/* 提升 Element UI 弹出层层级，防止下拉菜单被遮挡 */
.el-popper {
  z-index: 3000 !important;
}
</style>
