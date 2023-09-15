const latestVersion = 10;
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
    '/css/app.css',
    '/scripts/bootstrap/bootstrap.bundle.min.js',
    '/scripts/themeManager.js'
];

const imagesToCacheUrls = [
    "adjara_gudju.webp",
    "amareto_sour.webp",
    "aviation.webp",
    "baby_ginnes.webp",
    "bellini.webp",
    "bloody_mary.webp",
    "blue_lady.webp",
    "blue_lagoone.webp",
    "bmw.webp",
    "boulevardier.webp",
    "brain_damage.webp",
    "bronx.webp",
    "brulee.webp",
    "crimea.webp",
    "cuba_libre.webp",
    "daiquiri.webp",
    "dart_weider.webp",
    "dead_russian.webp",
    "disappointment.webp",
    "east_8_hold_up.webp",
    "el_diablo.webp",
    "fitzgerald.webp",
    "flamingo.webp",
    "french_75.webp",
    "gin_buk.webp",
    "gin_tonic.webp",
    "ginger_apple.webp",
    "god_father.webp",
    "god_mather.webp",
    "gold_rush.webp",
    "greek_spritz.webp",
    "hemingway_daiquiri.webp",
    "irish_coffee.webp",
    "kominsky_method.webp",
    "long_iceland.webp",
    "lutik.webp",
    "margarita.webp",
    "martinez.webp",
    "mojito.webp",
    "negroni_spagliato.webp",
    "negroni.webp",
    "new_york_sour.webp",
    "old_fashioned.webp",
    "olexandra.webp",
    "orange_blossom.webp",
    "patriot_junior.webp",
    "patriot.webp",
    "pina_collada.webp",
    "rossini.webp",
    "sazerak.webp",
    "screwdriver.webp",
    "sea_breeze.webp",
    "sex_on_the_beach.webp",
    "shotgun.webp",
    "spritz.webp",
    "tequila_sunrise.webp",
    "water_mellon_spritz.webp",
    "white_lady.webp",
    "whyskey_buk.webp",
    "whyskey_sour.webp",
    "zomby.webp"
].map((img) => baseUrl + img);
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

const cacheFirst = async ({ request, preloadResponsePromise, fallbackUrl }) => {
    const responseFromCache = await caches.match(request);
    if (responseFromCache) {
        return responseFromCache;
    }

    const preloadResponse = await preloadResponsePromise;
    if (preloadResponse) {
        console.info("using preload response", preloadResponse);
        putInCache(request, preloadResponse.clone());
        return preloadResponse;
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
    if (defaultFilesToCacheUrls.includes(event.request.url) || imagesToCacheUrls.includes(event.request.url)) {
        event.respondWith(
            cacheFirst({
                request: event.request,
                preloadResponsePromise: event.preloadResponse,
                fallbackUrl: "/logo-centred.svg",
            }),
        );
    } else {
        event.respondWith(fetch(event.request));
    }
});

self.addEventListener("activate", (event) => {
    event.waitUntil(
        Promise.all([enableNavigationPreload(), deleteOldCaches()])
    );
});