<template>
  <el-form :model="localUser" :rules="rules" ref="userForm" label-width="120px">
    <el-form-item label="用户名" prop="username">
      <el-input v-model="localUser.username"></el-input>
    </el-form-item>
    <el-form-item label="邮箱" prop="email">
      <el-input v-model="localUser.email"></el-input>
    </el-form-item>
    <el-form-item label="角色" prop="role">
      <el-select v-model="localUser.role">
        <el-option label="用户" value="User"></el-option>
        <el-option label="管理员" value="Admin"></el-option>
      </el-select>
    </el-form-item>
    <el-form-item label="头像" prop="avatar">
      <el-input v-model="localUser.avatar"></el-input>
    </el-form-item>
    <el-form-item label="密码" prop="password">
      <el-input v-model="localUser.password" type="password"></el-input>
    </el-form-item>
    <el-form-item label="余额" prop="pointsBalance">
      <el-input v-model.number="localUser.pointsBalance" type="number"></el-input>
    </el-form-item>
    <el-form-item>
      <el-button type="primary" @click="onSubmit">保存</el-button>
    </el-form-item>
  </el-form>
</template>

<script>
export default {
  props: {
    user: {
      type: Object,
      default: () => ({})
    }
  },
  data () {
    return {
      localUser: { ...this.user },
      rules: {
        username: [
          { required: true, message: '请输入用户名', trigger: 'blur' }
        ],
        email: [
          { required: true, message: '请输入邮箱', trigger: 'blur' },
          { type: 'email', message: '请输入有效的邮箱地址', trigger: 'blur' }
        ],
        role: [
          { required: true, message: '请选择角色', trigger: 'change' }
        ],
        avatar: [
          { required: false, message: '请输入头像链接', trigger: 'blur' }
        ],
        password: [
          { required: true, message: '请输入密码', trigger: 'blur' }
        ],
        pointsBalance: [
          { required: true, message: '请输入余额', trigger: 'blur' },
          { type: 'number', message: '余额必须为数字值', trigger: 'blur' }
        ]
      }
    }
  },
  methods: {
    onSubmit () {
      this.$refs.userForm.validate((valid) => {
        if (valid) {
          this.$emit('save', this.localUser)
        } else {
          console.log('error submit!!')
          return false
        }
      })
    }
  },
  watch: {
    user: {
      handler (newUser) {
        this.localUser = { ...newUser }
      },
      deep: true,
      immediate: true
    }
  }
}
</script>
