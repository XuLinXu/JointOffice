<template>
  <div v-if="tabname === 'work'" class="main-layout-side-wrapper">
    <div>
      左侧浮动框
    </div>
  </div>
</template>
<script type="text/ecmascript-6">
import { mapGetters } from 'vuex'
export default {
  name: 'Right',
  computed: {
    ...mapGetters([
      'tabname'
    ])
  }
}
</script>
<style rel="stylesheet/scss" lang="scss" scoped>
  .main-layout-side-wrapper {
    top: 0px !important;
    min-width: 300px;
    width: 28%;
    float: right;
    right: 10px;
    position: absolute;
  }

  .main-layout-side-wrapper div {
    background: white;
    -webkit-box-shadow: 0 0 2px rgba(0,0,0,.2);
    box-shadow: 0 0 2px rgba(0,0,0,.2);
    -webkit-border-radius: 2px;
    border-radius: 2px;
  }

</style>
