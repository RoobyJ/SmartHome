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

export interface TemperatureDto {
    id: number;
    date: Date;
    temperature: number;
    garageId: number;
}