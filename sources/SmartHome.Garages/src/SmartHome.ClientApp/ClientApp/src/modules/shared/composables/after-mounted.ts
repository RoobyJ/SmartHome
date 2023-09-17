import { nextTick, onMounted, ref } from 'vue';

/**
 * Provides boolean flag that is set to true after the component is mounted, so the teleport can be used.
 **/
export function useAfterMounted() {
    const afterMounted = ref(false);

    onMounted(async () => {
        await nextTick(() => {
            afterMounted.value = true;
        });
    });

    return { afterMounted };
}
