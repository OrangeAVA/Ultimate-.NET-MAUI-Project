using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using EducationalApp.Data;
using EducationalApp.Models;
using EducationalApp.Services;
using System.Collections.ObjectModel;

namespace EducationalApp.ViewModels;

public partial class CourseDescriptionViewModel : BaseViewModel, IQueryAttributable
{

    public void ApplyQueryAttributes(IDictionary<string, object> query)
    {
        _courseId = query["courseId"] as string;
    }

    private string? _courseId { get; set; }

    [ObservableProperty]
    public string buttonText;

    [ObservableProperty]
    public Course course;

    [ObservableProperty]
    public string heartIcon;

    [ObservableProperty]
    public bool isEnabledButton = false;

    private readonly DatabaseContext _databaseContext;
    private readonly ICourseService _courseService;

    public CourseDescriptionViewModel(DatabaseContext databaseContext, ICourseService courseService)
    {
        HeartIcon = "heart.png";
        ButtonText = "Register";
        _databaseContext = databaseContext;
        _courseService = courseService;
    }

    public async void ConsultCourse()
    {
        IsEnabledButton = false;
        var id = Guid.Parse(_courseId);

        Course = (await _courseService.List()).FirstOrDefault(x => x.Id.ToString() == _courseId);

        if (Course != null)
        {
            this.IsFavorite();
        }

        if((await _databaseContext.GetFileteredAsync<RegisterCourse>(
                   x => x.Course_Id == _courseId)).FirstOrDefault() != null)
        {
            ButtonText = "Unregister";
        }
    }

    public async void IsFavorite()
    {
        try
        {
            var idCourse = Course.Id.ToString() ?? string.Empty;
            if (!string.IsNullOrWhiteSpace(idCourse))
            {
                var favorite = (await _databaseContext.GetFileteredAsync<Favorite>(
                    x => x.Course_Id == idCourse))
                    .Select(x => (Guid?)x.Id)
                    .FirstOrDefault()?.ToString();

                if (!string.IsNullOrWhiteSpace(favorite))
                {
                    HeartIcon = "heartselect.png";
                }
            }
        }
        catch (Exception ex)
        {
            Console.WriteLine(ex.Message);
        }
    }

    [RelayCommand]
    public async Task HeartIconTap()
    {
        await this.AddOrRemoveToFavorites();
    }

    private async Task AddOrRemoveToFavorites()
    {
        try
        {
            if (HeartIcon.Equals("heart.png"))
            {
                var favorite = new Favorite()
                {
                    Id = Guid.NewGuid(),
                    Course_Id = Course.Id.ToString()
                };
                if (await _databaseContext.AddItemAsync(favorite))
                {
                    HeartIcon = "heartselect.png";
                }
            }
            else
            {
                var idCourse = Course.Id.ToString();
                var favorites = (await _databaseContext.GetFileteredAsync<Favorite>
                            (x => x.Course_Id == idCourse)).Select(x => x.Id).ToList();
                if (favorites is null) return;
                foreach (var item in favorites)
                {
                    if (await _databaseContext.DeleteItemByKeyAsync<Favorite>(item))
                    {
                        HeartIcon = "heart.png";
                    }
                }
            }
        }
        catch (Exception ex)
        {
            Console.WriteLine(ex.Message);
        }
    }

    [RelayCommand]
    public async Task TapButton()
    {
        if (!ButtonText.Equals("Unregister"))
        {
            await this.Register();
        }
        else
        {
            await this.Unregister();
        }
    }

    private async Task Register()
    {
        try
        {
            var registerCourse = new RegisterCourse()
            {
                Id = Guid.NewGuid(),
                Course_Id = Course.Id.ToString()
            };
            if (await _databaseContext.AddItemAsync(registerCourse))
            {
                ButtonText = "Unregister";
            }
        }
        catch (Exception ex)
        {
            Console.WriteLine(ex.Message);
        }
    }

    private async Task Unregister()
    {
        try
        {
            var removed = false;
            var idCourse = Course.Id.ToString();
            var itemForRemove = (await _databaseContext.GetFileteredAsync<RegisterCourse>
                        (x => x.Course_Id == idCourse)).FirstOrDefault();
            if (itemForRemove is null) return;
            if (await _databaseContext.DeleteItemAsync<RegisterCourse>(itemForRemove))
            {
                removed = true;
            }
            if (removed)
                ButtonText = "Register";
        }
        catch (Exception ex)
        {
            Console.WriteLine(ex.Message);
        }
    }
}
