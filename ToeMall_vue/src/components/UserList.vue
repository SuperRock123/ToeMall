<template>
  <div>
    <el-row>
      <el-col :span="8">
        <el-input v-model="searchKeyword" placeholder="搜索用户名"></el-input>
      </el-col>
      <el-col :span="8">
        <el-select v-model="searchRole" placeholder="选择角色">
          <el-option label="所有角色" value=""></el-option>
          <el-option label="用户" value="User"></el-option>
          <el-option label="管理员" value="Admin"></el-option>
        </el-select>
      </el-col>
      <el-col :span="8" class="text-right">
        <el-button type="primary" @click="searchUsers">搜索</el-button>
        <el-button type="primary" @click="showCreateUserDialog">添加用户</el-button>
      </el-col>
    </el-row>
    <el-table :data="users" style="width: 100%">
      <el-table-column prop="username" label="用户名"></el-table-column>
      <el-table-column prop="email" label="邮箱"></el-table-column>
      <el-table-column prop="role" label="角色"></el-table-column>
      <el-table-column prop="pointsBalance" label="余额"></el-table-column>
      <el-table-column label="操作">
        <template slot-scope="scope">
          <el-button size="mini" @click="editUser(scope.row)">编辑</el-button>
          <el-button size="mini" type="danger" @click="confirmDeleteUser(scope.row)">删除</el-button>
        </template>
      </el-table-column>
    </el-table>
    <el-pagination
      background
      layout="prev, pager, next"
      :total="totalUsers"
      :page-size="pageSize"
      @current-change="handlePageChange"
    ></el-pagination>
    <el-dialog :visible.sync="userDialogVisible" title="用户信息">
      <user-form :user="selectedUser" @save="saveUser"></user-form>
    </el-dialog>
    <el-dialog
      title="确认删除"
      :visible.sync="deleteDialogVisible"
      width="30%"
      @close="deleteDialogVisible = false"
    >
      <span>确定要删除这个用户吗？</span>
      <span slot="footer" class="dialog-footer">
        <el-button @click="deleteDialogVisible = false">取消</el-button>
        <el-button type="primary" @click="deleteUser">确定</el-button>
      </span>
    </el-dialog>
  </div>
</template>

<script>
import { getUsers, searchUsers, addUserByAdmin, updateUser, deleteUser } from '@/api/users'
import UserForm from '@/components/UserForm.vue'

export default {
  components: { UserForm },
  data () {
    return {
      users: [],
      searchKeyword: '',
      searchRole: '',
      userDialogVisible: false,
      deleteDialogVisible: false,
      selectedUser: null,
      userToDelete: null,
      currentPage: 1,
      pageSize: 20,
      totalUsers: 0
    }
  },
  methods: {
    fetchUsers () {
      getUsers(this.currentPage, this.pageSize).then(response => {
        this.users = response.data.users
        this.totalUsers = response.data.pagination.totalCount
      })
    },
    searchUsers () {
      if (this.searchKeyword === '') {
        this.fetchUsers()
        return
      }
      searchUsers(this.searchKeyword, this.currentPage, this.pageSize).then(response => {
        this.users = response.data.users
        this.totalUsers = response.data.pagination.totalCount
      })
    },
    showCreateUserDialog () {
      this.selectedUser = null
      this.userDialogVisible = true
    },
    editUser (user) {
      this.selectedUser = user
      this.userDialogVisible = true
    },
    saveUser (user) {
      if (user.userId) {
        updateUser(user.userId, user.username, user.email, user.avatar, user.role, user.pointsBalance, user.password).then(this.fetchUsers)
      } else {
        addUserByAdmin(user.username, user.password, user.email, user.pointsBalance, user.role, user.avatar).then(this.fetchUsers)
      }
      this.userDialogVisible = false
    },
    confirmDeleteUser (user) {
      this.userToDelete = user
      this.deleteDialogVisible = true
    },
    deleteUser () {
      deleteUser(this.userToDelete.userId).then(() => {
        this.fetchUsers()
        this.deleteDialogVisible = false
      })
    },
    handlePageChange (page) {
      this.currentPage = page
      this.fetchUsers()
    }
  },
  mounted () {
    this.fetchUsers()
  }
}
</script>
