import Vue from 'vue'
import Router from 'vue-router'

// in development-env not use lazy-loading, because lazy-loading too many pages will cause webpack hot update too slow. so only in production use lazy-loading;
// detail: https://panjiachen.github.io/vue-element-admin-site/#/lazy-loading

Vue.use(Router)

/* Layout */
import Layout from '../views/layout/Layout'

/**
* hidden: true                   if `hidden:true` will not show in the sidebar(default is false)
* alwaysShow: true               if set true, will always show the root menu, whatever its child routes length
*                                if not set alwaysShow, only more than one route under the children
*                                it will becomes nested mode, otherwise not show the root menu
* redirect: noredirect           if `redirect:noredirect` will no redirect in the breadcrumb
* name:'router-name'             the name is used by <keep-alive> (must set!!!)
* meta : {
    title: 'title'               the name show in submenu and breadcrumb (recommend set)
    icon: 'svg-name'             the icon show in the sidebar,
  }
**/
export const constantRouterMap = [
  { path: '/login', component: () => import('@/views/login/index'), hidden: true },
  { path: '/404', component: () => import('@/views/404'), hidden: true },

  /* {
    path: '/',
    component: Layout,
    redirect: '/dashboard',
    name: 'Dashboard',
    hidden: true,
    children: [{
      path: 'dashboard',
      component: () => import('@/views/dashboard/index')
    }]
  },*/
  // 发布
  {
    path: '/release',
    component: Layout,
    type: 'work',
    children: [{
      path: 'index',
      name: 'Release',
      component: () => import('@/views/release/index'),
      meta: { title: 'release', icon: 'release' }
    }]
  },
  // 工作圈
  {
    path: '/',
    component: Layout,
    type: 'work',
    redirect: '/dashboard',
    children: [{
      path: 'dashboard',
      name: 'Dashboard',
      component: () => import('@/views/dashboard/index'),
      meta: { title: 'dashboard', icon: 'work' }
    }]
  },
  /* {
    path: '/example',
    component: Layout,
    redirect: '/example/table',
    name: 'Example',
    meta: { title: 'Example', icon: 'example' },
    children: [
      {
        path: 'table',
        name: 'Table',
        component: () => import('@/views/table/index'),
        meta: { title: 'Table', icon: 'table' }
      },
      {
        path: 'tree',
        name: 'Tree',
        component: () => import('@/views/tree/index'),
        meta: { title: 'Tree', icon: 'tree' }
      }
    ]
  },*/
  // 我要处理
  {
    path: '/dispose',
    component: Layout,
    type: 'work',
    redirect: '/dispose/logprompt',
    name: 'dispose',
    meta: { title: 'dispose', icon: 'dispose' },
    children: [{
      path: 'logprompt',
      component: () => import('@/views/table/index'),
      meta: { title: 'logprompt', icon: 'log' }
    },
    {
      path: 'auditprompt',
      component: () => import('@/views/tree/index'),
      meta: { title: 'auditprompt', icon: 'audit' }
    },
    {
      path: 'taskprompt',
      component: () => import('@/views/tree/index'),
      meta: { title: 'taskprompt', icon: 'task' }
    },
    {
      path: 'scheduleprompt',
      component: () => import('@/views/tree/index'),
      meta: { title: 'scheduleprompt', icon: 'schedule' }
    },
    {
      path: 'instructprompt',
      component: () => import('@/views/tree/index'),
      meta: { title: 'instructprompt', icon: 'Instruction' }
    }]
  },
  // 我的消息
  {
    path: '/mynews',
    component: Layout,
    type: 'work',
    redirect: '/dispose/logprompt',
    name: 'mynews',
    meta: { title: 'mynews', icon: 'my_news' },
    children: [{
      path: 'myreceipt',
      component: () => import('@/views/table/index'),
      meta: { title: 'myreceipt', icon: 'table' }
    },
    {
      path: 'atmy',
      component: () => import('@/views/tree/index'),
      meta: { title: 'atmy', icon: 'tree' }
    },
    {
      path: 'atdepartment',
      component: () => import('@/views/tree/index'),
      meta: { title: 'atdepartment', icon: 'tree' }
    },
    {
      path: 'workreply',
      component: () => import('@/views/tree/index'),
      meta: { title: 'workreply', icon: 'tree' }
    },
    {
      path: 'mypraise',
      component: () => import('@/views/tree/index'),
      meta: { title: 'mypraise', icon: 'tree' }
    }]
  },
  // 我的工作
  {
    path: '/mywork',
    component: Layout,
    type: 'work',
    children: [{
      path: 'index',
      name: 'mywork',
      component: () => import('@/views/form/index'),
      meta: { title: 'mywork', icon: 'form' }
    }]
  },
  // 工作文档
  {
    path: '/workdocument',
    component: Layout,
    type: 'work',
    children: [{
      path: 'index',
      name: 'workdocument',
      component: () => import('@/views/form/index'),
      meta: { title: 'workdocument', icon: 'form' }
    }]
  },
  // 我的收藏
  {
    path: '/mycollection',
    component: Layout,
    type: 'work',
    children: [{
      path: 'index',
      name: 'mycollection',
      component: () => import('@/views/form/index'),
      meta: { title: 'mycollection', icon: 'form' }
    }]
  },
  // 我的关注
  {
    path: '/myattention',
    component: Layout,
    type: 'work',
    children: [{
      path: 'index',
      name: 'myattention',
      component: () => import('@/views/form/index'),
      meta: { title: 'myattention', icon: 'form' }
    }]
  },
  // 我的应用
  {
    path: '/myapply',
    component: Layout,
    type: 'apply',
    redirect: '/myapply/index',
    children: [{
      path: 'index',
      name: 'myapply',
      component: () => import('@/views/form/index'),
      meta: { title: 'myapply', icon: 'form' }
    }]
  },
  /* {
    path: '/nested',
    component: Layout,
    redirect: '/nested/menu1',
    name: 'Nested',
    meta: {
      title: 'Nested',
      icon: 'nested'
    },
    children: [
      {
        path: 'menu1',
        component: () => import('@/views/nested/menu1/index'), // Parent router-view
        name: 'Menu1',
        meta: { title: 'Menu1' },
        children: [
          {
            path: 'menu1-1',
            component: () => import('@/views/nested/menu1/menu1-1'),
            name: 'Menu1-1',
            meta: { title: 'Menu1-1' }
          },
          {
            path: 'menu1-2',
            component: () => import('@/views/nested/menu1/menu1-2'),
            name: 'Menu1-2',
            meta: { title: 'Menu1-2' },
            children: [
              {
                path: 'menu1-2-1',
                component: () => import('@/views/nested/menu1/menu1-2/menu1-2-1'),
                name: 'Menu1-2-1',
                meta: { title: 'Menu1-2-1' }
              },
              {
                path: 'menu1-2-2',
                component: () => import('@/views/nested/menu1/menu1-2/menu1-2-2'),
                name: 'Menu1-2-2',
                meta: { title: 'Menu1-2-2' }
              }
            ]
          },
          {
            path: 'menu1-3',
            component: () => import('@/views/nested/menu1/menu1-3'),
            name: 'Menu1-3',
            meta: { title: 'Menu1-3' }
          }
        ]
      },
      {
        path: 'menu2',
        component: () => import('@/views/nested/menu2/index'),
        meta: { title: 'menu2' }
      }
    ]
  },

  {
    path: 'external-link',
    component: Layout,
    children: [
      {
        path: 'https://panjiachen.github.io/vue-element-admin-site/#/',
        meta: { title: 'External Link', icon: 'link' }
      }
    ]
  },*/

  { path: '*', redirect: '/404', hidden: true }
]

export default new Router({
  // mode: 'history', //后端支持可开
  scrollBehavior: () => ({ y: 0 }),
  routes: constantRouterMap
})
