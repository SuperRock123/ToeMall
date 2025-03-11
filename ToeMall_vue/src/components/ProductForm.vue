<template>
  <el-form :model="localProduct" :rules="rules" ref="productForm" label-width="120px">
    <el-form-item label="商品名称" prop="name">
      <el-input v-model="localProduct.name"></el-input>
    </el-form-item>
    <el-form-item label="描述" prop="description">
      <el-input v-model="localProduct.description"></el-input>
    </el-form-item>
    <el-form-item label="价格" prop="price">
      <el-input v-model.number="localProduct.price" type="number"></el-input>
    </el-form-item>
    <el-form-item label="库存" prop="stockQuantity">
      <el-input v-model.number="localProduct.stockQuantity" type="number"></el-input>
    </el-form-item>
    <el-form-item label="分类" prop="categoryId">
      <el-select v-model="localProduct.categoryId">
        <el-option
          v-for="category in categories"
          :key="category.categoryId"
          :label="category.name"
          :value="category.categoryId"
        ></el-option>
      </el-select>
    </el-form-item>
    <el-form-item label="图片" prop="picture">
      <el-input v-model="localProduct.picture"></el-input>
    </el-form-item>
    <el-form-item>
      <el-button type="primary" @click="onSubmit">保存</el-button>
    </el-form-item>
  </el-form>
</template>

<script>
export default {
  props: {
    product: {
      type: Object,
      default: () => ({})
    },
    categories: {
      type: Array,
      default: () => []
    }
  },
  data () {
    return {
      localProduct: { ...this.product },
      rules: {
        name: [
          { required: true, message: '请输入商品名称', trigger: 'blur' }
        ],
        description: [
          { required: false, message: '请输入商品描述', trigger: 'blur' }
        ],
        price: [
          { required: true, message: '请输入商品价格', trigger: 'blur' },
          { type: 'number', message: '价格必须为数字值', trigger: 'blur' }
        ],
        stockQuantity: [
          { required: false, message: '请输入库存数量', trigger: 'blur' },
          { type: 'number', message: '库存数量必须为数字值', trigger: 'blur' }
        ],
        categoryId: [
          { required: true, message: '请选择分类', trigger: 'change' }
        ],
        picture: [
          { required: true, message: '请输入图片链接', trigger: 'blur' }
        ]
      }
    }
  },
  methods: {
    onSubmit () {
      this.$refs.productForm.validate((valid) => {
        if (valid) {
          this.$emit('save', this.localProduct)
        } else {
          console.log('error submit!!')
          return false
        }
      })
    }
  },
  watch: {
    product: {
      handler (newProduct) {
        this.localProduct = { ...newProduct }
      },
      deep: true,
      immediate: true
    }
  }
}
</script>
