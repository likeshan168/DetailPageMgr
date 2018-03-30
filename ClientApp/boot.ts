import './css/site.css';
import 'bootstrap';
import Vue from 'vue';
import VueRouter from 'vue-router';
import VueCookie from 'vue-cookies'

import 'iview/dist/styles/iview.css';
import iview from 'iview';
import store from "./vuex/store";
import {routes} from "./routes/routes";


Vue.use(VueRouter);
Vue.use(iview);
Vue.use(VueCookie);

let router = new VueRouter({mode: 'history', routes});

router.beforeEach((to: any, from: any, next: any) => {
    iview.LoadingBar.start();
    if (!to.fullPath.startsWith("/login")) {
        let token = VueCookie.get('auth_token');
        if (token) {
            next();
        } else {
            next({
                path: '/login',
                query: {redirect: to.fullPath}
            })
        }
    } else {
        next();
    }
});

router.afterEach(route => {
    iview.LoadingBar.finish();
});

let vue = new Vue({
    el: '#app-root',
    router: router,
    render: h => h(require('./components/app/app.vue.html')),
    store
});

