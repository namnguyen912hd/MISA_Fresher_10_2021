import Vue from 'vue'
import VueRouter from 'vue-router'
import ProductList from '../views/product/ProductList.vue'
import ProductDetail from '../views/product/ProductDetail.vue'
import CalculationUnitList from '../views/calculationunit/CalculationUnitList.vue'

Vue.use(VueRouter)

export const router = new VueRouter({
    mode: 'history',
    routes: [
        {path: '/products', component: ProductList},
        {path: '/productdetail', component: ProductDetail},
        {path: '/CalculationUnit', component: CalculationUnitList},
        // {path: '', redirect: '/products'},
    ]
})