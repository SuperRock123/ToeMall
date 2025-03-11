import request from '@/utils/request.js'

// 获取商品列表，支持分页和排序
export function getProducts (page = 1, pageSize = 20, categoryId = null, sortBy = 'createdAt', ascending = false) {
  return request.get('/Products/list', {
    params: {
      page,
      pageSize,
      categoryId,
      sortBy,
      ascending
    }
  })
}
// 响应示例
// {
//     "statusCode": 200,
//     "message": "获取商品列表成功",
//     "data": {
//       "products": [
//         {
//           "productId": 1,
//           "name": "Product Name",
//           "description": "Product Description",
//           "price": 100.00,
//           "stockQuantity": 10,
//           "picture": "http://example.com/image.jpg",
//           "category": {
//             "categoryId": 1,
//             "name": "Category Name"
//           },
//           "createdAt": "2025-03-08T00:00:00Z",
//           "updatedAt": "2025-03-08T00:00:00Z"
//         }
//       ],
//       "pagination": {
//         "currentPage": 1,
//         "pageSize": 20,
//         "totalCount": 100,
//         "totalPages": 5,
//         "categoryId": null,
//         "sortBy": "createdAt",
//         "ascending": false
//       }
//     }
//   }
// 搜索商品，支持分页和排序
export function searchProducts (keyword, page = 1, pageSize = 20, categoryId = null, sortBy = 'createdAt', ascending = false) {
  return request.get('/Products/search', {
    params: {
      keyword,
      page,
      pageSize,
      categoryId,
      sortBy,
      ascending
    }
  })
}
// 响应示例
// {
//     "statusCode": 200,
//     "message": "搜索商品成功",
//     "data": {
//       "products": [
//         {
//           "productId": 1,
//           "name": "Product Name",
//           "description": "Product Description",
//           "price": 100.00,
//           "stockQuantity": 10,
//           "picture": "http://example.com/image.jpg",
//           "category": {
//             "categoryId": 1,
//             "name": "Category Name"
//           },
//           "createdAt": "2025-03-08T00:00:00Z",
//           "updatedAt": "2025-03-08T00:00:00Z"
//         }
//       ],
//       "pagination": {
//         "currentPage": 1,
//         "pageSize": 20,
//         "totalCount": 100,
//         "totalPages": 5,
//         "keyword": "search keyword",
//         "categoryId": null,
//         "sortBy": "createdAt",
//         "ascending": false
//       }
//     }
//   }

// 获取商品详情
export function getProductDetail (productId) {
  return request.get(`/Products/detail/${productId}`)
}
// 响应示例
// {
//     "statusCode": 200,
//     "message": "获取商品详情成功",
//     "data": {
//       "productId": 1,
//       "name": "Product Name",
//       "description": "Product Description",
//       "price": 100.00,
//       "stockQuantity": 10,
//       "picture": "http://example.com/image.jpg",
//       "category": {
//         "categoryId": 1,
//         "name": "Category Name",
//         "description": "Category Description"
//       },
//       "createdAt": "2025-03-08T00:00:00Z",
//       "updatedAt": "2025-03-08T00:00:00Z"
//     }
//   }
// 创建商品（需要管理员权限）
export function createProduct (name, price, categoryId, stockQuantity = 0, picture = ' ', description = ' ') {
  return request.post('/Products/create', {
    name,
    description,
    price,
    stockQuantity,
    categoryId,
    picture
  })
}
// 响应示例
// {
//     "statusCode": 201,
//     "message": "商品创建成功",
//     "data": {
//       "productId": 1,
//       "name": "Product Name",
//       "description": "Product Description",
//       "price": 100.00,
//       "stockQuantity": 10,
//       "picture": "http://example.com/image.jpg",
//       "category": {
//         "categoryId": 1,
//         "name": "Category Name"
//       },
//       "createdAt": "2025-03-08T00:00:00Z",
//       "updatedAt": "2025-03-08T00:00:00Z"
//     }
//   }
// 更新商品（需要管理员权限）
export function updateProduct (productId, name, price, categoryId, stockQuantity = 0, picture = ' ', description = ' ') {
  return request.put(`/Products/update/${productId}`, {
    name,
    description,
    price,
    stockQuantity,
    categoryId,
    picture
  })
}
// 响应示例
// {
//     "statusCode": 200,
//     "message": "商品更新成功",
//     "data": {
//       "productId": 1,
//       "name": "Product Name",
//       "description": "Product Description",
//       "price": 100.00,
//       "stockQuantity": 10,
//       "picture": "http://example.com/image.jpg",
//       "category": {
//         "categoryId": 1,
//         "name": "Category Name"
//       },
//       "createdAt": "2025-03-08T00:00:00Z",
//       "updatedAt": "2025-03-08T00:00:00Z"
//     }
//   }
// 删除商品（需要管理员权限）
export function deleteProduct (productId) {
  return request.delete(`/Products/delete/${productId}`)
}
// 响应示例
// {
//     "statusCode": 200,
//     "message": "商品删除成功",
//     "data": {
//       "deletedProductId": 1
//     }
//   }
