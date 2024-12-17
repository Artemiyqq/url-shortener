export class ShortenedUrlDto {
    constructor(public id: number,
                public shortUrl: string,
                public longUrl: string) {}
}