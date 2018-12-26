class HttpService {
    public static readonly instance = new HttpService();

    private constructor() {

    }

    async getAsync<T>(url: string): Promise<T> {
        try {
            const result = await $.ajax(url, { type: "GET" });
            return result as T;
        } catch (e) {
            alert(e);
        }
        return null;
    }

    async postAsync<T>(url: string, data?: any): Promise<T> {
        try {
            const result = await $.ajax({
                url: url,
                type: "POST",
                dataType: "json",
                data: JSON.stringify(data),
                contentType: 'application/json; charset=utf-8'
            });
            return result as T;
        } catch (e) {
            alert(e);
        }
        return null;
    }

}