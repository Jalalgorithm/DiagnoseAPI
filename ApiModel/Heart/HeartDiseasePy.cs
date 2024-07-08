namespace DiagnoseAPI.ApiModel.Heart
{
    public class HeartDiseasePy
    {
        public int Age { get; set; }
        public string Sex { get; set; }
        public string ChestPainType { get; set; }
        public int RestingBP { get; set; }
        public int Cholesterol { get; set; }
        public int FastingBS { get; set; }
        public string RestingECG { get; set; }
        public int MaxHR { get; set; }
        public string ExerciseAngina { get; set; }
        public float OldPeak { get; set; }
        public string ST_Slope { get; set; }
    }
}
