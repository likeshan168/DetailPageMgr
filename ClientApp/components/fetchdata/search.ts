import Vue from 'vue';
import {Component} from 'vue-property-decorator';


@Component
export default class SearchComponent extends Vue {
    
    formInline = {
        searchWords: ''
    };
}