import { createApp } from 'vue';
import App from './app.vue';
import { initializeComponents } from './modules/core/core.components';
import i18n from './plugins/i18n';
import pinia from './plugins/pinia';
import router from './plugins/router';
import vuetify from './plugins/vuetify';

const app = createApp(App);
app.use(i18n);
app.use(router);
app.use(pinia);
app.use(vuetify);

initializeComponents(app);

router
    .isReady()
    .then(() => app.mount('#app'))
    .catch(e => console.error(e));
