import { computed, ref } from 'vue';

export interface CountdownTimerOptions {
    seconds: number;
    callback: () => void;
}

export function useCountdownTimer(opt?: CountdownTimerOptions) {
    let callback: (() => void) | null = null;
    const intervalHandle = ref<ReturnType<typeof setInterval> | undefined>();
    const seconds = ref(0);
    const isCounting = computed(() => intervalHandle.value != null);

    function initializeCountdown(opt: CountdownTimerOptions) {
        if (opt.seconds < 1 || opt.seconds > 60) {
            throw new Error('Small intervals are allowed (1-60 seconds)');
        }

        stopCountdown();
        seconds.value = opt.seconds;
        callback = opt.callback;
    }

    function skipCountdown() {
        if (!isCounting.value) return;

        if (callback != null) callback();
        stopCountdown();
    }

    function startCountdown() {
        if (isCounting.value) return;

        intervalHandle.value = setInterval(() => {
            seconds.value = seconds.value - 1;

            if (seconds.value === 0) {
                if (callback != null) callback();
                stopCountdown();
            }
        }, 1000);
    }

    function stopCountdown() {
        if (!isCounting.value) return;

        clearInterval(intervalHandle.value);

        intervalHandle.value = undefined;
        seconds.value = 0;
    }

    if (opt != null) initializeCountdown(opt);

    return { seconds, isCounting, startCountdown, stopCountdown, skipCountdown, initializeCountdown };
}
