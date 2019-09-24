interface IVerifyRequest {
    answer: string,
    captcha: string,
}

interface IVerifyResponse {
    code: number,
    message: string,
}