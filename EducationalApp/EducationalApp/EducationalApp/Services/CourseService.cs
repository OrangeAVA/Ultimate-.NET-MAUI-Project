using EducationalApp.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;

namespace EducationalApp.Services
{
    public class CourseService : ICourseService
    {

        private List<Course> courses = new List<Course>();
        public async Task<List<Course>?> List()
        {
            await this.GetData();
            return courses;
        }

        private async Task GetData()
        {
            courses = new List<Course>();
            try
            {
                string fileName = "Courses.json";

                using var stream = await FileSystem.OpenAppPackageFileAsync(fileName);
                using var reader = new StreamReader(stream);
                var json = await reader.ReadToEndAsync();

                var options = new JsonSerializerOptions
                {
                    PropertyNameCaseInsensitive = true 
                };

                courses = JsonSerializer.Deserialize<List<Course>>(json, options) ?? new List<Course>();
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.ToString());
            }
        }
    }

    public interface ICourseService
    {
        Task<List<Course>?> List();
    }
}
