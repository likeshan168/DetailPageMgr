import Vue from 'vue';
import {Component} from 'vue-property-decorator';
import {mapActions} from 'vuex';
import {DetailPage} from "../../models/DetailPage";




@Component({
    components: {
        SearchComponent: require('./search.vue.html'),
        newForm: require('./newForm.vue.html')
    }
})

export default class FetchDataComponent extends Vue {
    detailPages: DetailPage[] = [];
    columns = [
        {
            type: 'selection',
            width: 60,
            align: 'center'
        },
        {
            type: 'index',
            width: 60,
            align: 'center'
        },
        {
            title: '名称',
            key: 'name',
            sortable: true
        },
        {
            title: '内容',
            key: 'htmlContent'
        },
        {
            title: '链接',
            key: 'url'
        },
        {
            title: '备注',
            key: 'remark'
        }
    ];
    loading = true;


    makeDialogShow():void{
        this.$store.dispatch('makeDialogShow', true);
    }

    mounted() {
        fetch('api/DetailPageData/DetailPages')
            .then(response => response.json() as Promise<DetailPage[]>)
            .then(data => {
                this.detailPages = data;
                this.loading = false;
            });
    };

}

