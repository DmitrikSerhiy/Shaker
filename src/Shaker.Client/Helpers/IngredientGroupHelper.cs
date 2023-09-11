namespace Shaker.Client.Helpers; 

public static class IngredientGroupHelper {
    public static string GetGroupName(this IngredientGroup group) {
        return group switch
        {
            IngredientGroup.None => "",
            IngredientGroup.Strong => "Міцні напої",
            IngredientGroup.Middle => "Слабоалкогольні напої",
            IngredientGroup.Alcofree => "Безалкогольні напої",
            IngredientGroup.Juice => "Соки",
            IngredientGroup.Liqueur => "Лікери",
            IngredientGroup.Etc => "Різне",
            _ => ""
        };
    }
}