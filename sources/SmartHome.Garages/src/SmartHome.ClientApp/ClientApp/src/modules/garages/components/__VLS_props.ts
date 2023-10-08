import type { CyclicHeatTaskDto } from '@/modules/core/services/api/api.models';

const __VLS_props = defineProps({
heatTask: { type: Object as PropType<CyclicHeatTaskDto | null>, required: true }
});
