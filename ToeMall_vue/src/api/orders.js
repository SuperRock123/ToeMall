import request from '@/utils/request'

/**
 * 获取订单列表，支持分页和排序
 * @param {number} [page=1] - 页码，默认为 1
 * @param {number} [pageSize=20] - 每页记录数，默认为 20
 * @param {string} [status=null] - 订单状态，默认为 null
 * @param {string} [sortBy='createdAt'] - 排序字段，支持 'createdAt', 'status', 'total'
 * @param {boolean} [ascending=false] - 是否升序排序，默认为 false
 * @returns {Promise} - 返回包含订单列表和分页信息的响应
 */
export function getOrders (page = 1, pageSize = 20, status = null, sortBy = 'createdAt', ascending = false) {
  return request.get('/Orders/list', {
    params: {
      page,
      pageSize,
      status,
      sortBy,
      ascending
    }
  })
}
// {
//   "statusCode": 200,
//   "message": "获取订单列表成功",
//   "data": {
//     "orders": [
//       {
//         "orderId": 1,
//         "orderStatus": "Unpaid",
//         "productName": "Product Name",
//         "price": 100.00,
//         "totalPrice": 200.00,
//         "product": {
//           "productId": 1,
//           "name": "Product Name",
//           "price": 100.00
//         },
//         "user": {
//           "userId": 1,
//           "username": "User Name"
//         },
//         "quantity": 2,
//         "createdAt": "2025-03-08T00:00:00Z",
//         "updatedAt": "2025-03-08T00:00:00Z"
//       }
//     ],
//     "pagination": {
//       "currentPage": 1,
//       "pageSize": 20,
//       "totalCount": 100,
//       "totalPages": 5,
//       "status": null,
//       "sortBy": "createdAt",
//       "ascending": false
//     }
//   }
// }
/**
 * 获取订单详情
 * @param {number} orderId - 订单 ID
 * @returns {Promise} - 返回包含订单详情的响应
 */
export function getOrderDetail (orderId) {
  return request.get(`/Orders/detail/${orderId}`)
}
// {
//   "statusCode": 200,
//   "message": "获取订单详情成功",
//   "data": {
//     "orderId": 1,
//     "orderStatus": "Unpaid",
//     "productName": "Product Name",
//     "price": 100.00,
//     "totalPrice": 200.00,
//     "product": {
//       "productId": 1,
//       "name": "Product Name",
//       "price": 100.00,
//       "description": "Product Description",
//       "picture": "http://example.com/image.jpg"
//     },
//     "user": {
//       "userId": 1,
//       "username": "User Name"
//     },
//     "quantity": 2,
//     "createdAt": "2025-03-08T00:00:00Z",
//     "updatedAt": "2025-03-08T00:00:00Z"
//   }
// }
/**
 * 创建订单
 * @param {number} productId - 商品 ID
 * @param {number} quantity - 购买数量
 * @returns {Promise} - 返回包含创建的订单信息的响应
 */
export function createOrder (productId, quantity) {
  return request.post('/Orders/create', {
    productId,
    quantity
  })
}
// {
//   "statusCode": 200,
//   "message": "订单创建成功",
//   "data": {
//     "orderId": 1,
//     "orderStatus": "Unpaid",
//     "productName": "Product Name",
//     "price": 100.00,
//     "totalPrice": 200.00,
//     "product": {
//       "productId": 1,
//       "name": "Product Name",
//       "price": 100.00
//     },
//     "user": {
//       "userId": 1,
//       "username": "User Name"
//     },
//     "quantity": 2,
//     "createdAt": "2025-03-08T00:00:00Z",
//     "updatedAt": "2025-03-08T00:00:00Z"
//   }
// }
/**
 * 支付订单
 * @param {number} orderId - 订单 ID
 * @returns {Promise} - 返回包含更新后的订单信息的响应
 */
