export interface GarageDetailsDto {
    id: number;
    name: string;
    heaterStatus: boolean;
}

export interface CyclicHeatRequestDto {
    garageId: number;
    monday: string;
    tuesday: string;
    wednesday: string;
    thursday: string;
    friday: string;
    saturday: string;
    sunday: string;
}