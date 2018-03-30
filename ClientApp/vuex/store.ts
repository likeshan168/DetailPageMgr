import Vue from 'vue';
import Vuex from 'vuex';
import {DetailPage} from "../models/DetailPage";
import iview from "iview";
import {Store} from "vuex";
import {ResponseResult} from "../models/ResponseResult";
import VueCookie from 'vue-cookies';
import {PageResult} from "../models/PageResult";

Vue.use(Vuex);

export default new Store({
    state: {
        showDialog: false,
        // detailPages: [{}],
        loadingData: true,
        isEdit: false,
        currentDetailPage: {
            id: 0,
            name: '',
            productNo: '',
            htmlContent: '',
            remark: '',
            url: ''
        },
        showPreviewDialog: false,
        currentUser: {},
        logining: false,
        page: {
            data: [{}],
            pageNumber: 1,
            pageSize: 5,
            searchWord: "",
            total: 0
        }
    },
    mutations: {
        changeDialogStatus(state, value) {
            state.showDialog = value;
        },
        changePreviewDialogStatus(state, value) {
            state.showPreviewDialog = value;
        },
        initData(state, values) {
            state.page.data = values;
        },
        addData(state, value) {
            state.page.data.push(value);
        },
        deleteData(state, value){
            // state.page.data.push(value);
           let index = state.page.data.indexOf(value);
           state.page.data.splice(index,1);
        },
        changeLoadingStatus(state, value) {
            state.loadingData = value;
        },
        updateDetailPage(state, value) {
            let dps = (state.page.data as DetailPage[]).filter(p => p.id == value.id);
            if (dps && dps.length) {
                dps[0].name = value.name;
                dps[0].remark = value.remark;
                dps[0].htmlContent = value.htmlContent;
                dps[0].productNo = value.productNo;
                dps[0].url = value.url;
            }
        },
        changeEditStatus(state, value) {
            state.isEdit = value;
        },
        resetCurrentDetailPage(state) {
            state.currentDetailPage.name = '';
            state.currentDetailPage.productNo = '';
            state.currentDetailPage.htmlContent = '';
            state.currentDetailPage.remark = '';
        },
        setCurrentDetailPage(state, value) {
            state.currentDetailPage = value;
        },
        setCurrentUser(state, user) {
            state.currentUser = user;
        },
        setLoginStatus(state, value) {
            state.logining = value;
        },
        setPageSize(state, value) {
            state.page.pageSize = value;
        },
        setPageNumber(state, value) {
            state.page.pageNumber = value;
        },
        setTotal(state, value) {
            state.page.total = value;
        },
        setSearchWords(state, value){
            state.page.searchWord = value;
        }
    },
    actions: {
        makeDialogShow({commit, state}, value) {
            commit('changeDialogStatus', value);
        },
        makePreviewDialogShow({commit, state}, value) {
            commit('changePreviewDialogStatus', value);
        },
        deleteDetailPage({commit, state}, detailPage) {
            state.loadingData = true;
            fetch(`api/DetailPageData/Delete/?id=${detailPage.id}&productNo=${detailPage.productNo}`, {
                method: 'POST',
                headers: {
                    'HeaderAuthorization': VueCookie.get('auth_token')
                },
            }).then(response => response.json() as Promise<ResponseResult>)
                .then(data => {
                    if (data.isSuccess) {
                        // this.$store.state.dispatch("getDetailPages");
                        //删除前端的数据
                        commit('deleteData', detailPage);
                        state.loadingData = false;
                        iview.Message.info(data.message);
                    } else {
                        // this.loading = false;
                        state.loadingData = false;
                        iview.Message.error(data.message);
                    }
                }).catch(err=>{
                state.loadingData = false;
                iview.Message.error("删除失败");
            });
        },
        addDetailPage({commit, state}, detailPage) {
            commit('addData', detailPage);
        },
        updateDetailPage({commit, state}, detailPage) {
            commit('updateDetailPage', detailPage);
        },
        changeEditStatus({commit, state}, value) {
            commit('changeEditStatus', value);
        },
        resetCurrentDetailPage({commit, state}) {
            commit('resetCurrentDetailPage');
        },
        setCurrentDetailPage({commit, state}, detailPage) {
            commit('setCurrentDetailPage', detailPage);
        },
        setCurrentUser({commit, state}, user) {
            commit('setCurrentUser', user);
        },
        setLoginStatus({commit, state}, value) {
            commit('setLoginStatus', value);
        },
        changeLoadingStatus({commit, state}, value) {
            commit('changeLoadingStatus', value);
        },
        initData({commit, state}, value) {
            commit('initData', value);
        },
        setPageSize({commit, state}, value) {
            commit('setPageSize', value);
        },
        setPageNumber({commit, state}, value) {
            commit('setPageNumber', value);
        },
        setTotal({commit, state}, value) {
            commit('setTotal', value);
        },
        setSearchWords({commit, state}, value){
            commit('setSearchWords', value);
        }
    }
});