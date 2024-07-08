using DiagnoseAPI.ApiModel.Base;
using DiagnoseAPI.ApiModel.Heart;
using DiagnoseAPI.Data;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using System.Diagnostics;
using System.Net;

namespace DiagnoseAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class HeartController : ControllerBase
    {
        private readonly ApplicationDbContext applicationDb;

        public HeartController(ApplicationDbContext applicationDb)
        {
            this.applicationDb = applicationDb;
        }

        [HttpPost("HeartDiseaseStatus")]
        public async Task<ApiResponse> GetHeartDiseaseStatus(HeartDiseaseDetectionDto diseaseDetectionDto)
        {
            string errorMessage = default;

            var result = default(object);

            try
            {
                if (diseaseDetectionDto is null)
                {
                    Response.StatusCode = (int)HttpStatusCode.BadRequest;

                    return new ApiResponse
                    {
                        ErrorMessage = "Invaid details provided"
                    };
                }

                var patientData = new HeartDiseaseDetection.ModelInput()
                {
                    Age = diseaseDetectionDto.Age,
                    Sex = diseaseDetectionDto.Sex,
                    Cp = diseaseDetectionDto.Cp,
                    Trestbps = diseaseDetectionDto.Trestbps,
                    Chol = diseaseDetectionDto.Chol,
                    Fbs = diseaseDetectionDto.Fbs,
                    Restecg = diseaseDetectionDto.Restecg,
                    Thalach = diseaseDetectionDto.Thalach,
                    Exang = diseaseDetectionDto.Exang,
                    Oldpeak = diseaseDetectionDto.Oldpeak,
                    Slope = diseaseDetectionDto.Slope,
                    Ca = diseaseDetectionDto.Ca,
                    Thal = diseaseDetectionDto.Thal,

                };

                var predictResult = HeartDiseaseDetection.Predict(patientData);

                string message;

                if (predictResult.Target == 1)
                {
                    message = "Patient is Positive";
                }

                else if (predictResult.Target == 0)
                {
                    message = "Patient is negative";
                }
                else
                {
                    Response.StatusCode = (int)HttpStatusCode.NotFound;

                    return new ApiResponse
                    {
                        ErrorMessage = "No result found"
                    };
                }

                result = message;
            }
            catch (Exception ex)
            {
                Response.StatusCode = (int)HttpStatusCode.InternalServerError;
                errorMessage = ex.Message;
            }

            return new ApiResponse
            {
                Result = result,
                ErrorMessage = errorMessage
            };

           


        }

        [HttpPost("DetectionPy")]
        public async Task<ApiResponse> GetResultFromPython(HeartDiseasePy heartDisease)
        {
            string errorMessage = default;

            var result = default(object);
            try
            {
                if (heartDisease == null)
                {
                    Response.StatusCode = (int)HttpStatusCode.BadRequest;

                    return new ApiResponse
                    {
                        ErrorMessage = "Check your input again"
                    };
                }


                string inputJson = JsonConvert.SerializeObject(heartDisease);

                inputJson = inputJson.Replace("\"", "\\\"");

                var startInfo = new ProcessStartInfo
                {
                    FileName = @"C:\Users\AHMEED\Desktop\Diagnostics\HeartDetector\venv\Scripts\python.exe",
                    Arguments = $@"C:\Users\AHMEED\Desktop\Diagnostics\HeartDetector\predict.py ""{inputJson}""",
                    RedirectStandardOutput = true,
                    RedirectStandardError = true,  
                    UseShellExecute = false,
                    CreateNoWindow = true
                };

                using (var process = Process.Start(startInfo))
                {
                    process.WaitForExit();
                    var outputTask = process.StandardOutput.ReadToEndAsync();
                    var errorTask =  process.StandardError.ReadToEndAsync();

                    await Task.WhenAll(outputTask, errorTask);

                    var output = outputTask.Result;
                    var error = errorTask.Result;

                    if (process.ExitCode != 0)
                    {
                        Response.StatusCode = (int)HttpStatusCode.NotFound;
                        return new ApiResponse
                        {
                            ErrorMessage = $"process with python code failed{error}"
                        };
                    }

                    result = output;
                }
            }
            catch (Exception ex)
            {
                Response.StatusCode = (int)HttpStatusCode.InternalServerError;
                errorMessage = ex.Message;
            }


            return new ApiResponse
            {
                Result = result,
                ErrorMessage = errorMessage
            };


        }


    }


}
