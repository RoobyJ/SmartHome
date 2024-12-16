import type { DayInWeek } from '../../core.models';

export interface GarageDetailsDto {
    id: number;
    name: string;
    heaterStatus: boolean | null;
    temperature: number | null;
}

export interface HeatTaskBase {
    id: number;
    garageId: number;
    isActive: boolean;
}

export interface CyclicHeatTaskDto extends HeatTaskBase {
    time: string;
    daysInWeekSelected: number[];
}

export interface CustomHeatTaskDto extends HeatTaskBase {
    date: Date;
}

export interface TemperatureDto {
    id: number;
    date: Date;
    temperature: number;
    garageId: number;
}

export interface NewCustomHeatTaskDto {
    date: Date;
}

export interface NewCyclicHeatTaskDto {
    time: string;
    daysInWeekSelected: DayInWeek[];
}

export interface SetHeatTaskActiveDto {
    id: number;
    active: boolean;
    isCyclic: boolean;
}
