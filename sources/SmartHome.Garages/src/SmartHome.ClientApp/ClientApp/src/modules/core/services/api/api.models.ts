export interface GarageDetailsDto {
    id: number;
    name: string;
    heaterStatus: boolean;
}

export interface CyclicHeatTaskDto {
    id: number;
    garageId: number;
    time: string;
    daysInWeekSelected: number[];
}