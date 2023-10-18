import { createRouter, createWebHistory } from 'vue-router'
import HomeView from '@/modules/home/views/home-page.vue'
import GarageListView from '@/modules/garages/views/garage-list.vue'
import GaragePageView from '@/modules/garages/views/garage-page.vue'
import DefaultView from '@/modules/core/views/default-view.vue'
import TemperatureView from '@/modules/garages/views/temperature-view.vue'
import TaskView from '@/modules/garages/views/task-view.vue'
import { View } from './view-definitions';

const router = createRouter({
  history: createWebHistory(import.meta.env.BASE_URL),
  routes: [
    {
      path: '/',
      component: DefaultView,
      children: [
        {
          path: '',
          name: View.homeView,
          component: HomeView,
        },
        {
          path: 'garages',

          component: DefaultView,
          children: [
            {
              path: '',
              name: View.garageListView,
              component: GarageListView,
            },
            {
              path: ':garageId/garage',
              name: View.garagePageView,
              component: GaragePageView
            },
            {
              path: ':garageId/temperatures',
              name: View.temperatureView,
              component: TemperatureView
            },
            {
              path: ':garageId/heat-tasks',
              name: View.taskView,
              component: TaskView
            },
          ]
        },
      ]
    },
  ]
},
)

export default router
