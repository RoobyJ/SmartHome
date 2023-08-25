import { createApp } from 'vue';
import App from './app.vue';
import cmplCore from './plugins/cmpl-core';
import i18n from './plugins/i18n';
import pinia from './plugins/pinia';
import router from './plugins/router';
import vuetify from './plugins/vuetify';

const app = createApp(App);
app.use(i18n);
app.use(router);
app.use(pinia);
app.use(vuetify);
app.use(cmplCore);

router
    .isReady()
    .then(() => app.mount('#app'))
    .catch(e => console.error(e));
