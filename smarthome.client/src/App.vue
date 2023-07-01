<template>
  <v-app>
    <top-bar v-if="$route.meta.displayTopBar" />
    <notification-holder />
    <v-main>
      <router-view :key="$route.path" />
    </v-main>
    <vue-progress-bar />
    <error-dialog />
  </v-app>
</template>

<script lang="ts">
import ErrorDialog from '@/modules/core/components/error-dialog.vue';
import NotificationHolder from '@/modules/core/components/notifications-holder.vue';
import TopBar from '@/modules/core/components/top-bar.vue';
import { Component, Vue } from 'vue-property-decorator';

@Component({ components: { TopBar, ErrorDialog, NotificationHolder } })
export default class App extends Vue {
  private isRefreshing = false;

  public override created(): void {
    document.addEventListener(
        'serviceWorkerUpdated',
        (event) => {
          // eslint-disable-next-line @typescript-eslint/no-explicit-any
          const registration = (event as any).detail;

          if (!registration || !registration.waiting) return;

          // If detected a new service worker (which means that the app was updated)
          // then update the service worker immediately.
          registration.waiting.postMessage('skipWaiting');
        },
        { once: true }
    );

    if (navigator.serviceWorker) {
      navigator.serviceWorker.addEventListener('controllerchange', () => {
        if (this.isRefreshing) return;
        this.isRefreshing = true;

        // if the service worker has changed, then reload the page
        window.location.reload();
      });
    }
  }
}
</script>

<style lang="scss">
@import '@/styles/main.scss';
</style>
