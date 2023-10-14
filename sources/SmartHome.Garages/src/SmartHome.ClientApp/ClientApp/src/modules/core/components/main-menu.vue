<template>
    <v-navigation-drawer v-model="mainMenuVisible" class="main-menu" color="drawer" width="200" :scrim="false">
        <v-list nav density="compact" open-strategy="multiple" select-strategy="independent" class="main-menu-list">
            <template v-for="item in mainMenu" :key="item.title + ' group'">
                <v-list-item
                    class="main-menu-list__item ml-2"
                    :class="item.isActive ? 'bg-active-menu-item' : 'bg-menu-item'"
                    @click="openView(item.view)"
                >
                    {{ item.title }}
                </v-list-item>
                <v-divider />
            </template>
        </v-list>
    </v-navigation-drawer>
</template>

<script lang="ts" setup>
import { mainMenuItems } from '@/modules/core.constants';
import { View } from '@/router/view-definitions';
import { computed, onMounted } from 'vue';
import { useRouter } from 'vue-router';
import { useDisplay } from 'vuetify';

interface MainMenuGroupViewModel {
    view: View;
    title: string;
    isActive: boolean;
}

const router  = useRouter();

const openView = async (view: View) => await router.push({ name: view });

const mainMenu = computed<MainMenuGroupViewModel[]>(() => {
    return mainMenuItems.map(({ mainView, title }) => ({
        view: mainView,
        title: title,
        isActive:
            router.currentRoute.value.name === mainView
    }));
});


const emit = defineEmits(['update:modelValue']);
const props = defineProps({
    modelValue: { type: Boolean, required: true, default: false },
});
const mainMenuVisible = computed({
    get() {
        return props.modelValue;
    },
    set(value) {
        emit('update:modelValue', value);
    },
});

const { mobile } = useDisplay();

onMounted(() => {
    // when opening the page, display the main menu for large displays
    // and hide the main menu for mobile devices
    mainMenuVisible.value = !mobile.value;
});
</script>

<style lang="scss">
.v-navigation-drawer.main-menu {
    box-shadow: 0px 3px 6px #6161b829;
    border-right-width: 0;

    .v-list.main-menu-list {
        display: flex;
        flex-direction: column;
        justify-content: center;

        padding-left: 0;
        padding-right: 0;
        font-weight: 500;
        font-size: 13px;

        &.main-menu-list--with-left-border {
            margin: 16px 0 4px 24px;
            padding: 0 16px;
            border-left: 2px solid #1b93d5;
        }
    }

    .v-list-item--active > .v-list-item__overlay,
    .v-list-item[aria-haspopup='menu'][aria-expanded='true'] > .v-list-item__overlay {
        opacity: 0 !important;
    }

    .v-list-group__header.v-list-item--active:not(:focus-visible) .v-list-item__overlay {
        opacity: 0.12;
    }

    .v-divider.v-main-menu-divider {
        height: 2px;
        min-height: 2px;
        opacity: 1;
    }

    .v-list-item {
        &.list-item__identity-manager {
            .v-list-item__prepend > .v-img {
                margin-inline-end: 8px;
            }
            .v-list-item-title {
                color: #203f9a;
                opacity: 0.8;
                font-weight: 600;
            }
        }

        &.list-item__user-details {
            .v-list-item-title {
                font-size: 18px;
                font-weight: 600;
            }

            .v-list-item-subtitle {
                font-size: 12px;
                opacity: 1;
            }
        }
    }
}
</style>
