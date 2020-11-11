import Cookies from 'js-cookie'

const TokenKey = 'User-Token'
const MemberIdKey = 'User-MemberId'

export function setToken(token) {
  return Cookies.set(TokenKey, token)
}

export function setMemberId(memberid) {
  return Cookies.set(MemberIdKey, memberid)
}

export function setUserTabs(tabs) {
  return Cookies.set(getMemberId(), JSON.stringify(tabs))
}

export function getToken() {
  return Cookies.get(TokenKey)
}

export function getMemberId() {
  return Cookies.get(MemberIdKey)
}

export function getUserTabs() {
  var tabs = Cookies.get(getMemberId())
  if (tabs === undefined || tabs === null || JSON.stringify(tabs) === '{}' || tabs === '') {
    return null
  } else {
    return JSON.parse(tabs)
  }
}

export function removeToken() {
  return Cookies.remove(TokenKey)
}

export function removeMemberId() {
  return Cookies.remove(MemberIdKey)
}

export function removeUserTabs() {
  var tabs = Cookies.get(getMemberId())
  if (tabs === undefined || tabs === null || JSON.stringify(tabs) === '{}' || tabs === '') {
    return false
  } else {
    return Cookies.remove(getMemberId())
  }
}
export function removes() {
  removeToken()
  removeMemberId()
  removeUserTabs()()
}

