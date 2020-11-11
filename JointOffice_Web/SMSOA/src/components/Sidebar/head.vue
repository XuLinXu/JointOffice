<template>
  <div v-if="!isCollapse" class="head-img-wrap">
    <div v-if="isShow" style="width: 180px;">
      <span class="status-setting">设置</span>
    </div>
    <div class="head-img-wrapper fn-clear">
      <div class="img-wrap fn-clear">
        <img :src="avatar">
        <div class="img-wrap-profile">
          <p class="profile-username">{{ name }}</p>
          <p class="profile-status empty">
            <span class="status-desc" title=""><span v-for="role in roles" :key="role">{{ role }}</span></span>
          </p>
        </div>
      </div>
    </div>
  </div>
</template>
<script>
import { mapGetters } from 'vuex'
export default{
  name: 'Head',
  computed: {
    ...mapGetters([
      'sidebar',
      'name',
      'roles',
      'avatar',
      'tabname'
    ]),
    isCollapse() {
      return !this.sidebar.opened
    },
    isShow() {
      return this.tabname === 'work'
    }
  }
}
</script>
<style scoped>
  .head-img-wrap {
    position: relative;
    border-bottom: none;
    background-color: transparent;
    color: rgb(191, 203, 217);
    margin: 0 0 15px;
    -webkit-box-shadow: 0 0 2px rgba(254, 254, 254, 0.2);
    box-shadow: 0 0 2px rgba(252, 252, 252, 0.2);
    -webkit-border-radius: 2px;
    border-radius: 2px;
  }
  .head-img-wrapper {
    padding: 25px 10px 20px;
  }
  .head-img-wrap .img-wrap .img-wrap-profile {
    overflow: hidden;
    text-overflow: ellipsis;
    white-space: nowrap;
    cursor: default;
  }
  .head-img-wrap .img-wrap .img-wrap-profile .profile-username {
    display: inline-block;
    padding: 3px 0;
    line-height: 16px;
    font-size: 16px;
    color: #ffffff;
  }
  .head-img-wrap .img-wrap .img-wrap-profile .profile-status {
    font-size: 12px;
    color: rgb(191, 203, 217);
    line-height: 20px;
  }
  * {
    margin: 0;
    padding: 0;
  }
  .status-setting {
    font-size: 12px;
    float: right;
    color: rgba(184, 190, 255, 0.65);
    padding: 10px 10px 0px 0px;
    cursor: pointer;
    z-index: 1000;
  }
  .head-img-wrap .img-wrap img {
    width: 40px;
    height: 40px;
    margin-right: 15px;
    float: left;
    -webkit-border-radius: 5px;
    border-radius: 5px;
  }
</style>
