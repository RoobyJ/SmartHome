import { createRouter, createWebHistory } from 'vue-router'
import HomeView from '@/modules/home/views/home-page.vue'
import GarageListView from '@/modules/garages/views/garage-list.vue'
import { View } from './view-definitions';

const router = createRouter({
  history: createWebHistory(import.meta.env.BASE_URL),
  routes: [
    {
      path: '/',
      name: View.homeView,
      component: HomeView
    },
    {
      path: '/garages',
      name: View.garageListView,
      component: GarageListView
    }
  ]
})

export default router
