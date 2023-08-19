export interface IdTokenClaims {
    sub: string;
    email: string;
    name: string;
    givenName: string;
    familyName: string;
    picture: string | null;
}
