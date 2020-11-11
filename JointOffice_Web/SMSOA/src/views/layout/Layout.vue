<template>
  <div>
    <div>
      <navbar class="isFixed navbar-container"/>
    </div>
    <div :class="classObj" class="app-wrapper">
      <div v-if="device==='mobile'&&sidebar.opened" class="drawer-bg" @click="handleClickOutside"/>
      <div>
        <div class="main-container">
          <sidebar class="sidebar-container"/>
          <app-main/>
          <Right/>
          <!--可自定义按钮的样式、show/hide临界点、返回的位置  -->
          <!--如需文字提示，可在外部添加element的<el-tooltip></el-tooltip>元素  -->
          <el-tooltip placement="top" content="回到顶部">
            <back-to-top :custom-style="myBackToTopStyle" :visibility-height="30" :back-position="0" transition-name="fade"/>
          </el-tooltip>
        </div>
      </div>
    </div>
  </div>
</template>

<script>
import { Navbar, Sidebar, AppMain } from './components'
import ResizeMixin from './mixin/ResizeHandler'
import BackToTop from '@/components/BackToTop'
import Right from './components/Right'
export default {
  name: 'Layout',
  components: {
    Navbar,
    Sidebar,
    AppMain,
    BackToTop,
    Right
  },
  mixins: [ResizeMixin],
  data() {
    return {
      searchBarFixed: false,
      myBackToTopStyle: {
        right: '50px',
        bottom: '50px',
        width: '40px',
        height: '40px',
        'border-radius': '4px',
        'line-height': '45px', // 请保持与高度一致以垂直居中 Please keep consistent with height to center vertically
        background: '#e7eaf1'// 按钮的背景颜色 The background color of the button
      }
    }
  },
  computed: {
    sidebar() {
      return this.$store.state.app.sidebar
    },
    device() {
      return this.$store.state.app.device
    },
    classObj() {
      return {
        hideSidebar: !this.sidebar.opened,
        openSidebar: this.sidebar.opened,
        withoutAnimation: this.sidebar.withoutAnimation,
        mobile: this.device === 'mobile'
      }
    }
  },

  destroyed() {
    window.removeEventListener('scroll', this.handleScroll)
  },
  methods: {
    handleClickOutside() {
      this.$store.dispatch('CloseSideBar', { withoutAnimation: false })
    }
  }
}
</script>

<style rel="stylesheet/scss" lang="scss" scoped>
  @import "src/styles/mixin.scss";
  #app .isFixed{
    position: fixed;
    top: 0px;
    z-index:999;
    width:100%;
  }
  .app-wrapper {
    @include clearfix;
    position: relative;
    width: 100%;
    top: 75px;
    &.mobile.openSidebar{
      position: fixed;
      top: 0;
    }
  }
  .drawer-bg {
    background: #000;
    opacity: 0.3;
    width: 100%;
    top: 0;
    height: 100%;
    position: absolute;
    z-index: 999;
  }
</style>
