using DiagnoseAPI.ApiModel.Base;
using DiagnoseAPI.ApiModel.Heart;
using DiagnoseAPI.Data;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

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

        [HttpGet("HeartDiseaseStatus")]
        public async Task<ApiResponse> GetHeartDiseaseStatus(HeartDiseaseDetectionDto diseaseDetectionDto)
        {
            string errorMessage = default;

            var result = default(object);

            if(diseaseDetectionDto is null)
            {

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

            if(predictResult.Target==1)
            {
                result = "Patient is Positive";
            }

            else if(predictResult.Target==0)
            {
                re
            }



        }
    }


}
