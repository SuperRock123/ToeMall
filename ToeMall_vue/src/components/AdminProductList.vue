<template>
  <div>
    <el-row>
      <el-col :span="12">
        <el-input v-model="searchKeyword" placeholder="搜索商品"></el-input>
      </el-col>
      <el-col :span="12" class="text-right">
        <el-button type="primary" @click="searchProducts">搜索</el-button>
        <el-button type="primary" @click="showCreateProductDialog">添加商品</el-button>
      </el-col>
    </el-row>
    <el-row>
      <el-col :span="8">
        <el-select v-model="sortBy" placeholder="排序字段">
          <el-option label="价格" value="price"></el-option>
          <el-option label="库存" value="stock"></el-option>
          <el-option label="创建时间" value="createdAt"></el-option>
        </el-select>
      </el-col>
      <el-col :span="8">
        <el-select v-model="ascending" placeholder="排序方式">
          <el-option label="升序" :value="true"></el-option>
          <el-option label="降序" :value="false"></el-option>
        </el-select>
      </el-col>
    </el-row>
    <el-table :data="products" style="width: 100%">
      <el-table-column prop="name" label="商品名称"></el-table-column>
      <el-table-column prop="price" label="价格"></el-table-column>
      <el-table-column prop="stock" label="库存"></el-table-column>
      <el-table-column label="操作">
        <template slot-scope="scope">
          <el-button size="mini" @click="editProduct(scope.row)">编辑</el-button>
          <el-button size="mini" type="danger" @click="confirmDeleteProduct(scope.row)">删除</el-button>
        </template>
      </el-table-column>
    </el-table>
    <el-pagination
      background
      layout="prev, pager, next"
      :total="totalProducts"
      :page-size="pageSize"
      @current-change="handlePageChange"
    ></el-pagination>
    <el-dialog :visible.sync="productDialogVisible" title="商品信息">
      <product-form :product="selectedProduct" :categories="categories" @save="saveProduct"></product-form>
    </el-dialog>
    <el-dialog
      title="确认删除"
      :visible.sync="deleteDialogVisible"
      width="30%"
      @close="deleteDialogVisible = false"
    >
      <span>确定要删除这个商品吗？</span>
      <span slot="footer" class="dialog-footer">
        <el-button @click="deleteDialogVisible = false">取消</el-button>
        <el-button type="primary" @click="deleteProduct">确定</el-button>
      </span>
    </el-dialog>
  </div>
</template>

<script>
import { getProducts, searchProducts, createProduct, updateProduct, deleteProduct } from '@/api/products'
import { getAllCategories } from '@/api/category'
import ProductForm from '@/components/ProductForm.vue'

export default {
  components: { ProductForm },
  data () {
    return {
      products: [],
      categories: [],
      searchKeyword: '',
      sortBy: 'createdAt',
      ascending: false,
      productDialogVisible: false,
      deleteDialogVisible: false,
      selectedProduct: null,
      productToDelete: null,
      currentPage: 1,
      pageSize: 20,
      totalProducts: 0
    }
  },
  methods: {
    fetchProducts () {
      console.log('fetchProducts', this.currentPage, this.pageSize, this.sortBy, this.ascending)
      getProducts(this.currentPage, this.pageSize, null, this.sortBy, this.ascending).then(response => {
        this.products = response.data.products
        this.totalProducts = response.data.pagination.totalCount
      })
    },
    searchProducts () {
      console.log('searchProducts', this.searchKeyword, this.currentPage, this.pageSize, this.sortBy, this.ascending)
      if (this.searchKeyword === '') {
        this.fetchProducts()
        return
      }
      searchProducts(this.searchKeyword, this.currentPage, this.pageSize, null, this.sortBy, this.ascending).then(response => {
        console.log('searchProducts', response)
        this.products = response.data.products
        this.totalProducts = response.data.pagination.totalCount
      })
    },
    fetchCategories () {
      getAllCategories().then(response => {
        this.categories = response.data
      })
    },
    showCreateProductDialog () {
      this.selectedProduct = null
      this.productDialogVisible = true
    },
    editProduct (product) {
      this.selectedProduct = product
      this.productDialogVisible = true
    },
    saveProduct (product) {
      if (product.productId) {
        updateProduct(product.productId, product.name, product.price, product.categoryId, product.stockQuantity, product.picture, product.description).then(this.fetchProducts)
      } else {
        createProduct(product.name, product.price, product.categoryId, product.stockQuantity, product.picture, product.description).then(this.fetchProducts)
      }
      this.productDialogVisible = false
    },
    confirmDeleteProduct (product) {
      this.productToDelete = product
      this.deleteDialogVisible = true
    },
    deleteProduct () {
      deleteProduct(this.productToDelete.productId).then(() => {
        this.fetchProducts()
        this.deleteDialogVisible = false
      })
    },
    handlePageChange (page) {
      this.currentPage = page
      this.fetchProducts()
    }
  },
  mounted () {
    this.fetchProducts()
    this.fetchCategories()
  }
}
</script>
