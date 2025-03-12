import Vue from 'vue'
import VueRouter from 'vue-router'
import HomeView from '@/views/home'
import ProductListView from '@/views/ProductListView.vue'
import { getToken } from '@/utils/CookieUtil'
Vue.use(VueRouter)

/**
 * 路由配置说明：
 *
 * 基础路由：
 * - / : 商品列表页面（首页）
 * - /cart : 购物车页面
 * - /orders : 订单列表页面（需要登录）
 *
 * 用户认证路由：
 * - /login : 用户登录页面
 * - /register : 用户注册页面
 *
 * 路由元信息：
 * - requiresAuth: true 表示需要登录才能访问
 */

const routes = [
  {
    path: '/',
    redirect: '/home',
    children: [

    ]
  },
  {
    path: '/home',
    component: HomeView,
    name: 'home',
    meta: {
      requiresAuth: false,
      title: '首页',
      description: 'ToeMall 首页'
    }
  },
  {
    path: '/search',
    name: 'search',
    component: () => import('@/views/search'),
    meta: {
      requiresAuth: true,
      title: '搜索',
      description: '搜索商品'
    }
  },
  {
    path: '/user',
    name: 'user',
    component: () => import('@/views/user'),
    meta: {
      requiresAuth: true,
      title: '用户信息',
      description: '查看和修改个人信息'
    }
  },
  {
    path: '/products',
    name: 'products',
    component: ProductListView,
    meta: {
      requiresAuth: true,
      title: '商品列表',
      description: '浏览所有商品，支持分类过滤和搜索'
    }
  },
  {
    path: '/cart',
    name: 'cart',
    component: () => import('@/views/cart/index.vue'),
    meta: {
      requiresAuth: true,
      title: '购物车',
      description: '查看已选商品，修改数量，结算下单'
    }
  },
  {
    path: '/orders',
    name: 'orders',
    component: () => import('@/views/order'),
    meta: {
      requiresAuth: true,
      title: '我的订单',
      description: '查看所有订单历史，跟踪订单状态'
    }
  },
  {
    path: '/login',
    name: 'login',
    component: () => import('../views/LoginView.vue'),
    meta: {
      requiresAuth: false,
      title: '用户登录',
      description: '登录账户，访问个人功能'
    }
  },
  {
    path: '/register',
    name: 'register',
    component: () => import('../views/RegisterView.vue'),
    meta: {
      requiresAuth: false,
      title: '用户注册',
      description: '创建新账户'
    }
  },
  {
    path: '/admin',
    name: 'admin',
    component: () => import('@/views/admin'),
    meta: {
      requiresAuth: true,
      title: '管理员',
      description: '管理员页面'
    }
  }
]

const router = new VueRouter({
  routes
})

// 全局路由守卫
router.beforeEach((to, from, next) => {
  // 设置页面标题
  document.title = to.meta.title ? `${to.meta.title} - ToeMall` : 'ToeMall'

  // 检查是否需要登录权限
  const requiresAuth = to.matched.some(record => record.meta.requiresAuth)
  const isAuthenticated = getToken()

  if (requiresAuth && !isAuthenticated) {
    // 需要登录但未登录，重定向到登录页
    next({
      path: '/login',
      query: { redirect: to.fullPath } // 保存原目标路径
    })
  } else {
    next()
  }
})

export default router
