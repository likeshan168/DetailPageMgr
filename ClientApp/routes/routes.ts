export const routes = [
    {path: '/login', component: require('../components/login/login.vue.html')},
    {
        path: '/',
        meta: {
            // 添加该字段，表示进入这个路由是需要登录的
            requireAuth: true,
        },
        component:require('../components/navmenu/mymenu.vue.html'),
        children: [{
            path: '', component: require('../components/home/home.vue.html')
        }]
    },
    {
        path: '/',
        meta: {
            // 添加该字段，表示进入这个路由是需要登录的
            requireAuth: true,
        },
        component:require('../components/navmenu/mymenu.vue.html'),
        children: [{
            path: '/fetchdata', component: require('../components/fetchdata/fetchdata.vue.html')
        }]
    }
];