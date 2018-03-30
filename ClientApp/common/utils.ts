export function isEmptyObject(obj: object) {
    console.log(obj);
    for (let key in obj) {
        return false;
    }
    return true;
}
