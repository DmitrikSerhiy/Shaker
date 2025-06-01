function setThemeManager() {
    setTheme(getPreferredTheme())

    window.matchMedia('(prefers-color-scheme: dark)').addEventListener('change', () => {
        const storedTheme = getStoredTheme()
        if (storedTheme !== 'light' && storedTheme !== 'dark') {
            setTheme(getPreferredTheme())
        }
    })
}

function onThemeChanged() {
    const themeSwitcher = document.querySelector('#dark-switch')
    const theme = themeSwitcher.checked ? 'dark' : 'light';
    setStoredTheme(theme);
    setTheme(theme);
    showActiveTheme(theme, true);
    muteUnmuteImages();
}

function initThemeSwitcher() {
    showActiveTheme(getPreferredTheme());
};


const setTheme = (theme) => {
    if (theme === 'auto' && window.matchMedia('(prefers-color-scheme: dark)').matches) {
        document.documentElement.setAttribute('data-bs-theme', 'dark')
    } else {
        document.documentElement.setAttribute('data-bs-theme', theme)
    }

    console.log('theme is set to ' + theme)
}

const getStoredTheme = () => localStorage.getItem('theme');
const setStoredTheme = theme => localStorage.setItem('theme', theme);

const showActiveTheme = (theme, focus = false) => {
    const themeSwitcher = document.querySelector('#dark-switch')
    const themeSwitcherLabel = document.querySelector('#dark-switch-label')

    if (!themeSwitcher) {
        return
    }

    const themeSwitcherText = theme === 'dark' ? 'на очі не бачу' : 'ще тверезий';
    themeSwitcherLabel.textContent = themeSwitcherText;
    themeSwitcher.setAttribute('aria-label', themeSwitcherText)
    themeSwitcher.checked = theme === 'dark';

    if (focus) {
        themeSwitcher.focus()
    }
}

const getPreferredTheme = () => {
    const storedTheme = getStoredTheme()
    if (storedTheme) {
        return storedTheme
    }

    return window.matchMedia('(prefers-color-scheme: dark)').matches ? 'dark' : 'light'
}

const muteImages = () => {
    document.querySelectorAll('.cocktail-image')
        .forEach(image => {
            image.classList.add('cocktail-image-dark');
        });
}

const unmuteImages = () => {
    document.querySelectorAll('.cocktail-image')
        .forEach(image => {
            image.classList.remove('cocktail-image-dark')
        });
}

function muteUnmuteImages() {
    if (getPreferredTheme() === 'dark') {
        muteImages();
    } else {
        unmuteImages();
    }
}