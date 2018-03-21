import Vue from "vue";
import {Component, Prop} from "vue-property-decorator";
import iview from "iview";
import {DetailPage} from "../../models/DetailPage";

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
            { required: true, message: '请输入名称', trigger: 'blur' }
            ],
        productNo: [
            { required: true, message: '请输入商品编码', trigger: 'blur' }
            ],
        htmlContent: [
            { required: true, message: '请填写内容', trigger: 'blur' }
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
        if(flag){
            this.makeDialogShow(false);
        }else{
            // event.preventDefault();
            // iview.Modal.visible = true;
        }
        this.isLoading = false;
    }

    cancel() {
        // this.$Message.info('Clicked cancel');
        console.log("close...");
        this.makeDialogShow(false);
        // iview.Message.info('Clicked cancel')
    }
    
    onVisibleChange(visible: boolean){
        if (!visible) {
            this.makeDialogShow(false);
        }
    }
}