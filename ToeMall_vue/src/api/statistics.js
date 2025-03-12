import request from '@/utils/request'

// 获取用户统计信息
// @returns {Promise} - 返回包含用户统计信息的响应
export function getUserStatistics () {
  return request.get('/Statistics/users')
}

// 响应示例
// {
//     "statusCode": 200,
//     "message": "获取用户统计信息成功",
//     "data": {
//       "totalUsers": 1000,
//       "activeUsers": 800,
//       "newUsersToday": 10
//     }
//   }

// 获取产品统计信息
// @returns {Promise} - 返回包含产品统计信息的响应
export function getProductStatistics () {
  return request.get('/Statistics/products')
}

// 响应示例
// {
//     "statusCode": 200,
//     "message": "获取产品统计信息成功",
//     "data": {
//       "totalProducts": 500,
//       "lowStockProducts": 50,
//       "categoriesCount": 20
//     }
//   }

// 获取订单统计信息
// @returns {Promise} - 返回包含订单统计信息的响应
export function getOrderStatistics () {
  return request.get('/Statistics/orders')
}

// 响应示例
// {
//     "statusCode": 200,
//     "message": "获取订单统计信息成功",
//     "data": {
//       "totalOrders": 100,
//       "totalRevenue": 50000.00,
//       "pendingOrders": 10,
//       "todayOrders": 5,
//       "averageOrderValue": 500.00,
//        近一周的订单情况
//       "dailyStats": [
//         {
//           "date": "2025-03-08T00:00:00Z",
//           "generatedOrdersCount": 10,
//           "completedOrdersCount": 8,
//           "totalRevenue": 4000.00
//         },
//         {
//           "date": "2025-03-07T00:00:00Z",
//           "generatedOrdersCount": 12,
//           "completedOrdersCount": 10,
//           "totalRevenue": 5000.00
//         }
//         // 其他日期的统计数据...
//       ]
//     }
//   }

// 获取综合仪表盘统计信息
// @returns {Promise} - 返回包含仪表盘统计信息的响应
export function getDashboardStatistics () {
  return request.get('/Statistics/dashboard')
}

// 响应示例
// {
//     "statusCode": 200,
//     "message": "获取仪表盘统计信息成功",
//     "data": {
//       "totalUsers": 1000,
//       "totalOrders": 200,
//       "totalRevenue": 100000.00,
//       "totalProducts": 500,
//       "recentOrders": [
//         {
//           "orderId": 1,
//           "productName": "Product 1",
//           "totalPrice": 100.00,
//           "orderStatus": "Paid",
//           "createdAt": "2025-03-08T00:00:00Z"
//         },
//         {
//           "orderId": 2,
//           "productName": "Product 2",
//           "totalPrice": 200.00,
//           "orderStatus": "Paid",
//           "createdAt": "2025-03-07T00:00:00Z"
//         }
//         // 其他最近订单的数据...
//       ]
//     }
//   }
