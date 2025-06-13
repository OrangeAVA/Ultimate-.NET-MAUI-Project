using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using EducationalApp.Data;
using EducationalApp.Models;
using EducationalApp.Services;
using System.Collections.ObjectModel;
using System.Text.Json;

namespace EducationalApp.ViewModels;

public partial class MyCoursesViewModel : BaseViewModel
{
    [ObservableProperty]
    public List<Course> coursesList;

    [ObservableProperty]
    public ObservableCollection<Course> myCourses;

    [ObservableProperty]
    public bool isLoading = true;

    [ObservableProperty]
    public string? searchText;

    private readonly ICourseService _courseService;
    private readonly DatabaseContext _databaseContext;

    public MyCoursesViewModel(ICourseService courseService, DatabaseContext databaseContext)
    {
        CoursesList = new List<Course>();
        MyCourses = new ObservableCollection<Course>();
        _courseService = courseService;
        _databaseContext = databaseContext;
    }

    public async void GetData()
    {
        IsLoading = true;
        try
        {

            var registerCourses = (await _databaseContext.GetAllAsync<RegisterCourse>()).ToList();

            MyCourses.Clear();
            CoursesList = await _courseService.List() ?? new List<Course>();

            if (CoursesList != null)
            {
                foreach (var item in CoursesList.Where(x => registerCourses.Any(y => y.Course_Id == x.Id.ToString())))
                {
                    MyCourses.Add(item);
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
        await Shell.Current.GoToAsync($"CourseContentView?courseId={Id}");
    }
}