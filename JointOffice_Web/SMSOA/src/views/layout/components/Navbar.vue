<template>
  <el-header class="navbar">
    <el-row>
      <el-col :span="5">
        <el-tooltip :content="$t('navbar.breadcrumb')">
          <hamburger :toggle-click="toggleSideBar" :is-active="sidebar.opened" class="hamburger-container"/>
        </el-tooltip>
        <breadcrumb v-if="isShow" class="breadcrumb-container" />
      </el-col>
      <el-col :span="17">
        <Tab/>
      </el-col>
      <el-col :span="2">
        <div class="right-menu">
          <div>
            <!--最大化-->
            <el-tooltip v-if="isShow" :content="$t('navbar.screenfull')" effect="dark" placement="bottom">
              <screenfull class="screenfull right-menu-item"/>
            </el-tooltip>
          </div>
        </div>
      </el-col>
      <el-col :span="2">
        <div class="right-menu">
          <el-dropdown class="avatar-container" trigger="click">
            <!--头像-->
            <div class="avatar-wrapper">
              <img :src="avatar" class="user-avatar">
              <i class="el-icon-caret-bottom"/>
            </div>
            <!--下拉菜单-->
            <el-dropdown-menu slot="dropdown">
              <router-link to="/">
                <el-dropdown-item>
                  {{ $t('navbar.dashboard') }}
                </el-dropdown-item>
              </router-link>
              <router-link to="/">
                <el-dropdown-item>
                  {{ $t('navbar.userinfo') }}
                </el-dropdown-item>
              </router-link>
              <el-dropdown-item divided>
                <span style="display:block;" @click="logout">{{ $t('navbar.logOut') }}</span>
              </el-dropdown-item>
            </el-dropdown-menu>
          </el-dropdown>
        </div>
      </el-col>
    </el-row>
  </el-header>
</template>

<script>
import { mapGetters } from 'vuex'
import Breadcrumb from '@/components/Breadcrumb'
import Hamburger from '@/components/Hamburger'
import Screenfull from '@/components/Screenfull'
import Tab from './Tab'

export default {
  components: {
    Breadcrumb,
    Screenfull,
    Hamburger,
    Tab
  },
  computed: {
    ...mapGetters([
      'sidebar',
      'avatar',
      'device'
    ]),
    isShow() {
      return this.device === 'desktop'
    }
  },
  methods: {
    toggleSideBar() {
      this.$store.dispatch('ToggleSideBar')
    },
    logout() {
      this.$store.dispatch('LogOut').then(() => {
        location.reload() // 为了重新实例化vue-router对象 避免bug
      })
    }
  }
}
</script>

<style rel="stylesheet/scss" lang="scss" scoped>
  .el-header {
    padding: 0 5px !important;
  }
.navbar {
  background-color: white;
  line-height: 50px;
  border-radius: 0px !important;
  box-shadow: 0 1px 2px rgba(0,0,0,.2);
  .hamburger-container {
    line-height: 58px;
    height: 50px;
    float: left;
    padding: 0 10px;
  }
  .breadcrumb-container{
    float: left;
  }
  .right-menu {
      float: right;
      height: 100%;
    &:focus{
       outline: none;
     }
    .right-menu-item {
      display: inline-block;
      margin: 0 8px;
    }
    .screenfull {
      position: absolute;
      right: 90px;
      top: 16px;
      color: red;
    }
    .avatar-container {
      height: 50px;
      margin-right: 30px;
      .avatar-wrapper {
        cursor: pointer;
        margin-top: 5px;
        position: relative;
        .user-avatar {
          width: 40px;
          height: 40px;
          border-radius: 10px;
        }
        .el-icon-caret-bottom {
          position: absolute;
          right: -20px;
          top: 25px;
          font-size: 12px;
        }
      }
    }
  }
}
</style>

