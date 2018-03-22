import Vue from 'vue';
import {Component} from 'vue-property-decorator';
import {mapActions} from 'vuex';
import {DetailPage} from "../../models/DetailPage";
import iview from 'iview';
import {ResponseResult} from "../../models/ResponseResult";

@Component({
    components: {
        SearchComponent: require('./search.vue.html'),
        newForm: require('./newForm.vue.html')
    }
})

export default class FetchDataComponent extends Vue {
    // detailPages: DetailPage[] = [];
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
            title: '商品编码',
            key: 'productNo',
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
        },
        {
            title: '操作',
            key: 'action',
            width: 200,
            align: 'center',
            render: (h: any, params: any) => {
                return h('div', [
                    h('Button', {
                        props: {
                            type: 'primary',
                            size: 'small'
                        },
                        style: {
                            marginRight: '5px'
                        },
                        on: {
                            click: () => {
                                // this.show(params.index)
                            }
                        }
                    }, '预览'),
                    h('Button', {
                        props: {
                            type: 'info',
                            size: 'small'
                        },
                        style: {
                            marginRight: '5px'
                        },
                        on: {
                            click: () => {
                                // let current = (this.$store.state.detailPages as DetailPage[]).filter(p =>)
                                this.$store.dispatch('setCurrentDetailPage', {...params.row});
                                this.makeDialogShow(true);
                            }
                        }
                    }, '编辑'),
                    h('Button', {
                        props: {
                            type: 'error',
                            size: 'small'
                        },
                        on: {
                            click: () => {
                                // this.remove(params.index)
                                console.log(params);
                                console.log(params.row.id);
                                iview.Modal.confirm({
                                    title: '提示',
                                    content: '<p>确认删除吗</p>',
                                    onOk: () => {
                                        this.delete(params.row.id);
                                    }
                                });
                            }
                        }
                    }, '删除')
                ]);
            }
        }
    ];
    // loading = true;


    makeDialogShow(value: boolean): void {
        this.$store.dispatch('changeEditStatus', value);
        this.$store.dispatch('makeDialogShow', true);
    }

    preview(id: number): void {

    }

    delete(id: number): void {
        this.$store.dispatch("deleteDetailPage", id);
    }

    getData():void{
        this.$store.dispatch("getDetailPages");
    }
    
    get detailPages(){
        return this.$store.state.detailPages;
    }
    
    get loading(){
        return this.$store.state.loadingData;
    }
    
    mounted() {
        this.getData();
    };

}

