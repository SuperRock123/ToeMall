<!-- filepath: d:\jobApplications\ToeMallDemo\toemall-dev\src\components\BackToTop.vue -->
<template>
    <transition name="fade">
      <div v-show="visible" class="back-to-top" @click="scrollToTop">
        <el-tooltip content="回到顶部" placement="left">
          <el-button type="primary" icon="el-icon-arrow-up" circle></el-button>
        </el-tooltip>
      </div>
    </transition>
  </template>

<script>
export default {
  data () {
    return {
      visible: false
    }
  },
  methods: {
    handleScroll () {
      this.visible = window.scrollY > 200
    },
    scrollToTop () {
      window.scrollTo({
        top: 0,
        behavior: 'smooth'
      })
    }
  },
  mounted () {
    window.addEventListener('scroll', this.handleScroll)
  },
  beforeDestroy () {
    window.removeEventListener('scroll', this.handleScroll)
  }
}
</script>

  <style scoped>
  .back-to-top {
    position: fixed;
    bottom: 50px;
    right: 30px;
    z-index: 1000;
  }

  .fade-enter-active, .fade-leave-active {
    transition: opacity 0.5s;
  }

  .fade-enter, .fade-leave-to /* .fade-leave-active in <2.1.8 */ {
    opacity: 0;
  }
  </style>
