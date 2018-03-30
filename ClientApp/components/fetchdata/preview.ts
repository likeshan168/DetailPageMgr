import Vue from 'vue';

import {Component, Prop} from 'vue-property-decorator';
import {DetailPage} from "../../models/DetailPage";

@Component
export default class Preview extends Vue{
    // @Prop() showDialog: boolean;
    
    // @Prop() row: DetailPage;
    
    get crow(){
        return this.$store.state.currentDetailPage;
    }
    
    get showPreviewDialog(){
        return this.$store.state.showPreviewDialog;
    }
    
    ok(){
        this.$store.dispatch("makePreviewDialogShow", false);
    }

    onVisibleChange(visible: boolean) {
        if (!visible) {
             this.$store.dispatch("makePreviewDialogShow", false);
        }
    }
    
}