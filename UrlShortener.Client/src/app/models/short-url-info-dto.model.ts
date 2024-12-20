export class ShortUrlInfoDto {
    constructor(public createdBy: string,
                public createdDate: Date,
                public usageCount: number,
                ) {}
}