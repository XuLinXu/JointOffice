<template>
  <div class="work-content">
    <div class="app-tab tab-menu el-card" style="margin-bottom: 13px;">
      <el-tabs v-model="activeName">
        <el-tab-pane v-for="item in tabList" :loading="loading" :label="item.name" :key="item.type" :name="item.name">
          <keep-alive>
            <tab-pane v-if="activeName==item.name" :type_index="String(item.type)" @search="getSearchWorkList"/>
          </keep-alive>
        </el-tab-pane>
      </el-tabs>
    </div>
    <div v-loading="loading">
      <!-- // 3-->
      <el-card v-for="(work,index) in workList" v-if="work.type === '3'" :key="index" :index="index" class="box-card box-card-item">
        <div slot="header">
          <el-row>
            <el-col :span="16">
              <div class="head-img-wrap">
                <div class="head-img-wrapper fn-clear">
                  <div class="img-wrap fn-clear">
                    <img :src="work.workGetIPublishTaskList[0].picture">
                    <div class="img-wrap-profile">
                      <p class="profile-username">{{ work.workGetIPublishTaskList[0].name }}</p>
                      <p class="profile-status empty">
                        <span class="status-desc" title="">
                          {{ work.workGetIPublishTaskList[0].createDate }} - {{ work.workGetIPublishTaskList[0].range }} - {{ work.workGetIPublishTaskList[0].phoneModel }}
                        </span>
                      </p>
                    </div>
                  </div>
                </div>
              </div>
            </el-col>
            <el-col :span="7">
              <div class="head-status">
                <p class="project-name">
                  <span>{{ work.type }}</span>
                  <span v-if="work.workGetIPublishTaskList[0].state !== null && work.workGetIPublishTaskList[0].state !== ''"> - {{ work.workGetIPublishTaskList[0].state }}</span>
                </p>
                <div>
                  <p class="status-name">
                    <el-progress v-if="work.workGetIPublishTaskList[0].isExe === 0" :text-inside="true" :percentage="0" :show-text="false"/>
                    <el-progress v-if="work.workGetIPublishTaskList[0].isExe !== 0" :text-inside="true" :percentage="work.workGetIPublishTaskList[0].isExe/work.workGetIPublishTaskList[0].isExecutor * 100" :show-text="false"/>
                    <span style="float: right;">{{ work.workGetIPublishTaskList[0].executorNum }}</span>
                  </p>
                </div>
              </div>
            </el-col>
          </el-row>
        </div>
        <span style="line-height:36px;font-size:17px;color:#666;">
          <i style="color:#20a0ff; font-style: normal;"> {{ work.workGetIPublishTaskList[0].name }}</i> 月度目标
        </span>
      </el-card>
    </div>
  </div>
</template>
<script>
import tabPane from './../tabPane'
import { mapGetters } from 'vuex'
import { getWorkList } from '@/api/work'
export default {
  components: { tabPane },
  data() {
    return {
      activeName: '全部',
      tabList: null,
      loading: false,
      workList: null
    }
  },
  computed: {
    ...mapGetters([
      'userTabs'
    ])
  },
  created() {
    this.getTabList()
  },
  methods: {
    getTabList() {
      if (this.userTabs === null) {
        this.loading = true
        this.$store.dispatch('GetTabList').then(res => {
          this.loading = false
          this.tabList = this.userTabs
        }).catch(() => {
          this.loading = false
        })
      } else {
        this.tabList = this.userTabs
      }
    },
    getSearchWorkList(search) {
      this.loading = true
      getWorkList(search).then(response => {
        this.loading = false
        this.workList = response.workGetAllList
      }).catch(err => {
        this.loading = false
        this.$message({
          message: err,
          type: 'error'
        })
      })
    }
  }
}
</script>
<style rel="stylesheet/scss" lang="scss" scoped>
  .app-tab{
    padding: 10px 10px 0px 10px;
  }
  .box-card .tab-menu {
    padding: 10px !important;
    float: left;
    align-content: center;
  }
  .box-card .el-tabs__nav-wrap::after {
    background-color: inherit !important;
  }
  .dashboard-container .el-card {
    -webkit-box-shadow: 0 2px 12px 0 rgba(0,0,0,.1);
    box-shadow: 0 2px 12px 0 rgba(0,0,0,.1);
  }
  .dashboard-container.el-card {
    border: 1px solid #ebeef5;
    background-color: #fff;
    color: #303133;
    -webkit-transition: .3s;
    transition: .3s;
  }
  .dashboard-container.el-card, .el-message {
    border-radius: 4px;
    overflow: hidden;
  }
  .dashboard-container .box-card-item {
    margin-top: 5px;
  }

  .work-content .img-wrap-profile {
    overflow: hidden;
    text-overflow: ellipsis;
    white-space: nowrap;
    cursor: default;
    line-height: 20px;
  }
  .work-content .img-wrap-profile .profile-username {
    font-size: 14px;
    color: #001528;
  }
  .work-content .img-wrap-profile .status-desc {
    font-size: 12px;
    color: rgb(191, 203, 217);
    cursor: pointer;
  }
  * {
    margin: 0;
    padding: 0;
  }
  .work-content .img-wrap img {
    width: 40px;
    height: 40px;
    margin-right: 15px;
    float: left;
    -webkit-border-radius: 5px;
    border-radius: 25px;
  }
  .work-content .box-card-item .el-card__header {
    padding: 0px 0px !important;
  }
  .box-card-item .head-img-wrap {
    margin:-20px;
    padding:10px 10px 0px 10px;
  }
  .box-card-item .head-status {
    float: right;
    margin:-20px;
    padding:10px 10px 0px 10px;
  }
  .box-card-item .head-status p {
    right: 10px;
    color: #409EFF;
  }
  .box-card-item .head-status .project-name {
    font-weight: bold;
    font-size: 15px;
  }
  .box-card-item .head-status .status-name {
    font-size: 13px;
    line-height: 25px;
  }

</style>
