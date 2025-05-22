using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using EducationalApp.Models;
using EducationalApp.Services;
using System.Collections.ObjectModel;
using System.Text.Json;

namespace EducationalApp.ViewModels;

public partial class SearchViewModel : BaseViewModel
{

    [ObservableProperty]
    public List<Course> coursesList;

    [ObservableProperty]
    public ObservableCollection<Course> filteredCoursesList;

    [ObservableProperty]
    public bool isLoading = true;

    [ObservableProperty]
    public string? searchText;

    private readonly ICourseService _courseService;

    public SearchViewModel(ICourseService courseService)
	{
        CoursesList = new List<Course>();
        FilteredCoursesList = new ObservableCollection<Course>();
        _courseService = courseService;
    }

    public async void GetData()
    {
        IsLoading = true;
        try
        {
            FilteredCoursesList.Clear();
            CoursesList = await _courseService.List() ?? new List<Course>();

            if (CoursesList != null)
            {
                foreach (var item in CoursesList)
                {
                    FilteredCoursesList.Add(item);
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

    partial void OnSearchTextChanged(string? value)
    {
        if (string.IsNullOrWhiteSpace(value))
        {
            FilteredCoursesList.Clear();
            foreach (var product in CoursesList)
            {
                FilteredCoursesList.Add(product);
            }
        }
        else
        {
            var filtered = CoursesList.Where(p =>
                (p.Name?.Contains(value, StringComparison.OrdinalIgnoreCase) ?? false) ||
                (p.Description?.Contains(value, StringComparison.OrdinalIgnoreCase) ?? false)).ToList();

            FilteredCoursesList.Clear();
            foreach (var product in filtered)
            {
                FilteredCoursesList.Add(product);
            }
        }
    }

    [RelayCommand]
    public async Task Description(Guid Id)
    {
        await Shell.Current.GoToAsync($"CourseDescriptionView?courseId={Id}");
    }
}