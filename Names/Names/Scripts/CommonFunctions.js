


let SendHttpRequest = (method, url, data = null) => {

    const promise = new Promise((resolve, reject) => {
        const xhr = new XMLHttpRequest();
        xhr.open(method, url);
        xhr.responseType = 'json';
        xhr.onloadend = () => {
            if (xhr.status != 200)
                reject(xhr.status)
            else if (xhr.status == 200) {
                if (xhr.response != null) {
                    resolve(xhr.response)
                }
            }
        }

        if (method == 'GET') {
            xhr.send();
        } else {
            if (data === null || data === 'undefined') {

                reject('You send a request without values.');
            }
            xhr.send(data);
        }
    });
    return promise;
}