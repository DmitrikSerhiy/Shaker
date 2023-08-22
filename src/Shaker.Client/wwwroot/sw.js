const latestVersion = 7;
const VERSION = 'v' + latestVersion;
const baseUrl = 'https://myshaker.blob.core.windows.net/shaker-img/';

const defaultFilesToCacheUrls = [
    '/',
    '/index.html',
    '/favicon.png',
    '/icon-192.png',
    '/logo.png',
    '/logo-centred.svg',
    '/css/fonts/ReImagineUAMalevichVF.ttf',
    '/css/bootstrap/bootstrap.min.css',
    '/css/app.css'
];

const imagesToCacheUrls = [
    baseUrl + 'aviation.webp',
    baseUrl + 'blody_mary.webp',
    baseUrl + 'bmw.webp',
    baseUrl + 'boulevardier.webp',
    baseUrl + 'brain_damage.webp',
    baseUrl + 'cuba_lible.webp',
    baseUrl + 'daiquiri.webp',
    baseUrl + 'dart_weider.webp',
    baseUrl + 'dead_russian.webp',
    baseUrl + 'fitzgerald.webp',
    baseUrl + 'flamingo.webp',
    baseUrl + 'gin_tonic.webp',
    baseUrl + 'ginger_apple.webp',
    baseUrl + 'hemingway_daiquiri.webp',
    baseUrl + 'irish_coffee.webp',
    baseUrl + 'long_iceland_ice_tea.webp',
    baseUrl + 'lyutik.webp',
    baseUrl + 'mojito.webp',
    baseUrl + 'negroni_spagliato.webp',
    baseUrl + 'negroni.webp',
    baseUrl + 'new_york_sour.webp',
    baseUrl + 'old_fashioned.webp',
    baseUrl + 'pina_collada.webp',
    baseUrl + 'spritz.webp',
    baseUrl + 'tequila_sunrise.webp',
    baseUrl + 'zomby.webp',
];

const addResourcesToCache = async (resources) => {
    const cache = await caches.open(VERSION);
    await cache.addAll(resources);
};

const putInCache = async (request, response) => {
    const cache = await caches.open(VERSION);
    await cache.put(request, response);
};

const deleteCache = async (key) => {
    await caches.delete(key);
};

const deleteOldCaches = async () => {
    const cacheKeepList = [VERSION];
    const keyList = await caches.keys();
    const cachesToDelete = keyList.filter((key) => !cacheKeepList.includes(key));
    await Promise.all(cachesToDelete.map(deleteCache));
};


const cacheFirst = async ({ request, fallbackUrl }) => {
    const responseFromCache = await caches.match(request);
    if (responseFromCache) {
        return responseFromCache;
    }

    try {
        const responseFromNetwork = await fetch(request);
        putInCache(request, responseFromNetwork.clone());
        return responseFromNetwork;
    } catch (error) {
        const fallbackResponse = await caches.match(fallbackUrl);
        if (fallbackResponse) {
            return fallbackResponse;
        }

        return new Response("Network error happened", {
            status: 408,
            headers: { "Content-Type": "text/plain" },
        });
    }
};

const enableNavigationPreload = async () => {
    if (self.registration.navigationPreload) {
        await self.registration.navigationPreload.enable();
    }
};

self.addEventListener('install', function(event) {
    event.waitUntil(addResourcesToCache(imagesToCacheUrls));
});

self.addEventListener("fetch", (event) => {
    if (defaultFilesToCacheUrls.includes(event.request.url)) {
        event.respondWith(cacheFirst({
            request: event.request,
            fallbackUrl: "/logo-centred.svg",
        }));
    } else {
        event.respondWith(fetch(event.request));
    }
});

self.addEventListener("activate", (event) => {
    event.waitUntil(enableNavigationPreload());
    event.waitUntil(deleteOldCaches());
});