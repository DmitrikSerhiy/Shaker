namespace Shaker.Client.Helpers; 

public static class GlassTypeHelper {
    public static string GetGlassName(this GlassType type) {
        return type switch
        {
            GlassType.None => "",
            GlassType.MartiniGlass => "Мартіні",
            GlassType.MargarithaGlass => "Маргарита",
            GlassType.WineGlass => "Келих для вина",
            GlassType.CollinsGlass => "Колінз",
            GlassType.Mug => "Кухоль",
            GlassType.NickAndNora => "Нік & Нора",
            GlassType.OldFashionedGlass => "Олд фешн",
            GlassType.ShotGlass => "Шот",
            GlassType.Tumbler => "Тумблер",
            GlassType.CoffeeGlass => "Айріш кава",
            GlassType.FluteGlass => "Флюте",
            GlassType.SnifterGlass => "Сніфтер",
            GlassType.HurricaneGlass => "Гарікейн",
            GlassType.ZombieGlass => "Зомбі",
            _ => ""
        };
    }
}