import Vue from "vue";
import {Component, Prop} from "vue-property-decorator";
import iview from "iview";
import {DetailPage} from "../../models/DetailPage";
import {ResponseResult} from "../../models/ResponseResult";
import VueCookie from "vue-cookies";

@Component({})

export default class newForm extends Vue {

    // detailPage: DetailPage = {
    //     id: 0,
    //     name: '',
    //     productNo: '',
    //     htmlContent: '',
    //     remark: '',
    //     url: ''
    // };

    isLoading: boolean = false;

    ruleValidate = {
        name: [
            {required: true, message: '请输入名称', trigger: 'blur'}
        ],
        productNo: [
            {required: true, message: '请输入商品编码', trigger: 'blur'}
        ],
        htmlContent: [
            {required: true, message: '请填写内容', trigger: 'blur'}
        ]
    };

    get isEdit(): boolean {
        return this.$store.state.isEdit;
    }

    get detailPage(): DetailPage {
        return this.$store.state.currentDetailPage;
    }

    get showDialog() {
        return this.$store.state.showDialog;
    };

    makeDialogShow(value: boolean): void {
        this.$store.dispatch('makeDialogShow', value);
    }

    ok(name: any) {
        this.isLoading = true;
        (<Vue>this.$refs[name]).validate((valid: boolean) => {
            if (valid) {
                let url = ``;
                if (this.isEdit) {
                    url = `api/DetailPageData/Edit?id=${this.detailPage.id}`
                } else {
                    url = 'api/DetailPageData/Create';
                }
                console.log(this.isEdit);
                console.log(url);
                //保存到数据库
                fetch(url, {
                    method: 'POST',
                    headers: {
                        "Content-Type": "application/json",
                        'HeaderAuthorization': VueCookie.get('auth_token')
                    },
                    body: JSON.stringify(this.detailPage)
                }).then(response => response.json() as Promise<ResponseResult>)
                    .then(data => {

                        if (data.isSuccess) {
                            iview.Notice.success({
                                title: '提示',
                                desc: '保存成功'
                            });
                            this.makeDialogShow(false);
                            //目的是保存之后清空表单中的内容
                            let page: DetailPage = {
                                id: this.detailPage.id,
                                name: this.detailPage.name,
                                productNo: this.detailPage.productNo,
                                htmlContent: this.detailPage.htmlContent,
                                remark: this.detailPage.remark,
                                url: this.detailPage.url,
                                masterImages: this.detailPage.masterImages,
                                detailImages: this.detailPage.detailImages
                            };
                            if (this.isEdit) {
                                this.$store.dispatch("updateDetailPage", page);
                            } else {
                                this.$store.dispatch("addDetailPage", page);
                            }

                            this.$store.dispatch("resetCurrentDetailPage");
                        } else {
                            iview.Notice.error({
                                title: '提示',
                                desc: '保存失败'
                            });
                        }

                        this.isLoading = false;

                    }).catch(err => {
                    iview.Notice.error({
                        title: '提示',
                        desc: '保存失败'
                    });
                    this.isLoading = false;
                });

            } else {
                this.isLoading = false;
            }
        });
    }

    cancel() {
        this.makeDialogShow(false);
    }

    onVisibleChange(visible: boolean) {
        if (!visible) {
            this.makeDialogShow(false);
        }
    }
}