// Please see documentation at https://docs.microsoft.com/aspnet/core/client-side/bundling-and-minification
// for details on configuring this project to bundle and minify static web assets.

// Write your JavaScript code.

const getCartCookieKey = (userId) => `cart_for_${userId}=`;

const getCartCookie = (userId) => {
    const cookiesString = document.cookie;
    const key = getCartCookieKey(userId);
    const cookies = cookiesString.split(';');
    for (const cookie of cookies) {
        const trimmedCookie = cookie.trim();
        if (trimmedCookie.startsWith(key)) {
            const cartCookie = trimmedCookie.substring(key.length);
            return JSON.parse(decodeURIComponent(cartCookie));
        }
    }

    return {};
}

const setCartCookieForProduct = (userId, productId, productCount) => {
    const cartCookie = getCartCookie(userId);
    cartCookie[productId] = productCount;
    const date = new Date()
    date.setTime(date.getTime() + getMilisecondsInDays(7))
    setCookie(getCartCookieKey(userId), JSON.stringify(cartCookie), date.toUTCString());
}

const removeProductFromCartCookie = (userId, productId) => {
    const cartCookie = getCartCookie(userId)
    const cartCookieKey = getCartCookieKey(userId);
    delete cartCookie[productId]
    const date = new Date()
    if (Object.keys(cartCookie).length > 0) {
        date.setTime(date.getTime() + getMilisecondsInDays(7))
    } else {
        date.setTime(0)
    }

    setCookie(cartCookieKey, JSON.stringify(cartCookie), date.toUTCString());
}

const setCookie = (keyWithEquals, value, expiresIn) => {
    document.cookie = `${keyWithEquals}${encodeURIComponent(value)};expires=${expiresIn};path=/;SameSite=Strict`;
}

const getMilisecondsInDays = (days) => 24 * 60 * 60 * 1000 * days;

const clampProductCountInput = (inputId, min, max) => {
    const input = document.getElementById(inputId)
    const inputValue = input.value
    if (inputValue > max) {
        input.value = max
    } else if (inputValue < min) {
        input.value = min
    }
}
