export interface HeatRequestDto {
    time: Date;
}

export interface OutsideTemperatureDto {
    id: number;
    date: Date;
    temperature: number;
    garageId: number;
}
