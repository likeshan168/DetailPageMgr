import Vue from 'vue';
import {Component} from 'vue-property-decorator';
import {mapActions} from 'vuex';
import {DetailPage} from "../../models/DetailPage";
import iview from 'iview';
import {ResponseResult} from "../../models/ResponseResult";
import uploadImage from './uploadImage';

@Component({
    components: {
        SearchComponent: require('./search.vue.html'),
        newForm: require('./newForm.vue.html'),
        uploadImage: require('./uploadImage.vue.html')
    }
})

export default class FetchDataComponent extends Vue {
    // detailPages: DetailPage[] = [];
    columns = [
        {
            type: 'expand',
            width: 50,
            render: (h: any, params: any) => {
                let details = params.row.detailImages as string[];
                let masters = params.row.masterImages as string[];
                let arr: object[] = [];
                let masterArr: object[] = [];
                if (details != null) {
                    details.forEach(p => {
                        arr.push({
                            name: p,
                            url: `Uploads/Detail/${params.row.productNo}/${p}`
                        })
                    });
                }
                if (masters != null) {
                    masters.forEach(p => {
                        masterArr.push({
                            name: p,
                            url: `Uploads/Master/${params.row.productNo}/${p}`
                        })
                    });
                }
                return h(uploadImage, {
                    props: {
                        row: params.row,
                        defaultDetailList: arr,
                        defaultMasterList: masterArr
                    },
                    on: {
                        click: () => {
                            console.log(params.row);
                        }
                    }
                })
            }
        },
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
                                        this.delete(params.row);
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
    expandrow(row: any, status: any) {
        // console.log(row);
        // console.log(status);
    }

    makeDialogShow(value: boolean): void {
        this.$store.dispatch('changeEditStatus', value);
        this.$store.dispatch('makeDialogShow', true);
    }

    preview(id: number): void {

    }

    delete(detailPage: DetailPage): void {
        this.$store.dispatch("deleteDetailPage", detailPage);
    }

    getData(): void {
        this.$store.dispatch("getDetailPages");
    }

    get detailPages() {
        return this.$store.state.detailPages;
    }

    get loading() {
        return this.$store.state.loadingData;
    }

    mounted() {
        this.getData();
    };

}

