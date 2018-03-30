
import  Vue from 'vue';
import {Component} from 'vue-property-decorator';
import VueCookie from "vue-cookies";

@Component
export default class Mymenu extends Vue{
    logout(){
        VueCookie.remove("auth_token");
        this.$router.push('/login');
    }
}