$(document).ready(async () => {

    $("#btnCaptcha").click(async () => {
        //调用验证码校验方法
        const answer = encodeURI($("#txtCaptcha").val() as string);

        const captcha = encodeURI(getCookie("Captcha"));

        var request: IVerifyRequest = {
            answer: answer,
            captcha: captcha
        };

        var result = await verifyAsync(request);
        alert(`message : ${result.message}, code : ${result.code}`);
    });
});
async function verifyAsync(request: IVerifyRequest): Promise<IVerifyResponse> {
    const url = "api/captcha/verify";
    const result = await HttpService.instance.postAsync<IVerifyResponse>(url, request);

    return result;
}

function getCookie(name) {
    //获取Cookie值
    const strCookie = document.cookie;
    const arrCookie = strCookie.split("; ");
    for (let i = 0; i < arrCookie.length; i++) {
        const arr = arrCookie[i].split("=");
        if (arr[0] === name) return arr[1];
    }
    return "";
}