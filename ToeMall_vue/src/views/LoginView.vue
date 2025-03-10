<template>
  <div class="login">
    <el-card class="login-card">
      <h1>登录</h1>
      <el-form @submit.prevent="handleLogin" class="login-form" :model="form" :rules="rules" ref="loginForm">
        <el-form-item label="用户名" prop="username">
          <el-input v-model="form.username" autocomplete="off"></el-input>
        </el-form-item>
        <el-form-item label="密码" prop="password">
          <el-input type="password" v-model="form.password" autocomplete="off"></el-input>
        </el-form-item>
        <el-form-item>
          <el-button type="primary" @click="handleLogin">登录</el-button>
          <router-link to="/register" class="register-link">没有账号？立即注册</router-link>
        </el-form-item>
      </el-form>
    </el-card>
  </div>
</template>

<script>
import { saveToken } from '@/utils/CookieUtil'
import { login } from '@/api/users'
export default {
  name: 'LoginView',
  data () {
    return {
      form: {
        username: '',
        password: ''
      },
      rules: {
        username: [
          { required: true, message: '请输入用户名', trigger: 'blur' }
        ],
        password: [
          { required: true, message: '请输入密码', trigger: 'blur' }
        ]
      }
    }
  },
  methods: {
    async handleLogin () {
      this.$refs.loginForm.validate(async (valid) => {
        if (valid) {
          const response = await login(this.form.username, this.form.password)
          console.log(response)
          if (response?.statusCode === 200) {
            // console.log('登录成功')
            saveToken(response.data.token)
            this.$router.replace('/')
            // console.log(getToken())
            // this.$router.push('/')
          } else {
            // console.log('登录失败')
          }
          // 这里将实现登录逻辑
        } else {
          console.log('表单验证失败')
          return false
        }
      })
    }
  }
}
</script>

<style scoped>
.login {
  max-width: 400px;
  margin: 40px auto;
  padding: 20px;
}
.login-card {
  padding: 20px;
}
.register-link {
  margin-left: 20px;
}
</style>
