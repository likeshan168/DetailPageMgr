import Vue from "vue";
import {Component, Prop} from "vue-property-decorator";
import iview from "iview";
import {DetailPage} from "../../models/DetailPage";
import {ResponseResult} from "../../models/ResponseResult";

@Component({})

export default class newForm extends Vue {

    detailPage: DetailPage = {
        id: 0,
        name: '',
        productNo: '',
        htmlContent: '',
        remark: '',
        url: ''
    };

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

    get showDialog() {
        return this.$store.state.showDialog
    };

    makeDialogShow(value: boolean): void {
        this.$store.dispatch('makeDialogShow', value);
    }

    ok(name: any) {
        let flag = true;
        this.isLoading = true;
        (<Vue>this.$refs[name]).validate((valid: boolean) => {
            flag = valid;
        });
        if (flag) {
            //保存到数据库
            fetch('api/DetailPageData/Create', {
                method: 'POST',
                headers: {
                    "Content-Type": "application/json"
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

    }

    cancel() {
        // this.$Message.info('Clicked cancel');
        console.log("close...");
        this.makeDialogShow(false);
        // iview.Message.info('Clicked cancel')
    }

    onVisibleChange(visible: boolean) {
        if (!visible) {
            this.makeDialogShow(false);
        }
    }
}