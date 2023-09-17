import { vi } from 'vitest';
import createFetchMocker from 'vitest-fetch-mock';

const fetchMocker = createFetchMocker(vi);
fetchMocker.enableMocks();
