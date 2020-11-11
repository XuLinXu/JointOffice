<template>
  <div class="app-container tab-menu">
    <el-tabs v-model="activeName" @tab-click="handleClick">
      <el-tab-pane name="work" label="工作">工作</el-tab-pane>
      <el-tab-pane name="contact" label="联系人">联系人</el-tab-pane>
      <el-tab-pane name="apply" label="应用">应用</el-tab-pane>
    </el-tabs>
  </div>
</template>
<script>
import store from '@/store'
export default {
  data() {
    return {
      activeName: 'work'
    }
  },
  methods: {
    handleClick(tab, e) {
      store.dispatch('SETTABNAME', tab.name)
      let path = '/404'
      if (tab.name === 'work') {
        path = '/dashboard'
      } else if (tab.name === 'contact') {
        path = '/dashboard'
      } else if (tab.name === 'apply') {
        path = '/myapply'
      }
      this.$router.push(path)
    }
  }
}
</script>

<style>
  .navbar .el-tabs__nav-wrap::after {
    background-color: inherit !important;
  }
  .navbar .tab-menu {
    padding: 10px !important;
    float: left;
  }
  .navbar .el-tab-pane {
    visibility: hidden;
    height: 0px;
  }
  .navbar .el-tabs__item.is-active {
    font-weight: bold;
  }
  .navbar .el-tabs__item {
    font-size: 18px;
  }
</style>
