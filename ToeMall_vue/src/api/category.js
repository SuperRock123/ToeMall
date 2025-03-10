import request from '@/utils/request'

/**
 * 添加分类（需要管理员权限）
 * @param {Object} category - 分类对象
 * @param {string} category.name - 分类名称
 * @param {string} category.description - 分类描述
 * @returns {Promise} - 返回包含创建的分类信息的响应
 */
export function createCategory (category) {
  return request.post('/Category/create', category)
}
// 响应示例
// {
//     "statusCode": 201,
//     "message": "分类创建成功",
//     "data": {
//       "categoryId": 1,
//       "name": "Category Name",
//       "description": "Category Description"
//     }
//   }
/**
 * 更新分类（需要管理员权限）
 * @param {number} id - 分类 ID
 * @param {Object} category - 分类对象
 * @param {string} category.name - 分类名称
 * @param {string} category.description - 分类描述
 * @returns {Promise} - 返回包含更新的分类信息的响应
 */
export function updateCategory (id, category) {
  return request.put(`/Category/update/${id}`, category)
}
// 响应示例
// {
//     "statusCode": 200,
//     "message": "分类更新成功",
//     "data": {
//       "categoryId": 1,
//       "name": "Updated Category Name",
//       "description": "Updated Category Description"
//     }
//   }
/**
 * 删除分类（需要管理员权限）
 * @param {number} id - 分类 ID
 * @returns {Promise} - 返回删除操作的响应
 */
export function deleteCategory (id) {
  return request.delete(`/Category/delete/${id}`)
}
// 响应示例
// {
//     "statusCode": 200,
//     "message": "分类删除成功",
//     "data": {
//       "deletedCategoryId": 1
//     }
//   }
/**
 * 按分类分页查询商品，无权限验证
 * @param {number} id - 分类 ID
 * @param {number} [page=1] - 页码，默认为 1
 * @param {number} [pageSize=20] - 每页记录数，默认为 20
 * @param {string} [sortBy='createdAt'] - 排序字段，支持 'createdAt', 'stock', 'category'
 * @param {boolean} [ascending=false] - 是否升序排序，默认为 false
 * @returns {Promise} - 返回包含商品列表和分页信息的响应
 */
export function getProductsByCategory (id, page = 1, pageSize = 20, sortBy = 'createdAt', ascending = false) {
  return request.get(`/Category/${id}/products`, {
    params: {
      page,
      pageSize,
      sortBy,
      ascending
    }
  })
}
// 响应示例
// {
//     "statusCode": 200,
//     "message": "获取分类商品列表成功",
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
//         "categoryId": 1,
//         "sortBy": "createdAt",
//         "ascending": false
//       }
//     }
//   }
/**
 * 返回所有分类数据，无权限验证
 * @returns {Promise} - 返回包含所有分类数据的响应
 */
export function getAllCategories () {
  return request.get('/Category/all')
}
// 响应示例
// {
//     "statusCode": 200,
//     "message": "获取所有分类成功",
//     "data": [
//       {
//         "categoryId": 1,
//         "name": "Category Name",
//         "description": "Category Description"
//       }
//     ]
//   }
