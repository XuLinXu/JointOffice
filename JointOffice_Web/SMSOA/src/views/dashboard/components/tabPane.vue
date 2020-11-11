<template>
  <div class="el-card-search">
    <el-form ref="form" :model="form">
      <el-form-item v-if="type_index in ['0']" :label="$t('public.category')">
        <el-radio-group v-model="form.default_type">
          <el-radio v-for="tab in tabs" :label="tab.name" :key="tab.type" :name="tab.name" :loading="loading" />
        </el-radio-group>
      </el-form-item>
      <el-form-item v-if="type_index in ['0','1']" :label="$t('public.start_stop_time')">
        <el-col :span="6">
          <el-date-picker :placeholder="$t('public.start_time')" v-model="form.beginTime" type="date" format="yyyy-MM-dd" style="width: 100%;"/>
        </el-col>
        <el-col :span="0.5" class="line">-</el-col>
        <el-col :span="6">
          <el-date-picker :placeholder="$t('public.stop_time')" v-model="form.stopTime" type="date" format="yyyy-MM-dd" style="width: 100%;"/>
        </el-col>
      </el-form-item>
      <el-form-item v-if="type_index in ['0','1']" :label="$t('public.keyword')">
        <el-col :span="10">
          <el-input v-model="form.name"/>
        </el-col>
      </el-form-item>
      <el-form-item >
        <el-button type="primary" @click="onSubmit">{{ $t('public.search') }}</el-button>
        <el-button @click="onCancel">{{ $t('public.reset') }}</el-button>
      </el-form-item>
    </el-form>
  </div>
</template>
<script>
import { getTabList } from '@/api/work'
import { getNowFormatDate, getAddFormatDate } from '@/utils/date'
export default {
  props: {
    type_index: {
      type: String,
      default: ''
    }
  },
  data() {
    return {
      form: {
        name: '',
        beginTime: getAddFormatDate(7),
        stopTime: getNowFormatDate(),
        type: '',
        loading: false,
        page: 0,
        count: 10,
        default_type: '全部'
      },
      tabs: []
    }
  },
  created() {
    this.getTabList()
  },
  methods: {
    onSubmit() {
      if (this.form.default_type === '') {
        this.form.type = '0'
      } else {
        var type = this.form.default_type
        this.tabs.forEach(function(c) {
          if (c.name === type) {
            type = c.type
            return false
          }
        })
        this.form.type = type
      }
      this.$emit('search', this.form)
    },
    onCancel() {
      this.$message({
        message: 'cancel!',
        type: 'warning'
      })
    },
    getTabList() {
      this.loading = true
      getTabList().then(response => {
        this.tabs = response.contentlist
        this.loading = false
      })
    }
  }
}
</script>
<style>
  .el-card-search .el-radio+.el-radio {
    margin-left: 15px !important;
  }

  .el-card-search .el-form-item {
    margin-bottom: 10px !important;
  }
</style>
