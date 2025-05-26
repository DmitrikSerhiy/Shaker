function toTop() {
    if (location.pathname === "/allcocktails") {
        let allCocktailsScrollable = document.getElementById("all-cocktails-content-wrapper");
        allCocktailsScrollable.scrollTo({
            top: 0,
            behavior: 'smooth'
        });
    } else if (location.pathname === "/mycocktails") {
        let myCocktailsScrollable = document.getElementById("my-cocktails-content-wrapper");
        myCocktailsScrollable.scrollTo({
            top: 0,
            behavior: 'smooth'
        });
    } else if (location.pathname === "/ingredients") {
        let myIngredientsScrollable = document.getElementById("my-ingredients-content-wrapper");
        myIngredientsScrollable.scrollTo({
            top: 0,
            behavior: 'smooth'
        });
    }
}