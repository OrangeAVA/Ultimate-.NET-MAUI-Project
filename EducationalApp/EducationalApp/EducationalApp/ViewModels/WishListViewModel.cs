using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using EducationalApp.Data;
using EducationalApp.Models;
using EducationalApp.Services;
using System.Collections.ObjectModel;
using System.Text.Json;

namespace EducationalApp.ViewModels;

public partial class WishListViewModel : BaseViewModel
{
    [ObservableProperty]
    public List<Course> coursesList;

    [ObservableProperty]
    public ObservableCollection<Course> wishList;

    [ObservableProperty]
    public bool isLoading = true;

    [ObservableProperty]
    public string? searchText;

    private readonly ICourseService _courseService;
    private readonly DatabaseContext _databaseContext;

    public WishListViewModel(ICourseService courseService, DatabaseContext databaseContext)
    {
        CoursesList = new List<Course>();
        WishList = new ObservableCollection<Course>();
        _courseService = courseService;
        _databaseContext = databaseContext;
    }

    public async void GetData()
    {
        IsLoading = true;
        try
        {

            var favorites = (await _databaseContext.GetAllAsync<Favorite>()).ToList();

            WishList.Clear();
            CoursesList = await _courseService.List() ?? new List<Course>();

            if (CoursesList != null)
            {
                foreach (var item in CoursesList.Where(x => favorites.Any(y => y.Course_Id == x.Id.ToString())))
                {
                    WishList.Add(item);
                }
            }

        }
        catch (JsonException jsonEx)
        {
            Console.WriteLine($"JSON Error: {jsonEx.Message}");
        }
        catch (Exception ex)
        {
            Console.WriteLine($"General Error: {ex.Message}");
        }
        finally
        {
            IsLoading = false;
        }
    }

    [RelayCommand]
    public async Task Description(Guid Id)
    {
        await Shell.Current.GoToAsync($"CourseDescriptionView?courseId={Id}");
    }
}