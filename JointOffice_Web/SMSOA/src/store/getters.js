const getters = {
  sidebar: state => state.app.sidebar,
  tabname: state => state.app.tabname,
  device: state => state.app.device,
  token: state => state.user.token,
  avatar: state => state.user.avatar,
  memberid: state => state.user.memberid,
  name: state => state.user.name,
  roles: state => state.user.roles,
  errorLogs: state => state.errorLog.logs,
  userTabs: state => state.work.userTabs
}
export default getters
