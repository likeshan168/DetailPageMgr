import Vue from 'vue';
import {Component} from 'vue-property-decorator';

@Component({
    components: {
        // MenuComponent: require('../navmenu/navmenu.vue.html')
        MenuComponent: require('../navmenu/mymenu.vue.html'),
    }
})
export default class AppComponent extends Vue {
}