export function payOrder (orderId) {
  return request.put(`/Orders/status/${orderId}`, { status: 'Paid' })
}
// 响应示例
// {
//   "statusCode": 200,
//   "message": "订单状态更新成功",
//   "data": {
//     "orderId": 1,
//     "orderStatus": "Paid",
//     "productName": "Product Name",
//     "price": 100.00,
//     "totalPrice": 200.00,
//     "product": {
//       "productId": 1,
//       "name": "Product Name",
//       "price": 100.00
//     },
//     "user": {
//       "userId": 1,
//       "username": "User Name"
//     },
//     "quantity": 2,
//     "createdAt": "2025-03-08T00:00:00Z",
//     "updatedAt": "2025-03-08T00:00:00Z"
//   }
// }
/**
 * 取消订单
 * @param {number} orderId - 订单 ID
 * @returns {Promise} - 返回包含更新后的订单信息的响应
 */
export function cancelOrder (orderId) {
  return request.put(`/Orders/status/${orderId}`, { status: 'Cancelled' })
}
// 响应示例
// {
//   "statusCode": 200,
//   "message": "订单状态更新成功",
//   "data": {
//     "orderId": 1,
//     "orderStatus": "Cancelled",
//     "productName": "Product Name",
//     "price": 100.00,
//     "totalPrice": 200.00,
//     "product": {
//       "productId": 1,
//       "name": "Product Name",
//       "price": 100.00
//     },
//     "user": {
//       "userId": 1,
//       "username": "User Name"
//     },
//     "quantity": 2,
//     "createdAt": "2025-03-08T00:00:00Z",
//     "updatedAt": "2025-03-08T00:00:00Z"
//   }
// }
/**
 * 更新订单状态
 * @param {number} orderId - 订单 ID
 * @param {string} status - 新的订单状态
 * @returns {Promise} - 返回包含更新后的订单信息的响应
 */
export function updateOrderStatus (orderId, status) {
  return request.put(`/Orders/status/${orderId}`, {
    status
  })
}
// {
//   "statusCode": 200,
//   "message": "订单状态更新成功",
//   "data": {
//     "orderId": 1,
//     "orderStatus": "Paid",
//     "productName": "Product Name",
//     "price": 100.00,
//     "totalPrice": 200.00,
//     "product": {
//       "productId": 1,
//       "name": "Product Name",
//       "price": 100.00
//     },
//     "user": {
//       "userId": 1,
//       "username": "User Name"
//     },
//     "quantity": 2,
//     "createdAt": "2025-03-08T00:00:00Z",
//     "updatedAt": "2025-03-08T00:00:00Z"
//   }
// }
/**
 * 搜索订单
 *
 * @param {string} [productName] - 可选，商品名称关键词
 * @param {string} [status] - 可选，订单状态 'Unpaid', 'Paid', 'Cancelled', 不传参数表示搜索所有状态
 * @param {number} [page=1] - 可选，页码，默认为 1
 * @param {number} [pageSize=20] - 可选，每页记录数，默认为 20
 * @returns {Promise} - 返回包含搜索结果的响应
 */
export function searchOrders (productName, status, page = 1, pageSize = 20) {
  return request.get('/Orders/search', {
    params: {
      productName,
      status,
      page,
      pageSize
    }
  })
}

// 示例响应
// {
//   "statusCode": 200,
//   "message": "搜索订单成功",
//   "data": {
//     "orders": [
//       {
//         "orderId": 1,
//         "orderStatus": "Unpaid",
//         "productName": "Product Name",
//         "price": 100.00,
//         "totalPrice": 200.00,
//         "product": {
//           "productId": 1,
//           "name": "Product Name",
//           "price": 100.00
//         },
//         "user": {
//           "userId": 1,
//           "username": "User Name"
//         },
//         "quantity": 2,
//         "createdAt": "2025-03-08T00:00:00Z",
//         "updatedAt": "2025-03-08T00:00:00Z"
//       }
//     ],
//     "pagination": {
//       "currentPage": 1,
//       "pageSize": 20,
//       "totalCount": 100,
//       "totalPages": 5,
//       "productName": "Product Name",
//       "status": "Unpaid"
//     }
//   }
// }
