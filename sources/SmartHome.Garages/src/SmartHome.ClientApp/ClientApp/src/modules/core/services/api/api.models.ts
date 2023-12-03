import type { DayInWeek } from "../../core.models";

export interface GarageDetailsDto {
    id: number;
    name: string;
    heaterStatus: boolean | null;
    temperature: number | null;
}

export interface CyclicHeatTaskDto {
    id: number;
    garageId: number;
    time: string;
    daysInWeekSelected: number[];
}

export interface CustomHeatTaskDto {
    id: number;
    date: Date;
}

export interface TemperatureDto {
    id: number;
    date: Date;
    temperature: number;
    garageId: number;
}

export interface HeatRequestDto {
    date: Date;
}

export interface CreateCyclicHeatTaskDto {
    time: string;
    daysInWeekSelected: DayInWeek[];
}