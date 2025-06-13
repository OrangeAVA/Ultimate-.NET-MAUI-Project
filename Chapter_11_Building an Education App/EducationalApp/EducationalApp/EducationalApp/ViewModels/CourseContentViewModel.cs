using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using EducationalApp.Data;
using EducationalApp.Models;
using EducationalApp.Services;

namespace EducationalApp.ViewModels;

public partial class CourseContentViewModel : BaseViewModel, IQueryAttributable
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
    public bool isEnabledButton = false;

    private readonly DatabaseContext _databaseContext;
    private readonly ICourseService _courseService;
    public CourseContentViewModel(DatabaseContext databaseContext, ICourseService courseService)
    {
        ButtonText = "Complete";
        _databaseContext = databaseContext;
        _courseService = courseService;
    }

    public async void ConsultCourse()
    {
        IsEnabledButton = true;
        var id = Guid.Parse(_courseId);

        Course = (await _courseService.List()).FirstOrDefault(x => x.Id.ToString() == _courseId);

        if ((await _databaseContext.GetFileteredAsync<RegisterCourse>(
                   x => x.Course_Id == _courseId && x.Completed)).FirstOrDefault() != null)
        {
            ButtonText = "Completed";
            IsEnabledButton = false;
        }
    }


    [RelayCommand]
    public async Task TapButton()
    {
        if (!ButtonText.Equals("Completed"))
        {
            await this.Complete();
        }
        else
        {
            await this.Completed();
        }
    }

    private async Task Complete()
    {
        try
        {
            var registerCourse = (await _databaseContext.GetFileteredAsync<RegisterCourse>(
                   x => x.Course_Id == _courseId && !x.Completed)).FirstOrDefault();
            if (registerCourse != null)
            {
                registerCourse.Completed = true;
                if (await _databaseContext.UpdateItemAsync(registerCourse))
                {
                    ButtonText = "Completed";
                    IsEnabledButton = false;
                }
            }
        }
        catch (Exception ex)
        {
            Console.WriteLine(ex.Message);
        }
    }

    private async Task Completed()
    {
        try
        {
           
        }
        catch (Exception ex)
        {
            Console.WriteLine(ex.Message);
        }
    }
}