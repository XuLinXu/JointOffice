/**
 * Created by Zhuangzuolong on 2018/9/29.
 */
export function getNowFormatDate() {
  return new Date()
}
export function getAddFormatDate(day) {
  return new Date().getTime() - 1000 * 60 * 60 * 24 * day
}
