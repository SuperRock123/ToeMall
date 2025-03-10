<!-- filepath: d:\jobApplications\ToeMallDemo\toemall-dev\src\views\RegisterView.vue -->
<template>
  <div class="register">
    <el-card class="register-card">
      <h1>注册</h1>
      <el-form @submit.prevent="handleRegister" class="register-form" :model="form" :rules="rules" ref="registerForm">
        <el-form-item label="用户名" prop="username">
          <el-input v-model="form.username" autocomplete="off"></el-input>
        </el-form-item>
        <el-form-item label="邮箱" prop="email">
          <el-input v-model="form.email" autocomplete="off"></el-input>
        </el-form-item>
        <el-form-item label="密码" prop="password">
          <el-input type="password" v-model="form.password" autocomplete="off"></el-input>
        </el-form-item>
        <el-form-item label="头像" prop="avatar">
          <el-input v-model="form.avatar" autocomplete="off"></el-input>
        </el-form-item>
        <el-form-item>
          <el-button type="primary" @click="handleRegister">注册</el-button>
            <router-link to="/login" class="login-link">已有账号？立即登录</router-link>
        </el-form-item>
      </el-form>
    </el-card>
  </div>
</template>

<script>
import { signup } from '@/api/users'
export default {
  name: 'RegisterView',
  data () {
    return {
      form: {
        username: '',
        email: '',
        password: '',
        avatar: ''
      },
      rules: {
        username: [
          { required: true, message: '请输入用户名', trigger: 'blur' }
        ],
        email: [
          { required: true, type: 'email', message: '请输入有效的邮箱地址', trigger: 'blur' }
        ],
        password: [
          { required: true, message: '请输入密码', trigger: 'blur' }
        ]
      }
    }
  },
  methods: {
    async handleRegister () {
      this.$refs.registerForm.validate(async (valid) => {
        if (valid) {
          const response = await signup(this.form.username, this.form.password, this.form.email, this.form.avatar)
          if (response?.statusCode === 200) {
            this.$router.push('/login')
          }
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
.register {
  max-width: 400px;
  margin: 40px auto;
  padding: 20px;
}
.register-card {
  padding: 20px;
}
.login-link {
  margin-left: 20px;
}
</style>
