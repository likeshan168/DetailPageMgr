import './css/site.css';
import 'bootstrap';
import Vue from 'vue';
import VueRouter from 'vue-router';

import Vuex, {Store} from 'vuex'

import 'iview/dist/styles/iview.css';
import iview from 'iview';
import {DetailPage} from "./models/DetailPage";
import {ResponseResult} from "./models/ResponseResult";

Vue.use(VueRouter);
Vue.use(iview);
Vue.use(Vuex);

let store = new Store({
    state: {
        showDialog: false,
        detailPages: [{}],
        loadingData: true,
        isEdit: false,
        currentDetailPage: {
            id: 0,
            name: '',
            productNo: '',
            htmlContent: '',
            remark: '',
            url: ''
        }
    },
    mutations: {
        changeDialogStatus(state, value) {
            state.showDialog = value;
        },

        initData(state, values) {
            state.detailPages = values;
        },
        addData(state, value) {
            state.detailPages.push(value);
            console.log(state.detailPages);
        },
        changeLoadingStatus(state, value) {
            state.loadingData = value;
        },
        updateDetailPage(state, value) {
           let dps = (state.detailPages as DetailPage[]).filter(p => p.id == value.id);
           if(dps && dps.length){
               dps[0].name = value.name;
               dps[0].remark = value.remark;
               dps[0].htmlContent = value.htmlContent;
               dps[0].productNo = value.productNo;
               dps[0].url = value.url;
           }
        },
        changeEditStatus(state, value){
            state.isEdit = value;
        },
        resetCurrentDetailPage(state){
            state.currentDetailPage.name = '';
            state.currentDetailPage.productNo = '';
            state.currentDetailPage.htmlContent = '';
            state.currentDetailPage.remark = '';
        },
        setCurrentDetailPage(state, value){
            state.currentDetailPage = value;
        }
    },
    actions: {
        makeDialogShow({commit, state}, value) {
            commit('changeDialogStatus', value);
        },

        getDetailPages({commit, state}) {
            fetch('api/DetailPageData/DetailPages')
                .then(response => response.json() as Promise<DetailPage[]>)
                .then(data => {
                    // state.detailPages = data;
                    // this.loading = false;
                    // state.loadingData = false;
                    commit('initData', data);
                    commit('changeLoadingStatus', false)
                });
        },
        deleteDetailPage({commit, state}, id) {
            state.loadingData = true;
            fetch(`api/DetailPageData/Delete/?id=${id}`, {
                method: 'POST'
            }).then(response => response.json() as Promise<ResponseResult>)
                .then(data => {
                    if (data.isSuccess) {
                        vue.$store.dispatch("getDetailPages");
                    } else {
                        // this.loading = false;
                        state.loadingData = false;
                        iview.Message.error(data.message);
                    }
                });
        },
        addDetailPage({commit, state}, detailPage) {
            commit('addData', detailPage);
        },
        updateDetailPage({commit, state}, detailPage) {
            commit('updateDetailPage', detailPage);
        },
        changeEditStatus({commit, state}, value){
            commit('changeEditStatus', value);
        },
        resetCurrentDetailPage({commit, state}){
            commit('resetCurrentDetailPage');
        },
        setCurrentDetailPage({commit, state}, detailPage){
            commit('setCurrentDetailPage', detailPage);
        }
    }
});

const routes = [
    // { path: '/', component: require('./components/home/home.vue.html') },
    {path: '/counter', component: require('./components/counter/counter.vue.html')},
    {path: '/', component: require('./components/fetchdata/fetchdata.vue.html')}
];

let vue = new Vue({
    el: '#app-root',
    router: new VueRouter({mode: 'history', routes: routes}),
    render: h => h(require('./components/app/app.vue.html')),
    store
});

