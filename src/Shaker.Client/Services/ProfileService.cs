using Shaker.Client.Dtos;

namespace Shaker.Client.Services; 

public sealed class ProfileService {
    private readonly DataService _dataService;

    public ProfileService(DataService dataService) {
        _dataService = dataService ?? throw new ArgumentNullException(nameof(dataService));
    }

    public async Task AddProfileAsync(string barName) {
        var profiles = await _dataService.LoadProfilesAsync();
        var barId = new Random().Next(100000, 999999);
        while (profiles.Any(p => p.Id == barId)) {
            barId = new Random().Next(100000, 999999);
        }
        
        var bar = new Bar {Name = barName, Id = barId};
        await _dataService.CreateBarAsync(bar);
        profiles.Add(new Profile {Id = bar.Id, Name = bar.Name});
        await _dataService.UpdateProfilesAsync(profiles);
    }

    public async Task DeleteProfileAsync(Profile profile) {
        var profiles = await _dataService.LoadProfilesAsync();
        profiles.Remove(profile);
        await _dataService.UpdateProfilesAsync(profiles);
    }
}