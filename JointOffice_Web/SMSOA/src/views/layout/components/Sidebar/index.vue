<template>
  <el-scrollbar wrap-class="scrollbar-wrapper">
    <el-menu
      :show-timeout="200"
      :default-active="$route.path"
      :collapse="isCollapse"
      mode="vertical"
      background-color="#304156"
      text-color="#bfcbd9"
      active-text-color="#409EFF"
    >
      <Head/>
      <sidebar-item v-for="route in routes" :key="route.name" :item="route" :base-path="route.path"/>
    </el-menu>
  </el-scrollbar>
</template>

<script>
import { mapGetters } from 'vuex'
import Head from '@/components/Sidebar/head'
import SidebarItem from './SidebarItem'

export default {
  components: { SidebarItem, Head },
  computed: {
    ...mapGetters([
      'sidebar'
    ]),
    routes() {
      return this.$router.options.routes
    },
    isCollapse() {
      return !this.sidebar.opened
    }
  }
}
</script>

<style scoped>
  #app .sidebar-container {
    top: 75px !important;
    -webkit-box-shadow: 0 0 2px rgba(0,0,0,.2);
    box-shadow: 0 0 2px rgba(0,0,0,.2);
    -webkit-border-radius: 2px;
    border-radius: 2px;
  }
</style>
