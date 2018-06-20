import './css/site.css';
import BootstrapVue from 'bootstrap-vue';

import Vue from 'vue';
import VueRouter from 'vue-router';
Vue.use(VueRouter);
Vue.use(BootstrapVue);

const routes = [
    { path: '/', component: require('./components/dependencytree/dependencytree.vue.html') }
];

new Vue({
    el: '#app-root',
    router: new VueRouter({ mode: 'history', routes: routes }),
    render: h => h(require('./components/app/app.vue.html'))
});
