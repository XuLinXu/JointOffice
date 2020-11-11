<template>
  <section :style="isWidth(tabname)" class="app-main">
    <transition name="fade-transform" mode="out-in">
      <router-view/>
    </transition>
  </section>
</template>

<script>
import { mapGetters } from 'vuex'
export default {
  name: 'AppMain',
  computed: {
    ...mapGetters([
      'tabname'
    ])
    // key() {
    //   return this.$route.name !== undefined ? this.$route.name + +new Date() : this.$route + +new Date()
    // }
  },
  methods: {
    isWidth(type) {
      if (type === 'work') {
        return 'width: 70%;'
      } else {
        return 'width: 100%;'
      }
    }
  }
}
</script>

<style scoped>
.app-main {
  /*50 = navbar  */
  min-height: calc(100vh - 50px);
  min-width: 600px;
  width: 70%;
  padding: 0 10px 0 10px;
}
</style>
