<template>
    <el-table :data="order.products" style="width: 100%">
      <el-table-column prop="productName" label="商品名称"></el-table-column>
      <el-table-column prop="quantity" label="数量"></el-table-column>
      <el-table-column prop="price" label="单价"></el-table-column>
      <el-table-column prop="totalPrice" label="总价"></el-table-column>
    </el-table>
  </template>

<script>
export default {
  props: {
    order: {
      type: Object,
      default: () => ({})
    }
  }
}
</script>
