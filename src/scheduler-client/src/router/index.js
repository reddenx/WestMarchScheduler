import Vue from 'vue'
import VueRouter from 'vue-router'
import HomePage from '@/views/HomePage'
import StatusPage from '@/views/StatusPage'

Vue.use(VueRouter)

const routes = [
  {
    path: '/',
    name: 'Home',
    component: HomePage
  },
  {
    path: '/:lookupKey',
    name: 'Status',
    component: StatusPage,
    props: true,
  }
]

const router = new VueRouter({
  routes
})

export default router