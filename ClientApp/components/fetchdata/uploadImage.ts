import Vue from 'vue'
import {Component, Prop, Watch} from "vue-property-decorator";
import iview from 'iview';
import {FileInfoResponse} from "../../models/FileInfoResponse";
import {DetailPage} from "../../models/DetailPage";
import {ResponseResult} from "../../models/ResponseResult";

@Component
export default class UploadImage extends Vue {
    //不要对属性进行赋值，操作也不要直接操作属性，而应该操作一个计算属性或者data中的数据（关于属性的），然后对这个计算属性/data中的数进行操作
    @Prop() row: DetailPage;
    @Prop() defaultDetailList: any[];
    @Prop() defaultMasterList: any[];

    //直接用defaultDetailList，页面上的图片不更新
    dList: any[] = [...this.defaultDetailList];
    mList: any[] = [...this.defaultMasterList];

    imgName = '';
    masterImgName = '';
    visible = false;
    masterVisible = false;
    uploadList: any[] = [];
    uploadMasterList: any[] = [];

    // @Watch('row', {deep: true})
    // watchRow(newValue: object, oldValue: object) {
    //     let detailPage = (<DetailPage>this.row);
    //     let arr: object[] = [];
    //     console.log(detailPage);
    //     if (detailPage.detailImages != null) {
    //         detailPage.detailImages.forEach(p => {
    //             arr.push({
    //                 name: p,
    //                 url: `Uploads/Detail/${detailPage.productNo}/${p}`
    //             })
    //         });
    //     }
    // }

    // get getRow(): DetailPage {
    //     return {...this.row} as DetailPage;
    // }

    handleView(name: string) {
        this.imgName = `Uploads/Detail/${this.row.productNo}/${name}`;
        this.visible = true;
    }

    handleMasterView(name: string) {
        this.masterImgName = `Uploads/Master/${this.row.productNo}/${name}`;
        this.masterVisible = true;
    }

    handleRemove(file: any) {
        //删除服务器上的图片
        iview.Modal.confirm({
            title: '提示',
            content: '<p>确认删除吗</p>',
            onOk: () => {
                fetch(`api/UploadImage/Delete?productNo=${this.row.productNo}&fileName=${file.name}`, {
                    method: 'POST'
                })
                    .then(response => response.json() as Promise<ResponseResult>)
                    .then(data => {
                        if (data.isSuccess) {
                            const fileList = (<Vue>this.$refs.upload).fileList;
                            (<Vue>this.$refs.upload).fileList.splice(fileList.indexOf(file), 1);
                            iview.Notice.info({
                                title: '提示',
                                desc: data.message
                            });
                        } else {
                            iview.Notice.error({
                                title: '提示',
                                desc: data.message
                            });
                        }
                    });
            }
        });
    }

    handleMasterRemove(file: any) {
        //删除服务器上的图片
        iview.Modal.confirm({
            title: '提示',
            content: '<p>确认删除吗</p>',
            onOk: () => {
                fetch(`api/UploadMasterImage/Delete?productNo=${this.row.productNo}&fileName=${file.name}`, {
                    method: 'POST'
                })
                    .then(response => response.json() as Promise<ResponseResult>)
                    .then(data => {
                        if (data.isSuccess) {
                            const fileList = (<Vue>this.$refs.masterUpload).fileList;
                            (<Vue>this.$refs.masterUpload).fileList.splice(fileList.indexOf(file), 1);
                            iview.Notice.info({
                                title: '提示',
                                desc: data.message
                            });
                        } else {
                            iview.Notice.error({
                                title: '提示',
                                desc: data.message
                            });
                        }
                    });
            }
        });
    }

    handleSuccess(res: any, file: any) {
        console.log(file);
        let rst = res as FileInfoResponse;
        if (rst.isSuccess) {
            file.url = rst.fileList[0].path + rst.fileList[0].name;
            file.name = rst.fileList[0].name;
            iview.Notice.info({
                title: '提示',
                desc: rst.message
            });
        } else {
            iview.Notice.error({
                title: '提示',
                desc: rst.message
            });
        }
    }

    handleMasterSuccess(res: any, file: any) {
        let rst = res as FileInfoResponse;
        if (rst.isSuccess) {
            file.url = rst.fileList[0].path + rst.fileList[0].name;
            file.name = rst.fileList[0].name;
            iview.Notice.info({
                title: '提示',
                desc: rst.message
            });
        } else {
            iview.Notice.error({
                title: '提示',
                desc: rst.message
            });
        }
    }

    handleFormatError(file: any) {
        iview.Notice.warning({
            title: 'The file format is incorrect',
            desc: 'File format of ' + file.name + ' is incorrect, please select jpg or png.'
        });
    }

    handleMasterFormatError(file: any) {
        iview.Notice.warning({
            title: 'The file format is incorrect',
            desc: 'File format of ' + file.name + ' is incorrect, please select jpg or png.'
        });
    }

    handleMaxSize(file: any) {
        iview.Notice.warning({
            title: 'Exceeding file size limit',
            desc: 'File  ' + file.name + ' is too large, no more than 2M.'
        });
    }

    handleMasterMaxSize(file: any) {
        iview.Notice.warning({
            title: 'Exceeding file size limit',
            desc: 'File  ' + file.name + ' is too large, no more than 2M.'
        });
    }


    handleBeforeUpload() {
        const check = this.uploadList.length < 30;
        if (!check) {
            iview.Notice.warning({
                title: 'Up to five pictures can be uploaded.'
            });
        }
        return check;
    }

    handleMasterBeforeUpload() {
        const check = this.uploadMasterList.length < 30;
        if (!check) {
            iview.Notice.warning({
                title: 'Up to five pictures can be uploaded.'
            });
        }
        return check;
    }

    mounted() {
        this.uploadList = (<Vue>this.$refs.upload).fileList;
        this.uploadMasterList = (<Vue>this.$refs.masterUpload).fileList;
    }
}