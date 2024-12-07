export class RegisterDto {
    constructor(
        public name: string,
        public login: string,
        public password: string
    ) {}
}