/**
 * Created by Zhuangzuolong on 2018/9/29.
 */
import request from '@/utils/request'

export function getTabList() {
  return request({
    url: '/Work/GetWorkList',
    method: 'post'
  })
}
export function getWorkList(data) {
  return request({
    url: '/Work/WorkListAll',
    method: 'post',
    data: data
  })
}
