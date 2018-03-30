import Vue from 'vue';
import {Component} from 'vue-property-decorator';
import {User} from "../../models/User";
import iview from 'iview';
import VueCookie from 'vue-cookies';

@Component
export default class Login extends Vue {
    user: User = {
        id: 0,
        userName: '',
        password: '',
        email: ''
    };
    get logining(){
        return this.$store.state.logining;
    }
    rules = {
        userName: [
            {required: true, message: '用户名不能为空', trigger: 'blur'}
        ],
        password: [
            {required: true, message: '密码不能为空', trigger: 'blur'}
        ]
    };

    handleSubmit() {
        (this.$refs['user'] as Vue).validate((valid: boolean) => {
            if (valid) {
                iview.LoadingBar.start();
                this.$store.dispatch('setLoginStatus', true);
                fetch('api/login', {
                    method: 'POST',
                    body: JSON.stringify(this.user),
                    headers: {
                        'Content-Type': 'application/json'
                    }
                }).then(response => response.json())
                    .then(data => {
                        this.$store.dispatch('setLoginStatus', false);
                        iview.LoadingBar.finish();
                        if (data.isSuccess) {
                            this.$store.dispatch('setCurrentUser', this.user);
                            VueCookie.set('auth_token', data.token);
                           this.$router.push(this.$route.query.redirect || '/');
                        } else {
                            iview.Message.error(data.message);
                        }

                    }).catch(err => {
                    iview.LoadingBar.error();
                    this.$store.dispatch('setLoginStatus', false);
                });
            } else {
                iview.Message.error('请输入必填项!');
            }
        })
    };
    
    handleReset(name: any) {
        (<Vue> this.$refs[name]).resetFields();
    }

}