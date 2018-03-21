import './css/site.css';
import 'bootstrap';
import Vue from 'vue';
import VueRouter from 'vue-router';

import Vuex, {Store} from 'vuex'

import 'iview/dist/styles/iview.css';
import iview from 'iview';

Vue.use(VueRouter);
Vue.use(iview);
Vue.use(Vuex);

let store = new Store({
    state: {
        showDialog: false
    },
    mutations: {
        changeDialogStatus(state, value) {
            state.showDialog = value;
        }
    },
    actions: {
        makeDialogShow({commit, state}, value) {
            commit('changeDialogStatus', value);
        }
    }
});

const routes = [
    // { path: '/', component: require('./components/home/home.vue.html') },
    {path: '/counter', component: require('./components/counter/counter.vue.html')},
    {path: '/', component: require('./components/fetchdata/fetchdata.vue.html')}
];

new Vue({
    el: '#app-root',
    router: new VueRouter({mode: 'history', routes: routes}),
    render: h => h(require('./components/app/app.vue.html')),
    store
});

