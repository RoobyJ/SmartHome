import { h, type Component } from 'vue';
import type { IconAliases, IconProps, IconSet } from 'vuetify';
import AddAppIcon from './add-app-icon.vue';
import CheckboxOffIcon from './checkbox-off-icon.vue';
import CheckboxOnIcon from './checkbox-on-icon.vue';
import SuccessIcon from './success-icon.vue';

// IMPORTANT: check readme.md for instructions about adding new icons

/**
 * Custom icons.
 *
 * Usage: `<v-icon>dm:add-app</v-icon>`
 **/
const components: Record<string, Component> = {
    'add-app': AddAppIcon,
    'checkbox-on': CheckboxOnIcon,
    'checkbox-off': CheckboxOffIcon,
    success: SuccessIcon,
};

/**
 * Custom icons
 *
 * Usage: `<v-icon>dm:add-app</v-icon>`
 **/
const dmIcons: IconSet = {
    component: (props: IconProps) => {
        const icon = String(props.icon);
        return h(props.tag, [h(components[icon], { class: 'v-icon__svg', role: 'img', 'aria-hidden': true })]);
    },
};

const aliases: Partial<IconAliases> = {
    checkboxOn: 'dm:checkbox-on',
    checkboxOff: 'dm:checkbox-off',
};

export { aliases, dmIcons };
