/**
 * Created by Zhuangzuolong on 2018/9/29.
 */
import { getTabList } from '@/api/work'
import { getUserTabs, setUserTabs } from '@/utils/auth'
// { 'type': '0', 'name': '全部' }, { 'type': '1', 'name': '审批' }, { 'type': '2', 'name': '日志' }, { 'type': '3', 'name': '任务' }, { 'type': '4', 'name': '日程' }, { 'type': '5', 'name': '指令' }, { 'type': '6', 'name': '公告' }, { 'type': '7', 'name': '分享' }
const work = {
  state: {
    userTabs: getUserTabs()// 用户个人类别
  },
  mutations: {
    SET_USERTABS: (state, tabs) => {
      state.userTabs = tabs
    }
  },
  actions: {
    // Tab 列表
    GetTabList({ commit }) {
      return new Promise((resolve, reject) => {
        getTabList().then(response => {
          const data = response
          setUserTabs(data.contentlist)
          commit('SET_USERTABS', data.contentlist)
          resolve()
        }).catch(error => {
          reject(error)
        })
      })
    }
  }
}

export default work
