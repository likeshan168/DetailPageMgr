import Vue from 'vue';
import {Component} from 'vue-property-decorator';
import iview from 'iview';
import VueCookie from "vue-cookies";
import {PageResult} from "../../models/PageResult";

@Component
export default class SearchComponent extends Vue {
    get searchWords(){
        return this.$store.state.page.searchWords;
    }
    changed(ev: any){
        console.log(ev.target.value);
        this.$store.dispatch('setSearchWords', ev.target.value);
    }

    search() {
        
        fetch('api/DetailPageData/DetailPages', {
            headers: {
                'HeaderAuthorization': VueCookie.get('auth_token'),
                'Content-Type': 'application/json'
            },
            method: 'POST',
            body: JSON.stringify(this.$store.state.page)
        }).then(response => {
            if (response.redirected) this.$router.push("/login");
            return response.json() as Promise<PageResult>;
        }).then(res => {
            this.$store.dispatch('initData', res.data);
            this.$store.dispatch('setTotal', res.total);
            this.$store.dispatch('changeLoadingStatus', false)
        }).catch(err => {
            console.log(err);
            this.$store.dispatch('changeLoadingStatus', false);
            iview.Message.error("获取数据异常");
        });
    }
}