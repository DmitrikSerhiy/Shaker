namespace Shaker; 

public sealed class IngredientService {
    private readonly Bar _bar;

    public IngredientService(Bar bar)
    {
        _bar = bar;
    }

    public void AddIngredient(Ingredient ingredient)
    {
        _bar.AvailableIngredients.Add(ingredient);
    }

    public void RemoveIngredient(int ingredientId)
    {
        _bar.AvailableIngredients.RemoveAll(i => i.Id == ingredientId);
    }

    public Ingredient GetIngredient(int ingredientId)
    {
        return _bar.AvailableIngredients.FirstOrDefault(i => i.Id == ingredientId);
    }

    public void UpdateIngredient(Ingredient updatedIngredient)
    {
        var existingIngredient = _bar.AvailableIngredients.FirstOrDefault(i => i.Id == updatedIngredient.Id);

        if (existingIngredient != null)
        {
            existingIngredient.Name = updatedIngredient.Name;
        }
    }
}