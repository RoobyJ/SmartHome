/**
 * plugins/vuetify.ts
 *
 * Framework documentation: https://vuetifyjs.com`
 */

// Styles
import '@mdi/font/css/materialdesignicons.css'
import 'vuetify/styles'
import * as labsComponents from 'vuetify/labs/components';
import * as components from 'vuetify/components';
import * as directives from 'vuetify/directives';

// Composables
import { createVuetify, type VuetifyOptions } from 'vuetify'

const config: VuetifyOptions = {theme: {
  defaultTheme: 'smartHome',
  themes: {
    smartHome: {
          dark: false,
          colors: {
              surface: '#ffffff',
              primary: '#2488cf',
              secondary: '#5CBBF6',
          },
      },
  },
},}

if (import.meta.env.DEV) {
  // Disable tree-shaking for DEV mode.
  config.components = { ...components, ...labsComponents };
  config.directives = { ...directives };
}

config.components = { ...config.components, ...labsComponents };

// https://vuetifyjs.com/en/introduction/why-vuetify/#feature-guides
export default createVuetify(config);
