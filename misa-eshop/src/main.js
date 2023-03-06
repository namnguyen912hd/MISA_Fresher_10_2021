import Vue from 'vue'
import App from './App.vue'
import { router } from './router/router'
import vSelect from 'vue-select'
import Vuelidate from 'vuelidate'
import VueMask from 'v-mask'
import { VTooltip } from 'v-tooltip'
import axios from 'axios'
import VueAxios from 'vue-axios'
import money from 'v-money'

Vue.directive('tooltip', VTooltip)

export const eventBus = new Vue()
Vue.config.productionTip = false
Vue.component('v-select', vSelect)
Vue.use(Vuelidate)
Vue.use(VueMask);
Vue.use(VueAxios, axios)
Vue.use(money, {precision: 4})
Vue.use(require('vue-shortkey'))

new Vue({
  router,
  vSelect,
  render: h => h(App),
}).$mount('#app')