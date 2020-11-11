import request from '@/utils/request'
// 登录
export function login(username, password) {
  const data = {
    'loginname': username,
    'loginpwd': password
  }
  return request({
    url: '/Verification/Login',
    method: 'post',
    data: data
  })
}
// 个人用户信息
export function getInfo(memberid) {
  return request({
    url: '/Contacts/GetMemberinfo',
    method: 'post',
    data: {
      memberid
    }
  })
}
// 登出
export function logout() {
  const data = {
    'type': 1
  }
  return request({
    url: '/Verification/SengModel',
    method: 'post',
    data: data
  })
}
