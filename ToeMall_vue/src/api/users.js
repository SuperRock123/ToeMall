import request from '@/utils/request.js'

export function login (username, password) {
  return request.post('/users/login', {
    username,
    password
  })
}
// 响应示例
// {
//   "statusCode": 200,
//   "message": "登录成功",
//   "data": {
//     "userId": 1,
//     "username": "User1",
//     "email": "user1@example.com",
//     "role": "User",
//     "avatar": "http://example.com/avatar1.jpg",
//     "token": "generated_token"
//   }
// }
// 注册
export function signup (username, password, email, avatar) {
  return request.post('/Users/signup', {
    username,
    email,
    passwordHash: password,
    avatar
  })
}
// 响应示例
// {
//   "statusCode": 200,
//   "message": "注册成功",
//   "data": null
// }
// 获取用户列表
export function getUsers (page = 1, pageSize = 20) {
  return request.get('/Users', {
    params: {
      page,
      pageSize
    }
  })
}
// 响应示例
// {
//   "statusCode": 200,
//   "message": "获取用户列表成功",
//   "data": {
//     "users": [
//       {
//         "userId": 1,
//         "username": "User1",
//         "email": "user1@example.com",
//         "role": "User",
//         "pointsBalance": 20,
//         "createdAt": "2025-03-08T00:00:00Z",
//         "updatedAt": "2025-03-08T00:00:00Z",
//         "avatar": "http://example.com/avatar1.jpg"
//       }
//     ],
//     "pagination": {
//       "currentPage": 1,
//       "pageSize": 20,
//       "totalCount": 100,
//       "totalPages": 5
//     }
//   }
// }
// 获取当前用户信息
export function getUserInfo () {
  return request.get('/Users/info')
}
// 响应示例
// {
//   "statusCode": 200,
//   "message": "获取用户信息成功",
//   "data": {
//     "userId": 1,
//     "username": "User1",
//     "email": "user1@example.com",
//     "role": "User",
//     "pointsBalance": 20,
//     "createdAt": "2025-03-08T00:00:00Z",
//     "updatedAt": "2025-03-08T00:00:00Z",
//     "avatar": "http://example.com/avatar1.jpg"
//   }
// }
// 更新用户信息
export function updateUser (userId, username, email, avatar, role, pointsBalance, passwordHash) {
  return request.put('/Users/update', {
    userId,
    username,
    email,
    avatar,
    role,
    pointsBalance,
    passwordHash
  })
}
// 响应示例
// {
//   "statusCode": 200,
//   "message": "用户信息更新成功，请重新登录",
//   "data": {
//     "userId": 1,
//     "username": "UpdatedUser",
//     "email": "updateduser@example.com",
//     "role": "User",
//     "pointsBalance": 20,
//     "createdAt": "2025-03-08T00:00:00Z",
//     "updatedAt": "2025-03-08T00:00:00Z",
//     "avatar": "http://example.com/avatar1.jpg"
//   }
// }
// 删除用户（管理员权限）
export function deleteUser (userId) {
  return request.delete(`/Users/${userId}`)
}
// 响应示例
// {
//   "statusCode": 200,
//   "message": "用户删除成功",
//   "data": {
//     "deletedUserId": 1
//   }
// }
// 搜索用户
export function searchUsers (username, page = 1, pageSize = 20) {
  return request.get('/Users/search', {
    params: {
      username,
      page,
      pageSize
    }
  })
}
// 响应示例
// {
//   "statusCode": 200,
//   "message": "搜索用户成功",
//   "data": {
//     "users": [
//       {
//         "userId": 1,
//         "username": "User1",
//         "email": "user1@example.com",
//         "role": "User",
//         "pointsBalance": 20,
//         "createdAt": "2025-03-08T00:00:00Z",
//         "updatedAt": "2025-03-08T00:00:00Z",
//         "avatar": "http://example.com/avatar1.jpg"
//       }
//     ],
//     "pagination": {
//       "currentPage": 1,
//       "pageSize": 20,
//       "totalCount": 100,
//       "totalPages": 5,
//       "searchTerm": "User1"
//     }
//   }
// }
