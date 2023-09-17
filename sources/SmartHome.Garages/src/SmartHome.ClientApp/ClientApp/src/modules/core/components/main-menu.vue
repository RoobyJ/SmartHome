<template>
    <v-navigation-drawer v-model="mainMenuVisible" class="main-menu" color="drawer" width="300">
        <template #prepend>
            <v-list-item
                class="bg-white list-item__identity-manager"
                lines="one"
                title="Identity Manager"
                :style="identityManagerTileStyle"
            >
                <template #prepend>
                    <v-img width="40" src="@/assets/cmpl-logo.svg" />
                </template>
            </v-list-item>

            <v-divider color="drawer" class="v-main-menu-divider" />

            <v-list-item class="bg-white list-item__user-details" lines="two" :title="name ?? ''">
                <template #prepend>
                    <v-avatar color="primary" size="small">{{ initials }}</v-avatar>
                </template>

                <v-list-item-subtitle class="d-flex align-center">
                    <v-icon color="readonly" size="12" class="mr-1">mdi-email-outline</v-icon>
                    <div class="text-truncate">
                        {{ email }}
                    </div>
                </v-list-item-subtitle>
            </v-list-item>

            <v-divider color="#F4F4F5" class="v-main-menu-divider" />
        </template>

        <v-list nav density="compact" open-strategy="multiple" select-strategy="independent" class="main-menu-list">
            <template v-for="item in mainMenu" :key="item.title + ' group'">
                <v-list-item
                    v-if="item.children.length === 0"
                    class="main-menu-list__item mb-3"
                    :class="item.isActive ? 'bg-active-menu-item' : 'bg-menu-item'"
                    @click="openView(item.view)"
                >
                    {{ t(item.title) }}
                    <template #prepend>
                        <v-avatar
                            :class="item.isActive ? 'bg-active-menu-icon' : 'bg-menu-icon'"
                            rounded="0"
                            size="32"
                            class="mr-3"
                        >
                            <v-icon :size="15">
                                {{ item.icon }}
                            </v-icon>
                        </v-avatar>
                    </template>
                </v-list-item>

                <v-list-group v-else :active="item.isActive" class="mb-3">
                    <template #activator="{ props: { onClick, ...restProps }, isOpen }">
                        <v-list-item
                            v-bind="restProps"
                            :active="item.isActive"
                            class="main-menu-list__item mb-0"
                            :class="item.isActive ? 'bg-active-menu-item' : 'bg-menu-item'"
                            @click="openGroupItemView(item.view, item.isActive, isOpen, onClick as any)"
                        >
                            {{ t(item.title) }}
                            <template #prepend>
                                <v-avatar
                                    :class="item.isActive ? 'bg-active-menu-icon' : 'bg-menu-icon'"
                                    rounded="0"
                                    size="32"
                                    class="mr-3"
                                >
                                    <v-icon :size="15">
                                        {{ item.icon }}
                                    </v-icon>
                                </v-avatar>
                            </template>
                        </v-list-item>
                    </template>
                    <v-list density="compact" class="main-menu-list main-menu-list--with-left-border">
                        <template v-for="childView in item.children" :key="childView.title + '-groupItem'">
                            <v-list-item :to="{ name: childView.view }" color="#0288d1">
                                {{ t(childView.title) }}
                            </v-list-item>
                        </template>
                    </v-list>
                </v-list-group>
            </template>
        </v-list>
    </v-navigation-drawer>
</template>

<script lang="ts" setup>
import { mainMenuItems } from '@/modules/core.constants';
import { View } from '@/plugins/router/view-definitions';
import { viewMetaDefinitions } from '@/plugins/router/view-metas';
import { storeToRefs } from 'pinia';
import type { StyleValue } from 'vue';
import { computed, onMounted } from 'vue';
import { useI18n } from 'vue-i18n';
import { useRouter } from 'vue-router';
import { useDisplay, useLayout } from 'vuetify';
import { useAccountStore } from '../store/account-store';

interface MainMenuChildViewModel {
    view: View;
    title: string;
}

interface MainMenuGroupViewModel {
    view: View;
    title: string;
    icon: string;
    isActive: boolean;
    children: MainMenuChildViewModel[];
}

const router = useRouter();

const openView = async (view: View) => await router.push({ name: view });

const openGroupItemView = async (view: View, isActive: boolean, isOpen: boolean, toggleMenu: () => void) => {
    // always open the specified view
    await openView(view);

    // but toggleMenu visibility by our custom conditions
    // !isActive && isOpen -> return
    // !isActive && !isOpen -> toggleMenu()
    // isActive && isOpen -> toggleMenu()
    // isActive && !isOpen -> toggleMenu()

    if (!isActive && isOpen) return;

    toggleMenu();
};

const { getLayoutItem } = useLayout();
const identityManagerTileStyle = computed<StyleValue>(() => {
    const height = getLayoutItem('app-bar')?.size;
    if (height == null || height <= 0) return {};

    return {
        minHeight: `${height}px`,
    };
});

const mainMenu = computed<MainMenuGroupViewModel[]>(() => {
    return mainMenuItems.map(({ mainView, children }) => ({
        view: mainView,
        title: viewMetaDefinitions[mainView].title,
        icon: viewMetaDefinitions[mainView].icon ?? '',
        isActive:
            router.currentRoute.value.name === mainView || children.includes(router.currentRoute.value.name as View),
        children: children.map(childView => ({
            view: childView,
            title: viewMetaDefinitions[childView].title,
        })),
    }));
});

const { name, initials, email } = storeToRefs(useAccountStore());

const { t } = useI18n();

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

        padding: 16px;
        font-weight: 500;
        font-size: 13px;

        &.main-menu-list--with-left-border {
            margin: 16px 0 4px 24px;
            padding: 0 16px;
            border-left: 2px solid #1b93d5;
        }

        .main-menu-list__item {
            padding-right: 16px;
            box-shadow: 0 2px 6px #6161b829;
            height: 48px;
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
